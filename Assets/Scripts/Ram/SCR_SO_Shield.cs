using UnityEngine;
[CreateAssetMenu(fileName = "MalwareProtection", menuName = "Scriptable Objects/MalwareProtection")]

public class SCR_SO_Shield : SCR_SO_Ram {
    public override void OnEquipRam() {
        var playerController = SCR_GameController.Instance.CurrentPlayer.GetComponent<SCR_FirstPersonController>();
        if (playerController)
        {
            playerController.shieldEquipped = true;
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
            playerController.shieldEquipped = false;
        }
        else
        {
            Debug.LogWarning("Couldn't find playerController on equip. RAM had no effects.");
        }
    }
}
