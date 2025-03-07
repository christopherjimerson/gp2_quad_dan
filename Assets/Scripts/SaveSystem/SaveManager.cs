using System;
using System.Linq;
using UnityEngine;

public static class SaveManager
{
    public static void SaveGame()
    {
        PlayerPrefs.DeleteAll();

        var idToSave = "";

        if (SCR_GameController.Instance.CurrentEquippedRam.Any())
        {
            idToSave = SCR_GameController.Instance.CurrentEquippedRam.Aggregate("", (current, scrSoRam) => current + (scrSoRam.Id + ","));
            idToSave = idToSave[..^1];
            PlayerPrefs.SetString("Active", idToSave);
            Debug.Log("Saved active ram: " + idToSave);
        }

        if (SCR_GameController.Instance.BackPackRam.Any())
        {
            idToSave = SCR_GameController.Instance.BackPackRam.Aggregate("", (current, scrSoRam) => current + (scrSoRam.Id + ","));
            idToSave = idToSave[..^1];
            PlayerPrefs.SetString("BackPack", idToSave);
            Debug.Log("Saved backpack ram: " + idToSave);
        }

        PlayerPrefs.SetInt("Floor", SCR_GameController.Instance.GetCurrentFloor());
    }

    public static int[] GetEquippedRam()
    {
        return PlayerPrefs.HasKey("Active") ? PlayerPrefs.GetString("Active").Split(',').Select(n => Convert.ToInt32(n)).ToArray() : null;
    }

    public static int[] GetBackPackRam()
    {
        return PlayerPrefs.HasKey("BackPack") ? PlayerPrefs.GetString("BackPack").Split(',').Select(n => Convert.ToInt32(n)).ToArray() : null;
    }

    public static int GetFloorNumber()
    {
        return PlayerPrefs.HasKey("Floor") ? PlayerPrefs.GetInt("Floor")-1 : 0;
    }
}
