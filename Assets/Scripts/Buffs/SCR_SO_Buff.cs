using UnityEngine;

public enum BuffType {SPD, HP, DMG}
public enum Rarity {NORMAL, RARE, EPIC}

[CreateAssetMenu(fileName = "Buff", menuName = "Create Buff")]
public class SCR_SO_Buff : ScriptableObject
{
    public BuffType Buff = BuffType.SPD;
    private Rarity Quality = Rarity.NORMAL;

    [SerializeField]
    private string displayName = "Buff name";
    [SerializeField]
    private string descrition = "This is a buff";

    private int valueMultiplier = 0, RamCost = 0;

    [Header("% Chance that the quality has cost")]
    public float chanceOfCostRare = 0.7f, chanceOfCostEpic = 0.7f;

    [Header("Quality multiplier")]
    public int NormalPercent = 5, RarePercent = 20, EpicPercent = 40;


    [Header("% chance of buff being this quality")]
    [SerializeField]
    private float RareChancePercentage = 15f, EpicChancePercentage = 5f;

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

    

    public void RandomizeQuality()
    {
        int n = Random.Range(0,100);

        if (n <= RareChancePercentage)
        {
            Quality = Rarity.RARE;

            float range = Random.Range(0, 100);
            Debug.Log("BUFF RANGE: " + range);

            RamCost = range <= chanceOfCostRare ? Random.Range(1, 20) : 0;

            valueMultiplier = RarePercent;
        }

        if (n <= EpicChancePercentage)
        {
            Quality = Rarity.EPIC;

            float range = Random.Range(0, 100);
            Debug.Log("BUFF RANGE: " + range);

            RamCost = range <= chanceOfCostEpic ? Random.Range(1, 40) : 0;
            valueMultiplier = EpicPercent;
        }

        if (!(n > RareChancePercentage)) return;
        Quality = Rarity.NORMAL;
        RamCost = 0;
        valueMultiplier = NormalPercent;

    }
}
