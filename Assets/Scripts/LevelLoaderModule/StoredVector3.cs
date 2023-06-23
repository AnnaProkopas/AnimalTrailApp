using System;
using UnityEngine;

namespace LevelLoaderModule
{
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
}