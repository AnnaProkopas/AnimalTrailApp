using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void LoadScene(int sceneID) 
    {
        StaticStore.AddScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(sceneID);
    }

    public void LoadPreviousScene()
    {
        SceneManager.LoadScene(StaticStore.PopSceneId());
    }
}
