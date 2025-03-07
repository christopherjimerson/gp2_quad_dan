using System;
using System.CodeDom.Compiler;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class SCR_Enemy : MonoBehaviour, IDamageble
{
    [SerializeField] private bool isAlive = true;
    [SerializeField] public int enemyHealth = 30;
    [SerializeField] public int maxEnemyHealth = 30;
    [SerializeField] public int enemyDamage = 3;
    [SerializeField] public bool canBeInfect = true;
    [SerializeField] public GameObject hitEffectPrefab;

    [Header("Infection Parameters")] [SerializeField]
    public bool isInfected;

    [SerializeField] public bool hasBeenInfected;
    [SerializeField] private float infectionRadius;
    [SerializeField] public int infectDamage;
    [SerializeField] public float infectionDuration;
    [SerializeField] public float infectionDamageRate;
    [SerializeField] private GameObject infectionDisplay;

    [Header("Stun Parameters")] [SerializeField]
    public bool isStunned;

    [SerializeField] private SCR_EnemyHitEffect hitEffectSpawner;
    int _initialhealth;
    bool _currentlyInfected;

    [SerializeField] private float detectionInterval = 1f; // Check every second
    [SerializeField] private float undetectedDurationThreshold = 15f;
    private float undetectedTimer = 0f;
    private bool isPlayerDetected = false;

    void Start()
    {
        _initialhealth = maxEnemyHealth;
        //StartCoroutine(DetectPlayer());

        AdjustHealthMult();
    }

    void Update()
    {
        if (isInfected && !hasBeenInfected)
        {
            SpreadInfeciton();
            InfectionDamage();

        }
    }

    void OnEnable()
    {
        SCR_EnemyController.onEnemyHealthMultChange += AdjustHealthMult;
    }

    void OnDisable()
    {
        SCR_EnemyController.onEnemyHealthMultChange -= AdjustHealthMult;
    }

    void Defeat()
    {
        isAlive = false;
        SCR_EnemyController.Instance.DefeatEnemy(gameObject);
        AudioManager.Instance.Play("Enemy_Death");
        if (Random.Range(0, 10) > 8)
            SCR_HealthOrbPool.Instance.SpawnHealthOrbs(transform.position);
    }

    void AdjustHealthMult()
    {

        var oldMaxHealth = maxEnemyHealth;
        var oldCurrentHealth = enemyHealth;
        maxEnemyHealth = (int)(SCR_EnemyController.Instance.EnemyHealthMultipler * _initialhealth);
        enemyHealth = (maxEnemyHealth - (oldMaxHealth - oldCurrentHealth));
        //Debug.Log("Enemy health changed from " + oldMaxHealth + " to " + maxEnemyHealth);
        //Debug.Log("Current health changed from " + oldCurrentHealth + " to " + enemyHealth);
    }

    public int GetHealth()
    {
        return enemyHealth;
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    public void TakeDamage(int damage)
    {
        enemyHealth -= damage;

        if (enemyHealth <= 0)
        {
            Defeat();
        }
    }

    public void HealUnit(int healAmount)
    {
        throw new NotImplementedException();
    }

    public void SpreadInfeciton()
    {
        Collider[] enemyInRange = Physics.OverlapSphere(transform.position, infectionRadius);
        foreach (Collider col in enemyInRange)
        {
            Debug.Log("Detected Other Enemy:" + col.gameObject.name);
            if (col.TryGetComponent<SCR_Enemy>(out SCR_Enemy enemy))
            {
                enemy.isInfected = true;
                enemy.infectDamage = 2;
            }
        }

    }

    public void Stunned(float stunDuration)
    {
        if (isStunned) return;
        isStunned = true;
        //if (TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        //{
        //    agent.isStopped = true;
        //}
        //if (this.TryGetComponent<MoveApproach>(out MoveApproach target))
        //{
        //    target._target = null;
        //}
        //if (TryGetComponent<MoveApproach>(out MoveApproach moveApproach))
        //{
        //    moveApproach.StopMovement(); 
        //}

        StartCoroutine(StunTimer(stunDuration));
    }

    public void InfectionDamage()
    {
        if (_currentlyInfected) return;
        _currentlyInfected = true;
        infectionDisplay.SetActive(true);
        StartCoroutine(DamageOverTime());
    }

    private IEnumerator DamageOverTime()
    {
        if (infectionDuration > 0 && enemyHealth > 0)
        {
            enemyHealth -= infectDamage;
            infectionDuration -= infectionDamageRate;
            Vector3 spawnPosition = this.transform.position + new Vector3(0, 2f, 0); 
            GameObject hitEffect = Instantiate(hitEffectPrefab, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(infectionDamageRate);
            StartCoroutine(DamageOverTime());
        }
        else if (infectionDuration <= 0 && enemyHealth > 0)
        {
            hasBeenInfected = true;
            infectionDisplay.SetActive(false);
        }

    }

    private IEnumerator StunTimer(float stunDuration)
    {
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
        TryGetComponent<AttackExplode>(out AttackExplode resetAttack);
        resetAttack._isChargingExplosion = false;
        resetAttack._isWindingDown = false;

        //AEnemy.Targets.Add(player);
        //if (TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        //{
        //    agent.isStopped = false;
        //}
        //if (this.TryGetComponent<MoveApproach>(out MoveApproach target))
        //{
        //    target._target = player;
        //}
        //if (TryGetComponent<MoveApproach>(out MoveApproach moveApproach))
        //{
        //    moveApproach.ResumeMovement();
        //    moveApproach.TargetClosest(mo); // Re-target after stun
        //}

    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    public void HitEffect()
    {
        if (hitEffectSpawner != null)
        {
            hitEffectSpawner.SpawnEffect(transform.position);
        }

    }

     private IEnumerator DetectPlayer()
    {
        while (isAlive)
        {
            yield return new WaitForSeconds(detectionInterval);
            Debug.Log("Checking");
            CheckLineOfSightToPlayer();
        }
    }

    private void CheckLineOfSightToPlayer()
    {
        if (SCR_GameController.Instance.CurrentPlayer == null) return;

        Vector3 directionToPlayer = (SCR_GameController.Instance.CurrentPlayer.transform.position - transform.position)
            .normalized;
        float distanceToPlayer = Vector3.Distance(transform.position,
            SCR_GameController.Instance.CurrentPlayer.transform.position);

        if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hitInfo, distanceToPlayer))
        {
            if (hitInfo.collider.gameObject == SCR_GameController.Instance.CurrentPlayer)
            {
                Debug.Log("Player Can Be seen");
                isPlayerDetected = true;
                undetectedTimer = 0f; 
                return;
            }
        }

        isPlayerDetected = false;
        undetectedTimer += detectionInterval;

        if (undetectedTimer >= undetectedDurationThreshold && !TryGetComponent<EnemyPowerTower>(out _))
        {
            TakeDamage(500);
        }
    } 
}
