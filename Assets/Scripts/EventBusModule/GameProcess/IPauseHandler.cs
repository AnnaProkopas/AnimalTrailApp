namespace EventBusModule.GameProcess
{
    public interface IPauseHandler : IGlobalSubscriber
    {
        void Pause();
        void Resume();
    }
}