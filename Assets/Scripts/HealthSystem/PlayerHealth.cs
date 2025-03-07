using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageble
{
    public int currentHealth;
    public int maxHealth;
    private SCR_HeadsUpDisplay hud;

    public delegate void OnPlayerDamaged(int damage); 
    public static event OnPlayerDamaged onPlayerDamaged;

    void Start()
    {
        hud = SCR_HeadsUpDisplay.Instance;
        currentHealth = maxHealth;
        hud.UpdateHealth(currentHealth, maxHealth);
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.H))
        {
            HealUnit(10);
            Debug.Log($"Player healed. Current Health: {GetHealth()}");
        }

        if (!IsAlive())
        {
            Debug.Log("Player is dead!");
        }
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    public virtual void TakeDamage(int damage)
    {
        if (!IsAlive()) return;
        AudioManager.Instance.PlayOne("Player_Damage");
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        hud.UpdateHealth(currentHealth, maxHealth);
        
        onPlayerDamaged?.Invoke(damage);

        if (currentHealth <= 0)
        {
            EventManager.Instance.PlayerDied();
            Destroy(gameObject);
        }
    }

    public void HealUnit(int healAmount)
    {
        if (!IsAlive()) return;
        currentHealth += healAmount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        currentHealth = Mathf.Max(currentHealth, 0);
        hud.UpdateHealth(currentHealth, maxHealth);

    }
}
