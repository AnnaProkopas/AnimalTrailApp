using UnityEngine;

namespace Points
{
    public class HealthPoint : MonoBehaviour
    {
        private Animator animator;
        private static readonly int State = Animator.StringToHash("State");
        private static readonly int Disable = Animator.StringToHash("Disable");

        private void Start()
        {
            animator = gameObject.GetComponent<Animator>();
        }
    
        public void ChangeValue(PointState state, PointDisabledState disabledState)
        {
            animator.SetFloat(State, (int)state);
            animator.SetFloat(Disable, (int)disabledState);
        }
    }
}