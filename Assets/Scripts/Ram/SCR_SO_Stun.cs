using UnityEngine;
[CreateAssetMenu(fileName = "DDoSAttack", menuName = "Scriptable Objects/DDoSAttack")]

public class SCR_SO_Stun : SCR_SO_Ram {
    public override void OnEquipRam() {
        var playerController = SCR_GameController.Instance.CurrentPlayer.GetComponent<SCR_FirstPersonController>();
        if (playerController)
        {
            playerController.stunEquipped = true;
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
            playerController.stunEquipped = false;
        }
        else
        {
            Debug.LogWarning("Couldn't find playerController on equip. RAM had no effects.");
        }
    }
}
