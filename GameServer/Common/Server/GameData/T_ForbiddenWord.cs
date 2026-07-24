using GameData;
using System;
using System.Collections.Generic;


public class T_ForbiddenWord : IRow
{
    #region Record Info
    public enum Records
    {
        tid,
        word,
    }
    #endregion

    public virtual int TID => _tid;
    public virtual string word => stringTable[_word];

    #region Repositories
    private int _tid;
    private int _word;
    #endregion

#pragma warning disable 649
#pragma warning disable 169
    private static string[] stringTable;
#pragma warning restore 169
#pragma warning restore 649

    public T_ForbiddenWord(){}
    public T_ForbiddenWord(int[] intTable, float[] floatTable, Int64[] int64Table)
    {
        #region Constructor
        _tid = intTable[0];
        _word = intTable[1];
        #endregion
    }

    public static T_ForbiddenWord Get(int tid) { return Excel.GetRow(SheetName.T_ForbiddenWord, (int)tid) as T_ForbiddenWord; }
    public static List<int> GetTIDs()
    {
        List<int> list = new List<int>();
        foreach(var iter in Excel.GetKeyList(SheetName.T_ForbiddenWord))
        {
             list.Add((int)iter);
        }
        return list;
    }
    
    public static List<T_ForbiddenWord> GetAll()
    {        
        return  Excel.GetList<T_ForbiddenWord>(SheetName.T_ForbiddenWord);
    }    
	
	public static T_ForbiddenWord GetRandom()
    {        
		return Excel.GetRandom<T_ForbiddenWord>(SheetName.T_ForbiddenWord);
    }    
}
