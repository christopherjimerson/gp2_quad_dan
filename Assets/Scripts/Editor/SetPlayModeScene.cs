using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


[InitializeOnLoad]
public class SetPlayModeScene : MonoBehaviour
{
    private const string StartFromRoot = "Tools/Load From Root";
    private const string SetStartScene = "Assets/Set Start Scene";
    private const string SelectedScene = "SelectedPlayModeScene";
    private static string _selectedPath;
    public static SceneAsset CompositionRoot;
    private static SceneAsset Root => CompositionRoot ?? AssetDatabase.LoadAssetAtPath<SceneAsset>(_selectedPath);

    static SetPlayModeScene()
    {
        var isOn = EditorPrefs.GetBool(StartFromRoot, false);
        _selectedPath = EditorPrefs.GetString(SelectedScene);
        EditorSceneManager.playModeStartScene = isOn ? Root : null;
    }

    [MenuItem(StartFromRoot)]
    private static void SetStartFromRoot()
    {
        var isOn = EditorPrefs.GetBool(StartFromRoot, false);
        Menu.SetChecked(StartFromRoot, isOn);
        EditorSceneManager.playModeStartScene = isOn ? null : Root;
        EditorPrefs.SetBool(StartFromRoot, !isOn);
    }

    [MenuItem(StartFromRoot, true)]
    private static bool SetStartFromRootValidate()
    {
        var isOn = EditorPrefs.GetBool(StartFromRoot, false);
        Menu.SetChecked(StartFromRoot, isOn);
        return !Application.isPlaying;
    }


    [MenuItem(SetStartScene)]
    private static void SetScene()
    {
        var newPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (_selectedPath == newPath)
        {
            _selectedPath = string.Empty;
            EditorSceneManager.playModeStartScene = null;
            Menu.SetChecked(SetStartScene, false);
        }
        else
        {
            _selectedPath = newPath;
            EditorSceneManager.playModeStartScene = Selection.activeObject as SceneAsset;
            Menu.SetChecked(SetStartScene, true);
        }
        EditorPrefs.SetString(SelectedScene, _selectedPath);
    }


    [MenuItem(SetStartScene, true)]
    private static bool SetSceneValidator()
    {
        var isSelected = _selectedPath == AssetDatabase.GetAssetPath(Selection.activeObject);
        Menu.SetChecked(SetStartScene, isSelected);
        return Selection.activeObject is SceneAsset;
    }
}