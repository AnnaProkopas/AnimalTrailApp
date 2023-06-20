using UnityEngine;

public class Car : MovableObject, IPlayerTriggered, INPCAnimalTriggered, ISavable
{
    public TriggeredObjectType Type { get => TriggeredObjectType.Car; }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
    
    public GameObject GetGameObject() {
        return gameObject;
    }

    public void OnPlayerTriggerEnter(Player player, PlayerState playerState) 
    {
        player.OnStartTakingDamage(GameConstants.DamageByType(Type));
    }
    
    public void OnPlayerTriggerExit(Player player, PlayerState playerState) 
    {
        player.OnFinishTakingDamage();
    }

    public void SetDirection(Vector3 toward)
    {
        direction = ((Vector2)(toward - transform.position)).normalized;
        speed = GameConstants.SpeedByType(TriggeredObjectType.Car);
    }
    
    public void OnNpcAnimalTriggerEnter(INPCAnimal npcAnimal)
    {
        npcAnimal.Disappear();
    }
}
