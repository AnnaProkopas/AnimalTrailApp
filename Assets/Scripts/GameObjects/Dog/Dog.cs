using System;
using System.Collections;
using UnityEngine;

public class Dog : MovableObject, IPlayerTriggered, ISavable
{
    private const int SimpleAnimate = 0;
    private const int AttackAnimate = 1;
    
    private const float AttackRadius = 0.6f;
    private float hauntingSpeed;
    private float walkingSpeed;

    private int AttackValue;
    private const int RestoreDuration = 3;
    
    private Vector2 startPosition;

    public TriggeredObjectType Type { get; } = TriggeredObjectType.Dog;

    [SerializeField]
    private Animator animator;
    
    private DogState currentState = DogState.Idle;

    private Player enemy;
    
    private bool restoring;
    private DateTime nextAttackTime;
    private DateTime lastChangedTime;
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int DirectionX = Animator.StringToHash("DirectionX");
    private static readonly int LastDirectionX = Animator.StringToHash("LastDirectionX");
    private static readonly int State = Animator.StringToHash("State");

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    private void Start()
    {
        startPosition = transform.position;
        AttackValue = GameConstants.DamageByType(Type);
        hauntingSpeed = GameConstants.SpeedByType(Type, (int)DogState.Haunting);
        walkingSpeed = GameConstants.SpeedByType(Type, (int)DogState.Walking);
    }

    protected void Update()
    {
        switch (currentState)
        {
            case DogState.Haunting:
                RunTo(enemy.GetPosition());
                if (direction.magnitude <= AttackRadius)
                    SetState(DogState.Attacking);
                break;
            case DogState.Attacking:
                if (GetDistance(enemy.GetPosition()) > AttackRadius)
                    SetState(DogState.Haunting);
                break;
            case DogState.Walking:
                if ((GetDirectionToHome() - direction).magnitude < 0)
                {
                    currentState = DogState.Idle;
                    speed = 0;
                }
                break;
        }

        animator.SetFloat(Speed, speed);

        animator.SetFloat(DirectionX, Mathf.Sign(direction.x));
        animator.SetFloat(LastDirectionX, lastDirectionX);
    }
    
    private IEnumerator RestoreAttackRoutine() 
    {
        restoring = true;

        while (restoring && enemy != null && enemy.State != PlayerState.Hidden && currentState == DogState.Attacking) 
        {
            DateTime currentTime = DateTime.Now;
            DateTime counter = nextAttackTime;
            bool isAdding = false;
            if (currentTime > counter) 
            {
                enemy.OnStartTakingDamage(AttackValue);
                
                DateTime timeToSub = lastChangedTime > counter ? lastChangedTime : counter;
                counter = timeToSub.AddSeconds(RestoreDuration);
                isAdding = true;
            }

            if (isAdding) 
            {
                lastChangedTime = DateTime.Now;
                nextAttackTime = counter;
            }

            yield return null;
        }

        if (enemy != null)
            enemy.OnFinishTakingDamage();
        
        restoring = false;
    }

    public void OnPlayerTriggerEnter(Player player, PlayerState playerState)
    {
        switch (playerState)
        {
            case PlayerState.Dead:
            case PlayerState.Dying:
                break;
            case PlayerState.Hidden:
                player.OnFinishTakingDamage();
                direction = GetDirectionToHome();
                SetState(DogState.Walking);
                break;
            default:
                enemy = player;
                if (GetDistance(player.GetPosition()) <= AttackRadius)
                {
                    SetState(DogState.Attacking);
                }
                else
                {
                    SetState(DogState.Haunting);
                }

                player.afterHideInGrass += AfterHideInGrass;
                break;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        var player = col.gameObject.GetComponent<Player>();
        if (player != null)
        {
            enemy = player;
            SetState(DogState.Attacking);
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        var player = col.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.OnFinishTakingDamage();
        }
    }

    public void OnPlayerTriggerExit(Player player, PlayerState playerState) 
    {
        player.OnFinishTakingDamage();
        SetState(DogState.Idle);
        enemy = null;
        player.afterHideInGrass -= AfterHideInGrass;
    }

    private void SetState(DogState state)
    {
        currentState = state;
        switch (state)
        {
            case DogState.Idle:
                animator.SetInteger(State, SimpleAnimate);
                speed = 0;
                break;
            case DogState.Walking:
                animator.SetInteger(State, SimpleAnimate);
                speed = walkingSpeed;
                break;
            case DogState.Attacking:
                animator.SetInteger(State, AttackAnimate);
                speed = 0;
                
                lastChangedTime = DateTime.Now;
                nextAttackTime = DateTime.Now;
                StartCoroutine(RestoreAttackRoutine());
                break;
            case DogState.Haunting:
                animator.SetInteger(State, AttackAnimate);
                speed = hauntingSpeed;
                
                RunTo(enemy.GetPosition());
                restoring = false;
                break;
        }
    }
    
    private void RunTo(Vector2 playerPosition) 
    {
        direction = playerPosition - rb.position;
    }
    
    private Vector2 GetDirectionToHome() 
    {
        return startPosition - rb.position;
    }

    private float GetDistance(Vector2 position)
    {
        Vector2 myPosition = transform.position;
        return (myPosition - position).magnitude;
    }
    
    private void AfterHideInGrass(Player player)
    {
        player.OnFinishTakingDamage();
        direction = GetDirectionToHome();
        SetState(DogState.Walking);
    }
}
