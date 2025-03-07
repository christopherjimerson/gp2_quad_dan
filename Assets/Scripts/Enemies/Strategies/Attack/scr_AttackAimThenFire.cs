using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AttackAimThenFire : AttackStrategy
{
    /*
    [SerializeField]
    protected GameObject cannonPivot;
    */
    [SerializeField]
    protected GameObject cannon;
    /*
    [SerializeField]
    protected float cannonRotationSpeed = 20f;
    */
    [SerializeField]
    protected GameObject projectile;
    [SerializeField]
    protected int projectileDmg = 8;
    [SerializeField]
    protected float projectileSpeed = 40f;
    
    [SerializeField]
    protected float startFireRange = 6f;
    [SerializeField]
    protected float stopFireRate = 9f;
    [SerializeField]
    protected Timer fireCooldown;

    public bool _fireReady = false;
    bool _targetInFireRange = false;
    GameObject _target = null;


    void OnEnable()
    {
        fireCooldown.timerDone += EnableFire;
    }
    
    void OnDisable()
    {
        fireCooldown.timerDone -= EnableFire;
    }

    void Start()
    {
        if (stopFireRate < startFireRange)
        {
            Debug.LogWarning("stopFireRate cannot be smaller than startFireRate. Defaulting stopFireRate to startFireRate + 5");
            stopFireRate = startFireRange + 5;
        }

        _fireReady = false;
        
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.2f, 0.1f, 0.8f);
        Gizmos.DrawWireSphere(transform.position, startFireRange);
        Gizmos.color = new Color(1f, 0.6f, 0.1f, 0.8f);
        Gizmos.DrawWireSphere(transform.position, stopFireRate);
    }
    
    public override void Execute(AEnemy enemy)
    {
        var targetIsValid = _target && _target.activeSelf;
        

        if (targetIsValid)
        {
            var distanceToTarget = Vector3.Distance(transform.position, _target.transform.position);
            //TODO: Refactor this out of being spaghetti ala marinara
            if (_targetInFireRange)
            {
                if (distanceToTarget > stopFireRate)
                {
                    _targetInFireRange = false;
                    enemy.GetComponent<MoveApproach>()?.ResumeMovement();

                }
                else
                {
                    enemy.GetComponent<MoveApproach>()?.StopMovement();
                    Aim(enemy);
                    bool isStunned = enemy.GetComponent<SCR_Enemy>().isStunned;
                    if (_fireReady && !isStunned) Shoot(enemy);
                }
            }
            else
            {
                if (distanceToTarget < startFireRange)
                {
                    _targetInFireRange = true;
                    EnterFireCooldown();
                }
            }

        }
        else { GetClosestTarget(enemy); }
        
    }

    void EnableFire()
    {
        _fireReady = true;
    }

    void GetClosestTarget(AEnemy enemy)
    {
        var distanceToClosest = enemy.PerceptionRadius;
                
        for (int i = 0; i < AEnemy.Targets.Count; i++)
        {
            var target = AEnemy.Targets[i];
            var distance = Vector3.Distance(target.transform.position, enemy.transform.position);
            if (distance < distanceToClosest)
            {
                //TODO: Check visibility?
                distanceToClosest = distance;
                _target = target;
            }
        } 
    }

    void Aim(AEnemy enemy)
    {
        //TODO: Do this:
        //Step 1: Figure out how to do this :sob: 
        if (_target == null) return;

        Vector3 targetPosition = _target.transform.position + Vector3.up; 
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Rotate the entire enemy towards the target
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); 
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 180f);

        // Rotate the cannon separately to aim precisely
        Vector3 cannonDirection = (targetPosition - cannon.transform.position).normalized;
        Quaternion cannonRotation = Quaternion.LookRotation(cannonDirection);
        cannon.transform.rotation = Quaternion.RotateTowards(cannon.transform.rotation, cannonRotation, Time.deltaTime * 180f);
    }

    void Shoot(AEnemy enemy)
    {
        /*
        //TODO: This would benefit from projectile pooling.
        Projectile newProj = Instantiate(
            original: projectile, 
            cannon.transform.position,
            Quaternion.identity
        ).GetComponent<Projectile>();
        newProj.Speed = projectileSpeed;
        newProj.Direction = (_target.transform.position - cannon.transform.position).normalized;
        */
        Debug.Log("Pew Pew!!!");
        /*
        Vector3 targetPosition = _target.transform.position + Vector3.up * .75f;
        Vector3 direction = (targetPosition - cannon.transform.position).normalized;
        
        GameObject newProjectile = Instantiate(
            projectile, 
            cannon.transform.position + (direction * 0.4f), 
            cannon.transform.rotation);
        SCR_ProjectileBehavior projectileScript = newProjectile.GetComponent<SCR_ProjectileBehavior>();
        projectileScript.SetDamage(projectileDmg);

        newProjectile.GetComponent<Rigidbody>().AddForce(direction.normalized * projectileSpeed, ForceMode.Impulse);
        */
        AudioManager.Instance.Play("Ranged_Attack");
        Vector3 targetPosition = _target.transform.position + Vector3.up * .75f;
        Vector3 direction = (targetPosition - cannon.transform.position).normalized;
        
        GameObject newProjectile = Instantiate(projectile, cannon.transform.position, cannon.transform.rotation);
        SCR_ProjectileBehavior projectileScript = newProjectile.GetComponent<SCR_ProjectileBehavior>();
        projectileScript.SetDamage(projectileDmg);


        newProjectile.GetComponent<Rigidbody>().AddForce(direction.normalized * projectileScript.projectileSpeed, ForceMode.Impulse);
        
        EnterFireCooldown();
    }

    void EnterFireCooldown()
    {
        _fireReady = false;
        fireCooldown.Restart();
    }

}
