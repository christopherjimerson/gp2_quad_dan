using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SCR_ChooseWeapon : MonoBehaviour {
    public int selectedWeapon;
    public bool canSelect;
    private SCR_PlayerInputHandler playerInputHandler;
    private SCR_HeadsUpDisplay headsUpDisplay;
    public GameObject weaponSelection;

    void Awake() {
        weaponSelection = gameObject.transform.parent.gameObject;

    }

    void Start() {
        StartCoroutine(WaitForPlayer());
    }

    private void OnDestroy() {
        playerInputHandler.OnInteract -= TrySelectedWeapon;
    }

    void OnTriggerEnter(Collider player) {
        canSelect = true;
        headsUpDisplay.interactionText.gameObject.SetActive(true);
        Debug.Log("Collided");
    }

    void OnTriggerExit(Collider player) {
        headsUpDisplay.interactionText.gameObject.SetActive(false);
        canSelect = false;
    }

    void ChoseWeapon(int weaponNumber) {

        if (weaponNumber == 1) {
            playerInputHandler.Weapon1.SetActive(true);
            playerInputHandler.Weapon2.SetActive(false);
            playerInputHandler.Weapon3.SetActive(false);
            SCR_GameController.Instance.weaponDataStorage.weaponSelected = true;
            SCR_GameController.Instance.weaponDataStorage.pistolSelected = true;
        } else if (weaponNumber == 2) {
            playerInputHandler.Weapon1.SetActive(false);
            playerInputHandler.Weapon2.SetActive(true);
            playerInputHandler.Weapon3.SetActive(false);
            SCR_GameController.Instance.weaponDataStorage.weaponSelected = true;
            SCR_GameController.Instance.weaponDataStorage.shotgunSelected = true;
        } else if (weaponNumber == 3) {
            playerInputHandler.Weapon1.SetActive(false);
            playerInputHandler.Weapon2.SetActive(false);
            playerInputHandler.Weapon3.SetActive(true);
            SCR_GameController.Instance.weaponDataStorage.weaponSelected = true;
            SCR_GameController.Instance.weaponDataStorage.rifleSelected = true;
        }

        SCR_GameController.Instance.PlayerChoseWeapon();
    }

    void TrySelectedWeapon() {
        if (canSelect && SCR_GameController.Instance.CurrentPlayer.GetComponent<SCR_FirstPersonController>().isPaused == false) {
            Debug.Log("Supposed to select");
            ChoseWeapon(selectedWeapon);
            weaponSelection.SetActive(false);
        }
    }

    void Update()
    {

    }

    private IEnumerator WaitForPlayer() {
        while (SCR_GameController.Instance.CurrentPlayer == null) {
            yield return null; // Wait for the next frame
        }
        playerInputHandler = SCR_GameController.Instance.CurrentPlayer.GetComponent<SCR_PlayerInputHandler>();
        if (playerInputHandler != null) {
            playerInputHandler.OnInteract += TrySelectedWeapon;
        } else {
            Debug.LogError("SCR_PlayerInputHandler not found on CurrentPlayer.");
        }
        headsUpDisplay = SCR_HeadsUpDisplay.Instance;
    }


}
