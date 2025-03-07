using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.VFX; // Import VFX namespace

public class SCR_Shotgun : MonoBehaviour
{
    [SerializeField] private float range = 100f;
    [SerializeField] private int damage = 5;
    [SerializeField] private Camera fpsCamera;
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private float hitSpotLifetime = 2f;
    private int adjustedDamage;

    [Header("Shotgun Parameters")]
    [SerializeField] private float spreadArea = 5f;
    [SerializeField] private int pelletsPerShot = 8;

    [Header("Reload Values")]
    [SerializeField] private int clipSize = 6;
    [SerializeField] private int shotsFired = 0;
    [SerializeField] private float reloadSpeed = 2.5f;
    [SerializeField] private bool _isReloading = false;
    [SerializeField] private bool clipEmpty = false;

    [Header("HUD Reference")]
    [SerializeField] private SCR_HeadsUpDisplay hud;
    public SCR_FloatingAmmo ammoOrb;
    [SerializeField] private Animator shotgunAnimation;

    [Header("Visual Effects")]
    [SerializeField] private VisualEffect muzzleFlashVFX; // Assign muzzle flash VFX in Inspector
    [SerializeField] private Transform muzzlePoint; // Assign muzzle point in Inspector

    void Awake()
    {
        hud = SCR_HeadsUpDisplay.Instance;
    }

    void Start()
    {
        if (hud.reticalOn)
        {
            hud.pointReticle.SetActive(false);
            hud.shotgunReticle.SetActive(true);
        }

        hud.UpdateAmmoCount(shotsFired, clipSize);
        shotgunAnimation.Play("Shotgun_Idle");

        adjustedDamage = (int)SCR_GameController.Instance.CurrentPlayer.GetComponent<SCR_FirstPersonController>().DamageMultiplier * damage;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && (shotsFired < clipSize) && !_isReloading)
        {
            ShootShotgun();
            AutoReload();
        }

        if (Input.GetKeyDown(KeyCode.R) && !_isReloading)
        {
            _isReloading = true;
            StartCoroutine(Reload());
        }
    }

    private void OnEnable()
    {
        _isReloading = false;
        hud.UpdateAmmoCount(shotsFired, clipSize);
        if (hud.reticalOn)
        {
            hud.pointReticle.SetActive(false);
            hud.shotgunReticle.SetActive(true);
        }

        shotgunAnimation.Play("Shotgun_Idle");
    }

    private void ShootShotgun()
    {
        shotgunAnimation.Play("Shotgun_Shoot", 0, 0f);
        shotsFired++;
        hud.UpdateAmmoCount(shotsFired, clipSize);
        AudioManager.Instance.PlayOne("Shotgun_Fire");

        // **Play muzzle flash VFX**
        if (muzzleFlashVFX)
        {
            muzzleFlashVFX.Play();
        }

        for (int i = 0; i < pelletsPerShot; i++)
        {
            // Generate a random spread direction for each of the pellets
            Vector3 spreadDirection = fpsCamera.transform.forward +
                                       new Vector3(
                                           Random.Range(-spreadArea, spreadArea),
                                           Random.Range(-spreadArea, spreadArea),
                                           Random.Range(-spreadArea, spreadArea)
                                       ).normalized * 0.1f;

            if (Physics.Raycast(fpsCamera.transform.position, spreadDirection, out RaycastHit hit, range))
            {
                Debug.Log($"Hit: {hit.collider.name}");
                IDamageble damageInstance = hit.collider.GetComponent<IDamageble>();
                if (damageInstance != null)
                {
                    if (hit.collider.TryGetComponent<SCR_Enemy>(out SCR_Enemy enemy))
                    {
                        enemy.HitEffect();
                        hit.collider.GetComponent<SCR_HitTwitchEffect>().TriggerTwitch();

                    }
                    damageInstance.TakeDamage(adjustedDamage);
                    Debug.Log("Damaged source");
                }

                GameObject hitEffect = Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                StartCoroutine(DestroyHitSpot(hitEffect, hitSpotLifetime));
            }
        }
    }

    private void AutoReload()
    {
        if (shotsFired == clipSize)
        {
            clipEmpty = true;
            _isReloading = true;
            StartCoroutine(Reload());
        }
    }

    private IEnumerator DestroyHitSpot(GameObject hitSpot, float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(hitSpot);
    }

    private IEnumerator Reload()
    {
        _isReloading = true;
        shotgunAnimation.Play("Shotgun_Reload", 0, 0f);
        AudioManager.Instance.PlayOne("Shotgun_Reload");
        Debug.Log("Reloading...");
        ammoOrb.StartReload(reloadSpeed);
        yield return new WaitForSeconds(reloadSpeed);
        shotsFired = 0;
        _isReloading = false;
        clipEmpty = false;
        hud.UpdateAmmoCount(shotsFired, clipSize);
        Debug.Log("Reload complete!");
    }
}
