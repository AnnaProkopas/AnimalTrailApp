using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Awards
{
    public class AwardsTable : MonoBehaviour
    {
        [SerializeField] private GameObject openedAwardSprite;
        [SerializeField] private GameObject openedAwardText;
        private void Awake()
        {
            Dictionary<AwardType, StoredAward> awards = AwardsStorage.Load().ToDictionary(x => x.type, x => x);
            
            AwardCell[] children = GetComponentsInChildren<AwardCell>();

            foreach (var cell in children)
            {
                if (awards.ContainsKey(cell.type))
                {
                    cell.SetEnabled(awards[cell.type].count);
                }
                else
                {
                    cell.SetDisabled();
                }

                cell.openAward += OpenAward;
            }
        }

        void OpenAward(Sprite sprite, string text)
        {
            gameObject.SetActive(false);
            
            openedAwardSprite.GetComponent<Image>().sprite = sprite;
            openedAwardSprite.SetActive(true);
            
            openedAwardText.GetComponentInChildren<TextMeshProUGUI>().text = text;
            // var verticalLayoutGroup = openedAwardText.GetComponent<VerticalLayoutGroup>();
            openedAwardText.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(openedAwardText.GetComponent<RectTransform>());
            // openedAwardText.GetComponent<>().ForceRebuildLayoutImmediate;
        }

        public void OnCloseAward()
        {
            openedAwardSprite.SetActive(false);
            openedAwardText.gameObject.SetActive(false);
            
            gameObject.SetActive(true);
        }
    }
}