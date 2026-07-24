

using Common;
using MySqlX.XDevAPI;

public class RankingInfo
{
    public RANKING_TYPE type;
    public int          season;
    public long         expireTime;
}

public class RankUser
{
    public int      rank        = 0;
    public long     uid         = 0;
    public long     cid         = 0;
    public string   nickname    = "";
    public int      profileTid  = 0;
    public int      score       = 0;
}


public abstract class BaseRanking
{
    public virtual RANKING_TYPE rankingType => RANKING_TYPE.NONE;

    public void Clear()
    {
        _season     = 0;
        _expireTime = 0;
        _rankList.Clear();
        _prepareRanking.Reuse();
        _prepareRanking.In((int)0);
        _responseUidList.Clear();
        _bestScore   = 0;
    }

    public bool Load(int season, long expireTime, List<RankUser>? rankUserList)
    {
        _season     = season;
        _expireTime = expireTime;

        if (null != rankUserList)
        {
            foreach (var v in rankUserList)
            {
                _rankList.Add(v.uid, v);
            }
        }

        _sort = true;

        return true;
    }

    public virtual void Update()
    {
        if (_expireTime < DateTime.Now.ToFileTime())
        {
            // 만료 시간 설정.
            _expireTime = NextExpireTime();

            // 랭킹 정보 캐싱.
            _rankList.Clear();
            _prepareRanking.Reuse();
            _prepareRanking.In((int)0);
            _responseUidList.Clear();
            _bestScore = 0;

            DBManager.SetSeason(rankingType, ++_season, _expireTime);
            return;
        }

        lock (this)
        {
            // 정렬
            if (_sort)
            {
                _sort = false;

                var sortedList = _rankList.OrderByDescending(x => x.Value.score).ToList();
                if (null == sortedList) return;

                int prevBestScore = _bestScore;
                int rank          = 0;
                int score         = int.MaxValue;
                for (int i = 0; i < sortedList.Count; ++i)
                {
                    if (score > sortedList[i].Value.score)
                    {
                        score = sortedList[i].Value.score;
                        ++rank;
                    }
                    
                    sortedList[i].Value.rank = rank;

                    // top 10.
                    if (10 >= rank)
                    {
                        if (1 == rank)
                        {
                            _bestScore = score;
                        }

                        // 최고 점수 변경 알림.
                        if (_bestScore > prevBestScore)
                        {
                            ChangeBestSrore(sortedList[i].Value);
                        }

                        // top 10 순위 등극.
                        ExecuteMission(MISSION_TYPE.RANK_TOP_N, sortedList[i].Value);
                    }
                }

                // ÀüÃ¼ ·©Å· ¼øÀ§ Áß n¸í ¸®½ºÆ®.
                int count = Math.Min(_sendRankingCount, _rankList.Count);

                _prepareRanking.Reuse();
                _prepareRanking.In(count);
                foreach (var e in sortedList)
                {
                    if (0 >= count--)
                        break;

                    _prepareRanking.Set(e.Value.rank, e.Value.cid, e.Value.nickname, e.Value.profileTid, e.Value.score);
                }
            }

            // 랭킹 정보를 전달해야되는 유저 처리.
            foreach (var v in _responseUidList)
            {
                var user = UserManager.Get.FindFromUid(v);
                if (user == null) continue;

                var packet = new C2G.RS_RANKING();
                AppendPacket(user, ref packet);
                user.Send(packet);
            }
            _responseUidList.Clear();
        }
    }

    public virtual void SetScore(long uid, long cid, string nickname, int profileTid, int score)
    {
        lock (this)
        {
            if (_rankList.TryGetValue(uid, out var rankUser))
            {
                // 갱신한 점수가 낮고, 프로필정보 동일, 닉넥임 동일 하면 리턴.
                if (rankUser.cid.Equals(cid) &&
                    rankUser.score > score && 
                    rankUser.profileTid.Equals(profileTid) && 
                    rankUser.Equals(nickname))
                {
                    return;
                }
            }
            else
            {
                rankUser = new RankUser { uid = uid };
                _rankList.Add(uid, rankUser);
            }

            // 소팅 설정.
            _sort = true;

            // 정보 다시 세팅.
            rankUser.cid        = cid;
            rankUser.nickname   = nickname;
            rankUser.profileTid = profileTid;
            rankUser.score      = Math.Max(rankUser.score, score);

            // 순위 정보 전달 해야되는 유저로 등록.
            _responseUidList.Add(uid);

            // 랭크 등록 미션.
            ExecuteMission(MISSION_TYPE.RANK, rankUser);

            // DB 저장.
            DBManager.SetRankingScore(rankingType, _season, rankUser.uid, rankUser.cid, rankUser.nickname, rankUser.profileTid, rankUser.score);
        }
    }

    public virtual void AddScore(long uid, long cid, string nickname, int profileTid, int score)
    {
        lock (this)
        {
            if (!_rankList.TryGetValue(uid, out var rankUser))
            {
                rankUser = new RankUser { uid = uid };
                _rankList.Add(uid, rankUser);
            }

            // 점수 증가 (최대 100 제한)
            SetScore(uid, cid, nickname, profileTid, rankUser.score + Math.Min(score, 100));
        }
    }

    public void AppendPacket(User user, ref C2G.RS_RANKING packet)
    {
        if (null == user)
            return;

        lock (this)
        {
            // 랭킹 타입
            packet.In((int)rankingType);

            // 내 랭킹 정보.
            bool isMyRank = _rankList.TryGetValue(user.uid, out var myRank);
            packet.In(isMyRank);
            if (isMyRank)
            {
                packet.Set(myRank.rank, myRank.cid, myRank.nickname, myRank.profileTid, myRank.score);
            }

            // 준비된 랭킹 정보 추가.
            packet.In(_prepareRanking);
        }
    }

    public virtual void ChangeBestSrore(RankUser rankUser)
    {
    }

    public virtual void ExecuteMission(MISSION_TYPE type, RankUser rankUser)
    {
        var user = UserManager.Get.FindFromUid(rankUser.uid);
        if (null == user) return;

        user.Post(() => 
        {
            user.ExecuteMission(type, rankUser.rank);
        });
    }

    public int GetSeason()
    {
        return _season;
    }

    public int GetCount()
    {
        return _rankList.Count;
    }
    
    public abstract long NextExpireTime();
    public DateTime expireDateTime => DateTime.FromFileTime(_expireTime);


    protected int                        _season            = 0;
    protected long                       _expireTime        = 0;
    protected Dictionary<long, RankUser> _rankList          = new Dictionary<long, RankUser>();
    private   C2G.RS_RANKING             _prepareRanking    = new C2G.RS_RANKING();
    private   List<long>                 _responseUidList   = new List<long>();
    private   int                        _bestScore         = 0;

    protected bool                       _sort              = false;
    private const int                    _sendRankingCount  = 100;
}
