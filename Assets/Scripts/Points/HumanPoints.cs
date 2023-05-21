using System;
using UnityEngine;
using UnityEngine.UIElements;

public class HumanPoints : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private HumanPointState currentState;
    private float _value = .0f;

    public float Value
    {
        get => _value;
        set
        {
            _value = Mathf.Min(value, 1f);
            if (_value <= .3f) 
                SetTexture(HumanPointState.Low);
            else if (_value <= .7)
                SetTexture(HumanPointState.Middle);
            else
                SetTexture(HumanPointState.High);
        }
    }

    private static readonly int State = Animator.StringToHash("State");

    public void Plus(float value)
    {
        Value = Mathf.Min(Value + value, 1f);
    }
    
    private void SetTexture(HumanPointState state)
    {
        if (currentState != state)
        {
            animator.SetInteger(State, (int)state);
            currentState = state;
        }
    }
}
