using System;
using System.Collections.Generic;
using System.Net.Mime;
using Awards;
using EventBusModule;
using EventBusModule.GameProcess;
using LevelLoaderModule;
using PlayerModule;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Game
{
    public class GameLevelNavigation : MonoBehaviour, IAwardHandler
    {
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject gameButtons;

        [SerializeField] private Texture awardTexture;
        [SerializeField] private GameObject lastAwardSprite;
        
        
        private List<AwardType> awardTypes = new List<AwardType>();
        // private GameObject movable = null;

        private void OnEnable()
        {
            EventBus.Subscribe(this);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

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

        public void HandleGettingAward(AwardType type)
        {
            awardTypes.Add(type);
            
            Sprite sprite = AwardsStorage.GetSprite(awardTexture, type);
            lastAwardSprite.GetComponent<Image>().sprite = sprite;
            lastAwardSprite.SetActive(true);
            if (awardTypes.Count > 1)
            {
                lastAwardSprite.GetComponentInChildren<Text>().text = awardTypes.Count + "";
            }
            // var v = center.transform.position;
            // movable = Instantiate(lastAwardSprite, v, Quaternion.identity);
            // movable.pare
            // movable.GetComponent<Image>().sprite = sprite;
            // movable.GetComponentInChildren<Text>().text = "";
        }

        // private void FixedUpdate()
        // {
        //     if (movable != null)
        //     {
        //         var goalP = lastAwardSprite.transform.position;
        //         var p = movable.transform.position;
        //         if ((goalP - p).magnitude < 5)
        //         {
        //             lastAwardSprite.GetComponent<Image>().sprite = movable.GetComponent<Image>().sprite;
        //             lastAwardSprite.SetActive(true);
        //             if (awardTypes.Count > 1)
        //             {
        //                 lastAwardSprite.GetComponentInChildren<Text>().text = awardTypes.Count + "";
        //             }
        //
        //             Destroy(movable);
        //             movable = null;
        //         }
        //         else
        //         {
        //             p += (goalP - p).normalized * 4;
        //         }
        //     }
        // }
    }
}
