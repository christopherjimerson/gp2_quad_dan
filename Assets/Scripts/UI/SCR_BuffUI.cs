using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SCR_BuffUI : MonoBehaviour
{
    public Button[] buttons;
    public GameObject buffUI;

    public Image[] buttonImages;

    public Image[] stats;
    private int totalSpeedAdded;
    private int totalHealthAdded;
    private int totalDamageAdded;
    public GameObject NewRamUnlockedButton, RAMUnlockedGameObject;
    //This is dumb, this should be a for loop, but apparently it does not like it when values get changed between, i suppose i could do buff123 assigned before and forloop the rest, but nah, this works and i dont want to think about it //Daniel
    private void OnEnable()
    {
        stats[0].gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = "+" + totalSpeedAdded.ToString() + "%";
        stats[1].gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = "+" + totalHealthAdded.ToString() + "%";
        stats[2].gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = "+" + totalDamageAdded.ToString() + "%";

        var buff = SCR_GameController.Instance.GetRandomBuff();
        buttons[0].gameObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = buff.GetName();
        buttons[0].gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text = buff.GetDescription();
        buttons[0].gameObject.transform.GetChild(3).GetComponent<TMP_Text>().text = $"+{buff.GetPercentValue().ToString()}%";

        buttonImages[0].color = buff.GetRarity() switch
        {
            Rarity.NORMAL => new Color32(109, 215, 254,255),
            Rarity.RARE => new Color32(255, 137, 239,255),
            Rarity.EPIC => new Color32(254, 237, 109, 255),
            _ => buttons[0].GetComponent<Image>().color
        };

        if (buff.GetRamCost() == 0)
            buttons[0].gameObject.transform.GetChild(1).gameObject.SetActive(false);
        else
            buttons[0].gameObject.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = buff.GetRamCost().ToString();

        buttons[0].onClick.AddListener(() => buff.DebugBuff());
        buttons[0].onClick.AddListener(() => SCR_GameController.Instance.AddBuff(buff));
        buttons[0].onClick.AddListener(() => AudioManager.Instance.PlayOne("Select_Mod"));
        buttons[0].onClick.AddListener(() => AddToTotal(buff, buff.GetPercentValue()));
        buttons[0].onClick.AddListener(() => SCR_GameController.Instance.ProgressLevel());
        buttons[0].onClick.AddListener(() => buffUI.SetActive(false));

        //---------------------------------------------------------------------------------------------------------------------------

        var buff1 = SCR_GameController.Instance.GetRandomBuff();
        buttons[1].gameObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = buff1.GetName();
        buttons[1].gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text = buff1.GetDescription();
        buttons[1].gameObject.transform.GetChild(3).GetComponent<TMP_Text>().text = $"+{buff1.GetPercentValue().ToString()}%";

        if (buff1.GetRamCost() == 0)
            buttons[1].gameObject.transform.GetChild(1).gameObject.SetActive(false);
        else
            buttons[1].gameObject.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = buff1.GetRamCost().ToString();

        buttonImages[1].color = buff1.GetRarity() switch
        {
            Rarity.NORMAL => new Color32(109, 215, 254, 255),
            Rarity.RARE => new Color32(255, 137, 239, 255),
            Rarity.EPIC => new Color32(254, 237, 109, 255),
            _ => buttons[0].GetComponent<Image>().color
        };

        buttons[1].onClick.AddListener(() => buff1.DebugBuff());
        buttons[1].onClick.AddListener(() => AudioManager.Instance.PlayOne("Select_Mod"));
        buttons[1].onClick.AddListener(() => SCR_GameController.Instance.AddBuff(buff1));
        buttons[1].onClick.AddListener(() => AddToTotal(buff1, buff1.GetPercentValue()));
        buttons[1].onClick.AddListener(() => SCR_GameController.Instance.ProgressLevel());
        buttons[1].onClick.AddListener(() => buffUI.SetActive(false));

        //---------------------------------------------------------------------------------------------------------------------------

        var buff2 = SCR_GameController.Instance.GetRandomBuff();
        buttons[2].gameObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = buff2.GetName();
        buttons[2].gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text = buff2.GetDescription();
        buttons[2].gameObject.transform.GetChild(3).GetComponent<TMP_Text>().text = $"+{buff2.GetPercentValue().ToString()}%";


        if (buff2.GetRamCost() == 0)
            buttons[2].gameObject.transform.GetChild(1).gameObject.SetActive(false);
        else
            buttons[2].gameObject.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = buff2.GetRamCost().ToString();

        buttonImages[2].color = buff2.GetRarity() switch
        {
            Rarity.NORMAL => new Color32(109, 215, 254,255),
            Rarity.RARE => new Color32(255, 137, 239,255),
            Rarity.EPIC => new Color32(254, 237, 109, 255),
            _ => buttons[0].GetComponent<Image>().color
        };

        buttons[2].onClick.AddListener(() => buff2.DebugBuff()); 
        buttons[2].onClick.AddListener(() => SCR_GameController.Instance.AddBuff(buff2));
        buttons[2].onClick.AddListener(() => AudioManager.Instance.PlayOne("Select_Mod"));
        buttons[2].onClick.AddListener(() => AddToTotal(buff2, buff2.GetPercentValue()));
        buttons[2].onClick.AddListener(() => SCR_GameController.Instance.ProgressLevel());
        buttons[2].onClick.AddListener(() => buffUI.SetActive(false));

        //---------------------------------------------------------------------------------------------------------------------------

        if (SCR_GameController.Instance.latestRam == null)
        {
            RAMUnlockedGameObject.SetActive(false);
            return;
        }
        RAMUnlockedGameObject.SetActive(true);
        NewRamUnlockedButton.transform.GetChild(0).GetComponent<TMP_Text>().text = SCR_GameController.Instance.latestRam.displayName;
        NewRamUnlockedButton.transform.GetChild(1).GetComponent<TMP_Text>().text = SCR_GameController.Instance.latestRam.cost.ToString();
        SCR_GameController.Instance.latestRam = null;

    }

    private void OnDisable()
    {
        buttons[0].gameObject.transform.GetChild(3).gameObject.SetActive(true);
        buttons[1].gameObject.transform.GetChild(3).gameObject.SetActive(true);
        buttons[2].gameObject.transform.GetChild(3).gameObject.SetActive(true);

        buttons[0].onClick.RemoveAllListeners();
        buttons[1].onClick.RemoveAllListeners();
        buttons[2].onClick.RemoveAllListeners();
    }

    private void AddToTotal(SCR_SO_Buff buff, int value) {
        if (buff.GetBuffType() == BuffType.SPD) 
        {
            totalSpeedAdded += value;
        }
        if (buff.GetBuffType() == BuffType.HP) 
        {
            totalHealthAdded += value;
        }
        if (buff.GetBuffType() == BuffType.DMG) 
        {
            totalDamageAdded += value;
        }
    }
}
