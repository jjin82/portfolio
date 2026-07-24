using GameData;
using System;
using System.Collections.Generic;


public class T_CharacterData : IRow
{
    #region Record Info
    public enum Records
    {
        tid,
        Enable,
        CharType,
        Name,
    }
    #endregion

    public virtual int TID => _tid;
    public virtual bool Enable => stringTable[_Enable] == "TRUE" ? true : false;
    public virtual CHAR_TYPE CharType =>  (CHAR_TYPE)System.Enum.Parse(typeof(CHAR_TYPE),stringTable[_CharType]);
    public virtual string Name => stringTable[_Name];

    #region Repositories
    private int _tid;
    private int _Enable;
    private int _CharType;
    private int _Name;
    #endregion

#pragma warning disable 649
#pragma warning disable 169
    private static string[] stringTable;
#pragma warning restore 169
#pragma warning restore 649

    public T_CharacterData(){}
    public T_CharacterData(int[] intTable, float[] floatTable, Int64[] int64Table)
    {
        #region Constructor
        _tid = intTable[0];
        _Enable = intTable[1];
        _CharType = intTable[2];
        _Name = intTable[3];
        #endregion
    }

    public static T_CharacterData Get(int tid) { return Excel.GetRow(SheetName.T_CharacterData, (int)tid) as T_CharacterData; }
    public static List<int> GetTIDs()
    {
        List<int> list = new List<int>();
        foreach(var iter in Excel.GetKeyList(SheetName.T_CharacterData))
        {
             list.Add((int)iter);
        }
        return list;
    }
    
    public static List<T_CharacterData> GetAll()
    {        
        return  Excel.GetList<T_CharacterData>(SheetName.T_CharacterData);
    }    
	
	public static T_CharacterData GetRandom()
    {        
		return Excel.GetRandom<T_CharacterData>(SheetName.T_CharacterData);
    }    
}
