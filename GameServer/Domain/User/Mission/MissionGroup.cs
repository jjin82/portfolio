using System;
using System.Collections.Generic;
using CommonStruct;

public class MissionGroup
{
    public MissionGroup(MISSION_TYPE type)
    {
        var datas = T_MissionData.GetAll().Where(x => x.Type.Equals(type));
        if (null == datas) return;

        foreach (var e in datas)
        {
            if (!e.Enable)
                continue;

            var mission = Mission.Alloc(type, e.TID);
            if(null == mission) continue;

            _list.Add(mission.tid, mission);
        }
    }

    public void Load(Mission mission)
    {
        mission.ResetProcess();

        if (!_list.TryGetValue(mission.tid, out var ms))
            return;

        ms.CopyFrom(mission);
    }

    public void Execute(User user, long condition1, long condition2)
    {
        var updateList = new List<Mission>();

        foreach (var e in _list)
        {
            // 리셋이 되는 미션의 경우 처리.
            e.Value.ResetProcess();

            // 보상 지급 완료.
            if (e.Value.IsReward())
                continue;

            // 미션 완료.
            if (e.Value.IsCompleted())
                continue;

            var data = T_MissionData.Get(e.Value.tid);
            if (null != data)
            {
                if (e.Value.Execute(user, data, condition1, condition2))
                {
                    updateList.Add(e.Value);
                }
            }
        }

        // client and db 송신.
        Send(user, updateList);

        // db 처리.
        DBManager.UpdateMissionList(user, updateList);
    }

    public void CheatExecute(User user, int condition1, int condition2)
    {
        if (ServerConfig.IsLive())
            return;

        var updateList = new List<Mission>();

        foreach (var e in _list)
        {
            // 리셋이 되는 미션의 경우 처리.
            e.Value.ResetProcess();

            // 보상 지급 완료.
            if (e.Value.IsReward())
                continue;

            // 미션 완료.
            if (e.Value.IsCompleted())
                continue;

            var data = T_MissionData.Get(e.Value.tid);
            if (null != data)
            {
                if (e.Value.Execute(user, data, condition1, condition2))
                {
                    updateList.Add(e.Value);
                }
            }
        }

        // client and db 송신.
        Send(user, updateList);

        // db 처리.
        DBManager.UpdateMissionList(user, updateList);
    }

    public void CheatClear(User user)
    {
        if (ServerConfig.IsLive())
            return;

        var updateList = new List<Mission>();

        foreach (var e in _list)
        {
            e.Value.Clear();
            updateList.Add(e.Value);
        }

        // client and db 송신.
        Send(user, updateList);

        // db 처리.
        DBManager.UpdateMissionList(user, updateList);
    }

    public void Reward(User user, int tid)
    {
        if (false == _list.TryGetValue(tid, out var mission))
            return;

        if (false == mission.IsCompleted())
            return;

        // 이미 보상 지급.
        if (mission.IsReward())
        {
            //Logger.FAIL($"already been rewarded.. mission( {mission.Type()} ), param1( {mission.Param1} )"); // 연타로 인해 높은 비율로 로그가 남아서 제외.
            return;
        }

        var data = T_MissionData.Get(tid);
        if (null == data) return;

        // 보상 지급.
        if (false == user.GiveReward(data.RewardType))
            return;

        // 보상 지급 세팅.
        mission._isReward = true;

        // client and db 송신.
        Send(user, mission);

        // db 처리.
        DBManager.UpdateMission(user, mission);
    }

    public bool Completed(User user, int tid)
    {
        if (false == _list.TryGetValue(tid, out var mission))
            return false;

        // 완료.
        return mission.IsCompleted();
    }

    private void Send(User user, Mission mission)
    {
        // client.
        user.Send(new C2G.RS_MISSION_UPDATE
        {
            tid             = mission.tid,
            param1          = mission.param1,
            param2          = mission.param2,
            reward          = mission.isReward,
            resetRemainTime = mission.ResetRemainSeconds,
        });
    }

    private void Send(User user, List<Mission> list)
    {
        if (0 == list.Count)
            return;

        var packet = new C2G.RS_MISSION_UPDATE_LIST();
        
        foreach (var e in list)
        {
            e.CopyTo(out var info);
            packet.Add(info);
        }
        
        user.Send(packet);
    }

    public Dictionary<int, Mission> _list = new Dictionary<int, Mission>();
}