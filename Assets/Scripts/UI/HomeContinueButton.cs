using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeContinueButton : MonoBehaviour
{
    private void Start()
    {
        if (!CanLoadOldLevel())
        {
            GetComponent<Button>().interactable = false;
        }
    }

    public bool CanLoadOldLevel()
    {
        return LevelLoader.HasSavedLevel();
    }
}