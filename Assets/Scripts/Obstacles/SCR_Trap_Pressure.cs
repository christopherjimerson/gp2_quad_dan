using UnityEngine;

public class SCR_Trap_Pressure : MonoBehaviour
{
    [SerializeField] private int trapDamage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(trapDamage);
        }
    }

}
