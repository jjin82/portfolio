

using HNET;

internal class GameAcceptor : HNET.Acceptor
{
    public override void OnConnected(int netId)
    {
        Logger.INFO_PRINT($"[ Network ] Accepted game server. [Game] - netId( {netId} ), IP( {GetRemoteHost(netId)?._host} )");
    }

    public override void OnDisconnected(int netId)
    {
        Logger.CRITICAL_PRINT($"[ Network ] Disconnected game server... [Game] - netId( {netId} )");

        if (false == ServerManager.Instance.Del(netId, out ServerInfo disconnectServerInfo))
            return;

        // 서버 UI 정보 업데이트.
        Program.UpdateServerInfo(disconnectServerInfo);
    }

    public override void OnRegMessage()
    {
        RegMessage<G2F.RQ_HEARTBEAT>(RQ_HEARTBEAT);
        RegMessage<G2F.RQ_RELAY_EX>(RQ_RELAY_EX); 

        RegMessage<G2F.RQ_GAME_SERVER_INFO>(RQ_GAME_SERVER_INFO);
        RegMessage<G2F.RQ_USER_COUNT_INFO>(RQ_USER_COUNT_INFO);
        RegMessage<G2F.RQ_GAME_DATA_HASH>(RQ_GAME_DATA_HASH);
    }

    public override void OnException(Exception ex)
    {
        Logger.EXCEPTION(ex);
    }

    void RQ_HEARTBEAT(int netId, G2F.RQ_HEARTBEAT packet)
    {
        Logger.PacketLog("--◀", packet);
    }

    void RQ_RELAY_EX(int netId, G2F.RQ_RELAY_EX packet)
    {
        //string s = "";
        //packet.Out(ref s);

        Send(netId, new G2F.RS_RELAY_EX());
    }

    void RQ_GAME_SERVER_INFO(int netId, G2F.RQ_GAME_SERVER_INFO packet)
    {
        Logger.PacketLog("--◀", packet);

        // 서버 메니져에 추가.
        ServerManager.Instance.Set(netId, packet.info);

        // 서버 UI 정보 업데이트.
        Program.UpdateServerInfo(packet.info);
    }

    private void RQ_USER_COUNT_INFO(int netId, G2F.RQ_USER_COUNT_INFO packet)
    {
        Logger.PacketLog("--◀", packet);

        // 서버 메니져에 정보 업데이트.
        ServerManager.Instance.UpdateUserCount(packet.serverId, packet.userCount);

        // 서버 리스트 UI 업데이트.
        Program.UpdateServerUserCount(packet.serverId, packet.userCount);
    }

    private void RQ_GAME_DATA_HASH(int netId, G2F.RQ_GAME_DATA_HASH packet)
    {
        Logger.PacketLog("--◀", packet);

        // 서버 리스트 UI 업데이트.
        Program.UpdateServerGameDataHash(packet.serverId, packet.hash);
    }
}
