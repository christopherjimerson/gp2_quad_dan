using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class SCR_Populate_Ram : MonoBehaviour
{
    UIDocument uiInventory;
    public GameObject unequipButton;

    public VisualTreeAsset modButtonTemplate;

    private void OnEnable() 
    {
        uiInventory = GetComponent<UIDocument>();

        foreach (var mod in SCR_GameController.Instance.CurrentEquippedRam)
        {
            SCR_Equipped_Slot newSlot = new SCR_Equipped_Slot(mod, modButtonTemplate, uiInventory);

            uiInventory.rootVisualElement.Q("ModList").Add(newSlot.button);
        }

        
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        unequipButton.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
