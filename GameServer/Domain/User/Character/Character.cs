

using C2G;
using Common;
using NerdFox.OpenAI.define;
using System;
using System.Reflection.Metadata.Ecma335;

public abstract partial class Character : Entity
{
    public Character(bool update = false) : base(update)
    {
    }

    public Character(long cid, bool update = false) : base(update)
    {
        _cid = cid;
    }

    public virtual void Initialize(User? owner, Param param)
    {
        _owner          = owner;
        _cid            = param.cid;
        _tid            = param.tid;
        _lovePoint      = param.lovePoint;
        _friendState    = param.friendState;
    }

    public RELATIONSHIP_TYPE GetRelationType()
    {
        RELATIONSHIP_TYPE relatinoship = RELATIONSHIP_TYPE.AWKWARDNESS;

        return relatinoship;
    }

    public void SetFriendState(FRIEND_STATE state)
    {
        _friendState = state;

        owner?.UpdateCharacter(this);
    }

    public virtual bool AddPushMessage(long roomId, string time, string message)
    {
        if(0 == roomId)
            return false;

        if (string.IsNullOrEmpty(time))
            return false;

        if (string.IsNullOrEmpty(message))
            return false;

        return true;
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

    public virtual void Send(C2GPacket packet)
    {
        owner?.Send(packet);
    }

    public virtual void Send(C2GPacketEx packet)
    {
        owner?.Send(packet);
    }

    public virtual void SendResultCode(RESULT_CODE code, RESULT_POPUP_TYPE popupType, string param = "")
    {
        owner?.SendResultCode(code, param);
    }

    public virtual void SendResultCode(RESULT_CODE code, string param = "")
    {
        owner?.SendResultCode(code, param);
    }

    public override uint GetJobId()
    {
        if (0 != gameKey)
        {
            var game = GameManager.Get.Find(gameKey);
            return game?.GetJobId() ?? key;
        }

        return key;
    }


    public virtual bool IsUser()        { return false; }
    public virtual bool IsOtherUser()   { return false; }
    public virtual bool IsNpc()         { return false; }
    public virtual bool IsHeroine()     { return false; }


    // ===================================================================================================================
    //
    public T_CharacterData data => T_CharacterData.Get(tid);            // 캐릭터 테이블 정보.
    
    public virtual CHAR_TYPE    charType    => data?.CharType ?? CHAR_TYPE.NONE;
    public virtual string       nickname    => data?.Name ?? "";
    public virtual int          profileTid  => _profileTid;


    // ===================================================================================================================
    //
    public User?            owner           => _owner;
    public long             ownerCid        => 0;
    public string           ownerNickname   => owner?.nickname ?? "";

    public virtual  long    uid             => 0;
    public long             cid             => _cid;
    public int              tid             => _tid;
    public uint             gameKey         => _gameKey;
    public FRIEND_STATE     friendState     => _friendState;


    // ===================================================================================================================
    //
    private User?           _owner          = null;
    public long             _cid            = Interlocked.Decrement(ref s_tempCid);
    public int              _tid            = 0;
    public int              _profileTid     = 0;
    public uint             _gameKey        = 0;
    public FRIEND_STATE     _friendState    = FRIEND_STATE.FRIENDS;
    public int              _lovePoint      = 0;


    // ===================================================================================================================
    //
    private static long s_tempCid = 0;
}


