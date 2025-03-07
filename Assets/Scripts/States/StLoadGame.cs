using UnityEngine;
using UnityEngine.SceneManagement;

public class StLoadGame : State
{
    AsyncOperation asyncOperation;

    public override void EnterState()
    {
        asyncOperation =  SceneManager.LoadSceneAsync(2,LoadSceneMode.Additive);

        asyncOperation.completed += SetActiveScene;
        
    }
    public override void UpdateState()
    {

    }
    public override void ExitState()
    {
        asyncOperation.completed -= SetActiveScene;
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(0));
        SceneManager.UnloadSceneAsync(2);
    }

    public void SetActiveScene(AsyncOperation s)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(2));
    }
}
