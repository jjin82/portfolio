using Common;

public class WatermelonDailyRanking : BaseRanking
{
    public override RANKING_TYPE rankingType => RANKING_TYPE.WATERMELON_DAILY;

    public override long NextExpireTime()
    {
        return UTIL.NextDayMidnight().ToFileTime();
    }
}