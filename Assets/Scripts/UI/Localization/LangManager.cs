using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class LangManager : MonoBehaviour
{
    public static LangManager instance;

    private static Languages selectedLanguage = Languages.RU;

    public static Languages SelectedLanguage
    {
        set
        {
            switch (value)
            {
                case Languages.RU:
                    PlayerPrefs.SetString("Language", "ru");
                    selectedLanguage = Languages.RU;
                    break;
                default:
                    PlayerPrefs.SetString("Language", "en-us");
                    selectedLanguage = Languages.EN;
                    break;
            }
            OnLanguageChange?.Invoke();
        }
        get { return selectedLanguage;  }
    }

    public static event LanguageChangeHandler OnLanguageChange;
    
    [SerializeField] public TextAsset textAsset;
    
    public delegate void LanguageChangeHandler();

    private static Dictionary<string, List<string>> _langDictionary;

    private void Awake()
    {
        instance = this;
        
        if (!PlayerPrefs.HasKey("Language"))
        {
            if (Application.systemLanguage == SystemLanguage.Russian)
            {
                PlayerPrefs.SetString("Language", "ru");
            }
            else
            {
                PlayerPrefs.SetString("Language", "en-us");
            }
        }
        
        LoadLang();
    }
    
    void Start()
    {
        LoadLang();
    }
    
    private static void LoadLang()
    {
        _langDictionary = new Dictionary<string, List<string>>();
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(instance.textAsset.text);

        // Debug.Log(xmlDocument["Records"]);
        foreach (XmlNode record in xmlDocument["Records"].ChildNodes)
        {
            string keyStr = record.Attributes["Name"].Value;

            var values = new List<string>();
            foreach (XmlNode translate in record["Translations"])
            {
                values.Add(translate.InnerText);
            }

            _langDictionary[keyStr] = values;
        }
    }

    public static string GetTranslate(string key)
    {
        if (_langDictionary == null)
        {
            LoadLang();
        }
        
        if (_langDictionary.ContainsKey(key))
        {
            return _langDictionary[key][(int)selectedLanguage];
        }

        return key;
    }
}

public enum Languages
{
    ANY = -1,
    EN = 0,
    RU = 1,
}