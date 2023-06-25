using System;
using UI.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace Awards
{
    public class AwardCell: MonoBehaviour
    {
        public delegate void OpenAward(Sprite image, string text);
        
        [SerializeField] public AwardType type;
        [SerializeField] private Sprite emptySprite;
        [SerializeField] private Sprite disabledSprite;
        [SerializeField] private Sprite mainSprite;

        public OpenAward openAward;
        
        private int count = 0;

        private void OnEnable()
        {
            gameObject.GetComponent<Button>().onClick.AddListener(() => OnClick());
        }

        public void SetEnabled(int count)
        {
            GetComponent<Image>().sprite = mainSprite;
            this.count = count > 0 ? count : 1;
        }
        
        public void SetDisabled()
        {
            GetComponent<Image>().sprite = emptySprite;
        }

        public bool isEnabled()
        {
            return count > 0;
        }

        public void OnClick()
        {
            if (count > 0)
            {
                string text = LangManager.GetTranslate("Awards_history_" + Enum.GetName(typeof(AwardType), type) + "_key");
                openAward.Invoke(mainSprite, text);
            }
            else
            {
                string text = LangManager.GetTranslate("Awards_hint_" + Enum.GetName(typeof(AwardType), type) + "_key");
                openAward.Invoke(disabledSprite, text);
            }
        }
    }
}