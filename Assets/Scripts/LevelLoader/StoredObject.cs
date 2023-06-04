using System;
using System.Collections.Generic;
using UnityEngine;

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