using UnityEngine;

[CreateAssetMenu(fileName = "SCR_SO_ReduceHealth", menuName = "Create InsanityEffect/SCR_SO_ReduceHealth")]
public class SCR_SO_ReduceHealth : SCR_SO_InsanityEffect
{

    public float percentage = 0.5f;
    
    protected override void ActivateLogic()
    {
        
        PlayerHealth pHealth = SCR_GameController.Instance.CurrentPlayer.GetComponent<PlayerHealth>();
        pHealth.currentHealth = (int)(pHealth.currentHealth * percentage);
        pHealth.maxHealth = (int)(pHealth.maxHealth * percentage);
        
    }
     
    protected override void DeactivateLogic()
    {
        
        PlayerHealth pHealth = SCR_GameController.Instance.CurrentPlayer.GetComponent<PlayerHealth>();
        pHealth.currentHealth = (int)(pHealth.currentHealth / percentage);
        pHealth.maxHealth = (int)(pHealth.maxHealth / percentage);
        
        
    }
}
