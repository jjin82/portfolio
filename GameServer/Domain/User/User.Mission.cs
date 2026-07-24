using CommonStruct;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class User : Entity
{
    public void LoadDBMission(List<Mission> missionList)
    {
        foreach (var e in missionList)
        {
            var type = e.type;

            var group = FindMissionGroup(type);
            if (null == group) continue;

            group.Load(e);
        }
    }

    public void UpdateMission()
    {
        ExecuteMission(MISSION_TYPE.LOGIN_AT_TIME);
    }

    public MissionGroup? FindMissionGroup(MISSION_TYPE type)
    {
        if (!_missionGroupList.TryGetValue(type, out var group))
        {
            group = new MissionGroup(type);
            _missionGroupList.Add(type, group);
        }

        return group;
    }

    public void ExecuteMission(MISSION_TYPE type, long condition1 = 0, long condition2 = 0)
    {
        var group = FindMissionGroup(type);
        if (null == group) return;

        group?.Execute(this, condition1, condition2);
    }

    public void CheatExecuteMission(MISSION_TYPE type, int condition1, int condition2)
    {
        var group = FindMissionGroup(type);
        if (null == group) return;

        group?.CheatExecute(this, condition1, condition2);
    }

    public void CheatClearMission()
    {
        foreach(var g in _missionGroupList.Values)
        {
            g.CheatClear(this);
        }
    }

    public void RewardMission(int tid)
    {
        var data = T_MissionData.Get(tid);
        if (null == data) return;

        var group = FindMissionGroup(data.Type);
        if (null == group) return;

        group.Reward(this, data.TID);
    }

    public bool CompletedMission(int tid)
    {
        var data = T_MissionData.Get(tid);
        if (null == data) return false;

        var group = FindMissionGroup(data.Type);
        if (null == group) return false;

        return group.Completed(this, data.TID);
    }

    public void SendMissionList()
    {
        int dailyRemainSeconds = UTIL.NextDayMidnightUtcRemainSeconds();      // 일일 미션 남은 시간(초).
        int weekRemainSeconds  = UTIL.NextWeekMidnightUtcRemainSeconds();     // 주간 미션 남은 시간(초).
        if (0 > dailyRemainSeconds || 0 > weekRemainSeconds)
        {
            dailyRemainSeconds = 0;
            weekRemainSeconds  = 0;

            Logger.CRITICAL($"error... mission remain time..... dailyRemainTime( {dailyRemainSeconds} ), dailyRemainTime( {weekRemainSeconds} )");
        }

        var sendPacket = new C2G.RS_MISSION_LIST(dailyRemainSeconds, weekRemainSeconds);
        if (0 == (_missionGroupList?.Count ?? 0))
        {
            Send(sendPacket);
            return;
        }

        foreach (var group in _missionGroupList)
        {
            foreach (var ms in group.Value._list)
            {
                ms.Value.CopyTo(out var info);
                sendPacket.Add(info);

                if (false == sendPacket.IsSuccess())
                {
                    Send(sendPacket);

                    // 새 패킷 생성.
                    sendPacket = new C2G.RS_MISSION_LIST(dailyRemainSeconds, weekRemainSeconds);
                    sendPacket.Add(info);
                }
            }
        }

        Send(sendPacket);
    }

    Dictionary<MISSION_TYPE, MissionGroup> _missionGroupList = new Dictionary<MISSION_TYPE, MissionGroup>();
}
