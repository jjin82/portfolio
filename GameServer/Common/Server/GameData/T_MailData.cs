using GameData;
using System;
using System.Collections.Generic;


public class T_MailData : IRow
{
    #region Record Info
    public enum Records
    {
        tid,
        Title,
        Content,
        PushMessageTid,
        RewardType,
        ItemTIDs,
        ItemCounts,
    }
    #endregion

    public virtual int TID => _tid;
    public virtual string Title => stringTable[_Title];
    public virtual string Content => stringTable[_Content];
    public virtual PUSH_MESSAGE PushMessageTid =>  (PUSH_MESSAGE)System.Enum.Parse(typeof(PUSH_MESSAGE),stringTable[_PushMessageTid]);
    public virtual REWARD_TYPE RewardType =>  (REWARD_TYPE)System.Enum.Parse(typeof(REWARD_TYPE),stringTable[_RewardType]);
    public virtual List<int> ItemTIDs => _ItemTIDs;
    public virtual int RandomItemTIDs() {return 0 == (ItemTIDs?.Count??0) ? 0 : ItemTIDs?[new Random().Next(ItemTIDs.Count)]??0;}
    public virtual List<int> ItemCounts => _ItemCounts;
    public virtual int RandomItemCounts() {return 0 == (ItemCounts?.Count??0) ? 0 : ItemCounts?[new Random().Next(ItemCounts.Count)]??0;}

    #region Repositories
    private int _tid;
    private int _Title;
    private int _Content;
    private int _PushMessageTid;
    private int _RewardType;
    public List<int> _ItemTIDs = new List<int>();
    public List<int> _ItemCounts = new List<int>();
    #endregion

#pragma warning disable 649
#pragma warning disable 169
    private static string[] stringTable;
#pragma warning restore 169
#pragma warning restore 649

    public T_MailData(){}
    public T_MailData(int[] intTable, float[] floatTable, Int64[] int64Table)
    {
        #region Constructor
        _tid = intTable[0];
        _Title = intTable[1];
        _Content = intTable[2];
        _PushMessageTid = intTable[3];
        _RewardType = intTable[4];
        //
        var tempItemTIDs = intTable[5];
        if(string.IsNullOrEmpty( stringTable[tempItemTIDs]) == false )
        {
            string[] arrayItemTIDs = stringTable[tempItemTIDs].Trim().Split(',');
            foreach (var iter in arrayItemTIDs)
            {
               string name = iter;
               name = name.Replace("[", "");
               name = name.Replace("]", "").Trim();
               if(string.IsNullOrEmpty(name) == false)
                   ItemTIDs.Add(int.Parse(name));
            }
        }
        //
        //
        var tempItemCounts = intTable[6];
        if(string.IsNullOrEmpty( stringTable[tempItemCounts]) == false )
        {
            string[] arrayItemCounts = stringTable[tempItemCounts].Trim().Split(',');
            foreach (var iter in arrayItemCounts)
            {
               string name = iter;
               name = name.Replace("[", "");
               name = name.Replace("]", "").Trim();
               if(string.IsNullOrEmpty(name) == false)
                   ItemCounts.Add(int.Parse(name));
            }
        }
        //
        #endregion
    }

    public static T_MailData Get(int tid) { return Excel.GetRow(SheetName.T_MailData, (int)tid) as T_MailData; }
    public static List<int> GetTIDs()
    {
        List<int> list = new List<int>();
        foreach(var iter in Excel.GetKeyList(SheetName.T_MailData))
        {
             list.Add((int)iter);
        }
        return list;
    }
    
    public static List<T_MailData> GetAll()
    {        
        return  Excel.GetList<T_MailData>(SheetName.T_MailData);
    }    
	
	public static T_MailData GetRandom()
    {        
		return Excel.GetRandom<T_MailData>(SheetName.T_MailData);
    }    
}
