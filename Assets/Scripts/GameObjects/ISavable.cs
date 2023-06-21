using UnityEngine;

namespace GameObjects
{
    public interface ISavable
    {
        TriggeredObjectType Type
        {
            get;
        }

        public Vector3 GetPosition();
    
        GameObject GetGameObject();
    }
}