

using C2G;
using Common;
using Microsoft.VisualBasic.Devices;
using NerdFox.http;
using NerdFox.http.define;
using System.Collections.Concurrent;
using System.ComponentModel;

public partial class GameConnector : HNET.Connector
{
    public GameConnector(int index, string token)
    {
        _index = index;
        _token = token;
    }

    public override void OnConnected()
    {
        //Logger.INFO_PRINT("[ Network ] Connected game server. [game]");

        if (false == s_clientList.TryAdd(_index, this))
        {
            Logger.INFO_PRINT($"Failed to s_clientList.TryAdd()...");
            return;
        }

        // 웹 커넥션. (개발 or 라이브 분기)
        var connector = NerdFoxConnector.Create(_index.ToString());
        if (null != connector)
        {
            connector.CtxPath = ServerConfig.GetNerdFoxHost();
        }

        // send request.
        NerdFoxPacket recvPacket = connector.Send(NERDFOX_API.GUEST_LOGIN);
        if (recvPacket.IsError())
        {
            Logger.ERROR($"failed to login... errorCode( {recvPacket.GetErrorCode()} ), errorMessage( {recvPacket.GetErrorMessage()} )");
            Disconnect();
            return;
        }

        var _transactionToken = recvPacket.GetResponseString("transaction_token");

        Send(new C2G.RQ_LOGIN(Define.MAIN_VERSION, _transactionToken, LOGIN_TYPE.GUEST, "dummy"));
    }

    public override void OnReconnected()
    {
        //Logger.INFO_PRINT("[ Network ] Reconnected game server. [game]");

        if (false == s_clientList.TryAdd(_index, this))
        {
            Logger.INFO_PRINT($"Failed to s_clientList.TryAdd()...");
            return;
        }

        Send(new C2G.RQ_LOGIN(Define.MAIN_VERSION, _transactionToken, LOGIN_TYPE.GUEST, "dummy"));
    }

    public override void OnDisconnected()
    {
        //Logger.CRITICAL_PRINT("[ Network ] Disconnected game server... [game]");

        // 세션 제거.
        s_clientList.TryRemove(_index, out var _);

        // 유저 제거. 
        s_userList.TryRemove(_cid, out var _);
    }

    public override void OnRegMessage()
    {
        RegMessage<C2G.RS_RESULT_CODE>(RS_RESULT_CODE); 
        RegMessage<C2G.RS_LOGIN>(RS_LOGIN);
        RegMessage<C2G.RS_RELOGIN>(RS_RELOGIN);
        
        RegMessage<C2G.RS_USER_PROFILE>(RS_USER_PROFILE);
        RegMessage<C2G.RS_USER_PUSH_TOKEN_GET>(RS_USER_PUSH_TOKEN_GET);

        RegMessage<C2G.RS_CHARACTER_LIST>(RS_CHARACTER_LIST); 
        RegMessage<C2G.RS_CHARACTER_ADD>(RS_CHARACTER_ADD); 
        RegMessage<C2G.RS_CHARACTER_UPDATE>(RS_CHARACTER_UPDATE); 
        RegMessage<C2G.RS_CHARACTER_PUSH_MESSAGE_SCHEDULE>(RS_CHARACTER_PUSH_MESSAGE_SCHEDULE); 
    }

    public bool         _login              = false;                    // 로그인 되었는지.
    public int          _index              = 0;
    public ulong        _userKey            = 0;                        // user key
    public long         _cid                = 0;                        // user cid
    public string       _nickname           = "";                       // nickname
    public int          _profileImgTid      = 0;
    public string       _token              = "";                       // token
    public string       _transactionToken   = "";                       // 트랜젝션 토큰

    private List<CommonStruct.CurrencyInfo> _currencyList = new List<CommonStruct.CurrencyInfo>();
    private List<CommonStruct.CharacterInfo> _characterList = new List<CommonStruct.CharacterInfo>();
}

/// 
/// request 
/// 
public partial class GameConnector : HNET.Connector
{
    void RequestCheat(string cheat, string param1, string param2, string param3, string param4, string param5)
    {
        Send(new C2G.RQ_CHEAT(cheat, param1, param2, param3, param4, param5));
    }

    void RequestUseItem()
    {

    }

    void RequestWithdraw()
    {
        Send(new C2G.RQ_WITHDRAW_ACCOUNT());
    }
}

/// 
/// static 
/// 
public partial class GameConnector : HNET.Connector
{
    public static void ConnectEx(int count)
    {
        string ip   = "127.0.0.1";
        int    port = Convert.ToInt32(Program.Port());

        Logger.INFO_PRINT($"[Network] {count} connection. ip( {ip} ), port( {port} ) ■ ■ ■");

        for (int i = 0; i < count; ++i)
        {
            int    index = s_index++;
            string token = NextToken();

            Task.Run(() =>
            {
                GameConnector client = new GameConnector(index, token);

                if (HNET.RESULT.FAIL == client.Connect(ip, port, false))
                {
                    Logger.INFO_PRINT("Failed to Connect");
                    return;
                }
            });
        }
    }

    public static void DisconnectEx()
    {
        foreach(var e in s_clientList)
        {
            e.Value.Disconnect();
        }
        s_clientList.Clear();
    }

    public static void ResetTokenSeq()
    {
        s_tokenSeq = 1;
    }

    public static int GetTokenSeq()
    {
        return s_tokenSeq;
    }

    public static string GetToken()
    {
        return "Dummy" + s_tokenSeq.ToString();
    }

    public static string NextToken()
    {
        string token = GetToken();
        
        ++s_tokenSeq;

        return token;
    }

    public static int GetSessionCount()
    {
        return s_clientList.Count;
    }

    public static int GetUserCount()
    {
        return s_userList.Count;
    }

    public static void Update()
    {
        foreach (var v in s_clientList.Values)
        {
            if (!v._login)
                continue;

            v.Send(new C2G.RQ_KEEP_ALIVE());
        }
    }

    public static void SendAll(C2GPacket packet)
    {
        foreach (var v in s_clientList.Values)
        {
            if(false == v._login)
                continue;

            v.Send(packet);
        }
    }

    public static int s_index    = 0;
    public static int s_tokenSeq = 1;
    public static ConcurrentDictionary<int,  GameConnector> s_clientList = new ConcurrentDictionary<int, GameConnector>();   // 접속된 클라이언트 (session)
    public static ConcurrentDictionary<long, long>          s_userList   = new ConcurrentDictionary<long, long>();           // 로그인 된 유저.
}