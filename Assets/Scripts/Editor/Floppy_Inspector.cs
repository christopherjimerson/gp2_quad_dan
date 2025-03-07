using UnityEditor;
using UnityEngine;

public class Floppy_Inspector : EditorWindow
{
    public string floppyName = "Floppy Name";
    public int Id, Cost;
    public FloppyType floppyType;
    public string Description = "";

    [MenuItem("Tools/Upgrades/Create Floppy")]
    public static void ShowWindow()
    {
        GetWindow(typeof(Floppy_Inspector));
    }

    private void OnGUI()
    {
        GUILayout.Label("Create new Enemy", EditorStyles.boldLabel);

        floppyName = EditorGUILayout.TextField("Enemy name", floppyName);
        Id = EditorGUILayout.IntField("Id", Id);
        Cost = EditorGUILayout.IntField("Cost", Cost);
        floppyType = (FloppyType)EditorGUILayout.EnumPopup("Floppy type", floppyType);

        EditorStyles.textField.wordWrap = true;
        Description = EditorGUILayout.TextArea(Description);

        if (GUILayout.Button("Create Floppy"))
        {
            CreateFloppy();
        }
    }

    void CreateFloppy()
    {
        SCR_SO_Ram floppy = CreateInstance<SCR_SO_Ram>();
        floppy.Id = Id;
        floppy.floppyType = floppyType;
        floppy.Description = Description;
        floppy.cost = Cost;
        AssetDatabase.CreateAsset(floppy, $"Assets/RamSO/{floppyName}.asset");
        AssetDatabase.SaveAssets();
        

        GameObject obj = (GameObject)AssetDatabase.LoadMainAssetAtPath("Assets/Prefabs/Controller/Game_Controller.prefab");

        obj.GetComponent<SCR_GameController>().AllRam.Add(floppy);

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = obj;
    }
}
