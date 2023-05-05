using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LangTextMeshPro : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    
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