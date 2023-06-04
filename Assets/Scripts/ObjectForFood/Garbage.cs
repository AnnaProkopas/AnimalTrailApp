
using System;

public class Garbage : Cake, ISavable
{
    private readonly TriggeredObjectType type = TriggeredObjectType.Garbage;
    public new TriggeredObjectType Type { get => type; }

    private void Start()
    {
        energyPoints = EnergyValues.Garbage;
    }
}
