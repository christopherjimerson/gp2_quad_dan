using UnityEngine;
[CreateAssetMenu(fileName = "SCR_SO_MiniMapRam", menuName = "Scriptable Objects/SCR_SO_MiniMapRam")]

public class SCR_SO_MiniMapRam : SCR_SO_Ram {
    public override void OnEquipRam() {
        if (!SCR_HeadsUpDisplay.Instance.miniMap.gameObject) { return; }
        SCR_HeadsUpDisplay.Instance.radarMap.gameObject.SetActive(true);

    }

    public override void OnUnEquipRam() {
        if (!SCR_HeadsUpDisplay.Instance.miniMap.gameObject) { return; }
        SCR_HeadsUpDisplay.Instance.radarMap.gameObject.SetActive(false);
    }
}
