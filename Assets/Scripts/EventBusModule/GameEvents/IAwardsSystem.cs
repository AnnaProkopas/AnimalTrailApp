using GameObjects;

namespace EventBusModule.GameEvents
{
    public interface IAwardsSystem : IGlobalSubscriber
    {
        void HandleEatJunkFood();
        
        void HandleEatHealthyFood();

        void HandleEnergyDeath();

        void HandleFoodDeath();

        void HandleDogAttack();

        void HandleHumanCrying();

        void HandleHumanEnjoying();

        void HandleCarCollision();

        void HandleCarSnackCollision();
    }
}