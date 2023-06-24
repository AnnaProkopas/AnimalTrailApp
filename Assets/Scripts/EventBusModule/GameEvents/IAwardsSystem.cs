using GameObjects;

namespace EventBusModule.GameEvents
{
    public interface IAwardsSystem : IGlobalSubscriber
    {
        void HandleEatJunkFood();
        
        void HandleEatHealthyFood();

        void HandleEnergyDeath();

        void HandleDogAttack();

        void HandleHumanCrying();

        void HandleHumanEnjoying();

        void HandleCarCollision();

        void HandleCarSnackSpawn();
        
        void HandleDeath();
        
        void HandleHideFrom();
    }
}