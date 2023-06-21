using GameHelpers;

namespace GameObjects.AnimalForFood
{
    public class Bird : Mouse, ISavable
    {
        private readonly TriggeredObjectType type = TriggeredObjectType.Bird;
        public new TriggeredObjectType Type { get => type; }
    
        private void Start()
        {
            energyPoints = GameConstants.EnergyByType(Type);
        }
    }
}
