using UnityEngine;

public enum BuffType {SPD, HP, DMG}
public enum Rarity {NORMAL, RARE, EPIC}

[CreateAssetMenu(fileName = "Buff", menuName = "Create Buff")]
public class SCR_SO_Buff : ScriptableObject
{
    public BuffType Buff;
    public Rarity Quality;

    [SerializeField]
    private string displayName = "Buff name";
    [SerializeField]
    private string descrition = "This is a buff";

    [SerializeField] public int valueMultiplier = 0, RamCost = 0;

    /*
    [Header("% Chance that the quality has cost")]
    public float chanceOfCostRare, chanceOfCostEpic;
  

    [Header("Quality multiplier")]
    public int NormalPercent, RarePercent, EpicPercent;


    [Header("% chance of buff being this quality")]
    [SerializeField]
    private float RareChancePercentage, EpicChancePercentage;
    */

    public int GetPercentValue()
    {
        return valueMultiplier;
    }

    public int GetRamCost()
    {
        return RamCost;
    }

    public string GetDescription()
    {
        return descrition;
    }

    public string GetName()
    {
        return displayName;
    }

    public BuffType GetBuffType()
    {
        return Buff;
    }

    public Rarity GetRarity()
    {
        return Quality;
    }

    public void DebugBuff()
    {
        Debug.Log($"name: {displayName}, Description: {descrition}\n" +
                  $"Buff: {Buff.ToString()}, Rarity:{Quality.ToString()}\n" +
                  $"RamCost: {RamCost.ToString()}, ValueToPlayer: {valueMultiplier.ToString()}");
    }


    /*
     * public void RandomizeQuality()
    {
        float n = Random.Range(0,100);

        if (n <= RareChancePercentage && n > EpicChancePercentage)
        {
            Debug.Log("Rare Buff" + n);
            Quality = Rarity.RARE;

            float range = Random.Range(0, 100);
            Debug.Log("BUFF RANGE: " + range);

            RamCost = range <= chanceOfCostRare ? Random.Range(1, 20) : 0;

            valueMultiplier = RarePercent;
            Debug.Log(valueMultiplier);

        }

        if (n <= EpicChancePercentage)
        {
            Debug.Log("Epic Buff" + n);
            Quality = Rarity.EPIC;

            float range = Random.Range(0, 100);
            Debug.Log("BUFF RANGE: " + range);

            RamCost = range <= chanceOfCostEpic ? Random.Range(1, 40) : 0;
            valueMultiplier = EpicPercent;
            Debug.Log(valueMultiplier);
        }

        if (n > RareChancePercentage) {
            Quality = Rarity.NORMAL;
            Debug.Log("Normal Buff" + n);
            RamCost = 0;
            valueMultiplier = NormalPercent;
            Debug.Log(valueMultiplier);

        }

        //if (!(n > RareChancePercentage)) return;

    }
    */


}
