using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class SCR_Equipped_Slot
{

    UIDocument uiInventory;


    public Button button;
    private SCR_SO_Ram mod;

    

    public void OnEnable() {
    }

    public SCR_Equipped_Slot(SCR_SO_Ram mod, VisualTreeAsset template, UIDocument parent) {
        
        TemplateContainer modButtonContainer = template.Instantiate();

        button = modButtonContainer.Q<Button>();
        this.mod = mod;
        

        button.text = mod.displayName + " " + mod.cost;
        
        button.RegisterCallback<ClickEvent>(OnClick);

        
    }

    public void OnClick(ClickEvent evt) {
        Debug.Log("Test");
        //unequipButton.SetActive(true);

        //unequipButton.GetComponent<Button>().RegisterCallback<ClickEvent>(OnClick);
    }

}
