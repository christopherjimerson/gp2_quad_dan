using UnityEngine;
[CreateAssetMenu(fileName = "SCR_SO_CrosshairRam", menuName = "Scriptable Objects/SCR_SO_CrosshairRam")]

public class SCR_SO_CrosshairRam : SCR_SO_Ram {
    public override void OnEquipRam() {
         SCR_HeadsUpDisplay.Instance.reticalOn = true; 
    }
    public override void OnUnEquipRam()
    {
        SCR_HeadsUpDisplay.Instance.reticalOn = false;
        SCR_HeadsUpDisplay.Instance.pointReticle.gameObject.SetActive(false);
        SCR_HeadsUpDisplay.Instance.shotgunReticle.gameObject.SetActive(false);
        SCR_HeadsUpDisplay.Instance.scopeReticle.gameObject.SetActive(false);
    }
}
