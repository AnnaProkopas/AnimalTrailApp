
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

    public static float SpeedByType(TriggeredObjectType type, int state = -1)
    {
        switch (type)
        {
            case TriggeredObjectType.Car:
                return 7f;
            case TriggeredObjectType.Dog:
                switch (state)
                {
                    case (int)DogState.Haunting:
                        return 0.3f;
                    case (int)DogState.Walking:
                        return 0.2f;
                }

                return 0f;
            case TriggeredObjectType.Mouse:
            case TriggeredObjectType.GrayMouse:
                return 0.5f;
            case TriggeredObjectType.Bird:
                return 0.5f;
            
            case TriggeredObjectType.Fox:
                return 2f;
        }
        return 0.0f;
    }
}
