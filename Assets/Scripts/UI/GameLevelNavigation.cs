using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLevelNavigation : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private EnergyManager energyManager;
    [SerializeField] private GameObject gameButtons;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        gameButtons.SetActive(false);
        Time.timeScale = 0f;
        energyManager.Pause();
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        gameButtons.SetActive(true);
        Time.timeScale = 1f;
        energyManager.Resume();
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
