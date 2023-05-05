using System.Collections.Generic;
using System.Linq;

public class CrossSceneStaticContainer
{
    private static List<int> SceneIdHistory = new List<int>();

    public static void addScene(int sceneId)
    {
        SceneIdHistory.Add(sceneId);
    }

    public static int getLastSceneId()
    {
        return SceneIdHistory.Last();
    }

    public static int popSceneId()
    {
        var result = SceneIdHistory.Last();
        SceneIdHistory.Remove(SceneIdHistory.Count - 1);
        return result;
    }
}