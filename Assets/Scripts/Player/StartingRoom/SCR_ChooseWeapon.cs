using Unity.VisualScripting;
using UnityEngine;

public class SCR_ChooseWeapon : MonoBehaviour
{
    public int selectedWeapon;
    public bool canSelect;
    private SCR_PlayerInputHandler playerInputHandler;
    public GameObject weaponSelection;

    void Awake()
    {
        weaponSelection = gameObject.transform.parent.gameObject;

    }

    void Start()
    {

        if (playerInputHandler == null)
        {
            Debug.Log("No current player");
        }
    }

    void OnTriggerEnter(Collider player)
    {
        canSelect = true;
        Debug.Log("Collided");
        //player.gameObject.CompareTag("Player") && 
        
    }

    void OnTriggerExit(Collider player)
    {
        canSelect = false;
    }

    void ChoseWeapon(int weaponNumber)
    {
        playerInputHandler = SCR_GameController.Instance.CurrentPlayer.GetComponent<SCR_PlayerInputHandler>();
        if (weaponNumber == 1)
        {
            playerInputHandler.Weapon1.SetActive(true);
            playerInputHandler.Weapon2.SetActive(false);
            playerInputHandler.Weapon3.SetActive(false);
            SCR_GameController.Instance.weaponDataStorage.weaponSelected = true;
            SCR_GameController.Instance.weaponDataStorage.pistolSelected = true;
        }
        else if (weaponNumber == 2)
        {
            playerInputHandler.Weapon1.SetActive(false);
            playerInputHandler.Weapon2.SetActive(true);
            playerInputHandler.Weapon3.SetActive(false);
            SCR_GameController.Instance.weaponDataStorage.weaponSelected = true;
            SCR_GameController.Instance.weaponDataStorage.shotgunSelected = true;
        }
        else if (weaponNumber == 3)
        {
            playerInputHandler.Weapon1.SetActive(false);
            playerInputHandler.Weapon2.SetActive(false);
            playerInputHandler.Weapon3.SetActive(true);
            SCR_GameController.Instance.weaponDataStorage.weaponSelected = true;
            SCR_GameController.Instance.weaponDataStorage.rifleSelected = true;
        }

        SCR_GameController.Instance.PlayerChoseWeapon();
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canSelect && SCR_GameController.Instance.CurrentPlayer.GetComponent<SCR_FirstPersonController>().isPaused == false)
        {
            Debug.Log("Supposed to select");
            ChoseWeapon(selectedWeapon);
            weaponSelection.SetActive(false);
        }

    }
}
