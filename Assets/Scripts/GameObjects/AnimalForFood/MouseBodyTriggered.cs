using UnityEngine;

namespace GameObjects.AnimalForFood
{
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
}