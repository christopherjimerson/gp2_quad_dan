using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SCR_SO_InsanityEffect", menuName = "Create InsanityEffect")]
public class SCR_SO_InsanityEffect : ScriptableObject
{
#region Variables
[SerializeField]
     protected bool isTriggered;
#endregion
#region Public Functions
     public void Activate()
     {
          Debug.Log("INSANITY_EFFECT_ACTIVATED : " + this.isTriggered);
          if (isTriggered) 
               return;

          ActivateLogic();
          isTriggered = true;
     }

     public void Deactivate()
     {
          Debug.Log("INSANITY_EFFECT_DEACTIVATED " + this.isTriggered);
          if (!isTriggered) 
               return;
          
          DeactivateLogic();
          isTriggered = false;
     }
#endregion
#region Virtual Functions
     protected virtual void ActivateLogic()
     {
          throw new System.NotImplementedException();
     }
     
     protected virtual void DeactivateLogic()
     {
          throw new System.NotImplementedException();
     }

     private void OnDestroy()
     {
        isTriggered = false;
     }
#endregion
}
