using GameData;
using System;
using System.Collections.Generic;


public class T_RewardData : IRow
{
    #region Record Info
    public enum Records
    {
        tid,
        Name,
        Image,
        Prefab,
        CurrencyType,
        CurrencyCount,
        CurrencyBonus,
        CurrencyPrefab,
        ItemTid,
        ItemCount,
        Explanation,
    }
    #endregion

    public virtual REWARD_TYPE TID => (REWARD_TYPE)_tid;
    public virtual List<CURRENCY_TYPE> CurrencyType => _CurrencyType;
    public virtual CURRENCY_TYPE RandomCurrencyType() {return 0 == (CurrencyType?.Count??0) ? CURRENCY_TYPE.NONE : CurrencyType?[new Random().Next(CurrencyType.Count)]??CURRENCY_TYPE.NONE;}
    public virtual List<int> CurrencyCount => _CurrencyCount;
    public virtual int RandomCurrencyCount() {return 0 == (CurrencyCount?.Count??0) ? 0 : CurrencyCount?[new Random().Next(CurrencyCount.Count)]??0;}
    public virtual List<int> CurrencyBonus => _CurrencyBonus;
    public virtual int RandomCurrencyBonus() {return 0 == (CurrencyBonus?.Count??0) ? 0 : CurrencyBonus?[new Random().Next(CurrencyBonus.Count)]??0;}
    public virtual List<int> ItemTid => _ItemTid;
    public virtual int RandomItemTid() {return 0 == (ItemTid?.Count??0) ? 0 : ItemTid?[new Random().Next(ItemTid.Count)]??0;}
    public virtual List<int> ItemCount => _ItemCount;
    public virtual int RandomItemCount() {return 0 == (ItemCount?.Count??0) ? 0 : ItemCount?[new Random().Next(ItemCount.Count)]??0;}

    #region Repositories
    private int _tid;
    public List<CURRENCY_TYPE> _CurrencyType = new List<CURRENCY_TYPE>();
    public List<int> _CurrencyCount = new List<int>();
    public List<int> _CurrencyBonus = new List<int>();
    public List<int> _ItemTid = new List<int>();
    public List<int> _ItemCount = new List<int>();
    #endregion

#pragma warning disable 649
#pragma warning disable 169
    private static string[] stringTable;
#pragma warning restore 169
#pragma warning restore 649

    public T_RewardData(){}
    public T_RewardData(int[] intTable, float[] floatTable, Int64[] int64Table)
    {
        #region Constructor
        _tid = intTable[0];
        //
        var tempCurrencyType = intTable[1];
        if(string.IsNullOrEmpty( stringTable[tempCurrencyType]) == false )
        {
            string[] arrayCurrencyType = stringTable[tempCurrencyType].Trim().Split(',');
            foreach (var iter in arrayCurrencyType)
            {
               string name = iter;
               name = name.Replace("[", "");
               name = name.Replace("]", "").Trim();
               if(string.IsNullOrEmpty(name) == false)
               {
                   CurrencyType.Add((CURRENCY_TYPE)System.Enum.Parse(typeof(CURRENCY_TYPE),name));
               }
            }
        }
        //
        //
        var tempCurrencyCount = intTable[2];
        if(string.IsNullOrEmpty( stringTable[tempCurrencyCount]) == false )
        {
            string[] arrayCurrencyCount = stringTable[tempCurrencyCount].Trim().Split(',');
            foreach (var iter in arrayCurrencyCount)
            {
               string name = iter;
               name = name.Replace("[", "");
               name = name.Replace("]", "").Trim();
               if(string.IsNullOrEmpty(name) == false)
                   CurrencyCount.Add(int.Parse(name));
            }
        }
        //
        //
        var tempCurrencyBonus = intTable[3];
        if(string.IsNullOrEmpty( stringTable[tempCurrencyBonus]) == false )
        {
            string[] arrayCurrencyBonus = stringTable[tempCurrencyBonus].Trim().Split(',');
            foreach (var iter in arrayCurrencyBonus)
            {
               string name = iter;
               name = name.Replace("[", "");
               name = name.Replace("]", "").Trim();
               if(string.IsNullOrEmpty(name) == false)
                   CurrencyBonus.Add(int.Parse(name));
            }
        }
        //
        //
        var tempItemTid = intTable[4];
        if(string.IsNullOrEmpty( stringTable[tempItemTid]) == false )
        {
            string[] arrayItemTid = stringTable[tempItemTid].Trim().Split(',');
            foreach (var iter in arrayItemTid)
            {
               string name = iter;
               name = name.Replace("[", "");
               name = name.Replace("]", "").Trim();
               if(string.IsNullOrEmpty(name) == false)
                   ItemTid.Add(int.Parse(name));
            }
        }
        //
        //
        var tempItemCount = intTable[5];
        if(string.IsNullOrEmpty( stringTable[tempItemCount]) == false )
        {
            string[] arrayItemCount = stringTable[tempItemCount].Trim().Split(',');
            foreach (var iter in arrayItemCount)
            {
               string name = iter;
               name = name.Replace("[", "");
               name = name.Replace("]", "").Trim();
               if(string.IsNullOrEmpty(name) == false)
                   ItemCount.Add(int.Parse(name));
            }
        }
        //
        #endregion
    }

    public static T_RewardData Get(REWARD_TYPE tid) { return Excel.GetRow(SheetName.T_RewardData, (int)tid) as T_RewardData; }
    public static List<REWARD_TYPE> GetTIDs()
    {
        List<REWARD_TYPE> list = new List<REWARD_TYPE>();
        foreach(var iter in Excel.GetKeyList(SheetName.T_RewardData))
        {
             list.Add((REWARD_TYPE)iter);
        }
        return list;
    }
    
    public static List<T_RewardData> GetAll()
    {        
        return  Excel.GetList<T_RewardData>(SheetName.T_RewardData);
    }    
	
	public static T_RewardData GetRandom()
    {        
		return Excel.GetRandom<T_RewardData>(SheetName.T_RewardData);
    }    
}
