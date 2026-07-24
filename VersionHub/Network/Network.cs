

internal class Network
{
    private static readonly Lazy<Network> _instance = new Lazy<Network>(() => new Network());
    public static Network Instance { get { return _instance.Value; } }

    public static bool Initialize()
    {
        if (HNET.RESULT.FAIL == Instance._clientAcceptor.Listen(ServerConfig.C2V.port))
        {
            Logger.CRITICAL("Failed to client acceptor....");
            return false;
        }

        return true;
    }

    private ClientAcceptor _clientAcceptor = new ClientAcceptor();
}
