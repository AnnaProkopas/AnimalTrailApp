using UnityEngine;

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
    }
    
    private void OnHideInGrass(Player player)
    {
    }
}
