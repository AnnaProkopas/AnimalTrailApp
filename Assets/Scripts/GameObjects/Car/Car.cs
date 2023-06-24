using EventBusModule;
using EventBusModule.GameEvents;
using GameHelpers;
using GameObjects.AnimalForFood;
using PlayerModule;
using UnityEngine;

namespace GameObjects.Car
{
    public class Car : MovableObject, IPlayerTriggered, INPCAnimalTriggered, ISavable
    {
        public TriggeredObjectType Type { get => TriggeredObjectType.Car; }

        public Vector3 GetPosition()
        {
            return transform.position;
        }
    
        public GameObject GetGameObject() {
            return gameObject;
        }

        public void OnPlayerTriggerEnter(Player player, PlayerState playerState) 
        {
            EventBus.RaiseEvent<IAwardsSystem>(h => h.HandleCarCollision());
            player.OnStartTakingDamage(GameConstants.DamageByType(Type));
        }
    
        public void OnPlayerTriggerExit(Player player, PlayerState playerState) 
        {
            player.OnFinishTakingDamage();
        }

        public void SetDirection(Vector3 toward)
        {
            direction = ((Vector2)(toward - transform.position)).normalized;
            speed = GameConstants.SpeedByType(TriggeredObjectType.Car);
        }
    
        public void OnNpcAnimalTriggerEnter(INPCAnimal npcAnimal)
        {
            npcAnimal.Disappear();
        }
    }
}
