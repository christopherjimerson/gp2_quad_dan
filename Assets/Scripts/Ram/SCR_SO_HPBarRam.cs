using UnityEngine;
[CreateAssetMenu(fileName = "SCR_SO_HPBarRam", menuName = "Scriptable Objects/SCR_SO_HPBarRam")]

public class SCR_SO_HPBarRam : SCR_SO_Ram
{
    public override void OnEquipRam()
    {
        if (!SCR_HeadsUpDisplay.Instance.health.gameObject) { return; }
        SCR_HeadsUpDisplay.Instance.health.gameObject.SetActive(true);

    }

    public override void OnUnEquipRam() {
        if (!SCR_HeadsUpDisplay.Instance.health.gameObject) { return; }
        SCR_HeadsUpDisplay.Instance.health.gameObject.SetActive(false);
    }
}
