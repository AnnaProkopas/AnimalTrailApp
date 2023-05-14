using UnityEngine;

public class Grass : MonoBehaviour, IPlayerTriggered
{
    public TriggeredObjectType Type { get => TriggeredObjectType.Grass; }
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

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
