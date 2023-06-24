using System.Collections.Generic;
using EventBusModule.GameEvents;

namespace Awards
{
    public class AwardsController : IAwardsSystem
    {
        private static AwardsController _instance;
        private static AwardsController Instance
        {
            get {
                if (_instance == null)
                {
                    _instance = new AwardsController();
                }

                return _instance;
            }
        }

        private List<GameEvent> history = new List<GameEvent>();

        public void HandleEatJunkFood()
        {
            Instance.history.Add(GameEvent.JunkFood);
        }

        public void HandleEatHealthyFood()
        {
            Instance.history.Add(GameEvent.HealthyFood);
        }

        public void HandleEnergyDeath()
        {
            Instance.history.Add(GameEvent.EnergyDeath);
        }

        public void HandleFoodDeath()
        {
            Instance.history.Add(GameEvent.FoodDeath);
        }

        public void HandleDogAttack()
        {
            Instance.history.Add(GameEvent.DogAttack);
        }

        public void HandleHumanCrying()
        {
            Instance.history.Add(GameEvent.HumanCrying);
        }

        public void HandleHumanEnjoying()
        {
            Instance.history.Add(GameEvent.HumanEnjoying);
        }
        
        public void HandleCarCollision()
        {
            Instance.history.Add(GameEvent.CarCollision);
        }
        
        public void HandleCarSnackCollision()
        {
            Instance.history.Add(GameEvent.CarSnackCollision);
        }
    }
}