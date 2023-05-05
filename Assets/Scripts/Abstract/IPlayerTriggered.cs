using UnityEngine;

public interface IPlayerTriggered
{
    TriggeredObjectType Type
    {
        get;
    }

    public Vector3 GetPosition();
    
    GameObject GetGameObject();
    
    public void OnPlayerTriggerEnter(Player player, PlayerState playerState);

    public void OnPlayerTriggerExit(Player player, PlayerState playerState)
    {
    }
}