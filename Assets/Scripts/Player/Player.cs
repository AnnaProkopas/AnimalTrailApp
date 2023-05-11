using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public delegate void AttackDelegate(Player player);

    public AttackDelegate onAttack;
    
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Joystick joystick;
    [SerializeField]
    private Points healthText;
    [SerializeField]
    private Text currentScoreText;
    [SerializeField]
    private EnergyManager energy;
    
    [SerializeField]
    private float speed;
    [SerializeField]
    private Animator animator;

    private PlayerState currentState;
    private Vector2 movement;
    private int lastDirectionX = 0;

    private int currentScore = 0;
    private int recordValueForFoodCounter;
    private int health = 10;

    public int Health { get => health; }
    public int Energy { get => energy.GetEnergyValue(); }
    public int Score { get => currentScore; }
    public PlayerState State { get => currentState; }

    private const int MaxHealth = 10;

    private const string AnimatorAttributeState = "State";
    private const string AnimatorAttributeSpeed = "Speed";
    private const string AnimatorAttributeDirectionX = "DirectionX";
    private const string AnimatorAttributeLastDirectionX = "LastDirectionX";
    
    private void Start()
    {
        recordValueForFoodCounter = PlayerRatingService.GetRecordFoodCounter();
    }

    private void Update()
    {
        switch (currentState)
        {
        case PlayerState.Dead:
            return;
        case PlayerState.Dying:
            movement = new Vector2(0, 0);
            animator.SetInteger(AnimatorAttributeState, (int)PlayerState.Dead);
            return;
        }

        movement.x = Mathf.Sign(joystick.Horizontal) * (Mathf.Abs(joystick.Horizontal) > .2f ? 1 : 0) * speed;
        movement.y = Mathf.Sign(joystick.Vertical) * (Mathf.Abs(joystick.Vertical) > .2f ? 1 : 0) * speed;
        float absMovement = Mathf.Abs(movement.x) + Mathf.Abs(movement.y);
        int signMovementX = (int)Mathf.Sign(movement.x);

        animator.SetInteger(AnimatorAttributeState, (int)currentState);
        animator.SetFloat(AnimatorAttributeSpeed, absMovement);
        animator.SetFloat(AnimatorAttributeDirectionX, signMovementX);
        animator.SetFloat(AnimatorAttributeLastDirectionX, lastDirectionX);
        if (movement.x != 0)
        {
            lastDirectionX = signMovementX;
        }
    }

    private void FixedUpdate() 
    {
        if (currentState != PlayerState.Dead) 
        {
            rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D (Collider2D other) 
    {
        IPlayerTriggered otherObject = other.gameObject.GetComponent<IPlayerTriggered>();
        if (otherObject != null) 
        {
            Debug.Log((otherObject.Type));
            otherObject.OnPlayerTriggerEnter(this, currentState);
        }
    }

    private void OnTriggerExit2D (Collider2D other) 
    {
        IPlayerTriggered otherObject = other.gameObject.GetComponent<IPlayerTriggered>();
        if (otherObject != null) 
        {
            otherObject.OnPlayerTriggerExit(this, currentState);
        }
    }

    private void IncreaseFoodCounter() 
    {
        currentScore++;
        recordValueForFoodCounter = Math.Max(currentScore, recordValueForFoodCounter);
        currentScoreText.text = currentScore + (recordValueForFoodCounter > currentScore ? ("(" + recordValueForFoodCounter + ")") : "");
    }

    private bool IsReadyForDeath()
    {
        return health == 0 || energy.GetEnergyValue() == 0;
    }

    private void StartDyingProcess()
    {
        if (currentState != PlayerState.Dead) currentState = PlayerState.Dying;
    }

    private void IfNotDyingSetState(PlayerState state)
    {
        switch(currentState)
        {
        case PlayerState.Dead:
        case PlayerState.Dying:
            break;
        default:
            currentState = state;
            break;
        }
    }

    private void ChangeHealth(int value) 
    {
        int newHealth = Math.Min(Math.Max(health + value, 0), MaxHealth);
        healthText.AnimatedChange(newHealth, newHealth - health);
        health = newHealth;
        if (IsReadyForDeath())
        {
            StartDyingProcess();
        }
    }

    public Vector2 GetPosition() 
    {
        return rb.position;
    }

    public void DieEvent() 
    {
        // switch (currentState)
        // {
        //     case PlayerState.Dead:
        //     case PlayerState.Dying:
        //         break;
        //     default:
        //         break;
        // }

        PlayerRatingService.AddRecord(currentScore);
        Destroy(gameObject);
        GameLevelNavigation.GameOver();
    }

    private void Eat(int energyPoints, int healthPoints)
    {
        energy.Add(energyPoints);
        ChangeHealth(healthPoints);
        SoundEffectHelper.instance.MakeEatSound();
    }
    
    public void EatHealthyFood(int energyPoints, int healthPoints)
    {
        Eat(energyPoints, healthPoints); 
        IncreaseFoodCounter();
    }
    
    public void EatJunkFood(int energyPoints, int healthPoints)
    {
        Eat(energyPoints, healthPoints); 
    }

    public void OnEnergyIsOver()
    {
        StartDyingProcess();
    }

    public void OnStartTakingDamage(int damage)
    {
        ChangeHealth(-damage);
        IfNotDyingSetState(PlayerState.Wounded);
    }

    public void OnFinishTakingDamage()
    {
        if (currentState != PlayerState.Hidden)
        {
            IfNotDyingSetState(PlayerState.Idle);
        }
    }

    public void EnableAttackMode()
    {
        IfNotDyingSetState(PlayerState.Attack);
        if (onAttack != null)
        {
            onAttack.Invoke(this);
            // Eat(result.energyPoints, result.healthPoints);
        }
    }

    public void DisableAttackMode()
    {
        IfNotDyingSetState(PlayerState.Idle);
    }

    public void UpdateOnLevelLoad(Vector3 position, int loadedHealth, int loadedEnergy, int score, int humanPoints, PlayerState state)
    {
        transform.position = position;
        health = loadedHealth;
        healthText.HiddenChange(loadedHealth);
        energy.Restart(loadedEnergy);
        currentScore = score;
        currentState = state;
    }
}
