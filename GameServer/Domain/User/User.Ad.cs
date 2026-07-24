

using Common;
using CommonStruct;

public class Ad
{
    public int remainTime => -1;

    public AD_TYPE  type        = AD_TYPE.NONE;
    public long     nextTime    = 0;
    public ulong    rewardToken = 0;
    public int      count       = 0;
}

public partial class User : Entity
{
    void LoadAd(List<Ad> adList)
    {
        // 무조건 생성.
        foreach (var e in T_AdData.GetAll())
        {
            _adList.Add(e.TID, new Ad
            {
                type        = e.TID,
                rewardToken   = (ulong)DateTime.Now.Ticks
            });
        }

        foreach (var e in adList)
        {
            if (false == _adList.TryGetValue(e.type, out var ad))
                continue;

            ad.nextTime     = e.nextTime;
            ad.rewardToken  = e.rewardToken;
            ad.count        = e.count;
        }
    }

    private void ResetAd()
    {
        if (0 == _reserveRewardKey)
            return;

        EndAd(_reserveRewardKey);
    }

    private void UpdateAd()
    {
        foreach (var v in _adList.Values)
        {
            var adData = T_AdData.Get(v.type);
            if (null == adData) continue;

            if (v.nextTime > DateTime.Now.ToFileTime())
                continue;

            // 다음 광고 시간.
            v.nextTime = UTIL.NextDayMidnight().ToFileTime();

            // 현재 광고 보상 키 설정.
            v.rewardToken = (ulong)DateTime.Now.Ticks;

            // 광고 수를 채워줌.
            v.count = adData.DailyAdCount;

            // 클라에 알림.
            Send(new C2G.RS_AD_UPDATE(v));

            // DB
            DBManager.UpdateAd(this, v);
        }
    }

    public Ad? ExecuteAd(AD_TYPE type, bool isReward = false)
    {
        var adData = T_AdData.Get(type);
        if (null == adData) return null;

        if (false == _adList.TryGetValue(type, out var adInfo))
            return null;

        const int LIMIT_DIA         =    10; // limit   10.
        const int LIMIT_MOBILE_DATA = 50000; // limit  500.
        const int LIMIT_ITEM_COUNT  =     5; // limit    5.

        switch (type)
        {
            case AD_TYPE.AD_REWARD_DIA:         AddDiamond(Math.Min(LIMIT_DIA, adData.RewardCount));              break;
            case AD_TYPE.AD_REWARD_MOBILE_DATA: AddMobileData(Math.Min(LIMIT_MOBILE_DATA, adData.RewardCount));   break;
            case AD_TYPE.AD_REWARD_GAME_TICKET: 
            case AD_TYPE.AD_REWARD_GAME_ITEM:
                {
                    AddItem(adData.RandomRewardValue(), Math.Min(LIMIT_ITEM_COUNT, adData.RewardCount));
                }
                break;
        }

        // 광고 보상 토큰 제거.
        adInfo.rewardToken = (ulong)DateTime.Now.Ticks;

        // 광고 수 감소.
        adInfo.count = Math.Max(0, --adInfo.count);

        // 클라에 알림.
        Send(new C2G.RS_AD_UPDATE(adInfo));

        // DB
        DBManager.UpdateAd(this, adInfo);

        // 유저가 광고를 봤을때만 기록.
        if (false == isReward)
        {
            // [REPORT] 광고 실행
            WebManager.Get.ReportExecuteAd(pid, uid, nickname, type);
        }

        return adInfo;
    }

    public void BeginAd(AD_TYPE type, ulong rewardToken)
    {
        if (false == _adList.TryGetValue(type, out var adInfo))
            return;

        if (0 >= adInfo.count)
            return;

        if (0 >= adInfo.rewardToken)
            return;

        if (false == adInfo.rewardToken.Equals(rewardToken))
            return;

        _reserveRewardKey = rewardToken;

        Send(new C2G.RS_AD_BEGIN
        {
            type        = type,
            rewardToken = rewardToken
        });

        // 생명 주기 10분.
        SetLifeTime(600);
    }

    public void EndAd(ulong rewardToken)
    {
        // 생명 주기 원복.
        SetLifeTime();

        if (_reserveRewardKey != rewardToken)
            return;

        foreach (var v in _adList.Values)
        {
            if (0 == v.rewardToken)
                continue;

            if (false == v.rewardToken.Equals(rewardToken))
                continue;

            ExecuteAd(v.type);
            break;
        }

        _reserveRewardKey = 0;
    }

    public void ResetActiveAd()
    {
        foreach(var v in T_AdData.GetAll())
        {
            if (false == _adList.TryGetValue(v.TID, out var adInfo))
                return;

            // 광고 볼 수 있는 시간 초기화.
            adInfo.count = 0;
        }
    }

    public void SendAdList()
    {
        var packet = new C2G.RS_AD_LIST();

        // ad list.
        foreach (var v in _adList.Values)
        {
            packet.Add(v);
        }
        Send(packet);
    }


    private Dictionary<AD_TYPE, Ad> _adList           = new Dictionary<AD_TYPE, Ad>();      // 광고
    private ulong                   _reserveRewardKey = 0;
}
