using GameData;
using System;
using System.Collections.Generic;


public class T_NoticeData : IRow
{
    #region Record Info
    public enum Records
    {
        tid,
        Usable,
        Title,
        Image,
        Content,
        Date,
    }
    #endregion

    public virtual int TID => _tid;
    public virtual bool Usable => stringTable[_Usable] == "TRUE" ? true : false;
    public virtual string Title => stringTable[_Title];
    public virtual string Image => stringTable[_Image];
    public virtual string Content => stringTable[_Content];
    public virtual string Date => stringTable[_Date];

    #region Repositories
    private int _tid;
    private int _Usable;
    private int _Title;
    private int _Image;
    private int _Content;
    private int _Date;
    #endregion

#pragma warning disable 649
#pragma warning disable 169
    private static string[] stringTable;
#pragma warning restore 169
#pragma warning restore 649

    public T_NoticeData(){}
    public T_NoticeData(int[] intTable, float[] floatTable, Int64[] int64Table)
    {
        #region Constructor
        _tid = intTable[0];
        _Usable = intTable[1];
        _Title = intTable[2];
        _Image = intTable[3];
        _Content = intTable[4];
        _Date = intTable[5];
        #endregion
    }

    public static T_NoticeData Get(int tid) { return Excel.GetRow(SheetName.T_NoticeData, (int)tid) as T_NoticeData; }
    public static List<int> GetTIDs()
    {
        List<int> list = new List<int>();
        foreach(var iter in Excel.GetKeyList(SheetName.T_NoticeData))
        {
             list.Add((int)iter);
        }
        return list;
    }
    
    public static List<T_NoticeData> GetAll()
    {        
        return  Excel.GetList<T_NoticeData>(SheetName.T_NoticeData);
    }    
	
	public static T_NoticeData GetRandom()
    {        
		return Excel.GetRandom<T_NoticeData>(SheetName.T_NoticeData);
    }    
}
