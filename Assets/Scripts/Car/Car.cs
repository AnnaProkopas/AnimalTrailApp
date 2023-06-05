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

    void Awake()
    {
        direction = new Vector2(0, 0);
    }

    public void SetDirection(Vector3 toward)
    {
        direction = ((Vector2)(toward - transform.position)).normalized;
    }
    
    public void OnNpcAnimalTriggerEnter(INPCAnimal npcAnimal)
    {
        npcAnimal.Disappear();
    }
}
