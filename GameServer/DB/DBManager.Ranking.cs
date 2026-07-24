

using Common;
using CommonStruct;
using Microsoft.VisualBasic.ApplicationServices;
using MySqlX.XDevAPI;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;

public partial class DBManager : BaseManager<DBManager>
{
    public static (bool result, Dictionary<RANKING_TYPE, RankingInfo>, Dictionary<RANKING_TYPE, List<RankUser>>) LoadRanking()
    {
        //====================================================================================================================================================
        // t_ranking_info
        //
        var rankingInfoList = new Dictionary<RANKING_TYPE, RankingInfo>();

        using (var con = new DB.MySql(DB_KIND.GF_RANKING))
        {
            con.PREPARE("SELECT t1.type, t1.season, t1.expire_time  FROM t_ranking_info t1 JOIN (SELECT type, MAX(season) AS max_season FROM t_ranking_info GROUP BY type) t2 ON t1.type = t2.type AND t1.season = t2.max_season");

            if (false == con.EXECUTE())
            {
                return default;
            }

            while (con.FETCH())
            {
                int outType = 0;
                var info = new RankingInfo();
                con.GET_DATA("type",        out outType); info.type = (RANKING_TYPE)outType;
                con.GET_DATA("season",      out info.season);
                con.GET_DATA("expire_time", out info.expireTime);

                rankingInfoList.Add(info.type, info);
            }
        }

        //====================================================================================================================================================
        // t_ranking_user
        //
        var rankUserList = new Dictionary<RANKING_TYPE, List<RankUser>>();

        foreach (var e in rankingInfoList)
        {
            using (var con = new DB.MySql(DB_KIND.GF_RANKING))
            {
                con.PREPARE("SELECT uid, cid, nickname, profile_tid, score FROM t_ranking_user WHERE type=@1 AND season=@2");
                {
                    con.SET_PARAM("@1", (int)e.Value.type);
                    con.SET_PARAM("@2", e.Value.season);
                }
                if (false == con.EXECUTE())
                {
                    return default;
                }
                var userList = new List<RankUser>();

                while (con.FETCH())
                {
                    var info = new RankUser();
                    con.GET_DATA("uid",         out info.uid);
                    con.GET_DATA("cid",         out info.cid);
                    con.GET_DATA("nickname",    out info.nickname);
                    con.GET_DATA("profile_tid", out info.profileTid);
                    con.GET_DATA("score",       out info.score);

                    userList.Add(info);
                }

                rankUserList.Add(e.Key, userList);
            }
        }

        return (true, rankingInfoList, rankUserList);
    }

    public static void SetSeason(RANKING_TYPE type, int season, long expireTime)
    {
        PostSlow(() =>
        {
            using (var con = new DB.MySql(DB_KIND.GF_RANKING))
            {
                con.PREPARE("INSERT INTO t_ranking_info(type, season, expire_time) VALUES(@1, @2, @3) AS new ON DUPLICATE KEY UPDATE expire_time = new.expire_time");
                {
                    con.SET_PARAM("@1", (int)type);
                    con.SET_PARAM("@2", season);
                    con.SET_PARAM("@3", expireTime);
                }

                if (0 == con.EXECUTE_UPDATE())
                {
                    Logger.CRITICAL($"failed to SetSeason()... type( {type} ), season( {season} ), expireTime( {expireTime} )");
                }
            }
        });
    }

    public static void SetRankingScore(RANKING_TYPE type, int season, long uid, long cid, string nickname, int profileTid, int score)
    {
        PostSlow(() =>
        {
            using (var con = new DB.MySql(DB_KIND.GF_RANKING))
            {
                con.PREPARE("INSERT INTO t_ranking_user(type, season, uid, cid, nickname, profile_tid, score) VALUES(@1, @2, @3, @4, @5, @6, @7) AS new ON DUPLICATE KEY UPDATE cid = new.cid, nickname = new.nickname, profile_tid = new.profile_tid, score = new.score");
                {
                    con.SET_PARAM("@1", type);
                    con.SET_PARAM("@2", season);
                    con.SET_PARAM("@3", uid);
                    con.SET_PARAM("@4", cid);
                    con.SET_PARAM("@5", nickname);
                    con.SET_PARAM("@6", profileTid);
                    con.SET_PARAM("@7", score);
                }

                if (0 == con.EXECUTE_UPDATE())
                {
                    Logger.CRITICAL($"SetRankingScore. uid( {uid} ), cid( {cid} ), nickname( {nickname} ), profileTid( {profileTid} ), type( {type} ), season( {season} ), score( {score} )");
                }
            }
        });
    }
}


