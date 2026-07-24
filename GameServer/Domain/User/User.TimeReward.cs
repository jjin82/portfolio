

using Mysqlx.Crud;
using Org.BouncyCastle.Asn1.Cms;
using System.Collections.Generic;
using System.Runtime.Serialization;

// ===================================================================================================================
//
//
public partial class User : Entity
{
    public void NewbieTimeReward()
    {
        // 신규 유저 최초 보상 시간.
        _giveRewardTime = DateTime.Now.AddMinutes(1).ToFileTime();

        // 신규 유저 보상 개수.
        _giveRewardCount = 10;
    }

    public void ResetTimeReward()
    {
        // 획득 가능한 보상 수 백업.
        _takeRewardCount = _giveRewardCount;

        // 다음 간격(분)
        var nextMinute = Math.Max(1, T_GlobalValueData.Get(GLOBAL_VALUE_TYPE.TIME_REWARD_INTERVAL).RandomValueIntArray());

        // 다음 보상 시간.
        _giveRewardTime = DateTime.Now.AddMinutes(nextMinute).ToFileTime();

        // 보상 개수.(분당 모바일 데이터 1개)
        _giveRewardCount = Math.Min(nextMinute, T_GlobalValueData.Get(GLOBAL_VALUE_TYPE.TIME_REWARD_INTERVAL).ValueInt);
    }

    public void UpdateTimeReward()
    {
        if (_giveRewardTime > DateTime.Now.ToFileTime())
            return;

        // 보상 지급 시간 설정.
        ResetTimeReward();

        // 시간 보상 받기.
        Send(new C2G.RS_TIME_REWARD_READY(_giveRewardTime));
    }

    public void TakeTimeReward(long rewardToken)
    {
        if (_giveRewardTime != rewardToken)
            return;

        // 시간 보상 결과.
        Send(new C2G.RS_TIME_REWARD_RESULT(_takeRewardCount));

        // 모바일 데이터 보상 지급.
        AddMobileData(_takeRewardCount);

        // 보상 지급 시간 설정.(지급 후 다시 설정)
        ResetTimeReward();
    }

    public void CheatTimeReward()
    {
        _giveRewardTime = 0;
    }

    private long _giveRewardTime  = DateTime.Now.AddMinutes(10).ToFileTime();
    private int  _giveRewardCount = 0;
    private int  _takeRewardCount = 0;
}