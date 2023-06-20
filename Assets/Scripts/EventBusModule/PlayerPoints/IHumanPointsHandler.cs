namespace EventBusModule.PlayerPoints
{
    public interface IHumanPointsHandler : IGlobalSubscriber
    {
        void HandleHumanPointsValue(float? currentValue, float? variation, bool isAnimated);
    }
}