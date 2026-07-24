


using Common;
using HNET;

internal class ClientAcceptor : HNET.Acceptor
{
    public override void OnConnected(int netId)
    {
        Logger.DEBUG($"[ Network ] Accepted client. - netId( {netId} ), IP( {GetRemoteHost(netId)?._host} )");
    }

    public override void OnDisconnected(int netId)
    {
        Logger.DEBUG($"[ Network ] Disconnected client...   netId( {netId} )");
    }

    public override void OnRegMessage()
    {
        RegMessage<C2V.RQ_VERSION_INFO_GET>(RQ_VERSION_INFO_GET);
    }

    public override void OnException(Exception ex)
    {
        Logger.EXCEPTION(ex);
    }

    void RQ_VERSION_INFO_GET(int netId, C2V.RQ_VERSION_INFO_GET packet)
    {
        Logger.PacketLog("▶--", packet);

        // 버전에 관련된 정보 전달.
        var versionInfo = VersionManager.GetVersion(packet.dataVersion);
        if (null == versionInfo)
        {
            Send(netId, new C2V.RS_RESULT_CODE(RESULT_CODE.FAIL_SERVER_MAINTENANCE));

            Logger.CRITICAL_PRINT($"not exist version... data version( {packet.dataVersion} )");
            return;
        }

        Send(netId, new C2V.RS_VERSION_INFO_GET(versionInfo.host, versionInfo.port, versionInfo.live));
    }
}
