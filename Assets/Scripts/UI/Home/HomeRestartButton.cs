using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Home
{
    public class HomeRestartButton : MonoBehaviour
    {
        public void LoadNewLevel()
        {
            LevelLoader.LevelLoader.Delete();
            SceneManager.LoadScene(1);
        }
    }
}