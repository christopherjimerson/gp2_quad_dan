using UnityEngine;

public enum FloppyType {UI, Movement, PowerUps}

[CreateAssetMenu(fileName = "Ram", menuName = "Create Ram")]
public class SCR_SO_Ram : ScriptableObject
{
    public int Id, cost;
    public FloppyType floppyType;
    public string displayName;
    public Sprite Icon;

    [TextArea(5, 20)]
    public string Description = "";

    public virtual void OnEquipRam()
    {
        
    }

    public virtual void OnUnEquipRam()
    {
     
    }
}
