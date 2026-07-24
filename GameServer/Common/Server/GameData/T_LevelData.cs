using GameData;
using System;
using System.Collections.Generic;


public class T_LevelData : IRow
{
    #region Record Info
    public enum Records
    {
        tid,
        nextExp,
    }
    #endregion

    public virtual int TID => _tid;
    public virtual int nextExp => _nextExp;

    #region Repositories
    private int _tid;
    private int _nextExp;
    #endregion

#pragma warning disable 649
#pragma warning disable 169
    private static string[] stringTable;
#pragma warning restore 169
#pragma warning restore 649

    public T_LevelData(){}
    public T_LevelData(int[] intTable, float[] floatTable, Int64[] int64Table)
    {
        #region Constructor
        _tid = intTable[0];
        _nextExp = intTable[1];
        #endregion
    }

    public static T_LevelData Get(int tid) { return Excel.GetRow(SheetName.T_LevelData, (int)tid) as T_LevelData; }
    public static List<int> GetTIDs()
    {
        List<int> list = new List<int>();
        foreach(var iter in Excel.GetKeyList(SheetName.T_LevelData))
        {
             list.Add((int)iter);
        }
        return list;
    }
    
    public static List<T_LevelData> GetAll()
    {        
        return  Excel.GetList<T_LevelData>(SheetName.T_LevelData);
    }    
	
	public static T_LevelData GetRandom()
    {        
		return Excel.GetRandom<T_LevelData>(SheetName.T_LevelData);
    }    
}
