using UnityEngine;

public class Car : MovableObject, IPlayerTriggered, INPCAnimalTriggered, ISavable
{
    private readonly TriggeredObjectType type = TriggeredObjectType.Car;

    public TriggeredObjectType Type { get => type; }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
    
    public GameObject GetGameObject() {
        return gameObject;
    }

    public void OnPlayerTriggerEnter(Player player, PlayerState playerState) 
    {
        player.OnStartTakingDamage(8);
    }
    
    public void OnPlayerTriggerExit(Player player, PlayerState playerState) 
    {
        player.OnFinishTakingDamage();
    }

    void Start()
    {
        direction = new Vector2(1, 0);
    }
    
    public void OnNpcAnimalTriggerEnter(INPCAnimal npcAnimal)
    {
        npcAnimal.Disappear();
    }
}
