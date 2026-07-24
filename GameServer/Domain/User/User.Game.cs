
using Common;
using Microsoft.VisualBasic.ApplicationServices;
using NerdFox.http;
using System.Security.Cryptography;
using System.Windows.Forms;
using static Mysqlx.Notice.Warning.Types;


public partial class User : Entity
{
    public void OnGameStart(Game game)
    {
        ExecuteMission(MISSION_TYPE.GAME_PLAY);

        if (game.IsSingle())
        {
            ExecuteMission(MISSION_TYPE.GAME_PLAY_SINGLE);
        }

        if (game.IsMulti())
        {
            ExecuteMission(MISSION_TYPE.GAME_PLAY_MULTI);
        }
    }

    public void OnGameResult(GAME_RESULT result, int score)
    {
        ResetCooltimeItem();

        // 게임 결과 패킷.
        var packet = new C2G.RS_GAME_END();
        packet.Set(result, score);
        Send(packet);
    }

    public void JoinGame(Game game)
    {
        if (null == game)
            return;

        _gameKey = game.key;
    }

    public void LeaveGame()
    {
        _gameKey = 0;
    }

    public bool IsGameRunning()
    {
        return (0 != _gameKey);
    }

    public bool CanCreateGame()
    {
        return !IsGameRunning();
    }

    public uint gameKey => _gameKey;
    public uint _gameKey = 0;
}




