


using Common;

internal partial class ClientAcceptor : HNET.Acceptor
{
    void RQ_KEEP_ALIVE(int netId, User? user, C2G.RQ_KEEP_ALIVE packet)
    {
        user?.SetLifeTime();
    }
    
    void RQ_WITHDRAW_ACCOUNT(int netId, User? user, C2G.RQ_WITHDRAW_ACCOUNT packet)
    {
        user?.Withdraw();
    }

    void RQ_LOGIN(int netId, C2G.RQ_LOGIN packet)
    {
        packet.Get(out var version, out var token, out var loginType, out var advertisingId);

        // 버전 체크
        if(false == version.Equals(ServerConfig.MainVersion()))
        {
            Network.SendResultCode(netId, RESULT_CODE.NOTIFY_APP_UPDATE);
            return;
        }

        // 토큰 인증 요청.
        WebManager.Get.RequestLogin(netId, token ?? "", advertisingId ?? "", loginType);
    }

    void RQ_LOGIN_OK(int netId, User? user, C2G.RQ_LOGIN_OK packet)
    {
        user?.LoginOK();
    }

    void RQ_USER_PUSH_TOKEN(int netId, User? user, C2G.RQ_USER_PUSH_TOKEN packet)
    {
        packet.Out(out string? pushToken);
        
        user?.SetPushToken(pushToken);
    }
    
    void RQ_USER_PROFILE(int netId, User? user, C2G.RQ_USER_PROFILE packet)
    {
        user?.ChangeProfile(packet.nickname, packet.profileTid);
    }

    void RQ_CHARACTER_FRIEND_REQUEST(int netId, User? user, C2G.RQ_CHARACTER_FRIEND_REQUEST packet)
    {
        packet.Get(out var nickname);

        user?.RequestFriend(nickname);
    }

    void RQ_CHARACTER_FRIEND_CANCEL(int netId, User? user, C2G.RQ_CHARACTER_FRIEND_CANCEL packet)
    {
        user?.CancelFriend(packet.cid);
    }

    void RQ_CHARACTER_FRIEND_RESPONSE(int netId, User? user, C2G.RQ_CHARACTER_FRIEND_RESPONSE packet)
    {
        if (packet.accepted)
        {
            user?.AcceptFriend(packet.cid);
        }
        else
        {
            user?.CancelFriend(packet.cid);
        }
    }

    void RQ_CHARACTER_DEL(int netId, User? user, C2G.RQ_CHARACTER_DEL packet)
    {
        user?.DelCharacter(packet.cid);
    }

    void RQ_SHOP_IAP(int netId, User? user, C2G.RQ_SHOP_IAP packet)
    {
        packet.Get(out var store, out string receipt, out string productId);

        // 인앱 결제.
        user?.InAppPurchase(store, receipt, productId);
    }

    void RQ_SHOP_BUY_PRODUCT(int netId, User? user, C2G.RQ_SHOP_BUY_PRODUCT packet)
    {
        // 상품 구매.
        user?.BuyProduct(packet.shopTid, packet.count);
    }

    void RQ_MAIL_OPEN(int netId, User? user, C2G.RQ_MAIL_OPEN packet)
    {
        // 메일 오픈.
        user?.OpenMail(packet.mailId);
    }

    void RQ_STORE_REVIEW(int netId, User? user, C2G.RQ_STORE_REVIEW packet)
    {
        //user.ExecuteMission(MISSION_TYPE.STORE_REVIEW);
    }

    void RQ_REPORT(int netId, User? user, C2G.RQ_REPORT packet)
    {
        packet.Get(out var kind, out var reason);

        DBManager.AddReport(user, kind, reason, (success) => 
        {
            user?.Send(new C2G.RS_REPORT(success));
        });
    }

    void RQ_AD_BEGIN(int netId, User? user, C2G.RQ_AD_BEGIN packet)
    {
        user.BeginAd(packet.type, packet.rewardToken);
    }

    void RQ_AD_END(int netId, User? user, C2G.RQ_AD_END packet)
    {
        user.EndAd(packet.rewardToken);
    }

    void RQ_DM_SEND(int netId, User? user, C2G.RQ_DM_SEND packet)
    {
        packet.Get(out var nickname, out var msg, out var parentDmId);

        user.SendDM(nickname, msg, parentDmId);
    }

    void RQ_ITEM_USE(int netId, User? user, C2G.RQ_ITEM_USE packet)
    {
        user?.UseItem(packet.itemTid);
    }

    void RQ_GAME_CREATE(int netId, User? user, C2G.RQ_GAME_CREATE packet)
    {
        GameManager.Get.Alloc(user);
    }

    void RQ_GAME_CANCEL(int netId, User? user, C2G.RQ_GAME_CANCEL packet)
    {
        GameManager.Get.Find(user.gameKey)?.Cancel(user);
    }

    void RQ_GAME_SCENE_COMPLETED(int netId, User? user, C2G.RQ_GAME_SCENE_COMPLETED packet)
    {
        GameManager.Get.Find(user.gameKey)?.LoadCompleted(user);
    }

    void RQ_GAME_OUT(int netId, User? user, C2G.RQ_GAME_OUT packet)
    {
        GameManager.Get.Find(user.gameKey)?.GameOut(user, packet);
    }

    void RQ_GAME_JOIN(int netId, User? user, C2G.RQ_GAME_JOIN packet)
    {
        GameManager.Get.Join(user, packet.isSingle);
    }

    void RQ_GAME_LEAVE(int netId, User? user, C2G.RQ_GAME_LEAVE packet)
    {
        GameManager.Get.Leave(user);
    }

    void RQ_GAME_SYNC(int netId, User? user, C2G.RQ_GAME_SYNC packet)
    {
        GameManager.Get.Find(user.gameKey)?.Sync(packet);
    }

    void RQ_TIME_REWARD_TAKE(int netId, User? user, C2G.RQ_TIME_REWARD_TAKE packet)
    {
        user.TakeTimeReward(packet.rewardToken);
    }

    void RQ_RANKING(int netId, User? user, C2G.RQ_RANKING packet)
    {
        RankingManager.Get.SendRanking(packet.type, user);
    }

    void RQ_MISSION_REWARD(int netId, User? user, C2G.RQ_MISSION_REWARD packet)
    {
        user.RewardMission(packet.missionTid);
    }
}
 