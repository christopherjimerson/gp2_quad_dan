using UnityEngine;

public class SCR_RamMenuInteract : MonoBehaviour
{
    private SCR_GameController gameController;
    private SCR_HeadsUpDisplay headsUpDisplay;
    public bool canTrigger;

    void OnTriggerEnter(Collider player)
    {
        canTrigger = true;
        headsUpDisplay.interactionText.gameObject.SetActive(true);
        Debug.Log("Collided");
        //player.gameObject.CompareTag("Player") && 

    }

    void OnTriggerExit(Collider player)
    {
        headsUpDisplay.interactionText.gameObject.SetActive(false);
        canTrigger = false;
    }

    void Start()
    {
        gameController = SCR_GameController.Instance.GetComponent<SCR_GameController>();
        headsUpDisplay = SCR_HeadsUpDisplay.Instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canTrigger && SCR_GameController.Instance.CurrentPlayer.GetComponent<SCR_FirstPersonController>().isPaused == false)
        {
            Debug.Log("Interacted");
            gameController.ToggleUpgradeUI(true);
            SCR_GameController.Instance.PlayerHasRam();
        }
    }
}
