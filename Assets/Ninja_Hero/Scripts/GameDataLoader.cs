using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Globalization;
using System.Runtime.InteropServices;
using SBS.XML;

public class GameDataLoader : MonoBehaviour
{
    protected static GameDataLoader instance = null;
    protected string localeToUse = "en";
    protected Dictionary<string, Dictionary<string, string>> strings = new Dictionary<string, Dictionary<string, string>>();
    public TextAsset localization;
    private bool isInited = false; // 是否已经初始化

    public string LocaleToUse
    {
        get
        {
            return localeToUse;
        }

    }

    public static GameDataLoader Instance
    {
        get
        {
            if (null == instance)
            {
                instance = new GameDataLoader();
            }

            instance.initDataLoader();
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    
    private void initDataLoader()
    {
        if (!isInited)
        {
            isInited = true;
            this.ParseLocalizationXML(localization.text);
        }
    }


    public void ParseLocalizationXML(string localeText)
    {
        XMLReader xmlReader = new XMLReader();
        XMLNode root = xmlReader.read(localeText).children[0] as XMLNode;        

        foreach (XMLNode record in root.children)
        {
            Dictionary<string, string> items = new Dictionary<string, string>();
            foreach (XMLNode item in record.children)
            {
                items.Add(item.tagName, item.cdata);
            }
            strings.Add(record.attributes["id"], items);
        }

//#if UNITY_IPHONE && !UNITY_EDITOR
//        localeToUse = _getLocale();
//#else
        localeToUse = getLocale();
//#endif
    
    }


    private string getLocale()
    {
        string locale = "en";
        switch (Application.systemLanguage)
        {
            case SystemLanguage.ChineseTraditional:
            case SystemLanguage.ChineseSimplified:
            case SystemLanguage.Chinese:
                locale = "zh";
                break;
            case SystemLanguage.English:
                locale = "en";
                break;          
        }

        return locale;
    }

    public string GetLocaleStringWithLocale(string locale, string str)
    {
        Dictionary<string, string> hash;
        string strOut;

        if (!strings.TryGetValue(locale, out hash))
            hash = strings["en"];
        
        if (hash.TryGetValue(str, out strOut))
            return strOut;

        // 查找失败再去默认语言包里查找，默认语言包为en
        if (hash != strings["en"])
        {
            if (strings["en"].TryGetValue(str, out strOut))
                return strOut;
        }

        return str; 
    }

    public string GetLocaleString(string str)
    {  
        return this.GetLocaleStringWithLocale(localeToUse, str);
    }

    public void Awake()
    {
        GameDataLoader.Instance = this;
       // String ok = GameDataLoader.Instance.GetLocaleString("ok"); // 测试
        DontDestroyOnLoad(gameObject);
    }


}
