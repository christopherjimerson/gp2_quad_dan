using UnityEngine;

public class SCR_Target_Ram_Treassure : MonoBehaviour, IDamageble
{
    [SerializeField]
    private int health = 100;
    public int GetHealth()
    {
        return health;
    }

    public void HealUnit(int healAmount)
    {
        throw new System.NotImplementedException();
    }

    public bool IsAlive()
    {
        return gameObject.activeSelf;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        Debug.Log("chest took damage");

        if (health <= 0)
        {
            Debug.Log("Player gained RAM");

            var ram = SCR_GameController.Instance.GetRandomNotEquippedRam();
            if (ram)
            {
                SCR_GameController.Instance.latestRam = ram;
                SCR_GameController.Instance.AddRamToBackpack(ram);
            } 

            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        

    }
}
