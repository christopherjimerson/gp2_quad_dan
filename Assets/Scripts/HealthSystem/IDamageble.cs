using UnityEngine;

public interface IDamageble
{

    int GetHealth();
    bool IsAlive();
    void TakeDamage(int damage);
    public void HealUnit(int healAmount);
}
