using EventBusModule;
using EventBusModule.Energy;
using EventBusModule.GameProcess;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLevelNavigation : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private EnergyManager energyManager;
    [SerializeField] private GameObject gameButtons;

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Pause();
            LevelLoader.Save();
        }
    }
    
    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            Pause();
            LevelLoader.Save();
        }
    }
    
    public void Pause()
    {
        pauseMenu.SetActive(true);
        gameButtons.SetActive(false);
        Time.timeScale = 0f;
        EventBus.RaiseEvent<IPauseHandler>(h => h.Pause());
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        gameButtons.SetActive(true);
        Time.timeScale = 1f;
        EventBus.RaiseEvent<IPauseHandler>(h => h.Resume());
    }

    public void Home()
    {
        LevelLoader.Save();
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    
    public static void GameOver()
    {
        LevelLoader.Delete();
        SceneManager.LoadScene(0);
    }
}
