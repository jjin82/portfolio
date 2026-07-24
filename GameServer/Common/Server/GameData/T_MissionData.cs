using GameData;
using System;
using System.Collections.Generic;


public class T_MissionData : IRow
{
    #region Record Info
    public enum Records
    {
        tid,
        Enable,
        Type,
        CharTID,
        Order,
        ResetType,
        NaviType,
        RewardType,
        Condition,
        Count,
        Title,
        Desc,
    }
    #endregion

    public virtual int TID => _tid;
    public virtual bool Enable => stringTable[_Enable] == "TRUE" ? true : false;
    public virtual MISSION_TYPE Type =>  (MISSION_TYPE)System.Enum.Parse(typeof(MISSION_TYPE),stringTable[_Type]);
    public virtual int CharTID => _CharTID;
    public virtual int Order => _Order;
    public virtual MISSION_RESET_TYPE ResetType =>  (MISSION_RESET_TYPE)System.Enum.Parse(typeof(MISSION_RESET_TYPE),stringTable[_ResetType]);
    public virtual NAVI_TYPE NaviType =>  (NAVI_TYPE)System.Enum.Parse(typeof(NAVI_TYPE),stringTable[_NaviType]);
    public virtual REWARD_TYPE RewardType =>  (REWARD_TYPE)System.Enum.Parse(typeof(REWARD_TYPE),stringTable[_RewardType]);
    public virtual List<int> Condition => _Condition;
    public virtual int RandomCondition() {return 0 == (Condition?.Count??0) ? 0 : Condition?[new Random().Next(Condition.Count)]??0;}
    public virtual int Count => _Count;
    public virtual string Title => stringTable[_Title];
    public virtual string Desc => stringTable[_Desc];

    #region Repositories
    private int _tid;
    private int _Enable;
    private int _Type;
    private int _CharTID;
    private int _Order;
    private int _ResetType;
    private int _NaviType;
    private int _RewardType;
    public List<int> _Condition = new List<int>();
    private int _Count;
    private int _Title;
    private int _Desc;
    #endregion

#pragma warning disable 649
#pragma warning disable 169
    private static string[] stringTable;
#pragma warning restore 169
#pragma warning restore 649

    public T_MissionData(){}
    public T_MissionData(int[] intTable, float[] floatTable, Int64[] int64Table)
    {
        #region Constructor
        _tid = intTable[0];
        _Enable = intTable[1];
        _Type = intTable[2];
        _CharTID = intTable[3];
        _Order = intTable[4];
        _ResetType = intTable[5];
        _NaviType = intTable[6];
        _RewardType = intTable[7];
        //
        var tempCondition = intTable[8];
        if(string.IsNullOrEmpty( stringTable[tempCondition]) == false )
        {
            string[] arrayCondition = stringTable[tempCondition].Trim().Split(',');
            foreach (var iter in arrayCondition)
            {
               string name = iter;
               name = name.Replace("[", "");
               name = name.Replace("]", "").Trim();
               if(string.IsNullOrEmpty(name) == false)
                   Condition.Add(int.Parse(name));
            }
        }
        //
        _Count = intTable[9];
        _Title = intTable[10];
        _Desc = intTable[11];
        #endregion
    }

    public static T_MissionData Get(int tid) { return Excel.GetRow(SheetName.T_MissionData, (int)tid) as T_MissionData; }
    public static List<int> GetTIDs()
    {
        List<int> list = new List<int>();
        foreach(var iter in Excel.GetKeyList(SheetName.T_MissionData))
        {
             list.Add((int)iter);
        }
        return list;
    }
    
    public static List<T_MissionData> GetAll()
    {        
        return  Excel.GetList<T_MissionData>(SheetName.T_MissionData);
    }    
	
	public static T_MissionData GetRandom()
    {        
		return Excel.GetRandom<T_MissionData>(SheetName.T_MissionData);
    }    
}
