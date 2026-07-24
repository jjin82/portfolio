using Common;

public class WatermelonTotalRanking : BaseRanking
{
    public override RANKING_TYPE rankingType => RANKING_TYPE.WATERMELON_TOTAL;

    public override long NextExpireTime()
    {
        return DateTime.MaxValue.ToFileTime();
    }
}