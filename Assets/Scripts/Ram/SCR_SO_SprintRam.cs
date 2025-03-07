using UnityEngine;
[CreateAssetMenu(fileName = "SCR_SO_SprintRam", menuName = "Scriptable Objects/SCR_SO_SprintRam")]
public class SCR_SO_SprintRam : SCR_SO_Ram {
    public override void OnEquipRam() {
        var playerController = SCR_GameController.Instance.CurrentPlayer.GetComponent<SCR_FirstPersonController>();
        if (playerController)
        {
            playerController.walkSpeed += 2f;
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
            playerController.walkSpeed -= 2f;
        }
        else
        {
            Debug.LogWarning("Couldn't find playerController on unequip. RAM had no effects.");
        }
    }
}
