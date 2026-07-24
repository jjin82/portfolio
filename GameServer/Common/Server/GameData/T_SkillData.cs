using GameData;
using System;
using System.Collections.Generic;


public class T_SkillData : IRow
{
    #region Record Info
    public enum Records
    {
        tid,
        SpawnItemTID,
        Name,
        CoolTime,
        Value,
        Prefab,
        Image,
    }
    #endregion

    public virtual SKILL_TYPE TID => (SKILL_TYPE)_tid;
    public virtual int SpawnItemTID => _SpawnItemTID;
    public virtual int CoolTime => _CoolTime;
    public virtual int Value => _Value;

    #region Repositories
    private int _tid;
    private int _SpawnItemTID;
    private int _CoolTime;
    private int _Value;
    #endregion

#pragma warning disable 649
#pragma warning disable 169
    private static string[] stringTable;
#pragma warning restore 169
#pragma warning restore 649

    public T_SkillData(){}
    public T_SkillData(int[] intTable, float[] floatTable, Int64[] int64Table)
    {
        #region Constructor
        _tid = intTable[0];
        _SpawnItemTID = intTable[1];
        _CoolTime = intTable[2];
        _Value = intTable[3];
        #endregion
    }

    public static T_SkillData Get(SKILL_TYPE tid) { return Excel.GetRow(SheetName.T_SkillData, (int)tid) as T_SkillData; }
    public static List<SKILL_TYPE> GetTIDs()
    {
        List<SKILL_TYPE> list = new List<SKILL_TYPE>();
        foreach(var iter in Excel.GetKeyList(SheetName.T_SkillData))
        {
             list.Add((SKILL_TYPE)iter);
        }
        return list;
    }
    
    public static List<T_SkillData> GetAll()
    {        
        return  Excel.GetList<T_SkillData>(SheetName.T_SkillData);
    }    
	
	public static T_SkillData GetRandom()
    {        
		return Excel.GetRandom<T_SkillData>(SheetName.T_SkillData);
    }    
}
