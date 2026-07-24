

using Common;
using Microsoft.VisualBasic.ApplicationServices;
using System.Numerics;

public class Player
{
    public Player(User user)
    {
        _user = user;
    }

    public void Join(Game game)
    {
        _user?.JoinGame(game);
    }

    public void Leave()
    {
        _user?.LeaveGame();
    }

    public void SetScore(int score)
    {
        _score = Math.Max(score, _score);
    }

    public void SetGameOutType(GAME_OUT gameOut)
    {
        _gameOut = gameOut;
    }

    public GAME_RESULT GetGameResult()
    {
        return _gameResult;
    }

    public void SetGameResult(GAME_RESULT gameResult)
    {
        _gameResult = gameResult;
    }

    public void SetLoadCompleted()
    {
        _loadCompleted = true;
    }

    public bool IsLoadCompleted()
    {
        return _loadCompleted;
    }

    public User             user            => _user;
    public long             uid             => _user.uid;
    public uint             key             => _user.key;
    public long             cid             => _user.cid;
    public string           nickname        => _user.nickname;
    public int              profileTid      => _user.profileTid;
    public bool             loadCompleted   => _loadCompleted;
    public int              score           => _score;
    public GAME_OUT         gameOut         => _gameOut;
    public GAME_RESULT      gameResult      => _gameResult;

    private User            _user           = null;
    private bool            _loadCompleted  = false;
    private int             _score          = 0;
    public GAME_OUT         _gameOut        = GAME_OUT.NONE;
    public GAME_RESULT      _gameResult     = GAME_RESULT.NONE;
}
