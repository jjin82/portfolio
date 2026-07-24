

using Common;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

public partial class DBManager : BaseManager<DBManager>
{
    public static void LoadPushMessage(out PriorityQueue<PushInfo, long> pushMessageList)
    {
        pushMessageList = new PriorityQueue<PushInfo, long>();

        using (var con = new DB.MySql(DB_KIND.GF_COMMON))
        {
            con.PREPARE("SELECT push_id, cid, execute_filetime FROM t_push_message WHERE execute_filetime!=0;");
            {
            }

            if (false == con.EXECUTE())
            {
                Logger.CRITICAL($"failed to LoadPushMessage()...");
                return;
            }

            while (con.FETCH())
            {
                con.GET_DATA("push_id",             out long pushId);
                con.GET_DATA("cid",                 out long cid);
                con.GET_DATA("execute_filetime",    out long executeFileTime);

                pushMessageList.Enqueue(new PushInfo(pushId, cid, executeFileTime), executeFileTime);
            }
        }
    }

    public static void AddPushMessage(PushParam param, Action<long> action)
    {
        PostSlow(() =>
        {
            long newPushId = 0;

            using (var con = new DB.MySql(DB_KIND.GF_COMMON))
            {
                con.PREPARE("INSERT INTO t_push_message(uid, cid, room_id, dm_id, title, message, execute_filetime, execute_datetime_utc, execute_datetime_local) VALUES(@1,@2,@3,@4,@5,@6,@7,@8,@9); SELECT LAST_INSERT_ID() AS push_id;");
                {
                    con.SET_PARAM("@1", param.receiverUid);
                    con.SET_PARAM("@2", param.senderCid);
                    con.SET_PARAM("@3", param.roomId);
                    con.SET_PARAM("@4", param.dmId);
                    con.SET_PARAM("@5", param.title);
                    con.SET_PARAM("@6", param.content);
                    con.SET_PARAM("@7", param.executeDateTimeUtc.ToFileTime());
                    con.SET_PARAM("@8", param.executeDateTimeUtc);
                    con.SET_PARAM("@9", param.executeDateTimeUtc.ToLocalTime());
                }

                if (false == con.EXECUTE())
                {
                    Logger.CRITICAL($"[A] failed to AddPushMessage()...  uid( {param.receiverUid} ), cid( {param.senderCid} ), room id( {param.roomId} ), title( {param.title} ), content( {param.content} ), execute datetime utc( {param.executeDateTimeUtc} )");
                    return;
                }

                if (con.FETCH())
                {
                    con.GET_DATA("push_id", out UInt64 tempPushId);
                    newPushId = (long)tempPushId;
                }
                else
                {
                    Logger.CRITICAL($"[B] failed to create push message...");
                    return;
                }
            }

            action?.Invoke(newPushId);
        });
    }

    public static void DelPushMessage(long pushId, User? user = null, Action<long>? action = null)
    {
        PostSlow(() =>
        {
            long executeFileTime = 0;
            using (var con = new DB.MySql(DB_KIND.GF_COMMON))
            {
                con.PREPARE("SELECT execute_filetime FROM t_push_message WHERE push_id=@1; UPDATE t_push_message SET execute_filetime=0 WHERE push_id=@1;");
                {
                    con.SET_PARAM("@1", pushId);
                }

                if (false == con.EXECUTE())
                {
                    Logger.FAIL($"failed to 1. DelPushMessage()... push id( {pushId} )");
                    return;
                }

                if (con.FETCH())
                {
                    con.GET_DATA("execute_filetime", out executeFileTime);
                }
                else
                {
                    Logger.FAIL($"failed to 2. DelPushMessage()... push id( {pushId} )");
                    return;
                }
            }

            user?.Post(() =>
            {
                action?.Invoke(executeFileTime);
            });
        });
    }

    public static void GetPushMessage(long pushId, Action<string, string, long, long, string> action)
    {
        PostSlow(() =>
        {
            long    uid     = 0;
            string  title   = "";
            string  message = "";
            long    roomId  = 0;
            long    dmId    = 0;

            using (var con = new DB.MySql(DB_KIND.GF_COMMON))
            {
                con.PREPARE("SELECT uid, room_id, dm_id, title, message FROM t_push_message WHERE push_id=@1 and execute_filetime!=0; UPDATE t_push_message SET execute_filetime=0 WHERE push_id=@1;");
                {
                    con.SET_PARAM("@1", pushId);
                }

                if (false == con.EXECUTE())
                {
                    Logger.FAIL($"[A] failed to GetPushMessage()... push id( {pushId} )");
                    return;
                }

                if (con.FETCH())
                {
                    con.GET_DATA("uid",     out uid);
                    con.GET_DATA("room_id", out roomId);
                    con.GET_DATA("dm_id",   out dmId);
                    con.GET_DATA("title",   out title);
                    con.GET_DATA("message", out message);
                }
                else
                {
                    Logger.FAIL($"[B] failed to GetPushMessage()... push id( {pushId} )");
                    return;
                }
            }

            string pushToken = "";

            // 유저의 push token 확보.
            using (var con = new DB.MySql(DB_KIND.GF_ACCOUNT))
            {
                con.PREPARE("SELECT push_token FROM t_account WHERE uid=@1");
                {
                    con.SET_PARAM("@1", uid);
                }

                if (false == con.EXECUTE())
                {
                    Logger.FAIL($"[C] failed to GetPushMessage()... push id( {pushId} )");
                    return;
                }

                if (con.FETCH())
                {
                    con.GET_DATA("push_token", out pushToken);
                }
                else
                {
                    Logger.FAIL($"[D] failed to GetPushMessage()... push id( {pushId} ), uid( {uid} )");
                    return;
                }
            }

            action?.Invoke(title, message, roomId, dmId, pushToken);
        });
    }
}
