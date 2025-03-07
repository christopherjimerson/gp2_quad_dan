using UnityEngine;

public class Unit : MonoBehaviour, IDamageble
{
    public int currentHealth;
    public int maxHealth;

    protected Unit(int setMaxHealth)
    {
        maxHealth = setMaxHealth;
        currentHealth = maxHealth;
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
        if (IsAlive())
        {
            currentHealth -= damage;
            currentHealth = Mathf.Max(currentHealth, 0);
        }
    }

    public void DamageUnit(int damageAmount)
    {
        throw new System.NotImplementedException();
    }

    public void HealUnit(int healAmount)
    {
        if (!IsAlive()) return;

        currentHealth += healAmount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }
}
