using System;
using System.Collections;
using UnityEngine;

public class Dog : MovableObject, IPlayerTriggered
{
    private const int SimpleAnimate = 0;
    private const int AttackAnimate = 1;
    
    private const float AttackRadius = 0.6f;
    private float hauntingSpeed = 0.3f;
    private float walkingSpeed = 0.2f;

    private const int AttackValue = 1;
    private const int RestoreDuration = 3;
    
    private readonly TriggeredObjectType type = TriggeredObjectType.Dog;
    public TriggeredObjectType Type { get => type; }
    
    [SerializeField]
    private Animator animator;
    
    private DogState currentState = DogState.Idle;
    private float lastDirectionX = 0;

    private Player enemy = null;
    
    private bool restoring = false;
    private DateTime nextAttackTime;
    private DateTime lastChangedTime;
    
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
    
    protected override void Update()
    {
        switch (currentState)
        {
            case DogState.Haunting:
                RunTo(enemy.GetPosition());
                if (Norm(direction) <= AttackRadius)
                    SetState(DogState.Attacking);
                break;
            case DogState.Attacking:
                if (GetDistance(enemy.GetPosition()) > AttackRadius)
                    SetState(DogState.Haunting);
                break;
        }

        if (speed != 0)
        {
            lastDirectionX = Mathf.Sign(direction.x);
        }

        animator.SetFloat("Speed", speed);

        animator.SetFloat("DirectionX", Mathf.Sign(direction.x));
        animator.SetFloat("LastDirectionX", lastDirectionX);
    }
    
    private IEnumerator RestoreRoutine() 
    {
        restoring = true;

        while (restoring && enemy != null && currentState == DogState.Attacking) 
        {
            DateTime currentTime = DateTime.Now;
            DateTime counter = nextAttackTime;
            bool isAdding = false;
            while (currentTime > counter) 
            {
                    isAdding = true;

                    enemy.OnStartTakingDamage(1);
                    
                    DateTime timeToSub = lastChangedTime > counter ? lastChangedTime : counter;
                    counter = timeToSub.AddSeconds(RestoreDuration);
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
                RunFrom(player.GetPosition());
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
        // player.onAttack -= OnAttack;
    }

    private void SetState(DogState state)
    {
        currentState = state;
        switch (state)
        {
            case DogState.Idle:
                animator.SetInteger("State", SimpleAnimate);
                speed = 0;
                break;
            case DogState.Walking:
                animator.SetInteger("State", SimpleAnimate);
                speed = walkingSpeed;
                break;
            case DogState.Attacking:
                animator.SetInteger("State", AttackAnimate);
                speed = 0;
                
                lastChangedTime = DateTime.Now;
                nextAttackTime = DateTime.Now;
                StartCoroutine(RestoreRoutine());
                break;
            case DogState.Haunting:
                animator.SetInteger("State", AttackAnimate);
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
    
    private void RunFrom(Vector2 playerPosition) 
    {
        direction = rb.position - playerPosition;
    }

    private float GetDistance(Vector2 position)
    {
        Vector2 myPosition = transform.position;
        return Norm(myPosition - position);
    }

    private float Norm(Vector2 position)
    {
        return position.x * position.x + position.y * position.y;
    }
}
