
namespace EventBusModule.Energy
{
    public interface IEnergyHandler : IGlobalSubscriber
    {
        void HandleTotalEnergy(int currentValue, int variation, bool isAnimated);
    }
}
