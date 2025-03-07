using UnityEngine;

public class SCR_RamMenuInteract : MonoBehaviour
{
    private SCR_GameController gameController;
    public bool canTrigger;

    void OnTriggerEnter(Collider player)
    {
        canTrigger = true;
        Debug.Log("Collided");
        //player.gameObject.CompareTag("Player") && 

    }

    void OnTriggerExit(Collider player)
    {
        canTrigger = false;
    }

    void Start()
    {
        gameController = SCR_GameController.Instance.GetComponent<SCR_GameController>();
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
