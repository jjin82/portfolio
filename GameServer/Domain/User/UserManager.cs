

using Common;
using CommonStruct;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using static Google.Protobuf.WellKnownTypes.Field.Types;


internal partial class UserManager : BaseManager<UserManager>
{
    public override bool Initialize()
    {

        return true;
    }

    

    public void PrepareLogin(LOGIN_TYPE loginType, int netId, string pid, string email, string accessToken)
    {
        // 로그인 처리 중인지 확인 및 등록. (5초)
        {
            if (_userPendingList.TryGetValue(pid, out var fileTime))
            {
                if (fileTime > DateTime.Now.ToFileTime())
                    return;
            }
            _userPendingList.TryAdd(pid, DateTime.Now.AddSeconds(5).ToFileTime());
        }
        
        // DB 로그인 처리.
        DBManager.Login(loginType, netId, pid, email, accessToken);
    }

    public bool Login(User user)
    {
        // 로그인 처리 중 등록이 되어 있는지 확인.
        if (false == _userPendingList.TryGetValue(user.pid, out var _))
        {
            Logger.ERROR(user, $"not exist pending user...");
            return false;
        }

        // 등록.
        if (false == Add(user))
            return false;

        // 로그인 처리 중 제거.
        _userPendingList.TryRemove(user.pid, out var _);

        // 총 로그인 유저수 기록.
        Interlocked.Increment(ref _accumulatedLogin);

        Logger.INFO(user, $"■ login~ OK.");
        
        return true;
    }

    public void Logout(User user)
    {
        if (null == user) 
            return;

        // 접속 종료 netId 백업.
        var disconnectNetId = user.netId;

        Del(user);

        // client 접속 종료.
        Network.Disconnect(disconnectNetId);

        Logger.INFO(user, $"□ logout... netId( {disconnectNetId} )");
    }

    public bool Add(User user)
    {
        lock(this)
        {
            // nickname.
            _nicknameList.AddOrUpdate(user.nickname, user.netId, (key, oldValue) => oldValue = user.netId);

            // uid.
            _uidList.AddOrUpdate(user.uid, user.netId, (key, oldValue) => oldValue = user.netId);

            // cid.
            _cidList.AddOrUpdate(user.cid, user.netId, (key, oldValue) => oldValue = user.netId);

            // pid
            _pidList.AddOrUpdate(user.pid, user.netId, (key, oldValue) => oldValue = user.netId);

            // netId.
            _netIdList.AddOrUpdate(user.netId, user, (key, oldValue) => oldValue = user);
        }

        // 최대 접속자 수 기록.
        _maximumConcurrentUser = Math.Max(GetMCU(), GetCCU());

        return true;
    }

    public void Del(User? user)
    {
        if (null == user)
        {
            Logger.CRITICAL("DelUser... user is null");
            return;
        }

        lock(this)
        {
            if (false == _netIdList.TryRemove(user.netId, out var _))
            {
                Logger.ERROR($"DelUser(netId)... pid( {user.pid} ), netId( {user.netId} ). _netIdList User remove fail.");
            }

            if (false == _pidList.TryRemove(user.pid, out int _))
            {
                Logger.ERROR($"DelUser(pid)... pid( {user.pid} ). _pidList User remove fail.");
            }

            if (false == _uidList.TryRemove(user.uid, out int _))
            {
                Logger.ERROR($"DelUser(uid)... pid( {user.pid} ), uid( {user.uid} ). _uidList User remove fail.");
            }

            if (false == _cidList.TryRemove(user.cid, out int _))
            {
                Logger.ERROR($"DelUser(cid)... pid( {user.pid} ), cid( {user.cid} ). _cids User remove fail.");
            }

            if (false == _nicknameList.TryRemove(user.nickname, out int _))
            {
                Logger.ERROR($"DelUser(userKey)... pid( {user.pid} ). _uidList User remove fail.");
            }
        }
    }

    public bool Relogin(int netId, User user)
    {
        if(null == user)
        {
            Logger.CRITICAL("DelUser... user is null");
            return false;
        }

        lock (this)
        {
            // netId 삭제.
            if (false == _netIdList.TryRemove(user.netId, out var _))
            {
                Logger.ERROR($"Relogin....  pid( {user.pid} ), netId( {user.netId} ). _netIdList user remove fail.");
            }

            // nickname.
            _nicknameList.AddOrUpdate(user.nickname, netId, (key, oldValue) => oldValue = netId);

            // uid.
            _uidList.AddOrUpdate(user.uid, netId, (key, oldValue) => oldValue = netId);

            // cid.
            _cidList.AddOrUpdate(user.cid, netId, (key, oldValue) => oldValue = netId);
            
            // pid
            _pidList.AddOrUpdate(user.pid, netId, (key, oldValue) => oldValue = netId);

            // netId.
            _netIdList.AddOrUpdate(netId, user, (key, oldValue) => oldValue = user);
        }

        // 재 로그인.
        user.Relogin(netId);

        return true;
    }

    public User? FindFromNetId(int netId)
    {
        if (false == _netIdList.TryGetValue(netId, out User? user))
            return null;

        return user;
    }

    public User? FindFromPID(string PID)
    {
        if (false == _pidList.TryGetValue(PID, out int netId))
            return null;

        return FindFromNetId(netId);
    }

    public User? FindFromUid(long uid)
    {
        if (false == _uidList.TryGetValue(uid, out int netId))
            return null;

        return FindFromNetId(netId);
    }

    public User? FindFromCid(long cid)
    {
        if (false == _cidList.TryGetValue(cid, out int netId))
            return null;

        return FindFromNetId(netId);
    }

    public User? FindFromNickname(string nickname)
    {
        if (false == _nicknameList.TryGetValue(nickname, out int netId))
            return null;

        return FindFromNetId(netId);
    }

    public void Send(long uid, C2GPacket packet)
    {
        FindFromUid(uid)?.Send(packet);
    }

    public void SendCid(long cid, C2GPacket packet)
    {
        FindFromCid(cid)?.Send(packet);
    }

    public void Send(long uid, C2GPacketEx packet)
    {
        FindFromUid(uid)?.Send(packet);
    }

    public void SendAll (C2GPacket packet)
    {
        foreach (var e in _netIdList.ToList())
        {
            e.Value?.Send(packet);
        }
    }

    public void SendAll(C2GPacketEx packet)
    {
        foreach (var e in _netIdList.ToList())
        {
            e.Value?.Send(packet);
        }
    }

    public void SendAll(Action<User> action)
    {
        foreach (var e in _netIdList.ToList())
        {
            action?.Invoke(e.Value);
        }
    }

    public int GetCCU() // Concurrent connected User
    {
        return _netIdList.Count;
    }

    public int GetMCU() // Maximum Concurrent User
    {
        return _maximumConcurrentUser;
    }

    public long GetAccumulatedLogin()
    {
        return _accumulatedLogin;
    }

    public int  _maximumConcurrentUser = 0;
    public long _accumulatedLogin      = 0;

    private ConcurrentDictionary<int,    User>  _netIdList      = new ConcurrentDictionary<int,    User>();            // key: netId,      value: User
    private ConcurrentDictionary<string, int>   _pidList        = new ConcurrentDictionary<string, int>();             // key: pid,        value: netId
    private ConcurrentDictionary<long,   int>   _uidList        = new ConcurrentDictionary<long,   int>();             // key: uid,        value: netId
    private ConcurrentDictionary<long,   int>   _cidList        = new ConcurrentDictionary<long,   int>();             // key: cid,        value: netId
    private ConcurrentDictionary<string, int>   _nicknameList   = new ConcurrentDictionary<string, int>();             // key: nickname,   value: netId

    // 로그인 펜딩
    private ConcurrentDictionary<string, long> _userPendingList = new ConcurrentDictionary<string, long>();         // key: pid, value: fileTime
}
