using PlayerModule;
using TMPro;
using UnityEngine;

namespace UI.HighScore
{
    public class HighScoreTable : MonoBehaviour
    {
        [SerializeField] private Transform contentContainer;
        private Transform entryLastHighScoreRow;
        private Transform entryTemplate;
    
        private void Awake()
        {
            entryTemplate = contentContainer.Find("highScoreEntryTemplate");
            entryLastHighScoreRow = contentContainer.Find("highScoreLastRow");
        
            entryTemplate.gameObject.SetActive(false);

            SetItemData(entryLastHighScoreRow, PlayerRatingService.GetLastScoreRecord());

            foreach (ScoreRecord score in PlayerRatingService.GetTopTenHighestScores())
            {
                AddItem(score);
            }
        }
    
        private void AddItem(ScoreRecord scoreItem)
        {
            Transform entryTransform = Instantiate(entryTemplate, contentContainer);
            SetItemData(entryTransform, scoreItem);
            entryTransform.transform.localScale = Vector2.one;
            entryTransform.gameObject.SetActive(true);
        }

        private void SetItemData(Transform entryTransform, ScoreRecord scoreItem)
        {
            entryTransform.Find("levelTitle").GetComponent<TextMeshProUGUI>().text = (scoreItem.level + 1).ToString();
            entryTransform.Find("foodsTitle").GetComponent<TextMeshProUGUI>().text = scoreItem.value.ToString();
            entryTransform.Find("humansTitle").GetComponent<TextMeshProUGUI>().text = scoreItem.humans.ToString();
            entryTransform.Find("dateTitle").GetComponent<TextMeshProUGUI>().text = scoreItem.GetDateTime().ToString("dd.MM");
        }
    }
}