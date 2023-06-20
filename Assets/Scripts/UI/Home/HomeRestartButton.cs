using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeRestartButton : MonoBehaviour
{
    public void LoadNewLevel()
    {
        LevelLoader.Delete();
        SceneManager.LoadScene(1);
    }
}