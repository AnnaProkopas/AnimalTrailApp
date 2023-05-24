using UnityEngine;

public class Garbage : MonoBehaviour, IPlayerTriggered
{
    [SerializeField]
    public int energyPoints = 2;
    [SerializeField]
    public int healthPoints = -2;

    private readonly TriggeredObjectType type = TriggeredObjectType.Garbage;

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
            case PlayerState.Attack:
                player.EatJunkFood(energyPoints, healthPoints);
                Destroy(gameObject);
                break;
            default:
                player.onAttack += OnAttack;
                break;
        }
    }

    public void OnPlayerTriggerExit(Player player, PlayerState state)
    {
        player.onAttack -= OnAttack;
    }

    private void OnAttack(Player player)
    {
        Destroy(gameObject);

        player.EatJunkFood(energyPoints, healthPoints);
    }
}
