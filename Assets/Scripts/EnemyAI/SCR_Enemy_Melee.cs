using UnityEngine;
using UnityEngine.AI;

public class SCR_Enemy_Melee : MonoBehaviour {
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private int damage = 5;
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float attackRadius = 2f;
    [SerializeField] private float attackCooldown = 1f;
    private float nextAttackTime = 0f;
    NavMeshAgent _agent; 
    private int currentPatrolIndex = 0; 
    private Transform player; 
    private bool chasingPlayer = false; 

    private void Start() {
        _agent = GetComponent<NavMeshAgent>();
        MoveToNextPatrolPoint();
    }

    private void Update() {
        if (chasingPlayer)
        {
            // Chase the player
            _agent.SetDestination(player.position);
            if (player != null && Time.time >= nextAttackTime)
            {
                Attack(player);
                nextAttackTime = Time.time + attackCooldown; 
            }
        }
        else
        {
            // Patrol logic
            if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
            {
                MoveToNextPatrolPoint();
            }
        }

        Collider[] playerInRange = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (Collider col in playerInRange)
        {
            if (col.CompareTag("Player"))
            {
                player = col.transform; 
                chasingPlayer = true;
                //Attack(player);
                break; 

            }
        }

    }

    private void MoveToNextPatrolPoint() {
        if (patrolPoints.Length == 0) return;

        _agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    private void Attack(Transform player)
    {
        if (Vector3.Distance(transform.position, player.position) <= attackRadius)
        {
            Debug.Log("Attack! Damage:" + damage);
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
