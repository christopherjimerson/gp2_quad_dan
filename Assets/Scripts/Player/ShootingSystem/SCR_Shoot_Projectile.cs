using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SCR_Shoot_Projectile : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform firePoint;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            FireProjectile();
            AudioManager.Instance.Play("Pew pew");
        }
    }

    void FireProjectile() {
        //GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        SCR_ProjectilePool.Instance.GetProjectile(firePoint);
    }
}
