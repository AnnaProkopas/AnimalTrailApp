using UnityEngine;
using UnityEngine.UI;

namespace UI.Localization
{
    public class LangText : MonoBehaviour
    {
        [SerializeField]
        private Text text;
    
        [SerializeField]
        private string key;

        private void Start()
        {
            Localize();
            LangManager.OnLanguageChange += OnLanguageChange;
        }

        private void Localize()
        {
            text.text = LangManager.GetTranslate(key);
        }

        private void OnLanguageChange()
        {
            Localize();
        }
    }
}