

using UnityEngine;

public class LangSwitchedText : LangTextMeshPro
{
    [SerializeField] private string enableKey;
    [SerializeField] private string disableKey;

    public void ChangeText(bool isEnable)
    {
        if (isEnable)
            key = enableKey;
        else
            key = disableKey;
        Localize();
    }
}