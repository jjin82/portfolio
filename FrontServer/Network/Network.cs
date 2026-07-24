

internal class Network
{
    private static readonly Lazy<Network> _instance = new Lazy<Network>(() => new Network());
    public static Network Instance { get { return _instance.Value; } }

    public static bool Initialize()
    {
        if (HNET.RESULT.FAIL == Instance._gameAcceptor.Listen(ServerConfig.G2F.port))
        {
            Logger.CRITICAL("Failed to game server acceptor....");
            return false;
        }

        if (HNET.RESULT.FAIL == Instance._clientAcceptor.Listen(ServerConfig.C2F.port))
        {
            Logger.CRITICAL("Failed to client acceptor....");
            return false;
        }

        return true;
    }

    public static int GetServerConnectedCount()
    {
        return Instance._gameAcceptor.GetAcceptedCount();
    }

    public static void Send(int netid, C2FPacket packet)
    {
        Instance._clientAcceptor.Send(netid, packet);

        Logger.PacketLog("--¢¸", packet);
    }

    public static void Send(int netid, G2FPacket packet)
    {
        Instance._gameAcceptor.Send(netid, packet);

        Logger.PacketLog("--¢¸", packet);
    }

    public static void SendAll(G2FPacket packet)
    {
        Instance._gameAcceptor.SendAll(packet);

        Logger.PacketLog("--¢¸", packet);
    }

    public static void SendAll(G2FPacketEx packet)
    {
        Instance._gameAcceptor.SendAll(packet);

        Logger.PacketLog("--¢¸", packet);
    }


    private ClientAcceptor _clientAcceptor = new ClientAcceptor();
    private GameAcceptor   _gameAcceptor   = new GameAcceptor();
}
