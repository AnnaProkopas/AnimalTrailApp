using UnityEngine;

public class Cake : MonoBehaviour, IPlayerTriggered, ISavable
{
    protected int energyPoints;
    [SerializeField] public int healthPoints = -1;

    [SerializeField] private TriggeredObjectType type = TriggeredObjectType.Cake;

    public TriggeredObjectType Type { get => type; }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
    
    public GameObject GetGameObject() {
        return gameObject;
    }

    private void Start()
    {
        energyPoints = GameConstants.EnergyByType(type);
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
