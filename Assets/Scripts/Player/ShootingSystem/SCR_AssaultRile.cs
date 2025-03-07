using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.VFX; // Import VFX namespace

public class SCR_AssaultRifle : MonoBehaviour
{
    [SerializeField] float range = 100f;
    [SerializeField] int damage = 2;
    [SerializeField] Camera fpsCamera;
    [SerializeField] GameObject hitEffectPrefab;
    [SerializeField] float hitSpotLifetime = 2f;
    private int adjustedDamage;

    [Header("Firing Values")]
    [SerializeField] private float fireRate = 0.1f;
    private bool isFiring = false;

    [Header("Reload Values")]
    [SerializeField] private int clipSize = 30;
    [SerializeField] private int shotsFired;
    [SerializeField] private float reloadSpeed = 2.0f;
    private bool _isReloading = false;
    private bool clipEmpty = false;

    [Header("HUD Reference")]
    [SerializeField] private SCR_HeadsUpDisplay hud;
    public SCR_FloatingAmmo ammoOrb;

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
        if (hud.reticalOn)
        {
            hud.pointReticle.SetActive(true);
            hud.shotgunReticle.SetActive(false);
        }

        adjustedDamage = (int)SCR_GameController.Instance.CurrentPlayer.GetComponent<SCR_FirstPersonController>().DamageMultiplier * damage;
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.R) && !_isReloading) || clipEmpty)
        {
            _isReloading = true;
            StartCoroutine(Reload());
        }

        if (Input.GetKey(KeyCode.Mouse0) && shotsFired < clipSize && !_isReloading)
        {
            if (!isFiring)
            {
                StartCoroutine(HandleContinuousFire());

            }

        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isFiring = false;
            StopMuzzleFlash();
        }
    }

    void OnEnable()
    {
        if (hud == null)
        {
            return;
        }
        _isReloading = false;

        hud.UpdateAmmoCount(shotsFired, clipSize);
        if (hud.reticalOn)
        {
            hud.pointReticle.SetActive(true);
            hud.shotgunReticle.SetActive(false);
        }
    }

    private void ShootRay()
    {
        AudioManager.Instance.PlayOne("AssaultRifle_Fire");
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

    private void StopMuzzleFlash()
    {
        if (muzzleFlashVFX)
        {
            muzzleFlashVFX.Stop();
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

    private IEnumerator HandleContinuousFire()
    {
        isFiring = true;
        while (isFiring && shotsFired < clipSize)
        {
            ShootRay();
            yield return new WaitForSeconds(fireRate);
        }
        AutoReload();
        isFiring = false;
        StopMuzzleFlash();
    }

    private IEnumerator DestroyHitSpot(GameObject hitSpot, float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(hitSpot);
    }

    private IEnumerator Reload()
    {
        _isReloading = true;
        AudioManager.Instance.PlayOne("AssaultRifle_Reload");
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
