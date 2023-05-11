using UnityEngine;

public class Mouse : MovableObject, IPlayerTriggered
{
    private readonly TriggeredObjectType type = TriggeredObjectType.Mouse;

    public TriggeredObjectType Type { get => type; }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
    
    public GameObject GetGameObject() {
        return gameObject;
    }

    [SerializeField]
    private Animator animator;
    [SerializeField]
    public int energyPoints = 5;
    [SerializeField]
    public int healthPoints = 1;

    Vector2 directionSign;
    float currentSpeed = 0;

    public void OnPlayerTriggerEnter(Player player, PlayerState playerState) 
    {
        switch (playerState)
        {
            case PlayerState.Attack:
                player.EatHealthyFood(energyPoints, healthPoints);
                Destroy(gameObject);
                break;
            default:
                RunAwayFrom(player.GetPosition());
                player.onAttack += OnAttack;
                break;
        }
    }

    public void OnPlayerTriggerExit(Player player, PlayerState state)
    {
        player.onAttack -= OnAttack;
    }

    protected override void Update()
    {
        float absX = Mathf.Abs(direction.x);
        float absY = Mathf.Abs(direction.y);

        animator.SetFloat("Speed", absX + absY);

        if (absX > absY) 
        {
            animator.SetFloat("DirectionX", Mathf.Sign(direction.x));
        } else {
            animator.SetFloat("DirectionY", Mathf.Sign(direction.y));
        }
    }

    private void RunAwayFrom(Vector2 playerPosition) 
    {
        direction = rb.position - playerPosition;
        directionSign.x = Mathf.Max(0, Mathf.Sign(direction.x));
        directionSign.y = Mathf.Sign(direction.y);
        currentSpeed = speed;
    }

    private void OnAttack(Player player)
    {
        Destroy(gameObject);

        player.EatHealthyFood(energyPoints, healthPoints);
    }
}
