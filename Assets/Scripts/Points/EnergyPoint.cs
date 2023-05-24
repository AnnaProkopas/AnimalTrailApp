
using UnityEngine;

public class EnergyPoint : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private static readonly int State = Animator.StringToHash("State");

    public void ChangeValue(PointState state)
    {
        animator.SetFloat(State, (int)state);
    }
}