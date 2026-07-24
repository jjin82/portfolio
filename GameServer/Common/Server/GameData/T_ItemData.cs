using GameData;
using System;
using System.Collections.Generic;


public class T_ItemData : IRow
{
    #region Record Info
    public enum Records
    {
        tid,
        Name,
        Desc,
        SkillTID,
        Image,
        Prefab,
    }
    #endregion

    public virtual int TID => _tid;
    public virtual SKILL_TYPE SkillTID =>  (SKILL_TYPE)System.Enum.Parse(typeof(SKILL_TYPE),stringTable[_SkillTID]);

    #region Repositories
    private int _tid;
    private int _SkillTID;
    #endregion

#pragma warning disable 649
#pragma warning disable 169
    private static string[] stringTable;
#pragma warning restore 169
#pragma warning restore 649

    public T_ItemData(){}
    public T_ItemData(int[] intTable, float[] floatTable, Int64[] int64Table)
    {
        #region Constructor
        _tid = intTable[0];
        _SkillTID = intTable[1];
        #endregion
    }

    public static T_ItemData Get(int tid) { return Excel.GetRow(SheetName.T_ItemData, (int)tid) as T_ItemData; }
    public static List<int> GetTIDs()
    {
        List<int> list = new List<int>();
        foreach(var iter in Excel.GetKeyList(SheetName.T_ItemData))
        {
             list.Add((int)iter);
        }
        return list;
    }
    
    public static List<T_ItemData> GetAll()
    {        
        return  Excel.GetList<T_ItemData>(SheetName.T_ItemData);
    }    
	
	public static T_ItemData GetRandom()
    {        
		return Excel.GetRandom<T_ItemData>(SheetName.T_ItemData);
    }    
}
