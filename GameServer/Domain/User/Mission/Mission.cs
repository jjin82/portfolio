using CommonStruct;
using System;
using System.Collections.Generic;
using System.Linq;
using static Mission;

public partial class Mission
{
    public virtual MISSION_TYPE Type() 
    { 
        return MISSION_TYPE.NONE; 
    }

    public virtual bool Clear()
    {
        _param1     = 0;
        _param2     = 0;
        _resetTime  = 0;    // 리셋타임 계산.
        _isReward   = false;

        return true;
    }

    public bool Initialize(int mssionTid)
    {
        _tid        = mssionTid;
        _resetTime  = CalcResetTime;    // 리셋타임 계산.

        return true;
    }

    public virtual bool Execute(User user, T_MissionData data, long condition1 = 0, long condition2 = 0)
    {
        // 조건을 초과하지 않음.
        if (count >= data.Count)
            return false;

        ++_count;

        return true;
    }

    public virtual bool ResetProcess()
    {
        if (0 == resetTime)
            return false;

        if (resetTime > DateTime.UtcNow.ToFileTime())
            return false;

        _param1 = 0;
        _param2 = 0;
        _resetTime = CalcResetTime;    // 리셋타임 계산.
        _isReward = false;

        return true;
    }

    public virtual bool IsReward()
    { 
        return isReward; 
    }

    public virtual bool IsCompleted()
    {
        var data = T_MissionData.Get(tid);
        if (null == data) return false;

        return (count >= data.Count);
    }

    public long CalcResetTime
    {
        get
        {
            var data = T_MissionData.Get(tid);
            if (null == data) return 0;

            long resetTime = 0;
            switch (data.ResetType)
            {
                case MISSION_RESET_TYPE.DAILY:  resetTime = UTIL.NextDayMidnightUtc().ToFileTime();  break;
                case MISSION_RESET_TYPE.WEEKLY: resetTime = UTIL.NextWeekMidnightUtc().ToFileTime(); break;
                default:
                    break;
            }

            return resetTime;
        }
    }

    public int ResetRemainSeconds
    {
        get
        {
            if (0 >= resetTime) return -1;

            return (int)(DateTime.FromFileTimeUtc(resetTime) - DateTime.UtcNow).TotalSeconds;
        }
    }

    public void CopyTo(out MissionInfo info)
    {
        info = new MissionInfo()
        {
            tid                 = tid,
            param1              = param1,
            param2              = param2,
            isReward            = isReward,
            resetRemainSeconds  = ResetRemainSeconds,
        };
    }

    public void CopyFrom(Mission ms)
    {
        _param1     = ms._param1;
        _param2     = ms._param2;
        _resetTime  = ms._resetTime;
        _isReward   = ms.isReward;
    }

    public int          tid        => _tid;
    public MISSION_TYPE type       => T_MissionData.Get(tid)?.Type ?? MISSION_TYPE.NONE;
    public long         param1     => _param1;
    public long         param2     => _param2;
    public long         resetTime  => _resetTime;
    public bool         isReward   => _isReward;
    public long         count      => _count;


    public int  _tid        = 0;
    public long _param1     = 0;
    public long _param2     = 0;
    public long _resetTime  = 0;
    public bool _isReward   = false;
    public long _count      { get => _param1; set => _param1 = value; }    // 기본적으로 Param1은 Count로 사용하고 있다.
}

public partial class Mission
{
    public static Mission Alloc(MISSION_TYPE type, int TID)
    {
        Mission ms = null;

        switch (type)
        {
            case MISSION_TYPE.STORE_REVIEW:                         ms = new StoreReview();             break;
            case MISSION_TYPE.LOGIN_DAILY:                          ms = new LoginDaily();              break;
            case MISSION_TYPE.LOGIN_STREAK_TIMES:                   ms = new LoginStreakTimes();        break;
            case MISSION_TYPE.LOGIN_AT_TIME:                        ms = new LoginAtTime();             break; 
            case MISSION_TYPE.GAME_PLAY:                            ms = new GamePlay();                break;
            case MISSION_TYPE.GAME_PLAY_SINGLE:                     ms = new GamePlaySingle();          break;
            case MISSION_TYPE.GAME_PLAY_MULTI:                      ms = new GamePlayMulti();           break;
            case MISSION_TYPE.CHAT_USER:                            ms = new ChatUser();                break;
            case MISSION_TYPE.CHAT_AI:                              ms = new ChatAI();                  break;
            case MISSION_TYPE.RANK:                                 ms = new Rank();                    break;
            case MISSION_TYPE.RANK_TOP_N:                           ms = new RankTopN();                break;
            case MISSION_TYPE.CHANGE_NICKNAME:                      ms = new ChangeNickname();          break;
            case MISSION_TYPE.SHOP_BUY_ONE_PLUS_ONE:                ms = new ShopBuyOnePlusOne();       break;
            case MISSION_TYPE.PHOTO_UNLOCK:                         ms = new PhotoUnlock();             break;
            case MISSION_TYPE.SCENARIO_PLAY:                        ms = new ScenarioPlay();            break;
            case MISSION_TYPE.TING_CREATE:                          ms = new TingCreate();              break;
            case MISSION_TYPE.DM_SEND:                              ms = new DMSend();                  break;
                
            default:
                {
                    Logger.CRITICAL($"not eixst mission.. type( {type} )");
                }
                break;
        }

        ms?.Initialize(TID);

        return ms;
    }
}

public partial class Mission
{
    //////////////////////////////////////////////////////////////////////////////////////////
    //
    // StoreReview
    //
    public class StoreReview : Mission
    {
        public override MISSION_TYPE Type()
        {
            return MISSION_TYPE.STORE_REVIEW;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////
    //
    // LoginDaily
    //
    public class LoginDaily : Mission
    {
        public override MISSION_TYPE Type()
        {
            return MISSION_TYPE.LOGIN_DAILY;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////
    //
    // LoginStreakTimes
    //
    public class LoginStreakTimes : Mission
    {
        public override MISSION_TYPE Type()
        {
            return MISSION_TYPE.LOGIN_STREAK_TIMES;
        }

        public override bool Execute(User user, T_MissionData data, long _, long __)
        {
            // 다음날 이후인가 판단.
            if (NextMidnight > DateTime.UtcNow.ToFileTime())
                return false;

            TimeSpan dateDiff = DateTime.UtcNow - DateTime.FromFileTimeUtc(NextMidnight);
            if (24 > dateDiff.TotalHours)
            {
                ++_count;
            }
            else
            {
                _count = 1;
            }

            NextMidnight = UTIL.NextDayMidnightUtc().ToFileTime();

            return true;
        }

        public long NextMidnight { get => _param2; set => _param2 = value; }
    }

    //////////////////////////////////////////////////////////////////////////////////////////
    //
    // LoginAtTime
    //
    public class LoginAtTime : Mission
    {
        public override MISSION_TYPE Type()
        {
            return MISSION_TYPE.LOGIN_AT_TIME;
        }

        public override bool Execute(User user, T_MissionData data, long _, long __)
        {
            if (0 == data._Condition.Count)
            {
                Logger.CRITICAL($"no mission conditions.. mission type( {Type()} )");
                return false;
            }

            // 정각(0분)과 시간을 체크.
            if (!DateTime.Now.Minute.Equals(0) || !DateTime.Now.Hour.Equals(data.Condition[0]))
                return false;

            _count = 1;

            return true;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////
    //
    // GamePlay
    //
    public class GamePlay : Mission
    {
        public override MISSION_TYPE Type()
        {
            return MISSION_TYPE.GAME_PLAY;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////
    //
    // GamePlaySingle
    //
    public class GamePlaySingle : Mission
    {
        public override MISSION_TYPE Type()
        {
            return MISSION_TYPE.GAME_PLAY_SINGLE;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////
    //
    // GamePlayMulti
    //
    public class GamePlayMulti : Mission
    {
        public override MISSION_TYPE Type()
        {
            return MISSION_TYPE.GAME_PLAY_MULTI;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////
    //
    // ChatUser
    //
    public class ChatUser : Mission
    {
        public override MISSION_TYPE Type()
        {
            return MISSION_TYPE.CHAT_USER;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////
    //
    // ChatAI
    //
    public class ChatAI : Mission
    {
        public override MISSION_TYPE Type()
        {
            return MISSION_TYPE.CHAT_AI;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////
    //
    // Rank
    //
    public class Rank : Mission
    {
        public override MISSION_TYPE Type()
        {
            return MISSION_TYPE.RANK;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////
    //
    // RankTopN
    //
    public class RankTopN : Mission
    {
        public override MISSION_TYPE Type()
        {
            return MISSION_TYPE.RANK_TOP_N;
        }

        public override bool Execute(User user, T_MissionData data, long condition1 = 0, long condition2 = 0)
        {
            if (0 == data._Condition.Count)
            {
                Logger.CRITICAL($"no mission conditions.. mission type( {Type()} )");
                return false;
            }

            long rank = condition1;
            if (rank > data._Condition[0])
                return false;

            _count = 1;

            return true;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////
    //
    // ChangeNickname
    //
    public class ChangeNickname : Mission
    {
        public override MISSION_TYPE Type()
        {
            return MISSION_TYPE.CHANGE_NICKNAME;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////
    //
    // ShopBuyOnePlusOne
    //
    public class ShopBuyOnePlusOne : Mission
    {
        public override MISSION_TYPE Type()
        {
            return MISSION_TYPE.SHOP_BUY_ONE_PLUS_ONE;
        }

        public override bool Execute(User user, T_MissionData data, long condition1 = 0, long condition2 = 0)
        {
            if (0 == data._Condition.Count)
            {
                Logger.CRITICAL($"no mission conditions.. mission type( {Type()} )");
                return false;
            }

            long shopTid = condition1;
            if (shopTid != data._Condition[0])
                return false;

            _count = 1;

            return true;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////
    //
    // PhotoUnlock
    //
    public class PhotoUnlock : Mission
    {
        public override MISSION_TYPE Type()
        {
            return MISSION_TYPE.PHOTO_UNLOCK;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////
    //
    // ScenarioPlay
    //
    public class ScenarioPlay : Mission
    {
        public override MISSION_TYPE Type()
        {
            return MISSION_TYPE.SCENARIO_PLAY;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////
    //
    // TingCreate
    //
    public class TingCreate : Mission
    {
        public override MISSION_TYPE Type()
        {
            return MISSION_TYPE.TING_CREATE;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////
    //
    // DMSend
    //
    public class DMSend : Mission
    {
        public override MISSION_TYPE Type()
        {
            return MISSION_TYPE.DM_SEND;
        }
    }
}


