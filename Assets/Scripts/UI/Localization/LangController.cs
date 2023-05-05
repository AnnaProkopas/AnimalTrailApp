
using UnityEngine;

public class LangController : MonoBehaviour
{
    public void SetLanguage(int language)
    {
        LangManager.SelectedLanguage = (Languages)language;
    }
}
