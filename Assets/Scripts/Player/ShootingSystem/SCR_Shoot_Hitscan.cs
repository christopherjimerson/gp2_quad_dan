using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.VFX; // Import VFX namespace

public class SCR_Shoot_Hitscan : MonoBehaviour
{
    [SerializeField] float range = 100f;
    [SerializeField] int damage = 5;
    [SerializeField] Camera fpsCamera;
    [SerializeField] GameObject hitEffectPrefab;
    [SerializeField] float hitSpotLifetime = 2f;
    private int adjustedDamage;

    [Header("Reload Values")]
    [SerializeField] private int clipSize = 30;
    [SerializeField] private int shotsFired;
    [SerializeField] private float reloadSpeed = 2.0f;
    public bool _isReloading = false;
    public bool clipEmpty = false;

    [Header("HUD Reference")]
    [SerializeField] private SCR_HeadsUpDisplay hud;
    public SCR_FloatingAmmo ammoOrb;

    [Header("Animator")]
    [SerializeField] private Animator gunAnimation;

    [Header("Visual Effects")]
    [SerializeField] private VisualEffect muzzleFlashVFX; // Assign muzzle flash VFX in Inspector
    [SerializeField] private Transform muzzlePoint; // Assign muzzle point in Inspector

    void Awake()
    {
        hud = SCR_HeadsUpDisplay.Instance;
    }

    void Start()
    {
        hud = SCR_HeadsUpDisplay.Instance;
        hud.UpdateAmmoCount(shotsFired, clipSize);

        if (!hud.pointReticle) return;
        if (hud.reticalOn)
        {
            hud.pointReticle.SetActive(true);
            hud.shotgunReticle.SetActive(false);
        }
        gunAnimation.Play("Pistol_Idle_Arms");

        adjustedDamage = (int)SCR_GameController.Instance.CurrentPlayer.GetComponent<SCR_FirstPersonController>().DamageMultiplier * damage;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && (shotsFired < clipSize) && !_isReloading && SCR_GameController.Instance.CurrentPlayer.GetComponent<SCR_FirstPersonController>().isPaused == false)
        {
            ShootRay();
            AutoReload();
        }

        if (Input.GetKeyDown(KeyCode.R) && !_isReloading)
        {
            _isReloading = true;
            StartCoroutine(Reload());
        }
    }

    void OnEnable()
    {
        if (hud == null)
        {
            return;
        }
        hud.UpdateAmmoCount(shotsFired, clipSize);
        _isReloading = false;

        if (hud.pointReticle)
        {
            hud.pointReticle.SetActive(true);
            hud.shotgunReticle.SetActive(false);
        }
        gunAnimation.Play("Pistol_Idle_Arms");
    }

    private void ShootRay()
    {
        gunAnimation.Play("Pistol_Shoot_Gun", 0, 0f);
        gunAnimation.Play("Pistol_Shoot_Arms", 1, 0f);
        AudioManager.Instance.PlayOne("Pistol_Fire");
        shotsFired++;
        hud.UpdateAmmoCount(shotsFired, clipSize);

        // **Play muzzle flash VFX**
        if (muzzleFlashVFX)
        {
            muzzleFlashVFX.Play();
        }

        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range))
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
        gunAnimation.Play("Pistol_Reload_Gun", 0, 0f);
        gunAnimation.Play("Pistol_Reload_Arms", 1, 0f);
        AudioManager.Instance.PlayOne("Pistol_Reload");
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
