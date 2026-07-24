

using Common;

internal class Network
{
    private static readonly Lazy<Network> _instance = new Lazy<Network>(() => new Network());
    public static Network Instance { get { return _instance.Value; } }

    public static bool Initialize()
    {
        if(HNET.RESULT.FAIL == Instance._frontConnector.Connect(ServerConfig.G2F.host, ServerConfig.G2F.port))
        {
            Logger.CRITICAL("failed to front server connector....");
            return false;
        }

        if (HNET.RESULT.FAIL == Instance._clientAcceptor.Listen(ServerConfig.C2G.port))
        {
            Logger.CRITICAL("failed to client acceptor....");
            return false;
        }

        return true;
    }

    public static void Disconnect(int netId)
    {
        Instance._clientAcceptor.Disconnect(netId);
    }

    public static int GetAccpetedCount()
    {
        return Instance._clientAcceptor.GetAcceptedCount();
    }

    public static void Send(G2FPacket packet)
    {
        Instance._frontConnector.Send(packet);

        Logger.PacketLog("¢º--", packet);
    }

    public static void Send(G2FPacketEx packet)
    {
        Instance._frontConnector.Send(packet);

        Logger.PacketLog("¢º--", packet);
    }

    public static void SendResultCode(int netId, RESULT_CODE code)
    {
        Send(netId, new C2G.RS_RESULT_CODE(code));
    }

    public static void SendResultCode(int netId, RESULT_CODE code, string param = "")
    {
        Send(netId, new C2G.RS_RESULT_CODE(code, RESULT_POPUP_TYPE.POPUP_SIMPLE_MESSAGE, param));
    }

    public static void Send(int netId, C2GPacket packet)
    {
        Instance._clientAcceptor.Send(netId, packet);

        Logger.PacketLog("--¢¸", packet, netId);
    }

    public static void Send(int netId, C2GPacketEx packet)
    {
        Instance._clientAcceptor.Send(netId, packet);

        Logger.PacketLog("--¢¸", packet, netId);
    }
    
    public static void SendAll(C2GPacket packet)
    {
        Instance._clientAcceptor.SendAll(packet);

        Logger.PacketLog("--¢¸", packet);
    }

    public static void SendAll(C2GPacketEx packet)
    {
        Instance._clientAcceptor.SendAll(packet);

        Logger.PacketLog("--¢¸", packet);
    }


    private ClientAcceptor _clientAcceptor = new ClientAcceptor();
    private FrontConnector _frontConnector = new FrontConnector();
}
