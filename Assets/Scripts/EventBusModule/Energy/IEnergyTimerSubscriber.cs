
namespace EventBusModule.Energy
{
    public interface IEnergyTimerSubscriber : IGlobalSubscriber
    {
        void HandleEnergyByPlayer(int variation);

        void Restart(int currentEnergy);
    }
}
