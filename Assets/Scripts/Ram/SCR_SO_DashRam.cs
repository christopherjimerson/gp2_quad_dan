using UnityEngine;

[CreateAssetMenu(fileName = "SCR_SO_DashRam", menuName = "Scriptable Objects/SCR_SO_DashRam")]

public class SCR_SO_DashRam : SCR_SO_Ram
{
    public override void OnEquipRam()
    {
        var playerController = SCR_GameController.Instance.CurrentPlayer.GetComponent<SCR_FirstPersonController>();
        if (playerController)
        {
            playerController.dashSOEquipped = true;
        }
        else
        {
            Debug.LogWarning("Couldn't find playerController on equip. RAM had no effects.");
        }
    }


    public override void OnUnEquipRam()
    {
        var playerController = SCR_GameController.Instance.CurrentPlayer.GetComponent<SCR_FirstPersonController>();
        if (playerController)
        {
            playerController.dashSOEquipped = false;
        }
        else
        {
            Debug.LogWarning("Couldn't find playerController on unequip. RAM had no effects.");
        }
    }
}
