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
        movement = directionSign * currentSpeed;

        float absX = Mathf.Abs(direction.x);
        float absY = Mathf.Abs(direction.y);

        animator.SetFloat("Speed", absX + absY);

        if (absX > absY) 
        {
            animator.SetFloat("DirectionX", Mathf.Sign(movement.x));
        } else {
            animator.SetFloat("DirectionY", Mathf.Sign(movement.y));
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
        // CollisionResult res = new CollisionResult();
        // res.healthPoints = healthPoints;
        // res.energyPoints = energyPoints;
        // res.type = type;
        // return res;
    }
}
