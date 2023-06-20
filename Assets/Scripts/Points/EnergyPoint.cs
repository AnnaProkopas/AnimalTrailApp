
using UnityEngine;

public class EnergyPoint : MonoBehaviour
{
    private Animator animator;
    private static readonly int State = Animator.StringToHash("State");

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    
    public void ChangeValue(PointState state)
    {
        animator.SetFloat(State, (int)state);
    }
}