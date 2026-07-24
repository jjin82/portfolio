
using HNET;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;

public class HostInfo
{
    public string   host = "none";
    public int      port = 0;
}

public class VersionInfo
{
    public string   version     = "";
    public string   host        = "";
    public int      port        = 0;
    public bool     live        = false;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class ServerInfo
{
    public ServerInfo() { }

    public string ToText
    {
        get { return $"netId( {_netId} ), Type( {_type} ), Version( {_version} ), ID( {_Id} ), Host( {(string)_host} ), Port( {_port} ), Name( {(string)_name} ), Hash( {(string)_hash} )"; }
    }

    public int                      _netId      = 0;
    public SERVER_TYPE              _type       = SERVER_TYPE.NONE;
    public HNET.Packet.String16     _mode       = "";
    public int                      _Id         = 0;
    public HNET.Packet.String128    _host       = "";
    public int                      _port       = 0;
    public int                      _userCount  = 0;
    public HNET.Packet.String64     _version    = "";
    public bool                     _connected  = false;
    public HNET.Packet.String128    _hash       = "";
    public HNET.Packet.String128    _name       = "";
    public float                    _cpu        = 0;
    public float                    _disk       = 0;
    public float                    _memory     = 0;
    public short                    _exception  = 0;
    public short                    _critical   = 0;
    public short                    _error      = 0;
    public short                    _fail       = 0;
};

public enum MODE
{
    NONE,

    DEBUG,
    RELEASE,
}

public enum SERVICE_MODE
{
    NONE,

    DEV,
    QA,
    LIVE,
}

public enum SERVER_TYPE
{
    NONE,
    FRONT,
    GAME,
}

public partial class ServerConfig
{
    public static bool IsConsoleLog()
    {
        // 리얼에는 무조건 사용 안함.
        if (IsLive())
            return false;

        return true;
    }

    public static DateTime GetOpenUtcTime()
    {
        var openTime = Config.ToString(s_fileName, "SERVICE", "openTime");
        if(string.IsNullOrEmpty(openTime))
        {
            return default(DateTime);
        }

        return DateTime.Parse(openTime);
    }

    public static SERVER_TYPE ServerType
    {
        get 
        {
#if FRONT_SERVER
            return SERVER_TYPE.FRONT;
#elif GAME_SERVER
            return SERVER_TYPE.GAME;
#else
            return SERVER_TYPE.NONE;
#endif
        }
    }

    static MODE s_mode = MODE.DEBUG;

    public static MODE GetMode()
    {
#if DEBUG
        s_mode = MODE.DEBUG;
#else
        s_mode = MODE.RELEASE;
#endif
        return s_mode;
    }

    public static string GetGroup()
    {
        var group = Config.ToString(s_fileName, "SERVICE", "group");
        if (string.IsNullOrEmpty(group))
        {
            group = "???";
        }

        return $"  ▶ {group} ◀ ";
    }

    public static string GetNerdFoxHost()
    {
        if (IsLive())
        {
            return "https://auth.nerdfox.co.kr";
        }

        return "http://211.253.24.120:45000";
    }

    public static bool IsMode(MODE mode)
    {
        return GetMode().Equals(mode);
    }

    static SERVICE_MODE s_serviceMode = SERVICE_MODE.NONE;

    public static SERVICE_MODE GetServiceMode()
    {
        if (s_serviceMode.Equals(SERVICE_MODE.NONE))
        {
            s_serviceMode = SERVICE_MODE.DEV;

            if (Config.ToString(s_fileName, "SERVICE", "MODE").Equals("LIVE"))
            {
                s_serviceMode = SERVICE_MODE.LIVE;
            }
            if (Config.ToString(s_fileName, "SERVICE", "MODE").Equals("QA"))
            {
                s_serviceMode = SERVICE_MODE.QA;
            }
        }

        return s_serviceMode;
    }

    public static bool IsDev()
    {
        return GetServiceMode().Equals(SERVICE_MODE.DEV);
    }

    public static bool IsQA()
    {
        return GetServiceMode().Equals(SERVICE_MODE.QA);
    }

    public static bool IsLive()
    {
        return GetServiceMode().Equals(SERVICE_MODE.LIVE);
    }

    public static string Name
    {
        get { return s_name; }
        set { s_name = value; }
    }

    public static string GetGameDataHash()
    {
#if GAME_SERVER
        return GameData.Excel.Hash;
#else
        return "...";
#endif
    }

    ///
    /// '1000 단위' 번호가 있는 포트는 서버간 연결. (31000, 32000.... 51000... n)
    ///
    public class DEFAULT_PORT
    {
        public static int CLIENT_TO_VERSION     = 1111;         // 클라이언트가 버전 서버 접속 포트.

        public static int CLIENT_TO_FRONT        = 10000;        // 클라이언트가 프론트 서버 접속 포트.
        public static int GAME_TO_FRONT          = 12000;        // 게임 서버가 프론트 서버 접속 포트.

        public static int CLIENT_TO_GAME         = 20000;        // 클라가 게임서버 접속 포트.
    };

    private static string       s_fileName  = "Config/Config.ini";
    private static string       s_name      = "";
    private static int          s_ID        = -1;
    private static string       s_host      = "";

    public static int ServerId()
    {
        if (-1 == s_ID)
        {
            s_ID = Config.ToInt(s_fileName, "LOCAL", "id");
        }

        return s_ID;
    }

    public static string MainHost()
    {
#if FRONT_SERVER
        return C2F.host;
#elif GAME_SERVER
        return C2G.host;
#else
        return "";
#endif
    }

    public static int MainPort()
    {
#if FRONT_SERVER
        return C2F.port;
#elif GAME_SERVER
        return C2G.port;
#else
        return 0;
#endif
    }

    public static ushort MainVersion()
    {
#if DEBUG
        var v = (ushort)Config.ToInt(s_fileName, "SERVICE", "version");
        if (0 != v) return v;
#endif

#if FRONT_SERVER
        return 0;
#elif GAME_SERVER
        return Common.Define.MAIN_VERSION;
#else
        return 0;
#endif
    }

    public static string Version()
    {
#if GAME_SERVER
        return T_GlobalValueData.Get(GLOBAL_VALUE_TYPE.DATA_VERSION)?.ValueString ?? "";
#else
        return "";
#endif
    }

    private static string LocalHost()
    {
        if (0 == s_host.Length)
        {
            s_host = "127.0.0.1";

            s_host = Config.ToString(s_fileName, "LOCAL", "host");
            if (0 == s_host.Length)
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        s_host = ip.ToString();
                        break;
                    }
                }
            }
        }

        return s_host;
    }

    public static HostInfo C2F
    {
        get
        {
            string host = Config.ToString(s_fileName, "CLIENT_TO_FRONT", "host");
            if (0 == host.Length)
            {
                host = LocalHost();
            }

            int port = Config.ToInt(s_fileName, "CLIENT_TO_FRONT", "port");
            if (0 == port)
            {
                port = (int)DEFAULT_PORT.CLIENT_TO_FRONT;
            }

            HostInfo info = new HostInfo();
            info.host = host;
            info.port = port;

            return info;
        }
    }

    public static HostInfo G2F
    {
        get
        {
            string host = Config.ToString(s_fileName, "GAME_TO_FRONT", "host");
            if (0 == host.Length)
            {
                host = LocalHost();
            }

            int port = Config.ToInt(s_fileName, "GAME_TO_FRONT", "port");
            if (0 == port)
            {
                port = (int)DEFAULT_PORT.GAME_TO_FRONT;
            }

            HostInfo info = new HostInfo();
            info.host = host;
            info.port = port;

            return info;
        }
    }

    public static HostInfo C2G
    {
        get 
        {
            string host = Config.ToString(s_fileName, "CLIENT_TO_GAME", "host");
            if (0 == host.Length)
            {
                host = LocalHost();
            }

            int port = Config.ToInt(s_fileName, "CLIENT_TO_GAME", "port");
            if (0 == port)
            {
                port = (int)DEFAULT_PORT.CLIENT_TO_GAME;
            }

            HostInfo info = new HostInfo();
            info.host = host;
            info.port = port;

            return info;
        }
    }

    public static HostInfo C2V
    {
        get
        {
            string host = Config.ToString(s_fileName, "CLIENT_TO_VERSION", "host");
            if (0 == host.Length)
            {
                host = LocalHost();
            }

            int port = Config.ToInt(s_fileName, "CLIENT_TO_VERSION", "port");
            if (0 == port)
            {
                port = (int)DEFAULT_PORT.CLIENT_TO_VERSION;
            }

            HostInfo info = new HostInfo();
            info.host = host;
            info.port = port;

            return info;
        }
    }
};

public partial class ServerConfig
{
    public static ServerInfo GetServerInfo()
    {
        return new ServerInfo
        {
            _type       = ServerType,
            _mode       = GetMode().ToString(),
            _Id         = ServerId(),
            _host       = MainHost(),
            _port       = MainPort(),
            _version    = Version(),
            _hash       = GetGameDataHash(),
            _name       = Name,
            _connected  = false,
            _exception  = (short)HNET.LOG.ExceptionCount,
            _critical   = (short)HNET.LOG.CriticalCount,
            _error      = (short)HNET.LOG.ErrorCount,
            _fail       = (short)HNET.LOG.FailCount,
        };
    }

    public static ServerInfo GetServerPerfomaneInfo()
    {
        return new ServerInfo
        {
            _type       = ServerType,
            _mode       = GetMode().ToString(),
            _Id         = ServerId(),
            _host       = MainHost(),
            _port       = MainPort(),
            _version    = Version(),
            _hash       = GetGameDataHash(),
            _name       = Name,
            _connected  = false,
            _cpu        = Performance.Cpu(),
            _disk       = Performance.Disk(),
            _memory     = Performance.Memory(),
            _exception  = (short)HNET.LOG.ExceptionCount,
            _critical   = (short)HNET.LOG.CriticalCount,
            _error      = (short)HNET.LOG.ErrorCount,
            _fail       = (short)HNET.LOG.FailCount,
        };
    }

    // 어셈블리의 빌드 날짜 및 시간 정보 가져오기
    public static string BuildDateTime()
    {
        // 어셈블리 파일의 경로 가져오기
        string filePath = Assembly.GetExecutingAssembly().Location;

        // 파일 정보 가져오기
        FileInfo fileInfo = new FileInfo(filePath);

        // 파일의 마지막으로 쓰여진 시간을 반환
        return fileInfo.LastWriteTime.ToString();
    }
}


