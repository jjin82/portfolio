using GameData;
using System;
using System.Collections.Generic;


public class T_AdData : IRow
{
    #region Record Info
    public enum Records
    {
        tid,
        DailyAdCount,
        RewardValue,
        RewardCount,
        Image,
        adID_aos,
        adID_ios,
    }
    #endregion

    public virtual AD_TYPE TID => (AD_TYPE)_tid;
    public virtual int DailyAdCount => _DailyAdCount;
    public virtual List<int> RewardValue => _RewardValue;
    public virtual int RandomRewardValue() {return 0 == (RewardValue?.Count??0) ? 0 : RewardValue?[new Random().Next(RewardValue.Count)]??0;}
    public virtual int RewardCount => _RewardCount;
    public virtual string adID_aos => stringTable[_adID_aos];
    public virtual string adID_ios => stringTable[_adID_ios];

    #region Repositories
    private int _tid;
    private int _DailyAdCount;
    public List<int> _RewardValue = new List<int>();
    private int _RewardCount;
    private int _adID_aos;
    private int _adID_ios;
    #endregion

#pragma warning disable 649
#pragma warning disable 169
    private static string[] stringTable;
#pragma warning restore 169
#pragma warning restore 649

    public T_AdData(){}
    public T_AdData(int[] intTable, float[] floatTable, Int64[] int64Table)
    {
        #region Constructor
        _tid = intTable[0];
        _DailyAdCount = intTable[1];
        //
        var tempRewardValue = intTable[2];
        if(string.IsNullOrEmpty( stringTable[tempRewardValue]) == false )
        {
            string[] arrayRewardValue = stringTable[tempRewardValue].Trim().Split(',');
            foreach (var iter in arrayRewardValue)
            {
               string name = iter;
               name = name.Replace("[", "");
               name = name.Replace("]", "").Trim();
               if(string.IsNullOrEmpty(name) == false)
                   RewardValue.Add(int.Parse(name));
            }
        }
        //
        _RewardCount = intTable[3];
        _adID_aos = intTable[4];
        _adID_ios = intTable[5];
        #endregion
    }

    public static T_AdData Get(AD_TYPE tid) { return Excel.GetRow(SheetName.T_AdData, (int)tid) as T_AdData; }
    public static List<AD_TYPE> GetTIDs()
    {
        List<AD_TYPE> list = new List<AD_TYPE>();
        foreach(var iter in Excel.GetKeyList(SheetName.T_AdData))
        {
             list.Add((AD_TYPE)iter);
        }
        return list;
    }
    
    public static List<T_AdData> GetAll()
    {        
        return  Excel.GetList<T_AdData>(SheetName.T_AdData);
    }    
	
	public static T_AdData GetRandom()
    {        
		return Excel.GetRandom<T_AdData>(SheetName.T_AdData);
    }    
}
