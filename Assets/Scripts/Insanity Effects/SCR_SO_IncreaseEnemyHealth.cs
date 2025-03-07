using UnityEngine;

[CreateAssetMenu(fileName = "SCR_SO_IncreaseEnemyHealth", menuName = "Create InsanityEffect/SCR_SO_IncreaseEnemyHealth")]
public class SCR_SO_IncreaseEnemyHealth : SCR_SO_InsanityEffect
{
    
    public float percentage = 0.5f;
    
    protected override void ActivateLogic()
    {
        Debug.Log(isTriggered);
        Debug.Log("ACTIVATE INCREASE ENEMY HEALTH");
        SCR_EnemyController.Instance.EnemyHealthMultipler += percentage;
    }
     
    protected override void DeactivateLogic()
    {
        Debug.Log(isTriggered);
        Debug.Log("DEACTIVATE INCREASE ENEMY HEALTH");
        SCR_EnemyController.Instance.EnemyHealthMultipler -= percentage;
        
    }
}
