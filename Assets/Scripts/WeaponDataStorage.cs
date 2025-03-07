using Unity.VisualScripting;
using UnityEngine;

public class WeaponDataStorage : MonoBehaviour
{
    //public GameObject pistol;
    //public GameObject shotgun;
    //public GameObject rifle;
    public SCR_PlayerInputHandler playerInputHandler;
    public bool weaponSelected;
    public bool pistolSelected = false;
    public bool shotgunSelected = false;
    public bool rifleSelected = false;

    public void EquipStorageWeapon()
    {
        playerInputHandler = SCR_GameController.Instance.CurrentPlayer.GetComponent<SCR_PlayerInputHandler>();
        if(weaponSelected){
            if (pistolSelected)
            {
                playerInputHandler.Weapon1.SetActive(true);
                playerInputHandler.Weapon2.SetActive(false);
                playerInputHandler.Weapon3.SetActive(false);
            }
            else if (shotgunSelected)

            {
                playerInputHandler.Weapon1.SetActive(false);
                playerInputHandler.Weapon2.SetActive(true);
                playerInputHandler.Weapon3.SetActive(false);
            }
            else if (rifleSelected)

            {
                playerInputHandler.Weapon1.SetActive(false);
                playerInputHandler.Weapon2.SetActive(false);
                playerInputHandler.Weapon3.SetActive(true);
            }

        }
        
    }
}
