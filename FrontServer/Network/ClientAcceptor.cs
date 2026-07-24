

using Common;
using C2F;

internal class ClientAcceptor : HNET.Acceptor
{
    public override void OnConnected(int netId)
    {
        Logger.INFO($"[ Network ] Accepted client. - netId( {netId} ), IP( {GetRemoteHost(netId)?._host} )");
    }

    public override void OnDisconnected(int netId)
    {
        Logger.INFO($"[ Network ] Disconnected client...   netId( {netId} )");
    }

    public override void OnRegMessage()
    {
        RegMessage<C2F.RQ_GAME_SERVER_ADDRESS_GET>(RQ_GAME_SERVER_ADDRESS_GET);

    }

    public override void OnException(Exception ex)
    {
        Logger.EXCEPTION(ex);
    }

    void RQ_GAME_SERVER_ADDRESS_GET(int netId, C2F.RQ_GAME_SERVER_ADDRESS_GET packet)
    {
        Logger.PacketLog("▶--", packet);

        try
        {
            // 적절한 서버 정보 찾아서 전송.
            ServerInfo? gameServerInfo = ServerManager.Instance.GetConnectGameServerInfo();
            if (null == gameServerInfo)
            {
                Logger.CRITICAL("★ not found Game Server.!!!");
                Send(netId, new C2F.RS_RESULT_CODE(RESULT_CODE.FAIL_SERVER_MAINTENANCE));
                return;
            }

            Network.Send(netId, new C2F.RS_GAME_SERVER_ADDRESS_GET
            {
                host = gameServerInfo._host,
                port = gameServerInfo._port,
            });
        }
        finally
        {
            //Disconnect(netId);
        }
    }
}
