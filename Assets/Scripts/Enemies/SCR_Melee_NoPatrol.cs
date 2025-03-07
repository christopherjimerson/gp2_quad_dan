using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class SCR_Enemy_Melee_NoPatrol : MonoBehaviour {
    public float detectionRadius = 10f;
    public int damage = 5;
    public float attackRadius = 2f;
    public float attackCooldown = 1f;
    private float nextAttackTime = 0f;
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
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > attackRadius)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
            }
            else
            {
                agent.isStopped = true;
                if (Time.time >= nextAttackTime)
                {
                    Attack();
                    nextAttackTime = Time.time + attackCooldown;
                }
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
        Debug.Log("Melee Attack! Damage: " + damage);
        player.GetComponent<PlayerHealth>().TakeDamage(damage);
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
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
