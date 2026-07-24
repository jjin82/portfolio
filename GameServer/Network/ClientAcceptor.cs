

using C2G;

internal partial class ClientAcceptor : HNET.Acceptor
{
    public override void OnConnected(int netId)
    {
        Logger.INFO($"[ Network ] Accepted client. - netId( {netId} ), IP( {GetRemoteHost(netId)?._host} )");
    }

    public override void OnDisconnected(int netId)
    {
        Logger.INFO($"[ Network ] Disconnected client...   netId( {netId} )");

        // 접속이 종료되면 life time n초 세팅하여 유저가 파괴 되도록 유도.
        UserManager.Get.FindFromNetId(netId)?.Destroy(2000);
    }

    public override void OnMessage(int netId, ushort type, byte[] buf)
    {
        _functor.MessageFunc(netId, buf);
    }

    public override void OnRegMessage()
    {
        RegMessage<C2G.RQ_KEEP_ALIVE>(RQ_KEEP_ALIVE);
        RegMessage<C2G.RQ_CHEAT>(RQ_CHEAT);

        RegMessage<C2G.RQ_WITHDRAW_ACCOUNT>(RQ_WITHDRAW_ACCOUNT);

        RegMessage<C2G.RQ_LOGIN>(RQ_LOGIN);
        RegMessage<C2G.RQ_LOGIN_OK>(RQ_LOGIN_OK);

        RegMessage<C2G.RQ_USER_PUSH_TOKEN>(RQ_USER_PUSH_TOKEN); 
        RegMessage<C2G.RQ_USER_PROFILE>(RQ_USER_PROFILE);

        RegMessage<C2G.RQ_CHARACTER_DEL>(RQ_CHARACTER_DEL);

        RegMessage<C2G.RQ_CHARACTER_FRIEND_REQUEST>(RQ_CHARACTER_FRIEND_REQUEST);
        RegMessage<C2G.RQ_CHARACTER_FRIEND_CANCEL>(RQ_CHARACTER_FRIEND_CANCEL); 
        RegMessage<C2G.RQ_CHARACTER_FRIEND_RESPONSE>(RQ_CHARACTER_FRIEND_RESPONSE);
        
        RegMessage<C2G.RQ_SHOP_IAP>(RQ_SHOP_IAP);
        RegMessage<C2G.RQ_SHOP_BUY_PRODUCT>(RQ_SHOP_BUY_PRODUCT);

        RegMessage<C2G.RQ_MAIL_OPEN>(RQ_MAIL_OPEN);

        RegMessage<C2G.RQ_STORE_REVIEW>(RQ_STORE_REVIEW); 

        RegMessage<C2G.RQ_REPORT>(RQ_REPORT);

        RegMessage<C2G.RQ_AD_BEGIN>(RQ_AD_BEGIN); 
        RegMessage<C2G.RQ_AD_END>(RQ_AD_END);

        RegMessage<C2G.RQ_DM_SEND>(RQ_DM_SEND);

        RegMessage<C2G.RQ_ITEM_USE>(RQ_ITEM_USE);

        RegMessage<C2G.RQ_GAME_CREATE>(RQ_GAME_CREATE);
        RegMessage<C2G.RQ_GAME_CANCEL>(RQ_GAME_CANCEL);
        RegMessage<C2G.RQ_GAME_SCENE_COMPLETED>(RQ_GAME_SCENE_COMPLETED); 
        RegMessage<C2G.RQ_GAME_OUT>(RQ_GAME_OUT);
        RegMessage<C2G.RQ_GAME_JOIN>(RQ_GAME_JOIN); 
        RegMessage<C2G.RQ_GAME_LEAVE>(RQ_GAME_LEAVE); 
        RegMessage<C2G.RQ_GAME_SYNC>(RQ_GAME_SYNC);

        RegMessage<C2G.RQ_TIME_REWARD_TAKE>(RQ_TIME_REWARD_TAKE);

        RegMessage<C2G.RQ_RANKING>(RQ_RANKING);

        RegMessage<C2G.RQ_MISSION_REWARD>(RQ_MISSION_REWARD);
    }

    public override void OnException(Exception ex)
    {
        Logger.EXCEPTION(ex);
    }
}
 