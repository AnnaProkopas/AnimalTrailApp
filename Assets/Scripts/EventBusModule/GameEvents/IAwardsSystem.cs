using GameObjects;

namespace EventBusModule.GameEvents
{
    public interface IAwardsSystem : IGlobalSubscriber
    {
        void HandleEatJunkFood(TriggeredObjectType type);
        
        void HandleEatHealthyFood(TriggeredObjectType type);

        void HandleEnergyDeath();

        void HandleFoodDeath();

        void HandleDogDeath();

        void HandleHumanCrying();

        void HandleHumanEnjoying();
    }
}