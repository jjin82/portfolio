

using C2G;
using Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

public partial class GameManager : BaseManager<GameManager>
{
    public override bool Initialize()
    {
        return true;
    }

    public void Alloc(User user)
    {
        if (!user.CanCreateGame())
        {
            user.SendResultCode(RESULT_CODE.FAIL_GAME_ALREADY_RUNNING);
            return;
        }

        var newGame = new Game();
        if (null != newGame) return;

        newGame?.Create(user);
    }

    public void Join(User user, bool single)
    {
        if (single)
        {
            Alloc(user);
            return;
        }

        // 유저의 빈자리가 있는 게임 리스트업
        var gameList = _games.Values.Where(x => x.IsFull()).ToList();
        if (0 == gameList.Count)
        {
            Alloc(user);
            return;
        }

        // 랜덤 선택
        var targetGame = gameList[UTIL.GetRandom(gameList.Count)];

        // 선택된 방에 참가
        targetGame.Join(user);
    }

    public void Leave(User user)
    {
        Find(user.gameKey)?.Leave(user);
    }

    public bool Add(Game newGame)
    {
        return _games.TryAdd(newGame.key, newGame);
    }

    public bool Del(uint gameKey)
    {
        return _games.TryRemove(gameKey, out var game);
    }

    public Game? Find(uint gameKey)
    {
        if (false == _games.TryGetValue(gameKey, out var game))
            return null;

        return game;
    }

    public void Logout(User user)
    {
        if (null == user)
            return;

        Find(user.gameKey)?.GameOut(user, GAME_OUT.LEAVE);
    }

    public int GetCount()
    {
        return _games.Count;
    }

    private ConcurrentDictionary<uint, Game> _games = new ConcurrentDictionary<uint, Game>();
}


