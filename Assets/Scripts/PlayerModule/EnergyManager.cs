using System;
using System.Collections;
using EventBusModule;
using EventBusModule.Energy;
using EventBusModule.GameProcess;
using UnityEngine;
using Utils;

namespace PlayerModule
{
    public class EnergyManager : MonoBehaviour, IEnergyTimerSubscriber, IPauseHandler
    {
        [SerializeField]
        private int maxEnergy;
    
        private int totalEnergy = 10;

        private DateTime nextEnergyTime;

        private DateTime lastChangedTime;

        [SerializeField]
        private int restoreDuration = 2;

        private bool restoring = false;

        private void Start() 
        {
            Restart(maxEnergy);
        }
    
        private void OnEnable()
        {
            EventBus.Subscribe(this);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        private IEnumerator RestoreRoutine() 
        {
            // UpdateEnergy();
            restoring = true;

            while (totalEnergy > 0 && restoring) 
            {
                DateTime currentTime = DateTime.Now;
                DateTime counter = nextEnergyTime;
                bool isRemoving = false;
                if (currentTime > counter) 
                {
                    if (totalEnergy > 0) 
                    {
                        isRemoving = true;
                    
                        totalEnergy--;            
                        EventBus.RaiseEvent<IEnergyHandler>(h => h.HandleTotalEnergy(totalEnergy, -1, false));

                        DateTime timeToSub = lastChangedTime > counter ? lastChangedTime : counter;
                        counter = AddDuration(timeToSub, restoreDuration);
                    }
                }

                if (isRemoving) 
                {
                    lastChangedTime = DateTime.Now;
                    nextEnergyTime = counter;
                }

                // UpdateEnergy();
                // Save();
                yield return null;
            }
            restoring = false;
        }

        private DateTime AddDuration(DateTime time, int duration) 
        {
            return time.AddSeconds(duration);
        }

        public void HandleEnergyByPlayer(int value) 
        {
            if ((totalEnergy == maxEnergy && value > 0) || (totalEnergy == 0 && value < 0)) 
            {
                return;
            }

            var prevEnergy = totalEnergy;
            totalEnergy = MathUtils.Clamp(totalEnergy + value, 0, maxEnergy);

            if (prevEnergy != totalEnergy)
            {
                EventBus.RaiseEvent<IEnergyHandler>(h => h.HandleTotalEnergy(totalEnergy, totalEnergy - prevEnergy, true));

                if (value < 0 && !restoring && totalEnergy == 1) 
                {
                    // Add time to life so as not to die from the timer immediately after dealing damage
                    nextEnergyTime = AddDuration(DateTime.Now, restoreDuration);
                    StartCoroutine(RestoreRoutine());
                }
            }
        }

        public int GetEnergyValue() 
        {
            return totalEnergy;
        }

        public void Pause()
        {
            restoring = false;
        }

        public void Resume()
        {
            lastChangedTime = DateTime.Now;
            nextEnergyTime = AddDuration(DateTime.Now, restoreDuration * 2);
        
            StartCoroutine(RestoreRoutine());
        }

        public void Restart(int currentEnergy)
        {
            totalEnergy = currentEnergy;
            EventBus.RaiseEvent<IEnergyHandler>(h => h.HandleTotalEnergy(totalEnergy, 0, false));

            lastChangedTime = DateTime.Now;
            nextEnergyTime = AddDuration(DateTime.Now, restoreDuration * 2);

            StartCoroutine(RestoreRoutine());
        }
    }
}
