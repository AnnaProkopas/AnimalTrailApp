﻿using UnityEngine;

namespace Points
{
    public class EnergyPoint : MonoBehaviour
    {
        private Animator animator;
        private static readonly int State = Animator.StringToHash("State");

        private void Awake()
        {
            animator = gameObject.GetComponent<Animator>();
        }
    
        public void ChangeValue(PointState state)
        {
            animator.SetFloat(State, (int)state);
        }
    }
}