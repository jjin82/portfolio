
using CommonStruct;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class G2FPacket : HNET.Packet
{
    public G2FPacket(ushort type) : base(type)
    {
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class G2FPacketEx : HNET.PacketEx
{
    public G2FPacketEx(ushort type) : base(type)
    {
    }
}

namespace G2F
{
    public enum TYPE : ushort
    {
        RQ_HEARTBEAT            = 1,
        RQ_RELAY_EX,
        RS_RELAY_EX,
        RS_SYSTEM_SETTING,  

        RQ_GAME_SERVER_INFO     = 1000,
        RS_GAME_SERVER_OPEN,
        RS_GAME_SERVER_CLOSE,
        RS_GAME_SERVER_LIST,

        RQ_USER_COUNT_INFO,
        
        RQ_GAME_DATA_HASH,

        RS_DATA_RELOAD,

        RS_WHITE_USER_LIST,
    };
}

namespace G2F
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class RQ_HEARTBEAT : G2FPacket
    {
        public RQ_HEARTBEAT() : base((ushort)TYPE.RQ_HEARTBEAT)
        {

        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class RQ_RELAY_EX : G2FPacketEx
    {
        public RQ_RELAY_EX() : base((ushort)TYPE.RQ_RELAY_EX)
        {

        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class RS_RELAY_EX : G2FPacketEx
    {
        public RS_RELAY_EX() : base((ushort)TYPE.RS_RELAY_EX)
        {

        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class RS_SYSTEM_SETTING : G2FPacket
    {
        public RS_SYSTEM_SETTING() : base((ushort)TYPE.RS_SYSTEM_SETTING)
        {

        }

        public bool consolePrint = false; 
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class RQ_GAME_SERVER_INFO : G2FPacket
    {
        public RQ_GAME_SERVER_INFO() : base((ushort)TYPE.RQ_GAME_SERVER_INFO)
        {

        }

        public RQ_GAME_SERVER_INFO(ServerInfo info) : base((ushort)TYPE.RQ_GAME_SERVER_INFO)
        {
            this.info = info;
        }

        public ServerInfo info = new ServerInfo();
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class RS_GAME_SERVER_LIST : G2FPacketEx
    {
        public RS_GAME_SERVER_LIST() : base((ushort)TYPE.RS_GAME_SERVER_LIST)
        {

        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class RQ_USER_COUNT_INFO : G2FPacket
    {
        public RQ_USER_COUNT_INFO() : base((ushort)TYPE.RQ_USER_COUNT_INFO)
        {

        }

        public int serverId  = 0;
        public int userCount = 0;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class RQ_GAME_DATA_HASH : G2FPacket
    {
        public RQ_GAME_DATA_HASH() : base((ushort)TYPE.RQ_GAME_DATA_HASH)
        {

        }

        public int          serverId    = 0;
        public String128    hash        = "";
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class RS_DATA_RELOAD : G2FPacket
    {
        public RS_DATA_RELOAD() : base((ushort)TYPE.RS_DATA_RELOAD)
        {

        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class RS_WHITE_USER_LIST : G2FPacketEx
    {
        public RS_WHITE_USER_LIST() : base((ushort)TYPE.RS_WHITE_USER_LIST)
        {

        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class RS_GAME_SERVER_OPEN : G2FPacket
    {
        public RS_GAME_SERVER_OPEN() : base((ushort)TYPE.RS_GAME_SERVER_OPEN)
        {

        }

        public int openVersion = 0;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class RS_GAME_SERVER_CLOSE : G2FPacket
    {
        public RS_GAME_SERVER_CLOSE() : base((ushort)TYPE.RS_GAME_SERVER_CLOSE)
        {

        }
    }
}