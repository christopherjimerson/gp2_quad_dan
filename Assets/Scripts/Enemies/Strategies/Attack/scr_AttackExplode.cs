using System;
using NUnit.Framework.Constraints;
using UnityEngine;

public class AttackExplode : AttackStrategy
{
    [SerializeField]
    protected float chargeUpRange = 3f;
    [SerializeField]
    protected float explosionRange = 6f;
    [SerializeField]
    protected int explosionDamage = 20;
    [SerializeField]
    protected Timer chargeUpTimer;
    [SerializeField]
    protected Timer windDownTimer;
    [SerializeField]
    protected ParticleSystem boom;

    private MoveApproach moveApproach;
    
    public bool _isChargingExplosion = false;
    public bool _isWindingDown = false;
    GameObject _target = null;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        chargeUpTimer.timerDone += Explode;
        windDownTimer.timerDone += EnableExplosion;
        chargeUpTimer.Stop();
        windDownTimer.Stop();

        moveApproach = GetComponent<MoveApproach>();

    }
    
    void OnDisable()
    {
        chargeUpTimer.timerDone -= Explode;
        windDownTimer.timerDone -= EnableExplosion;
        
    }
    
    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.2f, 0.1f, 0.8f);
        Gizmos.DrawWireSphere(transform.position, explosionRange);
        Gizmos.color = new Color(1f, 0.6f, 0.1f, 0.8f);
        Gizmos.DrawWireSphere(transform.position, chargeUpRange);
    }

    // Update is called once per frame
    
    public override void Execute(AEnemy enemy)
    {
        //throw new System.NotImplementedException();

        if (_isWindingDown || _isChargingExplosion) return;

        if (_target && _target.activeSelf && Vector3.Distance(transform.position, _target.transform.position) < chargeUpRange)
        {
                ChargeUp();
        }
        else
        {
            GetClosestTarget(enemy);
        }


    }

    public void ChargeUp()
    {
        _isChargingExplosion = true;
        chargeUpTimer.Restart();

        if (moveApproach)
        {
            moveApproach.StopMovement(); 
        }
    }

    public void Explode()
    {
        bool isStunned = this.GetComponent<SCR_Enemy>().isStunned;
        if (isStunned) return;
        boom.Play();
        Debug.Log("Boom!");

        //This breaks if the collection is modified :C
        for (int i = 0; i < AEnemy.Targets.Count; i++)
        {
            var target = AEnemy.Targets[i];
            if (Vector3.Distance(target.transform.position, transform.position) > explosionRange) { continue; }

            SCR_FirstPersonController fpsController = target.GetComponent<SCR_FirstPersonController>();
            if (!fpsController._isShieldActive)
            {
                PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
                if (playerHealth) { playerHealth.TakeDamage(explosionDamage); }
            }

        }
        
        _isChargingExplosion = false;
        _isWindingDown = true;
        chargeUpTimer.Stop();
        windDownTimer.Restart();
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
    

    public void EnableExplosion()
    {
        _isWindingDown = false;
        if (moveApproach)
        {
            moveApproach.ResumeMovement(); // Re-enable movement after explosion
        }
    }
    
}
