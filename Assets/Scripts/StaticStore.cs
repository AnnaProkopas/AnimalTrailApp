
public static class StaticStore
{
    private static LevelLoadType loadLevelType;
    
    public static void HomeOnLevelLoad(LevelLoadType type)
    {
        loadLevelType = type;
    }
    
    public static LevelLoadType GetLevelLoadType()
    {
        return loadLevelType;
    }
}

public enum LevelLoadType
{
    NewLevel,
    Continue
}