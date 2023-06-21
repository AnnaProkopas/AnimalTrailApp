using EventBusModule;
using EventBusModule.PlayerPoints;
using UnityEngine;

namespace Points
{
    public class HealthInterface : MonoBehaviour, IHealthHandler
    {
        [SerializeField] private HealthPoint[] points;

        private int value;

        private void OnEnable()
        {
            EventBus.Subscribe(this);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public void HandleHealthValue(int currentValue, int? variation, bool isAnimated)
        {
            if (isAnimated)
            {
                // ...
            }
        
            value = currentValue;
            UpdateView();
        }

        private void UpdateView()
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (i < value / 2)
                    points[i].ChangeValue(PointState.Full, PointDisabledState.None);
                else if (i == value / 2 && value % 2 == 1)
                    points[i].ChangeValue(PointState.Half, PointDisabledState.None);
                else
                    points[i].ChangeValue(PointState.Empty, PointDisabledState.None); 
            }
        }
    }
}
