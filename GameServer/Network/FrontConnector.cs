

internal class FrontConnector : HNET.Connector
{
    public override void OnConnected()
    {
        Logger.INFO_PRINT("[ Network ] Connected front server. [Front]");

        Network.Send(new G2F.RQ_GAME_SERVER_INFO(ServerConfig.GetServerInfo()));
    }

    public override void OnReconnected()
    {
        Logger.INFO_PRINT("[ Network ] Reconnected front server. [Front]");

        Network.Send(new G2F.RQ_GAME_SERVER_INFO(ServerConfig.GetServerInfo()));
    }

    public override void OnDisconnected()
    {
        Logger.CRITICAL_PRINT("[ Network ] Disconnected front server... [Front]");
    }

    public override void OnRegMessage()
    {
        RegMessage<G2F.RS_RELAY_EX>(RS_RELAY_EX);
    }

    void RS_RELAY_EX(G2F.RS_RELAY_EX packet)
    {

    }

    public override void OnException(Exception ex)
    {
        Logger.EXCEPTION(ex);
    }
}
