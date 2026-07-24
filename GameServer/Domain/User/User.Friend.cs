

using Common;
using Org.BouncyCastle.Asn1.Pkcs;
using System.Windows.Forms;

public partial class User : Entity
{
    public void RequestFriend(string targetNickname)
    {
        // 나 자신.
        if (_nickname.Equals(targetNickname))
            return;

        // 이미 친구인지 확인.
        if (null != FindCharacter(targetNickname))
        {
            Logger.FAIL(this, $"character already registered... invitee nickname( {targetNickname} )");
            return;
        }

        // 초대 대상 정보 DB에서 획득.
        DBManager.GetAccountFromNickname(targetNickname, (result, targetGameDB, targetUid, targetCid, targetProfileTid) =>
        {
            if (false == result)
            {
                SendResultCode(RESULT_CODE.FAIL_REQUEST_FRIEND_USER_UNAVAILABLE, targetNickname);
                return;
            }

            AddCharacter(targetCid, targetNickname, targetProfileTid, FRIEND_STATE.PENDING, (targetCharacter) =>
            {
                if (null == targetCharacter)
                    return;

                // 초대를 보낸 유저에게 보냈다고 알림.
                SendResultCode(RESULT_CODE.NOTIFY_REQUEST_INVITE_FRIEND, targetNickname);

                // 푸시 메시지.
                var pushMessageData = T_PushMessageData.Get(PUSH_MESSAGE.FRIEND_REQUEST);
                if (null != pushMessageData)
                {
                    PushManager.Get.AddPushMessage(new PushParam(targetUid, cid, string.Format(pushMessageData.Title, nickname), pushMessageData.Content, DateTime.UtcNow));
                }
            });
        });
    }

    public void AcceptFriend(long targetCid)
    {
        var friendChar = FindCharacter(targetCid);
        if (null == friendChar) return;

        // 친구 신청을 받은 경우만.
        if (false == friendChar.friendState.Equals(FRIEND_STATE.INVITED))
        {
            Logger.FAIL(this, $"failed to AcceptFriend()... target cid( {targetCid} ), friend state( {friendChar.friendState} )");
            return;
        }

        // 수락된 캐릭터를 친구로 상태로 변경.
        friendChar.SetFriendState(FRIEND_STATE.FRIENDS);

        // 친구가 된 유저도 친구로 상태 변경.
        var friendUser = UserManager.Get.FindFromCid(targetCid);
        if (null == friendUser)
        {
            DBManager.GetAccountFromCid(targetCid, (targetUid, targetGameDB, targetNickname, targetProfileTid) =>
            {
                DBManager.UpdateCharacter(GetJobId(), targetGameDB, targetCid, FRIEND_STATE.FRIENDS);

                // 푸시 메시지.
                var pushMessageData = T_PushMessageData.Get(PUSH_MESSAGE.FRIEND_ACCEPT);
                if (null != pushMessageData)
                {
                    PushManager.Get.AddPushMessage(new PushParam(targetUid, cid, string.Format(pushMessageData.Title, nickname), pushMessageData.Content, DateTime.UtcNow));
                }
            });
        }
        else
        {
            friendUser.Post(() =>
            {
                friendUser.FindCharacter(cid)?.SetFriendState(FRIEND_STATE.FRIENDS);

                // 친구가 되었다고 알림.
                friendUser.SendResultCode(RESULT_CODE.NOTIFY_ACCEPT_FRIEND_INVITATION, nickname);
            });
        }
    }

    public void CancelFriend(long targetCid)
    {
        DelCharacter(targetCid);
    }

    public void CheatAddFriend(string targetNickname)
    {
        // 나 자신.
        if (_nickname.Equals(targetNickname))
            return;

        // 이미 친구인지 확인.
        if (null != FindCharacter(targetNickname))
        {
            Logger.FAIL(this, $"character already registered... invitee nickname( {targetNickname} )");
            return;
        }

        // 초대 대상 정보 DB에서 획득.
        DBManager.GetAccountFromNickname(targetNickname, (result, targetGameDB, targetUid, targetCid, targetProfileTid) =>
        {
            AddCharacter(targetCid, targetNickname, targetProfileTid, FRIEND_STATE.PENDING, (newCharacter) =>
            {
                if (null == newCharacter)
                    return;

                Logger.INFO_PRINT(this, $"add friend... invitee nickname( {targetNickname} )");
            });
        });
    }
}
