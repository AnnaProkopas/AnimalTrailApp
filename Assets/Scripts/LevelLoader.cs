using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

using UnityEngine.SceneManagement;

public class LevelLoader: MonoBehaviour
{
    private const string HasSavedLevelField = "has_saved_level";
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
        
        foreach (var triggeredObject in FindInterfacesOfType<IPlayerTriggered>())
        {
            if (!IgnoreTypes.Contains(triggeredObject.Type))
            {
                level.triggeredObjects.Add(new StoredObject(triggeredObject.Type, triggeredObject.GetPosition()));
            }
        }

        var player = GetPlayer();
        level.SavePlayer(player.transform.position, player.Health, player.Energy, player.Score, 0, player.State);
        
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, level);
        file.Close();

        PlayerPrefs.SetInt(HasSavedLevelField, 1);
    }

    public static void Delete()
    {
        File.Delete(Application.persistentDataPath + "/gamesave.save");
        PlayerPrefs.SetInt(HasSavedLevelField, 0);
    }

    private static void Load()
    {
        int hasSavedLevel = PlayerPrefs.GetInt(HasSavedLevelField, 0);
        if (hasSavedLevel == 1)
        {
            var typeToObject = new Dictionary<TriggeredObjectType, GameObject>();
            foreach (var triggeredObject in FindInterfacesOfType<IPlayerTriggered>(true))
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
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            StoredLevel level = (StoredLevel)bf.Deserialize(file);
            file.Close();

            foreach (var triggeredObject in level.triggeredObjects)
            {
                var gmObj = Instantiate(typeToObject[triggeredObject.type], triggeredObject.position.Get(), Quaternion.identity);
                gmObj.SetActive(true);
            }

            foreach (var gmObj in typeToObject.Values)
                Destroy(gmObj);
            
            GetPlayer().UpdateOnLevelLoad(level.playerPosition.Get(), level.playerHealth, level.playerEnergy, level.playerScore, level.playerHumanPoints, (PlayerState)level.playerState);
        }
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

[Serializable]
public class StoredLevel
{
    public List<StoredObject> triggeredObjects;
    
    public StoredVector3 playerPosition;
    public int playerHealth;
    public int playerEnergy;
    public int playerScore;
    public float playerHumanPoints;
    public int playerState;
    
    public StoredLevel()
    {
        triggeredObjects = new List<StoredObject>();
    }
    
    public void SavePlayer(Vector3 position, int health, int energy, int score, int humanPoints, PlayerState state) 
        => (playerPosition, playerHealth, playerEnergy, playerScore, playerHumanPoints, playerState) 
            = (new StoredVector3(position), health, energy, score, humanPoints, (int)state);
}

[Serializable]
public class StoredObject
{
    public TriggeredObjectType type;
    public StoredVector3 position;
    public StoredObject(TriggeredObjectType type, Vector3 position)
    {
        this.type = type;
        this.position = new StoredVector3(position);
    }
}

[Serializable]
public class StoredVector3
{
    public float positionX;
    public float positionY;
    public float positionZ;

    public StoredVector3(Vector3 position)
    {
        positionX = position.x;
        positionY = position.y;
        positionZ = position.z;
    }

    public Vector3 Get()
    {
        return new Vector3(positionX, positionY, positionZ);
    }
}