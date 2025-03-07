using UnityEngine;

public enum LoadState {NEW, CONTINUE, DEMO}

public class GameManager : StateMachine
{
    public static GameManager Instance;

    public LoadState LoadState = LoadState.NEW;

    [Header("Toggle off if you want to run tests in your own scene")]
    public bool ShouldLoadMainMenu = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if(ShouldLoadMainMenu)
            SwitchState<StMainMenu>();
    }

    public void StartNewGame()
    {
        LoadState = LoadState.NEW;
        SwitchState<StLoadGame>();
    }

    public void ContinueGame()
    {
        LoadState = LoadState.CONTINUE;
        SwitchState<StLoadGame>();
    }
}
