using PlayerModule;
using UnityEngine;

namespace GameObjects.Grass
{
    public class Grass : MonoBehaviour, IPlayerTriggered
    {
        // TriggeredObjectType.Grass

        public void OnPlayerTriggerEnter(Player player, PlayerState state)
        {
            player.onHideInGrass += OnHideInGrass;
        }
    
        public void OnPlayerTriggerExit(Player player, PlayerState state)
        {
            player.onHideInGrass -= OnHideInGrass;
            switch (state)
            {
                case PlayerState.Attack:
                    player.EnableAttackMode();
                    break;
                default:
                    player.DisableAttackMode();
                    break;
            }
        }
    
        private void OnHideInGrass(Player player)
        {
        }
    }
}
