
using C2G;
using Common;

using Microsoft.VisualBasic;
using Microsoft.VisualBasic.ApplicationServices;
using System.Collections.Concurrent;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

public class RankingManager : BaseManager<RankingManager>
{
    public override bool Initialize()
    {
        // DB ∑ŒµÂ.
        if (!LoadDB())
            return false;

        return true;
    }

    private Dictionary<RANKING_TYPE, BaseRanking> NewRankingList()
    {
        var tempRankingList = new Dictionary<RANKING_TYPE, BaseRanking>();
        
        // ∑©≈∑ ≈∏¿‘ µÓ∑œ.
        {
            tempRankingList.Add(RANKING_TYPE.WATERMELON_DAILY,      new WatermelonDailyRanking());
            tempRankingList.Add(RANKING_TYPE.WATERMELON_TOTAL,      new WatermelonTotalRanking());
            tempRankingList.Add(RANKING_TYPE.TALK_INFLUENCER_TOTAL, new TalkInfluencerTotalRanking());
        }

        return tempRankingList;
    }

    public bool LoadDB() 
    {
        var newRankingList = NewRankingList();

        (bool result, var rankingInfoList, var rankingUserList) = DBManager.LoadRanking();
        if (false == result) return false;

        foreach (var e in rankingInfoList)
        {
            if (false == newRankingList.ContainsKey(e.Key))
            {
                Logger.CRITICAL($"not exist ranking... ranking type( {e.Key} )");
                continue;
            }

            // ∑©≈∑ ≈∏¿‘ ∫∞ ¡§∫∏ »πµÊ.
            if (false == rankingUserList.TryGetValue(e.Key, out var userList))
            {
                Logger.CRITICAL($"not exist ranking user... ranking type( {e.Key} )");
                continue;
            }

            // ∑©≈∑ ≈∏¿‘ ∫∞ ¿Ø¿˙ ºº∆√.
            newRankingList[e.Key].Load(e.Value.season, e.Value.expireTime, userList);
        }

        lock (this)
        {
            _rankingList = newRankingList;
        }

        return true;
    }

    public void Clear()
    {
        lock (this)
        {
            foreach (var v in _rankingList.Values)
            {
                v.Clear();
                v.Update();
            }
        }
    }

    public override void OnUpdate()
    {
        lock (this)
        {
            foreach (var v in _rankingList.Values)
            {
                v.Update();
            }
        }
    }

    public void SetScore(Character character, RANKING_TYPE type, int score)
    {
        if (null == character || 0 == score)
            return;

        lock (this)
        {
            if (false == _rankingList.TryGetValue(type, out var ranking))
                return;

            ranking.AddScore(character.uid, character.cid, character.nickname, character.profileTid, score);
        }
    }

    public void AddScore(Character character, RANKING_TYPE type, int addScore = 1)
    {
        if (null == character || 0 == addScore)
            return;

        lock (this)
        {
            if (false == _rankingList.TryGetValue(type, out var ranking))
                return;

            ranking.AddScore(character.uid, character.cid, character.nickname, character.profileTid, addScore);
        }
    }

    public void SendRanking(RANKING_TYPE type, User user)
    {
        lock (this)
        {
            if (false == _rankingList.TryGetValue(type, out var ranking))
                return;

            var packet = new RS_RANKING();
            ranking.AppendPacket(user, ref packet);

            user.Send(packet);
        }
    }

    public void SendRanking(User user)
    {
        lock (this)
        {
            var packet = new RS_RANKING();

            foreach (var v in _rankingList.Values)
            {
                v.AppendPacket(user, ref packet);
            }

            user.Send(packet);
        }
    }

    public int GetRankingSeason(RANKING_TYPE type)
    {
        if (false == _rankingList.TryGetValue(type, out var ranking))
            return 0;

        return ranking.GetSeason();
    }

    public int GetRankingCount(RANKING_TYPE type)
    {
        if (false == _rankingList.TryGetValue(type, out var ranking))
            return 0;

        return ranking.GetCount();
    }

    private Dictionary<RANKING_TYPE, BaseRanking> _rankingList = new Dictionary<RANKING_TYPE, BaseRanking>();
}
