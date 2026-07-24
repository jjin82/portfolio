using GameData;
using System;
using System.Collections.Generic;


public class T_ShopData : IRow
{
    #region Record Info
    public enum Records
    {
        tid,
        Sale,
        ShopCategoryType,
        ShopType,
        AdType,
        ProductName,
        PaymentCurrencyType,
        PaymentCount,
        RewardType,
        ProductType,
        GoogleProductID,
        AppleProductID,
    }
    #endregion

    public virtual int TID => _tid;
    public virtual bool Sale => stringTable[_Sale] == "TRUE" ? true : false;
    public virtual SHOP_CATEGORY_TYPE ShopCategoryType =>  (SHOP_CATEGORY_TYPE)System.Enum.Parse(typeof(SHOP_CATEGORY_TYPE),stringTable[_ShopCategoryType]);
    public virtual SHOP_TYPE ShopType =>  (SHOP_TYPE)System.Enum.Parse(typeof(SHOP_TYPE),stringTable[_ShopType]);
    public virtual AD_TYPE AdType =>  (AD_TYPE)System.Enum.Parse(typeof(AD_TYPE),stringTable[_AdType]);
    public virtual CURRENCY_TYPE PaymentCurrencyType =>  (CURRENCY_TYPE)System.Enum.Parse(typeof(CURRENCY_TYPE),stringTable[_PaymentCurrencyType]);
    public virtual float PaymentCount => _PaymentCount;
    public virtual REWARD_TYPE RewardType =>  (REWARD_TYPE)System.Enum.Parse(typeof(REWARD_TYPE),stringTable[_RewardType]);
    public virtual PRODUCT_TYPE ProductType =>  (PRODUCT_TYPE)System.Enum.Parse(typeof(PRODUCT_TYPE),stringTable[_ProductType]);
    public virtual string GoogleProductID => stringTable[_GoogleProductID];
    public virtual string AppleProductID => stringTable[_AppleProductID];

    #region Repositories
    private int _tid;
    private int _Sale;
    private int _ShopCategoryType;
    private int _ShopType;
    private int _AdType;
    private int _PaymentCurrencyType;
    private float _PaymentCount;
    private int _RewardType;
    private int _ProductType;
    private int _GoogleProductID;
    private int _AppleProductID;
    #endregion

#pragma warning disable 649
#pragma warning disable 169
    private static string[] stringTable;
#pragma warning restore 169
#pragma warning restore 649

    public T_ShopData(){}
    public T_ShopData(int[] intTable, float[] floatTable, Int64[] int64Table)
    {
        #region Constructor
        _tid = intTable[0];
        _Sale = intTable[1];
        _ShopCategoryType = intTable[2];
        _ShopType = intTable[3];
        _AdType = intTable[4];
        _PaymentCurrencyType = intTable[5];
        _PaymentCount = floatTable[0];
        _RewardType = intTable[6];
        _ProductType = intTable[7];
        _GoogleProductID = intTable[8];
        _AppleProductID = intTable[9];
        #endregion
    }

    public static T_ShopData Get(int tid) { return Excel.GetRow(SheetName.T_ShopData, (int)tid) as T_ShopData; }
    public static List<int> GetTIDs()
    {
        List<int> list = new List<int>();
        foreach(var iter in Excel.GetKeyList(SheetName.T_ShopData))
        {
             list.Add((int)iter);
        }
        return list;
    }
    
    public static List<T_ShopData> GetAll()
    {        
        return  Excel.GetList<T_ShopData>(SheetName.T_ShopData);
    }    
	
	public static T_ShopData GetRandom()
    {        
		return Excel.GetRandom<T_ShopData>(SheetName.T_ShopData);
    }    
}
