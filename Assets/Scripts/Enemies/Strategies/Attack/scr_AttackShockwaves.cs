using UnityEngine;


public class AttackShockwaves : AttackStrategy
{
    //TODO: This is the same as AimThenFire, REFACTOR.  
    
    [SerializeField]
    protected GameObject projectile;
    [SerializeField]
    protected float projectileSpeed = 4f;
    [SerializeField]
    protected float shockwaveDuration = 4f;
    
    [SerializeField]
    protected float startFireRange = 6f;
    [SerializeField]
    protected float stopFireRate = 9f;
    [SerializeField]
    protected Timer fireCooldown;

    bool _fireReady = false;
    private bool _targetInFireRange = false;
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
                }
                else
                {
                    if (_fireReady) Shoot(enemy);
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
    
    void Shoot(AEnemy enemy)
    {
        //TODO: This would benefit from projectile pooling.
        var direction = (_target.transform.position - transform.position).normalized;
        ProjectileReturning newProj = Instantiate(
            original: projectile, 
            transform.position + direction * 0.4f,
            Quaternion.identity
        ).GetComponent<ProjectileReturning>();

        newProj.SetUpAndFire(direction, projectileSpeed, shockwaveDuration);
        
        EnterFireCooldown();
    }

    void EnterFireCooldown()
    {
        _fireReady = false;
        fireCooldown.Restart();
    }

}
