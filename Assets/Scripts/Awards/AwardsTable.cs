using System;
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
        [SerializeField] private GameObject openedBackButton;
        
        [SerializeField] private GameObject mainButtonPanels;
        
        [SerializeField] private GameObject rootGameObject;
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
            mainButtonPanels.SetActive(false);
            
            openedAwardSprite.GetComponent<Image>().sprite = sprite;
            openedAwardSprite.SetActive(true);
            openedAwardText.GetComponentInChildren<TextMeshProUGUI>().text = text;
            openedAwardText.SetActive(true);
            openedBackButton.SetActive(true);
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(openedAwardText.GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate(rootGameObject.GetComponent<RectTransform>());
        }

        public void OnCloseAward()
        {
            openedAwardSprite.SetActive(false);
            openedAwardText.gameObject.SetActive(false);
            openedBackButton.SetActive(false);
            
            gameObject.SetActive(true);
            mainButtonPanels.SetActive(true);
        }
    }
}