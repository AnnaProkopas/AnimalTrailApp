using UnityEngine;

public class Cake : MonoBehaviour, IPlayerTriggered
{
    [SerializeField]
    public int energyPoints = 8;
    [SerializeField]
    public int healthPoints = -1;

    private readonly TriggeredObjectType type = TriggeredObjectType.Cake;

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
        switch (playerState)
        {
            case PlayerState.Dead:
            case PlayerState.Dying:
                break;
            default:
                player.EatJunkFood(energyPoints, healthPoints);
                Destroy(gameObject);
                break;
        }
    }
}
