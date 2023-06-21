using System;
using System.Collections.Generic;
using PlayerModule;
using UnityEngine;

namespace LevelLoader
{
    [Serializable]
    public class StoredLevel
    {
        public List<StoredObject> triggeredObjects;
    
        public StoredVector3 playerPosition;
        public int playerHealth;
        public int playerEnergy;
        public int playerScore;
        public int playerJunkFoodScore;
        public float playerHumanPoints;
        public int playerState;
    
        public StoredLevel()
        {
            triggeredObjects = new List<StoredObject>();
        }
    
        public void SavePlayer(Vector3 position, int health, int energy, int score, int junkFoodScore, float humanPoints, PlayerState state) 
            => (playerPosition, playerHealth, playerEnergy, playerScore, playerJunkFoodScore, playerHumanPoints, playerState) 
                = (new StoredVector3(position), health, energy, score, junkFoodScore, humanPoints, (int)state);
    }
}