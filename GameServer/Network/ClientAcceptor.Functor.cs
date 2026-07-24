using Common;
using Newtonsoft.Json;

// 함수.
public delegate void ClientMassage<P>(int netId, P buf);
public delegate void ClientUserMassage<P>(int netId, User? user, P buf);

abstract class ClientBaseFunc
{
    internal abstract void Message(int netId, User? user, byte[] buf);
};

class ClientFunc<P> : ClientBaseFunc where P : HNET.Packet, new()
{
    public ClientFunc(ClientMassage<P> func)
    {
        _func = func;
    }

    public ClientFunc(ClientUserMassage<P> func)
    {
        _funcUser = func;
    }

    internal override void Message(int netId, User? user, byte[] buf)
    {
        P packet = PacketConvert.ToPacket<P>(buf);
        if (null == packet) return;

        uint jobId = user?.GetJobId() ?? ++_jobSeqId;

        HNET.JOB.Post(jobId, () =>
        {
            if (Program.IsLogPacket(packet.Type()))
            {
                Logger.PacketLog("▶--", packet, netId);
            }
      
            if (null == _func)
            {
                if (null == user)
                {
                    Logger.ERROR($"not exist job user... netId( {netId} ),  packet( {packet} )");

                    Network.Send(netId, new C2G.RS_RESULT_CODE(RESULT_CODE.FAIL));
                    Network.Disconnect(netId);
                    return;
                }

                _funcUser(netId, user, packet);
            }
            else
            {
                _func(netId, packet);
            }
        });
    }

    private uint                  _jobSeqId   = 0;
    private ClientMassage<P>      _func       = null;
    private ClientUserMassage<P>  _funcUser   = null;
};


public class ClientMessageFunctor
{
    public ClientMessageFunctor()
    {
        for (int i = 0; i < HNET.Packet.MAX_TYPE; ++i)
            _funcs[i] = null;
    }

    public void RegMessage<P>(ClientUserMassage<P> func) where P : HNET.Packet, new()
    {
        P packet = new P();

        if (false == packet.GetType().IsLayoutSequential)
        {
            Logger.CRITICAL($"cannot be marshaled as an unmanaged structure= '{packet}'");
            return; 
        }

        try
        {
            ClientFunc<P> f = new ClientFunc<P>(func);
            _funcs[packet.Type()] = f;
        }
        catch (Exception e)
        {
            Logger.EXCEPTION(e);
        }
    }

    public void RegMessage<P>(ClientMassage<P> func) where P : HNET.Packet, new()
    {
        P packet = new P();

        if (false == packet.GetType().IsLayoutSequential)
        {
            Logger.CRITICAL($"cannot be marshaled as an unmanaged structure= '{packet}'");
            return;
        }

        try
        {
            ClientFunc<P> f = new ClientFunc<P>(func);
            _funcs[packet.Type()] = f;
        }
        catch (Exception e)
        {
            Logger.EXCEPTION(e);
        }
    }

    public bool MessageFunc(int netId, byte[] buf)
    {
        ushort type = BitConverter.ToUInt16(buf, HNET.Packet.OFFSET_TYPE);
        try
        {
            if (HNET.Packet.MAX_TYPE <= type || null == _funcs[type])
                return false;

            long tick = Environment.TickCount;
            {
                _funcs[type].Message(netId, UserManager.Get.FindFromNetId(netId), buf);
            }
            var elapsed = (Environment.TickCount - tick);
            if (100 < elapsed)
            {
                Performance.MaxDBLatencyWarningCount = Math.Max(++Performance.DBLatencyWarningCount, Performance.MaxDBLatencyWarningCount);

                Performance.MaxDBLatency = Math.Max(elapsed, Performance.MaxDBLatency);

                Logger.WARNING_PRINT($"＜Latency＞ -  packet type( {type} ), ● elapsed tick( {elapsed} )");
            }
        }
        catch(Exception e)
        {
            Logger.EXCEPTION(e, $"failed to message= {type}");
        }

        return true;
    }

    private ClientBaseFunc[] _funcs = new ClientBaseFunc[HNET.Packet.MAX_TYPE];
}


internal partial class ClientAcceptor : HNET.Acceptor
{
    protected void RegMessage<P>(ClientUserMassage<P> func) where P : HNET.Packet, new()
    {
        _functor.RegMessage(func);
    }

    protected void RegMessage<P>(ClientMassage<P> func) where P : HNET.Packet, new()
    {
        _functor.RegMessage(func);
    }

    ClientMessageFunctor _functor = new ClientMessageFunctor();
}
