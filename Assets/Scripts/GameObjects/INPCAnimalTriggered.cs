using GameObjects.AnimalForFood;

namespace GameObjects
{
    public interface INPCAnimalTriggered
    {
        public void OnNpcAnimalTriggerEnter(INPCAnimal npcAnimal);
    }
}