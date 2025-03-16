using System.Collections;
using UnityEngine;

public class SCR_RamMenuInteract : MonoBehaviour
{
    private SCR_GameController gameController;
    private SCR_HeadsUpDisplay headsUpDisplay;
    private SCR_PlayerInputHandler playerInputHandler;
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
        StartCoroutine(WaitForPlayer());
        gameController = SCR_GameController.Instance.GetComponent<SCR_GameController>();
        headsUpDisplay = SCR_HeadsUpDisplay.Instance;
    }

    private void OnDestroy() {
        playerInputHandler.OnInteract -= TerminalInteract;

    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.E) && canTrigger && SCR_GameController.Instance.CurrentPlayer.GetComponent<SCR_FirstPersonController>().isPaused == false)
        {
            Debug.Log("Interacted");
            gameController.ToggleUpgradeUI(true);
            SCR_GameController.Instance.PlayerHasRam();
        }*/
    }

    void TerminalInteract() {
        if(canTrigger && SCR_GameController.Instance.CurrentPlayer.GetComponent<SCR_FirstPersonController>().isPaused == false) {
            gameController.ToggleUpgradeUI(true);
            SCR_GameController.Instance.PlayerHasRam();
        }
    }

    private IEnumerator WaitForPlayer() {
        while (SCR_GameController.Instance.CurrentPlayer == null) {
            yield return null; // Wait for the next frame
        }
        playerInputHandler = SCR_GameController.Instance.CurrentPlayer.GetComponent<SCR_PlayerInputHandler>();
        if (playerInputHandler != null) {
            playerInputHandler.OnInteract += TerminalInteract;
        } else {
            Debug.LogError("SCR_PlayerInputHandler not found on CurrentPlayer.");
        }
        headsUpDisplay = SCR_HeadsUpDisplay.Instance;
    }
}
