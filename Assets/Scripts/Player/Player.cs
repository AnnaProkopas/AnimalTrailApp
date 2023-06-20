using System;
using EventBusModule;
using EventBusModule.Controls;
using EventBusModule.Energy;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class Player : MonoBehaviour, IEnergyPlayerHandler, IJoystickHandler
{
    public delegate void AttackDelegate(Player player);
    public delegate void SetActiveDelegate(JoyButtonState active);

    public AttackDelegate onAttack;
    public AttackDelegate onHideInGrass;
    public AttackDelegate afterHideInGrass;
    public AttackDelegate onMeetHuman;
    public SetActiveDelegate setActiveCustomJoyButton;
    
    private Rigidbody2D rb;
    [SerializeField] private HealthPoints healthPoints;
    [SerializeField] private Text currentScoreText;
    private int energy;
    [SerializeField] private HumanPoints humanPoints;
    
    [SerializeField] private float speed;
    private Animator animator;

    private PlayerState currentState;
    private Vector2 movement;
    private int lastDirectionX = 0;

    private int currentScore = 0;
    private int currentJunkFoodScore = 0;
    private int recordValueForFoodCounter;
    private int health = 6;
    private Random rnd;
    
    private static readonly int AnimatorAttributeState = Animator.StringToHash("State");
    private static readonly int AnimatorAttributeSpeed = Animator.StringToHash("Speed");
    private static readonly int AnimatorAttributeDirectionX = Animator.StringToHash("DirectionX");
    private static readonly int AnimatorAttributeLastDirectionX = Animator.StringToHash("LastDirectionX");

    public int Health { get => health; }
    public int Energy { get => energy; }
    public int Score { get => currentScore; }
    public int JunkFoodScore { get => currentJunkFoodScore; }
    public float HumanPoints { get => humanPoints.Value; }
    public PlayerState State { get => currentState; }

    private const int MaxHealth = 10;

    private void Start()
    {
        recordValueForFoodCounter = PlayerRatingService.GetRecordFoodCounter();
        rnd = new Random();
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        
        healthPoints.HiddenChange(health);
    }
    
    private void OnEnable()
    {
        EventBus.Subscribe(this);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(this);
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

        int signMovementX = (int)Mathf.Sign(movement.x);

        animator.SetInteger(AnimatorAttributeState, (int)currentState);
        animator.SetFloat(AnimatorAttributeSpeed, movement.SqrMagnitude());
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
            var movementPerTime = movement.normalized * speed;
            rb.MovePosition(rb.position + movementPerTime * Time.fixedDeltaTime);
        }
    }

    public void HandleDragJoystick(float horizontal, float vertical)
    {
        movement.x = Mathf.Sign(horizontal) * (Mathf.Abs(horizontal) > .2f ? 1 : 0);
        movement.y = Mathf.Sign(vertical) * (Mathf.Abs(vertical) > .2f ? 1 : 0);
    }

    private void OnTriggerEnter2D (Collider2D other) 
    {
        IPlayerTriggered otherObject = other.gameObject.GetComponent<IPlayerTriggered>();
        if (otherObject != null) 
        {
            otherObject.OnPlayerTriggerEnter(this, currentState);
            SetActiveCustomJoyButton();
        }
    }

    private void OnTriggerExit2D (Collider2D other) 
    {
        IPlayerTriggered otherObject = other.gameObject.GetComponent<IPlayerTriggered>();
        if (otherObject != null) 
        {
            otherObject.OnPlayerTriggerExit(this, currentState);
            SetActiveCustomJoyButton();
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
        return health == 0 || energy == 0;
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
        healthPoints.AnimatedChange(newHealth, newHealth - health);
        health = newHealth;
        if (IsReadyForDeath())
        {
            StartDyingProcess();
        }
    }

    private void ChangeHumanPoint(float value)
    {
        humanPoints.Plus(value);
    }

    public Vector2 GetPosition() 
    {
        return rb.position;
    }

    public void DieEvent() 
    {
        PlayerRatingService.AddRecord(currentScore, currentScore);
        Destroy(gameObject);
        GameLevelNavigation.GameOver();
    }

    private void Eat(int energyFoodValue, int healthPointsValue)
    {
        EventBus.RaiseEvent<IEnergyTimerHandler>(h => h.HandleEnergyByPlayer(energyFoodValue));
        ChangeHealth(healthPointsValue);
        SoundEffectHelper.instance.MakeEatSound(transform.position);
    }
    
    public void EatHealthyFood(int energyFoodValue, int healthPointsValue)
    {
        Eat(energyFoodValue, healthPointsValue); 
        IncreaseFoodCounter();
    }
    
    public void EatJunkFood(int energyFoodValue, int healthPointsValue)
    {
        Eat(energyFoodValue, healthPointsValue);
        ChangeHumanPoint(0.1f);
        currentJunkFoodScore++;
    }

    public void HandleTotalEnergy(int currentValue, int variation, bool isAnimated)
    {
        energy = currentValue;
        
        if (currentValue <= 0)
        {
            StartDyingProcess();
        }
    }

    public void OnStartTakingDamage(int damage)
    {
        ChangeHealth(-damage);
        IfNotDyingSetState(PlayerState.Wounded);
        SoundEffectHelper.instance.MakeFallSound(transform.position);
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
        if (onAttack != null)
        {
            IfNotDyingSetState(PlayerState.Attack);
            onAttack.Invoke(this);
        } 
        else if (onHideInGrass != null)
        {
            IfNotDyingSetState(PlayerState.Hidden);
            onHideInGrass.Invoke(this);
            afterHideInGrass?.Invoke(this);
        } 
        else if (onMeetHuman != null)
        {
            // Если дружелюбен к человеку
            if (humanPoints.Value * 10 < rnd.Next(10))
            {
                IfNotDyingSetState(PlayerState.LookAround);
                ChangeHumanPoint(0.05f);
            }
            else // Если нейтрален=негативен к человеку
            {
                IfNotDyingSetState(PlayerState.Attack);
            }
            onMeetHuman.Invoke(this);
        }
        else
        {
            IfNotDyingSetState(PlayerState.Attack);
        }
    }

    public void DisableAttackMode()
    {
        IfNotDyingSetState(PlayerState.Idle);
    }

    public void UpdateOnLevelLoad(Vector3 position, int loadedHealth, int loadedEnergy, int score, int junkFoodScore, float _humanPoints, PlayerState state)
    {
        transform.position = position;
        humanPoints.Value = _humanPoints;
        health = loadedHealth;
        healthPoints.HiddenChange(loadedHealth);
        // energy.Restart(loadedEnergy);
        currentScore = score;
        currentJunkFoodScore = junkFoodScore;
        currentState = state;
    }

    private void SetActiveCustomJoyButton()
    {
        if (onAttack != null)
        {
            setActiveCustomJoyButton?.Invoke(JoyButtonState.Eat);
        }
        else if (onHideInGrass != null)
        {
            setActiveCustomJoyButton?.Invoke(JoyButtonState.Hide);
        } 
        else if (onMeetHuman != null)
        {
            setActiveCustomJoyButton?.Invoke(JoyButtonState.Human);
        }
        else
        {
            setActiveCustomJoyButton?.Invoke(JoyButtonState.Simple);
        }
    }
}
