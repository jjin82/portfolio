using GameData;
using System;
using System.Collections.Generic;


public class T_Sound : IRow
{
    #region Record Info
    public enum Records
    {
        tid,
        AudioType,
        Name,
        Volum,
    }
    #endregion

    public virtual int TID => _tid;
    public virtual AUDIO_TYPE AudioType =>  (AUDIO_TYPE)System.Enum.Parse(typeof(AUDIO_TYPE),stringTable[_AudioType]);
    public virtual string Name => stringTable[_Name];
    public virtual float Volum => _Volum;

    #region Repositories
    private int _tid;
    private int _AudioType;
    private int _Name;
    private float _Volum;
    #endregion

#pragma warning disable 649
#pragma warning disable 169
    private static string[] stringTable;
#pragma warning restore 169
#pragma warning restore 649

    public T_Sound(){}
    public T_Sound(int[] intTable, float[] floatTable, Int64[] int64Table)
    {
        #region Constructor
        _tid = intTable[0];
        _AudioType = intTable[1];
        _Name = intTable[2];
        _Volum = floatTable[0];
        #endregion
    }

    public static T_Sound Get(int tid) { return Excel.GetRow(SheetName.T_Sound, (int)tid) as T_Sound; }
    public static List<int> GetTIDs()
    {
        List<int> list = new List<int>();
        foreach(var iter in Excel.GetKeyList(SheetName.T_Sound))
        {
             list.Add((int)iter);
        }
        return list;
    }
    
    public static List<T_Sound> GetAll()
    {        
        return  Excel.GetList<T_Sound>(SheetName.T_Sound);
    }    
	
	public static T_Sound GetRandom()
    {        
		return Excel.GetRandom<T_Sound>(SheetName.T_Sound);
    }    
}
