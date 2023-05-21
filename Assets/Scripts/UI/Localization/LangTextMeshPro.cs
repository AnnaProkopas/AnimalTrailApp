using TMPro;
using UnityEngine;

public class LangTextMeshPro : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    
    [SerializeField] protected string key;

    private void Start()
    {
        Localize();
        LangManager.OnLanguageChange += OnLanguageChange;
    }

    protected void Localize()
    {
        text.text = LangManager.GetTranslate(key);
    }

    private void OnLanguageChange()
    {
        Localize();
    }
}