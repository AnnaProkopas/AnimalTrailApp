using System;
using System.Collections;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    [SerializeField] 
    private Player player;
    
    [SerializeField]
    private EnergyPoints energyPoints;
    [SerializeField]
    private int maxEnergy;
    
    private int totalEnergy = 10;

    private int TotalEnergy
    {
        get => totalEnergy;
        set
        {
            totalEnergy = value;
            if (totalEnergy == 0)
            {
                player.OnEnergyIsOver();
            }
        }
    }

    private DateTime nextEnergyTime;

    private DateTime lastChangedTime;

    [SerializeField]
    private int restoreDuration = 2;

    private bool restoring = false;

    private void Start() 
    {
        Restart(maxEnergy);
        energyPoints.HiddenChange(TotalEnergy);
    }

    private IEnumerator RestoreRoutine() 
    {
        UpdateEnergy();
        restoring = true;

        while (TotalEnergy > 0 && restoring) 
        {
            DateTime currentTime = DateTime.Now;
            DateTime counter = nextEnergyTime;
            bool isAdding = false;
            if (currentTime > counter) 
            {
                if (TotalEnergy > 0) 
                {
                    isAdding = true;
                    TotalEnergy--;
                    DateTime timeToSub = lastChangedTime > counter ? lastChangedTime : counter;
                    counter = AddDuration(timeToSub, restoreDuration);
                }
            }

            if (isAdding) 
            {
                lastChangedTime = DateTime.Now;
                nextEnergyTime = counter;
            }

            UpdateEnergy();
            // Save();
            yield return null;
        }
        restoring = false;
    }

    private void UpdateEnergy() 
    { 
        energyPoints.HiddenChange(TotalEnergy);
    }

    private DateTime AddDuration(DateTime time, int duration) 
    {
        return time.AddSeconds(duration);
    }

    public void Add(int value) 
    {
        if (TotalEnergy == maxEnergy) 
        {
            return;
        }

        var prevEnergy = TotalEnergy;
        TotalEnergy = Math.Min(TotalEnergy + value, maxEnergy);
        energyPoints.AnimatedChange(TotalEnergy, TotalEnergy - prevEnergy);
    }

    public void Subtract(int value) 
    {
        if (TotalEnergy == 0) 
        {
            return;
        }

        var prevEnergy = TotalEnergy;
        TotalEnergy = Math.Max(TotalEnergy - value, 0);
        energyPoints.AnimatedChange(TotalEnergy, TotalEnergy - prevEnergy);

        if (!restoring) 
        {
            if (TotalEnergy == 1) 
            {
                // Add time to life so as not to die from the timer immediately after dealing damage
                nextEnergyTime = AddDuration(DateTime.Now, restoreDuration);
            }
            StartCoroutine(RestoreRoutine());
        }
    }

    public int GetEnergyValue() 
    {
        return TotalEnergy;
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
        TotalEnergy = currentEnergy;
        energyPoints.HiddenChange(currentEnergy);
        lastChangedTime = DateTime.Now;
        nextEnergyTime = AddDuration(DateTime.Now, restoreDuration * 2);

        StartCoroutine(RestoreRoutine());
    }
}
