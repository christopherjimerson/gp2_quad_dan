using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SCR_MainMenuController : MonoBehaviour
{
    public Button StartGameButton;
    public Button ContinueGameButton;

    public Button OptionsButton;
    public Button VolumeSettingsButton;
    public Button KeyBindSettingButton;

    public Button CreditsButton;
    public Button ExitGameButton;

    public GameObject MenuUI;

    public GameObject OptionsMenuUI;
    public GameObject VolumeSettingsMenuUI;
    public GameObject KeyBindSettingsMenuUI;

    public GameObject CreditsMenuUI;

    void Start()
    {
        AudioManager.Instance.Play("Music_Static");

    }

    public enum MainMenuState
    {
        Menu = 0,
        Options = 1,
        Credits = 2,
        VolumeSettings = 3,
        KeyBindSettings = 4
    }

    public MainMenuState State;

    public void SwitchState(MainMenuState aState)
    {
        MenuUI.SetActive(false);
        OptionsMenuUI.SetActive(false);
        CreditsMenuUI.SetActive(false);
        VolumeSettingsMenuUI.SetActive(false);
        KeyBindSettingsMenuUI.SetActive(false);

        switch (aState)
        {
            case MainMenuState.Menu:
                AudioManager.Instance.PlayOne("Select_Mod");
                MenuUI.SetActive(true);
                break;
            case MainMenuState.Options:
                AudioManager.Instance.PlayOne("Select_Mod");
                OptionsMenuUI.SetActive(true);
                break;
            case MainMenuState.Credits:
                AudioManager.Instance.PlayOne("Select_Mod");
                OptionsMenuUI.SetActive(true);
                CreditsMenuUI.SetActive(true);
                break;
            case MainMenuState.VolumeSettings:
                AudioManager.Instance.PlayOne("Select_Mod");
                OptionsMenuUI.SetActive(true);
                VolumeSettingsMenuUI.SetActive(true);
                break;
            case MainMenuState.KeyBindSettings:
                AudioManager.Instance.PlayOne("Select_Mod");
                OptionsMenuUI.SetActive(true);
                KeyBindSettingsMenuUI.SetActive(true);
                break;
        }

        State = aState;
    }
    public void SwitchState(int aState)
    {
        SwitchState((MainMenuState)aState);
    }

    public void exitGame()
    {
        Debug.Log("Exit game");
        Application.Quit();
    }

    public void continueGame()
    {
        //How to continue game ?
    }

    private void OnEnable()
    {
        StartGameButton.onClick.AddListener(GameManager.Instance.StartNewGame);
        ContinueGameButton.onClick.AddListener(GameManager.Instance.ContinueGame);
    }

    private void OnDisable()
    {
        StartGameButton.onClick.RemoveAllListeners();
        ContinueGameButton.onClick.RemoveAllListeners();
    }
}
