using System.Collections;
using System.Collections.Generic;

public partial class StringManager : BaseManager<StringManager>
{
    public override void OnUpdate()
    {

    }
}

public partial class StringManager : BaseManager<StringManager>
{
    public override bool Initialize()
    {
        var newDicString = new Dictionary<string, string>();

        foreach (var e in T_StringData.GetAll())
        {
            switch(T_GlobalValueData.Get(GLOBAL_VALUE_TYPE.LANGUAGE).ValueString)
            {
                case "KR":
                    {
                        newDicString.Add(e.Key, e.KO.Replace("\\n", "\n"));

                        s_defaultLanguage = LANGUAGE_TYPE.KO;
                    }
                    break;
                default:
                    {
                        newDicString.Add(e.Key, e.EN.Replace("\\n", "\n"));

                        s_defaultLanguage = LANGUAGE_TYPE.EN;
                    }
                    break;
            }

            if (e.Key.Contains("REPEAT_REQUEST_CHARACTER_TALK"))
            {
                repeatRequestString.Add(e.Key);
            }
        }

        // 정렬.
        repeatRequestString.Sort();

        // 세팅.
        dicString = newDicString;

        return true;
    }

    public string? GetString(string key)
    {
        if(dicString.TryGetValue(key, out string value) == false)
        {
            return "";
        }

        return value;
    }

    public static LANGUAGE_TYPE Language()
    {
        return s_defaultLanguage;
    }

    private Dictionary<string, string> dicString            = new Dictionary<string, string>();
    private List<string>               repeatRequestString  = new List<string>();
    

    private static LANGUAGE_TYPE s_defaultLanguage = LANGUAGE_TYPE.KO;
}
