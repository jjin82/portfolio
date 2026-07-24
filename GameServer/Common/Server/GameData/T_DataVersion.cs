using GameData;
using System;
using System.Collections.Generic;


public class T_DataVersion : IRow
{
    #region Record Info
    public enum Records
    {
        tid,
        Version,
        Host,
        Port,
        DevHost,
        DevPort,
        QAHost,
        QAPort,
    }
    #endregion

    public virtual int TID => _tid;
    public virtual string Version => stringTable[_Version];
    public virtual string Host => stringTable[_Host];
    public virtual int Port => _Port;
    public virtual string DevHost => stringTable[_DevHost];
    public virtual int DevPort => _DevPort;
    public virtual string QAHost => stringTable[_QAHost];
    public virtual int QAPort => _QAPort;

    #region Repositories
    private int _tid;
    private int _Version;
    private int _Host;
    private int _Port;
    private int _DevHost;
    private int _DevPort;
    private int _QAHost;
    private int _QAPort;
    #endregion

#pragma warning disable 649
#pragma warning disable 169
    private static string[] stringTable;
#pragma warning restore 169
#pragma warning restore 649

    public T_DataVersion(){}
    public T_DataVersion(int[] intTable, float[] floatTable, Int64[] int64Table)
    {
        #region Constructor
        _tid = intTable[0];
        _Version = intTable[1];
        _Host = intTable[2];
        _Port = intTable[3];
        _DevHost = intTable[4];
        _DevPort = intTable[5];
        _QAHost = intTable[6];
        _QAPort = intTable[7];
        #endregion
    }

    public static T_DataVersion Get(int tid) { return Excel.GetRow(SheetName.T_DataVersion, (int)tid) as T_DataVersion; }
    public static List<int> GetTIDs()
    {
        List<int> list = new List<int>();
        foreach(var iter in Excel.GetKeyList(SheetName.T_DataVersion))
        {
             list.Add((int)iter);
        }
        return list;
    }
    
    public static List<T_DataVersion> GetAll()
    {        
        return  Excel.GetList<T_DataVersion>(SheetName.T_DataVersion);
    }    
	
	public static T_DataVersion GetRandom()
    {        
		return Excel.GetRandom<T_DataVersion>(SheetName.T_DataVersion);
    }    
}
