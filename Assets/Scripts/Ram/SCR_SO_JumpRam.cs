using UnityEngine;

[CreateAssetMenu(fileName = "SCR_SO_JumpRam", menuName = "Scriptable Objects/SCR_SO_JumpRam")]
public class SCR_SO_JumpRam : SCR_SO_Ram {


    public override void OnEquipRam()
    {
        var playerController = SCR_GameController.Instance.CurrentPlayer.GetComponent<SCR_FirstPersonController>();
        if (playerController)
        {
            playerController.jumpSOEquipped = true;
        }
        else
        {
            Debug.LogWarning("Couldn't find playerController on equip. RAM had no effects.");
        }
    }


    public override void OnUnEquipRam() {
        var playerController = SCR_GameController.Instance.CurrentPlayer.GetComponent<SCR_FirstPersonController>();
        if (playerController)
        {
            playerController.jumpSOEquipped = false;
        }
        else
        {
            Debug.LogWarning("Couldn't find playerController on unequip. RAM had no effects.");
        }
    }

}
