using UnityEngine;
using System.Collections;

public class SCR_Enemy_Turret : MonoBehaviour {
    [Header("Detection & Firing")]
    public float detectionRadius = 10f;
    public int damage;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    public float projectileSpeed = 100f;
    public float rotationSpeed = 5f;

    private float nextAttackTime = 0f;
    private Transform player;
    private bool playerDetected = false;

    private void Update() {
        DetectPlayer();

        if (playerDetected && Time.time >= nextAttackTime)
        {
            RotateTowardsPlayer(); 
            Attack();
            nextAttackTime = Time.time + fireRate;
        }
    }

    private void RotateTowardsPlayer() {
        if (player == null) return;

        Vector3 targetDirection = (player.position - firePoint.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        firePoint.rotation = Quaternion.Slerp(firePoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    private void DetectPlayer() {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (Collider col in playerInRange)
        {
            if (col.CompareTag("Player"))
            {
                player = col.transform;
                playerDetected = true;
                return;
            }
        }

        playerDetected = false;
    }

    private void Attack() {
        if (player == null) return;

        Debug.Log("Fire!");
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        SCR_ProjectileBehavior projectileScript = projectile.GetComponent<SCR_ProjectileBehavior>();
        projectileScript.SetDamage(damage);
        Vector3 targetPosition = player.position + Vector3.up * 0.75f;
        Vector3 direction = (targetPosition - firePoint.position).normalized;
        projectile.GetComponent<Rigidbody>().AddForce(direction.normalized * projectileSpeed, ForceMode.Impulse);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

