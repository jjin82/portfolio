using Common;

public class TalkInfluencerTotalRanking : BaseRanking
{
    public override RANKING_TYPE rankingType => RANKING_TYPE.TALK_INFLUENCER_TOTAL;

    public override long NextExpireTime()
    {
        return DateTime.MaxValue.ToFileTime();
    }
}