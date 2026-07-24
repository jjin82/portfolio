


using Common;
using CommonStruct;
using Microsoft.VisualBasic.ApplicationServices;

public partial class User : Entity
{
    void LoadDM(List<DMInfo> dmList)
    {
        _dmList = dmList;
    }

    public void AddDM(string sendNickname, int sendProfileTid, string msg)
    {
        // dm 세팅.
        var dmInfo = new DMInfo
        {
            parentDmId      = 0,
            sendCid         = 0,
            sendNickname    = sendNickname,
            sendProfileTid  = sendProfileTid,
            recvCid         = cid,
            recvNickname    = nickname,
            recvProfileTid  = profileTid,
            msg             = msg,
            fileTime        = DateTime.UtcNow.ToFileTime()
        };

        DBManager.AddDM(uid, gameDB, ref dmInfo, () => 
        {
            Send(new C2G.RS_DM_UPDATE(dmInfo));
        });
    }

    public void SendDM(string recvNickname, string msg, long parentDmId)
    {
        // 나 자신.
        if (_nickname.Equals(recvNickname))
            return;

        var character = FindCharacter(recvNickname);
        if (null != character)
        {
            // npc에게는 쪽지를 보내지 않음.
            if (character.IsNpc())
            {
                SendResultCode(RESULT_CODE.FAIL_REQUEST_FRIEND_USER_UNAVAILABLE, recvNickname);
                return;
            }
        }

        // 금칙어 처리.
        var filteredMsg = ForbiddenWord.Get().GetMessage(msg, StringManager.Get.GetString("UI_FORBIDDEN_WORD") ?? "***", out var hasForbiddenWord);
        if (string.IsNullOrEmpty(filteredMsg)) return;
        
        // 쪽지를 받을 대상 정보 DB에서 획득.
        DBManager.GetAccountFromNickname(recvNickname, (result, recvGameDB, recvUid, recvCid, recvProfileTid) =>
        {
            if (false == result)
            {
                SendResultCode(RESULT_CODE.FAIL_REQUEST_FRIEND_USER_UNAVAILABLE, recvNickname);
                return;
            }

            // dm 세팅.
            var dmInfo = new DMInfo
            {
                parentDmId      = parentDmId,
                sendCid         = cid,
                sendNickname    = nickname,
                sendProfileTid  = profileTid,
                recvCid         = recvCid,
                recvNickname    = recvNickname,
                recvProfileTid  = recvProfileTid,
                msg             = filteredMsg,
                fileTime        = DateTime.UtcNow.ToFileTime()
            };

            if (false == DBManager.AddDM(this, recvUid, recvGameDB, ref dmInfo))
            {
                Logger.FAIL("failed to dm...");
                return;
            }

            // [나] 내 dm 업데이트.
            var packet = new C2G.RS_DM_UPDATE(dmInfo);
            Send(packet);

            // [상대] dm을 받을 유저.
            var recvCharacter = UserManager.Get.FindFromCid(recvCid);
            if (null != recvCharacter)
            {
                recvCharacter.Send(packet);
            }
            else
            {
                // 푸시 메시지.
                var pushMessageData = T_PushMessageData.Get(PUSH_MESSAGE.SEND_DM);
                if (null != pushMessageData)
                {
                    var title = string.Format(pushMessageData.Title, nickname);

                    PushManager.Get.AddPushMessage(new PushParam(dmInfo, recvUid, title, DateTime.UtcNow));
                }
            }

            ExecuteMission(MISSION_TYPE.DM_SEND);
        });
    }

    public void SendDMList()
    {
        if (0 >= _dmList.Count)
            return;

        Send(new C2G.RS_DM_LIST(_dmList));
    }

    private List<DMInfo> _dmList = new List<DMInfo>(); // DM
}


