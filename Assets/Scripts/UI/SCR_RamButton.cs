using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SCR_RamButton : MonoBehaviour 
{
    public SCR_SO_Ram ram;
    public TextMeshProUGUI buttonTitle;
    public SCR_GameController gameController;
    public GameObject partnerButton;
    public bool startEquipped;
    public bool isClicked;

    private void Start()
    {
        gameController = SCR_GameController.Instance;
        buttonTitle.text = ram.displayName;

        if (startEquipped)
        {
            this.gameObject.SetActive(true);
            gameController.EquipRam(ram);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    public void Unequip()
    {
        for (int i = 0; i < gameController.CurrentEquippedRam.Count; i++)
        {
            if (gameController.CurrentEquippedRam[i].Id == ram.Id)
            {
                gameController.UnEquipRam(ram);
                gameController.BackPackRam.Add(ram);
                partnerButton.gameObject.SetActive(true);
                this.gameObject.SetActive(false);
            }
        }

    }

    public void Equip()
    {
        for (int i = 0; i < gameController.BackPackRam.Count; i++)
        {
            if (gameController.BackPackRam[i].Id == ram.Id)
            {
                gameController.BackPackRam.Remove(ram);
                gameController.EquipRam(ram);
                partnerButton.gameObject.SetActive(true);
                this.gameObject.SetActive(false);
            }
        }
    }
}