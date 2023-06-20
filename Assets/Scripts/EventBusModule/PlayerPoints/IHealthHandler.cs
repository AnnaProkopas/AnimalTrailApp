namespace EventBusModule.PlayerPoints
{
    public interface IHealthHandler : IGlobalSubscriber
    {
        void HandleHealthValue(int currentValue, int? variation, bool isAnimated);
    }
}