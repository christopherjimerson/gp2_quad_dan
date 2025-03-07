using System;
using UnityEngine;
using UnityEngine.AI;

public class MoveApproach : MovementStrategy
{
    
    [SerializeField] protected float stopWhenWithin = 6.2f;
    
    public GameObject _target = null;
    private bool _canMove = true;
    private NavMeshAgent _agent;

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.4f, 1f, 0.2f, 0.6f);
        Gizmos.DrawWireSphere(transform.position, stopWhenWithin);
    }

    void Start() {
        _agent = GetComponent<NavMeshAgent>(); // Get reference to NavMeshAgent
    }

    void Update()
    {
        if ((TryGetComponent<SCR_Enemy>(out SCR_Enemy stunCheck)) && stunCheck.isStunned == true)
        {
            StopMovement();
        }
        else
        {
            ResumeMovement();
        }
    }


    public override void Execute(AEnemy enemy)
    {
        if (!_canMove || _agent == null) return;

        var targetIsValid = _target && _target.activeSelf;

        if (targetIsValid)
        {
            
            var distanceToTarget = Vector3.Distance(transform.position, _target.transform.position);

            if (distanceToTarget > enemy.PerceptionRadius) { _target = null; }
            else if (distanceToTarget > stopWhenWithin) //keep target but don't move
            {
                
                var directionToTarget = (_target.transform.position - transform.position).normalized;

                Vector3 moveTo = _target.transform.position - (directionToTarget * stopWhenWithin);
                //
                //Let's try it like that.
                
                Ray ray = new Ray(moveTo, Vector3.down);
                if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit))
                {
                    AudioManager.Instance.Play("Enemy_Idle");
                    enemy.Agent.destination = hit.point;  
                }
            }
            else
            {
                //enemy.Agent.destination = transform.position;
            }
        }
        else
        {
            TargetClosest(enemy);
        }
        Debug.Log("Printing Targets:");
        foreach (var target in AEnemy.Targets)
        {
            Debug.Log(target);
        }
        Debug.Log(_target);

    }

    public void TargetClosest(AEnemy enemy)
    {
        //Move towards closest target:
        var distanceToClosest = enemy.PerceptionRadius;
                
        foreach (var target in AEnemy.Targets)
        {
            var distance = Vector3.Distance(target.transform.position, enemy.transform.position);
            if (distance < distanceToClosest)
            {
                //TODO: Check visibility?
                distanceToClosest = distance;
                _target = target;
            }
        } 

    }

    public void StopMovement()
    {
        _canMove = false;
        if (_agent) _agent.isStopped = true;
    }

    public void ResumeMovement() {
        _canMove = true;
        if (_agent) _agent.isStopped = false; 
    }

    public void MoveTowardsTarget()
    {
        
    }


}
