using UnityEngine;

public class CarSnackSpawner : MonoBehaviour, IPlayerTriggered
{
    [SerializeField]
    private GameObject[] foods;

    private readonly TriggeredObjectType type = TriggeredObjectType.CarFoodSpawner;

    public TriggeredObjectType Type { get => type; }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void OnPlayerTriggerEnter(Player player, PlayerState playerState)
    {
        switch (playerState)
        {
            case PlayerState.Dead:
            case PlayerState.Dying:
            case PlayerState.Attack:
                break;
            default:
                Spawn(player.GetPosition() + (new Vector2(2, 0)));
                break;
        }
    }

    private void Spawn(Vector2 position)
    {
        int foodInd = Random.Range(0, foods.Length);
        Instantiate(foods[foodInd], position, Quaternion.identity);
    }
}
