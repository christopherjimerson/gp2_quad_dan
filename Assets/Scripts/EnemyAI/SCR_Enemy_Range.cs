using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class SCR_Enemy_Range : MonoBehaviour {
    public float detectionRadius = 10f;
    public int damage;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = .5f;
    public float nextAttackTime = 0f;
    public float projectileSpeed = 100f;
    private NavMeshAgent agent;
    private Transform player;
    private bool playerDetected = false;
    private Vector3[] patrolPoints;
    private int currentPatrolIndex = 0;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        GeneratePatrolPoints();
        MoveToNextPatrolPoint();
    }

    private void Update() {
        DetectPlayer();

        if (playerDetected && player != null)
        {
            agent.SetDestination(player.position);

            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + fireRate;
            }
        }
        else
        {
            Patrol();
        }
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
        Debug.Log("Fire!");
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        SCR_ProjectileBehavior projectileScript = projectile.GetComponent<SCR_ProjectileBehavior>();
        projectileScript.SetDamage(damage);
        Vector3 targetPosition = player.position + Vector3.up * .75f;
        Vector3 direction = (targetPosition - firePoint.position).normalized;
        projectile.GetComponent<Rigidbody>().AddForce(direction.normalized * projectileSpeed, ForceMode.Impulse);
    }

    private void GeneratePatrolPoints() {
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < 4; i++)
        {
            Vector3 randomPoint = transform.position + new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 2f, NavMesh.AllAreas))
            {
                points.Add(hit.position);
            }
        }
        patrolPoints = points.ToArray();
    }

    private void Patrol() {
        if (patrolPoints.Length == 0 || agent.pathPending || agent.remainingDistance > 0.5f) return;
        MoveToNextPatrolPoint();
    }

    private void MoveToNextPatrolPoint() {
        if (patrolPoints.Length == 0) return;
        agent.SetDestination(patrolPoints[currentPatrolIndex]);
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

}
