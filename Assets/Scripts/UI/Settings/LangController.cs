using UI.Localization;
using UnityEngine;

namespace UI.Settings
{
    public class LangController : MonoBehaviour
    {
        public void SetLanguage(int language)
        {
            LangManager.SelectedLanguage = (Languages)language;
        }
    }
}
