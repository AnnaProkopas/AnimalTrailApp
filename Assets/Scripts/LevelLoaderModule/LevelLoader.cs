﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Awards;
using EventBusModule;
using EventBusModule.Energy;
using EventBusModule.PlayerPoints;
using GameObjects;
using PlayerModule;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelLoaderModule
{
    public class LevelLoader: MonoBehaviour
    {
        private const string HasSavedLevelField = "has_saved_level";
        private const string FileName = "gamesave.save";
        
        private static readonly HashSet<TriggeredObjectType> IgnoreTypes = new HashSet<TriggeredObjectType>
        {
            TriggeredObjectType.CarFoodSpawner, 
            TriggeredObjectType.Default,
            TriggeredObjectType.Grass
        };

        private void Start()
        {
            Load();
        }

        public static bool HasSavedLevel()
        {
            return PlayerPrefs.GetInt(HasSavedLevelField, 0) == 1;
        }

        public static void Save()
        {
            StoredLevel level = new StoredLevel();
        
            foreach (var triggeredObject in FindInterfacesOfType<ISavable>())
            {
                if (!IgnoreTypes.Contains(triggeredObject.Type))
                {
                    level.triggeredObjects.Add(new StoredObject(triggeredObject.Type, triggeredObject.GetPosition()));
                }
            }

            var player = GetPlayer();
            level.SavePlayer(player.transform.position, player.Health, player.Energy, player.Score, player.JunkFoodScore, player.HumanPoints, player.State);
        
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/" + FileName);
            bf.Serialize(file, level);
            file.Close();

            PlayerPrefs.SetInt(HasSavedLevelField, 1);
        }

        public static void Delete()
        {
            File.Delete(Application.persistentDataPath + "/" + FileName);
            PlayerPrefs.SetInt(HasSavedLevelField, 0);
        }

        private static void Load()
        {
            int hasSavedLevel = PlayerPrefs.GetInt(HasSavedLevelField, 0);
            if (hasSavedLevel == 1)
            {
                var typeToObject = new Dictionary<TriggeredObjectType, GameObject>();
                foreach (var triggeredObject in FindInterfacesOfType<ISavable>(true))
                {
                    if (!IgnoreTypes.Contains(triggeredObject.Type))
                    {
                        var triggeredGameObject = triggeredObject.GetGameObject();
                        triggeredGameObject.SetActive(false);
                    
                        if (!typeToObject.ContainsKey(triggeredObject.Type))
                            typeToObject.Add(triggeredObject.Type, triggeredGameObject);
                        else
                            Destroy(triggeredGameObject);
                    }
                }
            
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/" + FileName, FileMode.Open);
                StoredLevel level = (StoredLevel)bf.Deserialize(file);
                file.Close();

                foreach (var triggeredObject in level.triggeredObjects)
                {
                    var gmObj = Instantiate(typeToObject[triggeredObject.type], triggeredObject.position.Get(), Quaternion.identity);
                    gmObj.SetActive(true);
                }

                foreach (var gmObj in typeToObject.Values)
                    Destroy(gmObj);
            
                GetPlayer().UpdateOnLevelLoad(level.playerPosition.Get(), level.playerHealth, level.playerEnergy, level.playerScore, level.playerJunkFoodScore, level.playerHumanPoints, (PlayerState)level.playerState);

                EventBus.RaiseEvent<IEnergyTimerSubscriber>(h => h.Restart(level.playerEnergy));
                EventBus.RaiseEvent<IHealthHandler>(h => h.HandleHealthValue(level.playerHealth, null, false));
                EventBus.RaiseEvent<IHumanPointsHandler>(h => h.HandleHumanPointsValue(level.playerHumanPoints, null, false));
            }

            new AwardsController();
        }

        private static Player GetPlayer()
        {
            return SceneManager.GetActiveScene().GetRootGameObjects()
                .SelectMany(go => go.GetComponentsInChildren<Player>(false)).First();
        }
        private static IEnumerable<T> FindInterfacesOfType<T>(bool includeInactive = false)
        {
            return SceneManager.GetActiveScene().GetRootGameObjects()
                .SelectMany(go => go.GetComponentsInChildren<T>(includeInactive));
        }
    }
}
