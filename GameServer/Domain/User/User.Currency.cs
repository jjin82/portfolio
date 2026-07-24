

using Common;
using CommonStruct;
using System.Collections.Generic;
using System.Reflection;

public partial class User : Entity
{
    private void LoadDBCurrency(Dictionary<CURRENCY_TYPE, Currency> currencyList)
    {
        if(null == currencyList)
        {
            currencyList = new Dictionary<CURRENCY_TYPE, Currency>();
        }

        _currencyList = currencyList;
    }

    public bool UpdateCurrency(Currency currency, Action? notify = null)
    {
        if (null == currency)
        {
            Logger.CRITICAL(this, $"not exist currency...");
            return false;
        }

        // 노티 처리가 필요하다면 우선 처리.
        notify?.Invoke();

        // 클라에 알림.
        Send(new C2G.RS_CURRENCY_UPDATE(currency.type, currency.GetCount()));

        // DB 처리.
        DBManager.UpdateCurrency(this, currency);

        return true;
    }

    public bool AddCurrency(CURRENCY_TYPE type, int addCount = 1, Action? notify = null)
    {
        if (0 == addCount)
            return false;

        switch (type)
        {
            case CURRENCY_TYPE.DIA:
                break;
            case CURRENCY_TYPE.MILEAGE:
                break;
            case CURRENCY_TYPE.MOBILE_DATA: 
                break;
            default:
                {
                    Logger.CRITICAL(this, $"non-accumulable currency... currency type( {type} )");
                    return false;
                }
        }

        var currency = GetCurrency(type);
        if (null == currency) return false;

        var prevCount = currency.count;

        currency._count = Math.Max(0, Math.Min((currency._count + addCount), 1000));

        Logger.INFO(this, $"add currency. type( {type} ), prev count( {prevCount} )  >>>  cur count( {currency._count} ), add count( {addCount} )");

        return UpdateCurrency(currency, notify);
    }

    public bool UseCurrency(CURRENCY_TYPE type, int count = 1)
    {
        if(0 >= count)
            return true;

        var currency = GetCurrency(type);
        if (null == currency) return false;

        if (count > currency.count)
            return false;

        return AddCurrency(type, -count);
    }

    public Currency? GetCurrency(CURRENCY_TYPE type)
    {
        if (false == _currencyList.TryGetValue(type, out var currency))
        {
            currency = new Currency { _type = type, _count = 0 };
            _currencyList.TryAdd(currency.type, currency);
        }

        return currency;
    }

    public bool IsEnoughCurrency(CURRENCY_TYPE type, int count = 1)
    {
        if(0 >= count)
            return true;

        var currency = GetCurrency(type);
        if (null == currency)
            return false;
        
        if (count > currency.count)
            return false;

        return true;
    }

    public bool HasDiamond(int count = 1)
    {
        return IsEnoughCurrency(CURRENCY_TYPE.DIA, count);
    }

    public bool HasMobileData(int count = 1)
    {
        return IsEnoughCurrency(CURRENCY_TYPE.MOBILE_DATA, count);
    }

    public bool IsEmptyCurrency()
    { 
        return (0 == _currencyList.Count);
    }

    public bool AddMobileData(int addCount = 1) 
    {
        return AddCurrency(CURRENCY_TYPE.MOBILE_DATA, addCount);
    }

    public bool UseMobileData(int useCount = 1)
    {
        return AddCurrency(CURRENCY_TYPE.MOBILE_DATA, -useCount);
    }

    public bool AddDiamond(int addCount = 1)
    {
        return AddCurrency(CURRENCY_TYPE.DIA, addCount);
    }

    public bool UseDiamond(int useCount = 1)
    {
        return AddCurrency(CURRENCY_TYPE.DIA, -useCount);
    }

    public void UseAIChatMobileData(int useMobileData)
    {
        // 데이터 사용.
        UseMobileData(useMobileData);
    }

    Dictionary<CURRENCY_TYPE, Currency> _currencyList = new Dictionary<CURRENCY_TYPE, Currency>();
}
