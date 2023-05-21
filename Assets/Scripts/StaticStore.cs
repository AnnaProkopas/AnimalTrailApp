using System.Collections.Generic;
using System.Linq;

public static class StaticStore
{
    private static LevelLoadType _loadLevelType;
    
    public static void HomeOnLevelLoad(LevelLoadType type)
    {
        _loadLevelType = type;
    }
    
    public static LevelLoadType GetLevelLoadType()
    {
        return _loadLevelType;
    }
    
    // Для иерархии сцен
    private static List<int> _sceneIdHistory = new List<int>();

    public static void AddScene(int sceneId)
    {
        _sceneIdHistory.Add(sceneId);
    }

    public static int GetLastSceneId()
    {
        return _sceneIdHistory.Last();
    }

    public static int PopSceneId()
    {
        var result = _sceneIdHistory.Last();
        _sceneIdHistory.Remove(_sceneIdHistory.Count - 1);
        return result;
    }
}

public enum LevelLoadType
{
    NewLevel,
    Continue
}