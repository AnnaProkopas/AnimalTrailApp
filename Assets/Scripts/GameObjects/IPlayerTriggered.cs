using PlayerModule;

namespace GameObjects
{
    public interface IPlayerTriggered
    {
        public void OnPlayerTriggerEnter(Player player, PlayerState playerState);

        public void OnPlayerTriggerExit(Player player, PlayerState playerState)
        {
        }
    }
}