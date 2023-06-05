
public struct GameConstants
{
    public static int EnergyByType(TriggeredObjectType type)
    {
        switch (type)
        {
            case TriggeredObjectType.Garbage:
                return 1;
            case TriggeredObjectType.Sausage:
                return 2;
            case TriggeredObjectType.Mouse:
            case TriggeredObjectType.GrayMouse:
            case TriggeredObjectType.Bird:
            case TriggeredObjectType.Meat:
            case TriggeredObjectType.RawMeat:
            case TriggeredObjectType.Cheese:
                return 3;
            case TriggeredObjectType.Cake:
                return 4;
        }

        return 0;
    }

    public static int DamageByType(TriggeredObjectType type)
    {
        switch (type)
        {
            case TriggeredObjectType.Car:
                return 4;
            case TriggeredObjectType.Dog:
                return 1;
        }

        return 0;
    }
}
