using UnityEngine;
using UnityEngine.UI;

public class SCR_ScreenChanging : MonoBehaviour
{

    public SCR_EnemyRoomSpawner thisRoomController;
    bool inRoom = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SCR_HeadsUpDisplay.Instance.miniMap.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        SCR_HeadsUpDisplay.Instance.miniMap.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        SCR_HeadsUpDisplay.Instance.miniMap.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void RoomClear(GameObject vs) {

        EventManager.Instance.OnRoomCompleted -= RoomClear;
        SCR_HeadsUpDisplay.Instance.miniMap.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        SCR_HeadsUpDisplay.Instance.miniMap.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        SCR_HeadsUpDisplay.Instance.miniMap.gameObject.transform.GetChild(2).gameObject.SetActive(false);
    }

//This should run when you step on the trigger box attatched to the game object
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {

        if(thisRoomController.state == SCR_EnemyRoomSpawner.RoomState.Clear)
        {
            //Do room is cleared code in here

            SCR_HeadsUpDisplay.Instance.miniMap.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            SCR_HeadsUpDisplay.Instance.miniMap.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            SCR_HeadsUpDisplay.Instance.miniMap.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        }
        else{
            if(!inRoom){
                EventManager.Instance.OnRoomCompleted += RoomClear;
                inRoom = true;
            }
            //Do active code here
            SCR_HeadsUpDisplay.Instance.miniMap.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            SCR_HeadsUpDisplay.Instance.miniMap.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            SCR_HeadsUpDisplay.Instance.miniMap.gameObject.transform.GetChild(2).gameObject.SetActive(true);
        }
    }
    }
}
