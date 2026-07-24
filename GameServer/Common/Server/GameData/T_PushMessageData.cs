using GameData;
using System;
using System.Collections.Generic;


public class T_PushMessageData : IRow
{
    #region Record Info
    public enum Records
    {
        tid,
        Title,
        Content,
    }
    #endregion

    public virtual PUSH_MESSAGE TID => (PUSH_MESSAGE)_tid;
    public virtual string Title => stringTable[_Title];
    public virtual string Content => stringTable[_Content];

    #region Repositories
    private int _tid;
    private int _Title;
    private int _Content;
    #endregion

#pragma warning disable 649
#pragma warning disable 169
    private static string[] stringTable;
#pragma warning restore 169
#pragma warning restore 649

    public T_PushMessageData(){}
    public T_PushMessageData(int[] intTable, float[] floatTable, Int64[] int64Table)
    {
        #region Constructor
        _tid = intTable[0];
        _Title = intTable[1];
        _Content = intTable[2];
        #endregion
    }

    public static T_PushMessageData Get(PUSH_MESSAGE tid) { return Excel.GetRow(SheetName.T_PushMessageData, (int)tid) as T_PushMessageData; }
    public static List<PUSH_MESSAGE> GetTIDs()
    {
        List<PUSH_MESSAGE> list = new List<PUSH_MESSAGE>();
        foreach(var iter in Excel.GetKeyList(SheetName.T_PushMessageData))
        {
             list.Add((PUSH_MESSAGE)iter);
        }
        return list;
    }
    
    public static List<T_PushMessageData> GetAll()
    {        
        return  Excel.GetList<T_PushMessageData>(SheetName.T_PushMessageData);
    }    
	
	public static T_PushMessageData GetRandom()
    {        
		return Excel.GetRandom<T_PushMessageData>(SheetName.T_PushMessageData);
    }    
}
