using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBodyTriggered : MonoBehaviour, INPCAnimal
{
    [SerializeField]
    public Mouse mouse;
    
    public void Disappear()
    {
        Destroy(mouse.gameObject);
    }

    private void OnTriggerEnter2D (Collider2D other) 
    {
        INPCAnimalTriggered otherObject = other.gameObject.GetComponent<INPCAnimalTriggered>();
        if (otherObject != null) 
        {
            otherObject.OnNpcAnimalTriggerEnter(this);
        }
    }
}