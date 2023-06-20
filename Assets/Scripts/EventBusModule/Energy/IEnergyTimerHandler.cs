
namespace EventBusModule.Energy
{
    public interface IEnergyTimerHandler : IGlobalSubscriber
    {
        void HandleEnergyByPlayer(int variation);

        void Restart(int currentEnergy);
    }
}
