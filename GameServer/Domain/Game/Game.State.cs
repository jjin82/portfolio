using Common;
using Microsoft.VisualBasic.ApplicationServices;
using System.Security.Cryptography;
using static Game;

public partial class Game
{
    public enum GAME_STATE
    {
        READY,      // 게임 시작 전
        START,      // 게임 시작
        PLAYING,    // 게임 중
        END,        // 게임 종료
        DESTROY,
    }

    public void InitializeState(int countdown)
    {
        _stateHandlers = new (Func<bool>, GAME_STATE)[]
        {
            (OnGameReady,       GAME_STATE.START),
            (OnGameStart,       GAME_STATE.PLAYING), 
            (OnGamePlaying,     GAME_STATE.END),
            (OnGameEnd,         GAME_STATE.DESTROY),
            (OnWaitDestroy,     GAME_STATE.DESTROY),
        };

        // 준비 종료 시간 설정.
        _readyEndTime = DateTime.Now.AddSeconds(countdown).ToFileTime();
    }

    private void UpdateState()
    {
        var (handler, nextState) = _stateHandlers[(int)_state];
        if (null != handler)
        {
            bool stay = handler.Invoke();
            if (!stay)
            {
                Logger.INFO($"상태 전환: {_state} → {nextState}");
                _state = nextState;
            }
        }
    }

    public void SetState(GAME_STATE newState)
    {
        _state = newState;
    }

    public virtual bool OnGameReady()
    {
        // 준비 종료 체크.
        if (_readyEndTime > DateTime.Now.ToFileTime())
            return true;

        foreach (var player in _playerList.Values)
        {
			// 클라이언트에 게임 씬로드 요청.
			player.user.Send(new C2G.RS_GAME_SCENE_LOAD());
        }
        
        return false; // 준비 끝나면 Playing으로 이동
    }

    public virtual bool OnGameStart()
    {
        // 모든 플레이어가 게임 로드가 완료 되었는가 판단.
        bool allLoadCompleted = _playerList.Values.All(g => g.IsLoadCompleted());
        if (!allLoadCompleted) return true;

        // 게임 시작.
        Send(new C2G.RS_GAME_START());

        // 참가자 알림.
        foreach (var player in _playerList.Values)
        {
            var user = UserManager.Get.FindFromCid(player.cid);
            if (null != user)
            {
                user.OnGameStart(this);
            }
            
            //var packet = new C2G.RS_ROOM_NOTIFY();
            //packet.SetExplain(roomId, StringManager.Get.GetString("GAME_PLAYER").Replace("#nickname#", player.nickname));
            //Send(packet);
        }

        return false; // 준비 끝나면 Playing으로 이동
    }

    public virtual bool OnGameEnd()
    {
        // 게임 제거까지 n초 대기.
        _destroyTime = DateTime.Now.AddSeconds(5).ToFileTime();

        foreach (var g in playerList.Values)
        {
            var resultMsg = "";
            switch (g.gameResult)
            {
                case GAME_RESULT.WIN:  resultMsg = StringManager.Get.GetString("GAME_PLAYER").Replace("#nickname#", g.nickname); break;
                case GAME_RESULT.LOSE: resultMsg = StringManager.Get.GetString("GAME_LOSER").Replace("#nickname#", g.nickname);  break;
            }

            if (!string.IsNullOrEmpty(resultMsg))
            {
                // 참가자 결과 알림.
                //var packet = new C2G.RS_ROOM_NOTIFY();
                //packet.SetExplain(roomId, resultMsg);
                //Send(packet);
            }

            // 플레이 결과 처리.
            OnGamerResult(g);
        }

        return false; // false → result으로 이동
    }

    public virtual void OnGamerResult(Player player)
    {
        var user = UserManager.Get.FindFromCid(player.cid);
        if (null == user) return;

        // 한 명 추가될 때마다 50% 증가
        double increasePercent = 50;

        // 인원수에 따라 점수 증가.
        int finalScore = (int)(player.score * (1.0 + (increasePercent / 100.0) * (playerList.Count - 1)));

        user?.OnGameResult(player.gameResult, finalScore);
    }

    public virtual bool OnGamePlaying()
    {
        if (CheckGameOver())
            return false;

        return true; // true → 계속 Playing 유지
    }

    public virtual bool OnWaitDestroy()
    {
        // 게임방 제거 체크.
        if (_destroyTime < DateTime.Now.ToFileTime())
            Destroy();

        return true;
    }

    public bool IsState(GAME_STATE state)
    {
        return _state.Equals(state);
    }


    // ===================================================================================================================
    //
    private GAME_STATE _state = GAME_STATE.READY;
    private (Func<bool> handler, GAME_STATE nextState)[] _stateHandlers;

    private long _readyEndTime = 0;
    private long _destroyTime  = 0;
	private long _endTime      = DateTime.MaxValue.ToFileTime();
}


