

using Common;
using CommonStruct;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;

public partial class DBManager : BaseManager<DBManager>
{
    public static void UpdateMission(User user, Mission mission)
    {
        PostSlow(user.GetJobId(), () =>
        {
            using (var con = new DB.MySql(user.gameDB))
            {
                con.PREPARE("INSERT INTO t_user_mission(uid, tid, param1, param2, resetTime, is_reward) VALUES(@1, @2, @3, @4, @5, @6) AS new ON DUPLICATE KEY UPDATE param1 = new.param1, param2 = new.param2, resetTime = new.resetTime, is_reward = new.is_reward");
                {
                    con.SET_PARAM("@1", user.uid);
                    con.SET_PARAM("@2", mission.tid);
                    con.SET_PARAM("@3", mission.param1);
                    con.SET_PARAM("@4", mission.param2);
                    con.SET_PARAM("@5", mission.resetTime);
                    con.SET_PARAM("@6", mission.isReward);
                }

                if (0 == con.EXECUTE_UPDATE())
                {
                    Logger.CRITICAL(user, $"failed to UpdateMission()... mission tid( {mission.tid} )");
                }
            }
        });
    }

    public static void UpdateMissionList(User user, List<Mission> infos)
    {
        if (0 == infos.Count)
            return;

        PostSlow(user.GetJobId(), () =>
        {
            var queryValues = UTIL.GetStringBuilder();
            if (null == queryValues)
            {
                Logger.CRITICAL("queryValues is null");
                return;
            }

            foreach (var e in infos)
            {
                if (0 != queryValues.Length)
                {
                    queryValues.Append(",");
                }
                queryValues.Append($"({user.uid},{e.tid},{e.param1},{e.param2},{e.resetTime},{(e.isReward ? 1 : 0)})");
            }

            using (var con = new DB.MySql(user.gameDB))
            {
                con.PREPARE($"INSERT INTO t_user_mission(uid,tid,param1,param2,resetTime,is_reward) VALUES{queryValues} AS new ON DUPLICATE KEY UPDATE param1 = new.param1, param2 = new.param2, resetTime = new.resetTime, is_reward = new.is_reward");

                if (0 == con.EXECUTE_UPDATE())
                {
                    Logger.CRITICAL($"RQ_MISSION_UPDATE_LIST..");
                }
            }
        });
    }
}
