using System;
using UnityEngine;
using UnityEngine.UI;

public class SCR_SpawnTrigger : MonoBehaviour {
    public SCR_EnemyRoomSpawner spawner;

    public event Action onPlayerTouched;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && spawner.state == SCR_EnemyRoomSpawner.RoomState.InActive) 
        {
            onPlayerTouched?.Invoke();
            SCR_HeadsUpDisplay.Instance.radarMap.SetActive(true);
            SCR_HeadsUpDisplay.Instance.miniMap.GetComponent<Image>().color = new Color32(63, 29, 0,255);
            gameObject.SetActive(false); 
        }
    }
}