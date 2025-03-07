using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SCR_ModRamUI : MonoBehaviour {
    //public static SCR_GameController GameController;  
    public Canvas modRamUI;

    public GameObject buttonPrefab;
    public GameObject buttonParentEquipped;
    public GameObject buttonParentUnEquipped;
    public TextMeshProUGUI ramCapacityText;
    public Image ramCapacityBackground;
    public SCR_InsanityController insanity;
    public SCR_GameController gameController;
    public SCR_Tooltip tooltipUI;
    public float maxRAM;
    public float usedRAM;
    public float memoryCapacity;
    public Color memoryDefaultColor;
    public Color memoryOverColor;

    //UI ram colors
    private Color UIRamColor = new Color(0, 253, 255);
    private Color MovementRamColor = new Color(255, 223, 0);
    private Color PowerUpsRamColor = new Color(255, 0, 153);
     
    private Dictionary<SCR_SO_Ram, GameObject> modButtons = new Dictionary<SCR_SO_Ram, GameObject>();

    void Start() {
        //ModRamUI.SetActive(false);
        //StartingRam();
        insanity = SCR_GameController.Instance.InsanityController;
        gameController = SCR_GameController.Instance.GetComponent<SCR_GameController>();
        maxRAM = insanity.MaxRam;
        memoryDefaultColor = ramCapacityBackground.color;
        ramCapacityBackground.transform.GetChild(3).GetComponent<TMP_Text>().text = "* No Effects";
        memoryCapacity = ((usedRAM / maxRAM) * 100f);
        ramCapacityText.text = (memoryCapacity.ToString() + "%");
    }

    void Update() {

    }

    private void OnEnable()
    {
        DisplayEquippedMods();
        AudioManager.Instance.Play("CRT_Static");
       
    }

    void StartingRam() {
        for (int i = 0; i < SCR_GameController.Instance.AllRamNotEquipped.Count; i++)
        {
            SCR_GameController.Instance.BackPackRam.Add(SCR_GameController.Instance.AllRamNotEquipped[i]);
        }
    }

    void DisplayEquippedMods() {
        foreach (var mod in SCR_GameController.Instance.CurrentEquippedRam)
        {
            if (!modButtons.ContainsKey(mod)) // Create button only if it doesn't exist
            {
                GameObject newButton = Instantiate(buttonPrefab);
                newButton.transform.SetParent(buttonParentEquipped.transform, false);
                newButton.transform.GetChild(0).GetComponent<TMP_Text>().text = mod.displayName;
                newButton.transform.GetChild(1).GetComponent<TMP_Text>().text = mod.cost.ToString();
                usedRAM += mod.cost;

                //newButton.GetComponentInChildren<TMP_Text>().text = mod.displayName;
                //newButton.GetComponentInChildren<TMP_Text>().text = mod.cost.ToString();

                //Change Color of ram
                ChangeColorOfRam(mod, newButton.GetComponent<Button>());

                newButton.GetComponent<Button>().onClick.AddListener(() => ToggleMod(mod));
                AddTooltip(newButton, mod);
                modButtons[mod] = newButton;
            }
            else
            {
                // Move existing button to Equipped parent
                modButtons[mod].transform.SetParent(buttonParentEquipped.transform, false);
                modButtons[mod].SetActive(true);
            }
        }

        foreach (var mod in SCR_GameController.Instance.BackPackRam)
        {
            if (!modButtons.ContainsKey(mod))
            {
                GameObject newButton = Instantiate(buttonPrefab);
                newButton.transform.SetParent(buttonParentUnEquipped.transform, false);

                newButton.transform.GetChild(0).GetComponent<TMP_Text>().text = mod.displayName;
                newButton.transform.GetChild(1).GetComponent<TMP_Text>().text = mod.cost.ToString();

                //Change Color of ram
                ChangeColorOfRam(mod, newButton.GetComponent<Button>());

                newButton.GetComponent<Button>().onClick.AddListener(() => ToggleMod(mod));
                AddTooltip(newButton, mod);
                modButtons[mod] = newButton;
            }
            else
            {
                // Move existing button to Backpack parent
                modButtons[mod].transform.SetParent(buttonParentUnEquipped.transform, false);
                modButtons[mod].SetActive(true);
            }
        }

    }

    void ToggleMod(SCR_SO_Ram mod) {
        AudioManager.Instance.PlayOne("Select_Mod");
        if (SCR_GameController.Instance.CurrentEquippedRam.Contains(mod))
        {
            SCR_GameController.Instance.CurrentEquippedRam.Remove(mod);
            SCR_GameController.Instance.BackPackRam.Add(mod);
            usedRAM -= mod.cost;
            memoryCapacity = ((usedRAM / maxRAM) * 100f);
            ramCapacityText.text = (memoryCapacity.ToString() + "%");
            ChangeMemoryColor();
            modButtons[mod].transform.SetParent(buttonParentUnEquipped.transform, false);
        }
        else
        {
            SCR_GameController.Instance.BackPackRam.Remove(mod);
            SCR_GameController.Instance.CurrentEquippedRam.Add(mod);
            usedRAM += mod.cost;
            memoryCapacity = ((usedRAM / maxRAM) * 100f);
            ramCapacityText.text = (memoryCapacity.ToString() + "%");
            ChangeMemoryColor();
            modButtons[mod].transform.SetParent(buttonParentEquipped.transform, false);


            float insanity = SCR_GameController.Instance.InsanityController.Insanity;

        }
    }

    void ChangeMemoryColor() {
        if (memoryCapacity > 100.0 + insanity.firstThresholdPercentage)
        {
            ramCapacityBackground.transform.GetChild(0).GetComponent<TMP_Text>().text = "Overload: Level 1";
            ramCapacityBackground.transform.GetChild(2).GetComponent<TMP_Text>().color = memoryOverColor;
            ramCapacityBackground.transform.GetChild(3).GetComponent<TMP_Text>().color = memoryOverColor;
            ramCapacityBackground.transform.GetChild(3).GetComponent<TMP_Text>().text = "* More Enemeies\n* Corrupted Visuals";
            ramCapacityBackground.color = memoryOverColor;
        }
        else if (memoryCapacity > 100.0 + insanity.secondThresholdPercentage)
        {
            ramCapacityBackground.transform.GetChild(0).GetComponent<TMP_Text>().text = "Overload: Level 2";
            ramCapacityBackground.transform.GetChild(2).GetComponent<TMP_Text>().color = memoryOverColor;
            ramCapacityBackground.transform.GetChild(3).GetComponent<TMP_Text>().color = memoryOverColor;
            ramCapacityBackground.transform.GetChild(3).GetComponent<TMP_Text>().text = "* More Enemeies\n* Corrupted Visuals\n* Move Malfunction";
            ramCapacityBackground.color = memoryOverColor;
        }
        else if (memoryCapacity > 100.0 + insanity.thirdThresholdPercentage)
        {
            ramCapacityBackground.transform.GetChild(0).GetComponent<TMP_Text>().text = "Overload: Level 3";
            ramCapacityBackground.transform.GetChild(2).GetComponent<TMP_Text>().color = memoryOverColor;
            ramCapacityBackground.transform.GetChild(3).GetComponent<TMP_Text>().color = memoryOverColor;
            ramCapacityBackground.transform.GetChild(3).GetComponent<TMP_Text>().text = "* More Enemeies\n* Corrupted Visuals\n* Move Malfunction\n* Sound Not Found";
            ramCapacityBackground.color = memoryOverColor;
        }
        else if (memoryCapacity > 100.0 + insanity.fourthThresholdPercentage)
        {
            ramCapacityBackground.transform.GetChild(0).GetComponent<TMP_Text>().text = "Max Overload";
            ramCapacityBackground.transform.GetChild(2).GetComponent<TMP_Text>().color = memoryOverColor;
            ramCapacityBackground.transform.GetChild(3).GetComponent<TMP_Text>().color = memoryOverColor;
            ramCapacityBackground.transform.GetChild(3).GetComponent<TMP_Text>().text = "* More Enemeies\n* Corrupted Visuals\n* Move Malfunction\n* Sound Not Found\n* Firewall Stronger";
            ramCapacityBackground.color = memoryOverColor;
        }
        else
        {
            ramCapacityBackground.transform.GetChild(0).GetComponent<TMP_Text>().text = "Memory Capacity";
            ramCapacityBackground.transform.GetChild(2).GetComponent<TMP_Text>().color = memoryDefaultColor;
            ramCapacityBackground.transform.GetChild(3).GetComponent<TMP_Text>().color = memoryDefaultColor;
            ramCapacityBackground.transform.GetChild(3).GetComponent<TMP_Text>().text = "* No Effects";
            ramCapacityBackground.color = memoryDefaultColor;
        }
    }

    void AddTooltip(GameObject button, SCR_SO_Ram mod) {
        EventTrigger eventTrigger = button.AddComponent<EventTrigger>();

        EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
        pointerEnter.eventID = EventTriggerType.PointerEnter;
        pointerEnter.callback.AddListener((eventData) => ShowTooltip(mod, button.transform));

        EventTrigger.Entry pointerExit = new EventTrigger.Entry();
        pointerExit.eventID = EventTriggerType.PointerExit;
        pointerExit.callback.AddListener((eventData) => HideTooltip());

        eventTrigger.triggers.Add(pointerEnter);
        eventTrigger.triggers.Add(pointerExit);
    }

    void ShowTooltip(SCR_SO_Ram mod, Transform buttonTransform) {
        //tooltipUI.ShowTooltip($"{mod.displayName}\nCost: {mod.cost}\n{mod.Description}", buttonTransform);
        tooltipUI.ShowTooltip($"{mod.Description}", buttonTransform);

    }

    void HideTooltip() {
        tooltipUI.HideTooltip();
    }

    private void SelectRam(SCR_SO_Ram Mod) {
        Debug.Log("Selected mod: " + Mod.displayName);
        SCR_GameController.Instance.EquipRam(Mod);
        DisplayEquippedMods();
    }

    public void ConfirmModSelection()
    {
        AudioManager.Instance.PlayOne("Start_Button");
        Debug.Log("Confirmed");
        //insanityController.OnValidate();
        AudioManager.Instance.Stop("CRT_Static");
       

        Debug.Log(Cursor.lockState);
        Debug.Log(Cursor.visible);

        Debug.Log("Curesur should yeet");

        SCR_GameController.Instance.AddAllEquippedRamToPlayer();
        gameController.ToggleUpgradeUI(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    
    void ChangeColorOfRam(SCR_SO_Ram currentRam, Button currentButton)
    {
        if (currentRam.floppyType == FloppyType.UI)
        {
            currentButton.GetComponent<Button>().image.color = UIRamColor;
        }
        else if (currentRam.floppyType == FloppyType.Movement)
        {
            currentButton.GetComponent<Button>().image.color = MovementRamColor;
        }

        else if (currentRam.floppyType == FloppyType.PowerUps)
        {
            currentButton.GetComponent<Button>().image.color = PowerUpsRamColor;
        }
    }
    
}

