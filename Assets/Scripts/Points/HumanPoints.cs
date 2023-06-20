using EventBusModule;
using EventBusModule.PlayerPoints;
using UnityEngine;
using Utils;

public class HumanPoints : MonoBehaviour, IHumanPointsHandler
{
    private Animator animator;
    private HumanPointState currentState;
    private float _value = .0f;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe(this);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(this);
    }

    public float Value
    {
        get => _value;
        set
        {
            _value = MathUtils.Clamp(value, 0f, 1f);
            if (_value <= .3f) 
                SetTexture(HumanPointState.Low);
            else if (_value <= .7)
                SetTexture(HumanPointState.Middle);
            else
                SetTexture(HumanPointState.High);
        }
    }

    private static readonly int State = Animator.StringToHash("State");

    public void HandleHumanPointsValue(float? currentValue, float? variation, bool isAnimated)
    {
        Value = currentValue ?? (Value + (variation ?? 0));
    }

    private void SetTexture(HumanPointState state)
    {
        if (currentState != state)
        {
            animator.SetFloat(State, (int)state);
            currentState = state;
        }
    }
}
