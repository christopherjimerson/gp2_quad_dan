using UnityEngine;

public class SCR_PauseMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetLevel() {
        SCR_GameController.Instance.TogglePauseMenuUI(false, 1);
        SCR_GameController.Instance.ResetLevel();
    }
}
