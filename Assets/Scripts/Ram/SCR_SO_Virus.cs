using UnityEngine;
[CreateAssetMenu(fileName = "VirusInjection", menuName = "Scriptable Objects/VirusInjection")]

public class SCR_SO_Virus : SCR_SO_Ram
{
    public override void OnEquipRam() {
        var playerController = SCR_GameController.Instance.CurrentPlayer.GetComponent<SCR_FirstPersonController>();
        if (playerController)
        {
            playerController.virusEquipped = true;
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
            playerController.virusEquipped = false;
        }
        else
        {
            Debug.LogWarning("Couldn't find playerController on equip. RAM had no effects.");
        }
    }
}
