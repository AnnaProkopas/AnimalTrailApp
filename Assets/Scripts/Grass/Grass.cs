using UnityEngine;

public class Grass : MonoBehaviour, IPlayerTriggered
{
    // TriggeredObjectType.Grass

    public void OnPlayerTriggerEnter(Player player, PlayerState state)
    {
        player.onHideInGrass += OnHideInGrass;
        player.SetActiveGreenJoyButton(true);
    }
    
    public void OnPlayerTriggerExit(Player player, PlayerState state)
    {
        player.onHideInGrass -= OnHideInGrass;
        player.SetActiveGreenJoyButton(false);
    }
    
    private void OnHideInGrass(Player player)
    {
    }
}
