using UnityEngine;

public class HealthPoint : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private static readonly int State = Animator.StringToHash("State");
    private static readonly int Disable = Animator.StringToHash("Disable");

    public void ChangeValue(PointState state, PointDisabledState disabledState)
    {
        animator.SetFloat(State, (int)state);
        animator.SetFloat(Disable, (int)disabledState);
    }
}