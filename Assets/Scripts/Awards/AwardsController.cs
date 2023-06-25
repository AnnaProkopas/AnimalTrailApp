using System.Collections.Generic;
using System.Linq;
using EventBusModule;
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

        public AwardsController()
        {
            _instance = this;
            EventBus.Subscribe(this);
        }

        private List<GameEvent> history = new List<GameEvent>();
        private List<GameEvent> History
        {
            get => Instance.history;
        }

        public void HandleEatJunkFood()
        {
            History.Add(GameEvent.JunkFood);

            if (History.Count - History.LastIndexOf(GameEvent.CarSnackSpawn) < 4)
            {
                AwardsStorage.Save(new List<AwardType> { AwardType.FoodCar });
            }
        }

        public void HandleEatHealthyFood()
        {
            int healthyCount = History.Count(x => x == GameEvent.HealthyFood);
            if ((healthyCount - 2) % 5 == 0)
            {
                AwardsStorage.Save(new List<AwardType> { AwardType.MouseEat });
            }
            
            History.Add(GameEvent.HealthyFood);
        }

        public void HandleEnergyDeath()
        {
            History.Add(GameEvent.Death);
            if (History.Contains(GameEvent.JunkFood) && History.Contains(GameEvent.HumanEnjoying))
            {
                AwardsStorage.Save(new List<AwardType> { AwardType.SickAnimal });
            }
        }

        public void HandleDogAttack()
        {
            History.Add(GameEvent.DogAttack);
        }
        
        public void HandleHideFrom()
        {
            if (History.Count - History.LastIndexOf(GameEvent.DogAttack) < 4)
            {
                AwardsStorage.Save(new List<AwardType> { AwardType.DogHide });
            }
            History.Add(GameEvent.HideEvent);
        }

        public void HandleHumanCrying()
        {
            History.Add(GameEvent.HumanCrying);
            AwardsStorage.Save(new List<AwardType> { AwardType.HumanCry });
        }

        public void HandleHumanEnjoying()
        {
            History.Add(GameEvent.HumanEnjoying);
            AwardsStorage.Save(new List<AwardType> { AwardType.HumanEnjoy });
        }
        
        public void HandleCarCollision()
        {
            History.Add(GameEvent.CarCollision);
        }
        
        public void HandleCarSnackSpawn()
        {
            History.Add(GameEvent.CarSnackSpawn);
        }

        public void HandleDeath()
        {
            List<AwardType> reasonsOfDeath = new List<AwardType>();

            History.Reverse();
            foreach (var eEvent in History.Take(3).ToList())
            {
                switch (eEvent)
                {
                    case GameEvent.CarCollision:
                        reasonsOfDeath.Add(AwardType.DieFastCar);
                        break;
                    case GameEvent.CarSnackSpawn:
                        reasonsOfDeath.Add(AwardType.FoodCar);
                        break;
                    case GameEvent.DogAttack:
                        reasonsOfDeath.Add(AwardType.DieDog);
                        break;
                    case GameEvent.JunkFood:
                        reasonsOfDeath.Add(AwardType.GarbageEat);
                        break;
                }
            }
            AwardsStorage.Save(reasonsOfDeath);
            
            EventBus.Unsubscribe(_instance);
        }
    }
}