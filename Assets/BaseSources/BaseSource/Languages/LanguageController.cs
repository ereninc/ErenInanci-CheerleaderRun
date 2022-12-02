//using Newtonsoft.Json;
using UnityEngine;

public class LanguageController : ControllerBaseModel
{
    private static LanguageModel language;

    public override void Initialize()
    {
        base.Initialize();
        DontDestroyOnLoad(this.gameObject);

        Languages Language = Languages.eng;

        switch (Application.systemLanguage)
        {
            case SystemLanguage.Arabic:
                Language = Languages.ara;
                break;
            case SystemLanguage.Chinese:
                Language = Languages.chi;
                break;
            case SystemLanguage.English:
                Language = Languages.eng;
                break;
            case SystemLanguage.French:
                Language = Languages.fre;
                break;
            case SystemLanguage.German:
                Language = Languages.ger;
                break;
            case SystemLanguage.Italian:
                Language = Languages.ita;
                break;
            case SystemLanguage.Japanese:
                Language = Languages.jpn;
                break;
            case SystemLanguage.Korean:
                Language = Languages.kor;
                break;
            case SystemLanguage.Russian:
                Language = Languages.rus;
                break;
            case SystemLanguage.ChineseSimplified:
                Language = Languages.chi;
                break;
            case SystemLanguage.ChineseTraditional:
                Language = Languages.chi;
                break;
            case SystemLanguage.Portuguese:
                Language = Languages.por;
                break;
            default:
                Language = Languages.eng;
                break;
        }

        string strValues = Resources.Load<TextAsset>("Languages/" + Language).text;
        //language = JsonConvert.DeserializeObject<LanguageModel>(strValues);
    }

    public static string Get(string key)
    {
        if (language == null)
        {
            return key;
        }
        else
        {
            return language.Words.Find(x => x.Key == key).Value;
        }
    }

    public static string Get(string key, int number)
    {
        if (language == null)
        {
            return key;
        }
        else
        {
            return language.Words.Find(x => x.Key == key).Value.Replace("(NO)", number.ToString());
        }
    }
    public static string Get(string key, float number)
    {
        if (language == null)
        {
            return key;
        }
        else
        {
            return language.Words.Find(x => x.Key == key).Value.Replace("(NO)", number.ToString());
        }
    }

    public static string Get(string key, long number)
    {
        if (language == null)
        {
            return key;
        }
        else
        {
            return language.Words.Find(x => x.Key == key).Value.Replace("(NO)", number.ToString());
        }
    }
}