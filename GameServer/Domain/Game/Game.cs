using C2G;
using Common;

public delegate bool SkillFunc(User user, T_SkillData skillData, Item item, Action<Item> useItemAction);

public partial class Game :  Entity
{
    protected override void OnCreate()
    {
        // 준비 카운트 다운.
        var gameCountdown = T_GlobalValueData.Get(GLOBAL_VALUE_TYPE.GAME_COUNTDOWN).ValueInt;

        // 게임 스테이트 초기화.
        InitializeState(gameCountdown);

        // 생성 알림.
        Send(new C2G.RS_GAME_CREATE(_playerList.First().Value.key, (byte)gameCountdown, _playerList));

        // ** game manager 추가.
        GameManager.Get.Add(this);

        // 누적 카운트.
        ++s_playCount;
    }

    protected override void OnDestroy()
    {
        foreach (var player in _playerList.Values)
        {
            player.Leave();
        }

        Send(new C2G.RS_GAME_DESTROY());

        // ** game manager 제거.
        GameManager.Get.Del(key);
    }

    protected override void OnUpdate()
    {
        UpdateState();
    }

    public void Cancel(User user)
    {
        if (!IsCancel())
            return;

        Send(new C2G.RS_GAME_CANCEL(user.cid));

        Destroy();
    }

    public bool Create(User createUser)
    {
        // 유저 추가.
        if (!Join(createUser))
            return false;

        base.Create();

        return true;
    }

    public void LoadCompleted(User user)
    {
        if (null == user)
            return;

        FindGamer(user.cid)?.SetLoadCompleted();
    }

    public virtual bool Join(User user, Action? notify = null)
    {
        if(null == user)
            return false;

        user.JoinGame(this);

        if (!_playerList.ContainsKey(user.cid))
        {
            _playerList.Add(user.cid, new Player(user));
        }

        if (null == notify)
        {
            //Send(new C2G.RS_GAME_JOIN(character.cid));
        }
        else
        {
            notify?.Invoke();
        }

        return true;
    }

    public virtual void Leave(User user, Action? notify = null)
    {
        GameOut(user, GAME_OUT.LEAVE);

        if (null == notify)
        {
            Send(new C2G.RS_GAME_LEAVE(user.cid));
        }
        else
        {
            notify?.Invoke();
        }
    }

    public virtual void GameOut(User user, C2G.RQ_GAME_OUT packet)
    {
        packet.Get(out var outType, out var score);

        GameOut(user, outType, score);
    }

    public virtual void GameOut(User user, GAME_OUT outType, int score = 0)
    {
        var player = FindGamer(user.cid);
        if (null == player) return;

        player.SetGameOutType(outType);
        player.SetScore(score);
    }

    public virtual bool CheckGameOver()
    {
        // 플레이어가 없으면 종료
        if (0 == playerList.Count)
            return true;

        var players = playerList.Values;

        // 0. 타임 오버.
        {

        }

        // 1. 성공한 유저가 있는 경우 → 즉시 종료
        if (players.Any(g => g.gameOut == GAME_OUT.SUCCESS))
        {
            foreach (var player in players)
            {
                player.SetGameResult(player.gameOut == GAME_OUT.SUCCESS ? GAME_RESULT.WIN : GAME_RESULT.LOSE);
            }

            return true;
        }

        // 2. 마지막 생존자만 남은 경우 (GAME_OUT.NONE 이 1명 이하)
        var noneCount     = players.Count(g => g.gameOut == GAME_OUT.NONE);
        var finishedCount = players.Count() - noneCount;

        if (finishedCount > 0 && noneCount <= 1)
        {
            foreach (var player in players)
            {
                player.SetGameResult(player.gameOut == GAME_OUT.NONE ? GAME_RESULT.WIN : GAME_RESULT.LOSE);
            }

            return true;
        }

        // 모든 플레이가 게임이 끝났는지 확인.
        return !_playerList.Values.All(g => g.gameOut.Equals(GAME_OUT.NONE));
    }

    public virtual void Sync(C2G.RQ_GAME_SYNC packet)
    {

    }

    public virtual bool UseItem(User user, Item item, Action<Item> useItemAction)
    {
        if (null == item)
            return false;

        var skillData = T_SkillData.Get(item.data.SkillTID);
        if (null == skillData)
        {
            Logger.ERROR(user, $"item has no skills to use... item tid( {item.tid} )");
            return false;
        }

        //return func.Invoke(user, skillData, item, useItemAction);
        return true;
    }

    protected virtual IEnumerable<RANKING_TYPE> GetRankingTypes()
    {
        // 기본값 없으면 빈 리스트 리턴해도 됨
        return Array.Empty<RANKING_TYPE>();
    }

    public Player FindGamer(long cid) 
    {
        if (!_playerList.TryGetValue(cid, out var player))
            return null;

        return player;
    }

    public void SetResult(Player player, GAME_RESULT result, int score)
    {

    }

    public void Send(C2GPacket packet, long excludeCid = 0)
    {
        foreach (var v in _playerList.Values)
        {
            if (excludeCid.Equals(v.cid))
                continue;

            UserManager.Get.FindFromCid(v.cid)?.Send(packet);
        }
    }

    public void Send(C2GPacketEx packet, long excludeCid = 0)
    {
        foreach (var v in playerList.Values)
        {
            if (excludeCid.Equals(v.cid))
                continue;

            UserManager.Get.FindFromCid(v.cid)?.Send(packet);
        }
    }

    public void BroadAllUser(C2GPacket packet)
    {
        Network.SendAll(packet);
    }

    public void BroadAllUser(C2GPacketEx packet)
    {
        Network.SendAll(packet);
    }

    public bool IsSingle()
    {
        return (1 == _playerList.Count);
    }

    public bool IsMulti()
    {
        return (false == IsSingle());
    }

    public bool IsCancel()
    {
        return IsState(GAME_STATE.READY);
    }

    public bool IsFull()
    {

		return (5 > _playerList.Count);
    }

    public Dictionary<long, Player>    playerList  => _playerList;


    private Dictionary<long, Player>   _playerList         = new Dictionary<long, Player>();            // cid, player
    private long                       _validCheckTime     = 0;

    // =================================================================================================================== 
    //
    public static int s_playCount = 0;
}


