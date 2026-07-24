

public class Currency
{
    public Currency()
    {
    }

    public int GetCount()
    {
        int resultCount = count;
        if (type.Equals(CURRENCY_TYPE.MOBILE_DATA))
        {
            resultCount = (count / 100); // 모바일 데이터는 100자리 절삭.
        }
        return resultCount;
    }

    public CURRENCY_TYPE type   => _type;
    public int           count  => _count;


    public CURRENCY_TYPE _type  = CURRENCY_TYPE.NONE;
    public int           _count = 0;
}
