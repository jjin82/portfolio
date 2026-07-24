

using Common;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Security.Cryptography;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

public partial class DBManager : BaseManager<DBManager>
{
    public static void AddCharacter(uint jobId, DB_KIND gameDB, long uid, Character.Param param, Action<bool, DB_KIND, long>? onResult = null)
    {
        PostSlow(jobId, () =>
        {
            bool    result              = true;
            long    getTargetUid        = 0;
            DB_KIND getTargetGameDB     = DB_KIND.GF_GAME01;
            string  getTargetNickname   = "";
            int     getTargetProfileTid = 0;

            try
            {
                // 신규 캐릭터의 경우 cid 생성 처리.
                if (param.IsNewCharacter())
                {
                    param.cid = AllocId();
                }

                using (var con = new DB.MySql(gameDB))
                {
                    con.PREPARE("INSERT INTO t_user_character(uid, cid, char_type, tid, state, love_point) VALUES(@1, @2, @3, @4, @5, @6) AS new ON DUPLICATE KEY UPDATE state = new.state, love_point = new.love_point");
                    {
                        con.SET_PARAM("@1", uid);
                        con.SET_PARAM("@2", param.cid);
                        con.SET_PARAM("@3", (sbyte)param.charType);
                        con.SET_PARAM("@4", param.tid);
                        con.SET_PARAM("@5", (sbyte)param.friendState);
                        con.SET_PARAM("@6", param.lovePoint);
                    }

                    if (false == con.EXECUTE())
                    {
                        result = false;
                        Logger.CRITICAL($"failed to AddCharacter()... uid( {uid} ), cid( {param.cid} ), charType( {param.charType} ), tid( {param.tid} ) ]");
                        return;
                    }

                    if (null != onResult)
                    {
                        GetAccountFromCid(param.cid, out getTargetGameDB, out getTargetUid, out getTargetNickname, out getTargetProfileTid);
                    }
                }
            }
            finally
            {
                onResult?.Invoke(result, getTargetGameDB, getTargetUid);
            }
        });
    }

    public static void AddCharacter(User user, Character.Param param, Action<bool, DB_KIND, long> onResult)
    {
        if (null == user)
            return;

        AddCharacter(user.GetJobId(), user.gameDB, user.uid, param, (result, targetGameDB, targetUid) =>
        {
            user.Post(() =>
            {
                onResult.Invoke(result, targetGameDB, targetUid);
            });
        });
    }

    public static void DelCharacter(uint jobId, DB_KIND gameDB, long uid, long targetCid, Action<long, DB_KIND>? onResult = null)
    {
        PostSlow(jobId, () =>
        {
            long    outTargetUid        = 0;
            DB_KIND outTargetGameDB     = DB_KIND.GF_GAME01;
            string  outTargetNickname   = "";
            int     outTargetProfileTid = 0;

            try
            {
                using (var con = new DB.MySql(gameDB))
                {
                    con.PREPARE("UPDATE t_user_character SET state=@3 WHERE uid=@1 AND cid=@2");
                    {
                        con.SET_PARAM("@1", uid);
                        con.SET_PARAM("@2", targetCid);
                        con.SET_PARAM("@3", (sbyte)FRIEND_STATE.NONE);
                    }

                    if (false == con.EXECUTE())
                    {
                        Logger.CRITICAL($"<1> failed to UpdateFriend()...");
                        return;
                    }
                }

                if (null != onResult)
                {
                    GetAccountFromCid(targetCid, out outTargetGameDB, out outTargetUid, out outTargetNickname, out outTargetProfileTid);
                }
            }
            finally
            {
                onResult?.Invoke(outTargetUid, outTargetGameDB);
            }
        });
    }

    public static void DelCharacter(User? user, long targetCid, Action<long, DB_KIND> onResult)
    {
        if (null == user)
            return;

        DelCharacter(user.GetJobId(), user.gameDB, user.uid, targetCid, (targetUid, targetGameDB) =>
        {
            user.Post(() =>
            {
                onResult.Invoke(targetUid, targetGameDB);
            });
        });
    }

    public static void UpdateCharacter(uint jobId, DB_KIND gameDB, long cid, FRIEND_STATE friendState)
    {
        PostSlow(jobId, () =>
        {
            using (var con = new DB.MySql(gameDB))
            {
                con.PREPARE("UPDATE t_user_character SET state=@2, WHERE cid=@1");
                {
                    con.SET_PARAM("@1", cid);
                    con.SET_PARAM("@2", (sbyte)friendState);
                }

                if (0 == con.EXECUTE_UPDATE())
                {
                    Logger.CRITICAL($"failed to UpdateCharacter()...  cid( {cid} ), friend state( {friendState} )");
                }
            }
        });
    }

    public static void UpdateCharacter(User user, Character? character)
    {
        if (null == character)
            return;

        UpdateCharacter(user.GetJobId(), user.gameDB, character.cid, character.friendState);
    }
}
