

using Common;
using CommonStruct;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Security.Cryptography;

public partial class DBManager : BaseManager<DBManager>
{
    public static bool AddMail(long uid, DB_KIND gameDB, int mailTid, Action<long, long>? onResult = null)
    {
        PostSlow(uid, () =>
        {
            var nowUtc = DateTime.UtcNow;

            long newMailId = 0;

            using (var con = new DB.MySql(gameDB))
            {
                con.PREPARE("INSERT INTO t_user_mail(uid, tid, create_filetime, create_datetime_utc, create_datetime_local) VALUES(@1,@2,@3,@4,@5); SELECT LAST_INSERT_ID() AS mail_id;");
                {
                    con.SET_PARAM("@1", uid);
                    con.SET_PARAM("@2", mailTid);
                    con.SET_PARAM("@3", nowUtc.ToFileTime());
                    con.SET_PARAM("@4", nowUtc);
                    con.SET_PARAM("@5", nowUtc.ToLocalTime());

                }

                if (false == con.EXECUTE())
                {
                    Logger.CRITICAL($"[A] failed to AddMail()... uid( {uid} ), tid( {mailTid} )");
                    return;
                }

                if (con.FETCH())
                {
                    con.GET_DATA("mail_id", out UInt64 tempMailId);
                    newMailId = (long)tempMailId;
                }
                else
                {
                    Logger.CRITICAL($"[B] failed to AddMail()... uid( {uid} ), tid( {mailTid} )");
                    return;
                }
            }

            onResult?.Invoke(newMailId, nowUtc.ToFileTime());
        });

        return true;
    }

    public static bool AddMail(User user, int mailTid, Action<long, long> onResult)
    {
        AddMail(user.uid, user.gameDB, mailTid, (mailId, createFileTime) => 
        {
            user.Post(() =>
            {
                onResult?.Invoke(mailId, createFileTime);
            });
        });

        return true;
    }

    public static bool UpdateMail(User user, Mail mail, Action onResult)
    {
        PostSlow(user.GetJobId(), () =>
        {
            using (var con = new DB.MySql(user.gameDB))
            {
                con.PREPARE("UPDATE t_user_mail SET open=@2 WHERE mail_id=@1");
                {
                    con.SET_PARAM("@1", mail.id);
                    con.SET_PARAM("@2", mail.open);
                }

                if (0 == con.EXECUTE_UPDATE())
                {
                    Logger.CRITICAL(user, $"failed to UpdateMail()... mail id( {mail.id} ), tid( {mail.tid} ), open( {mail.open} )");
                }
            }

            user.Post(() =>
            {
                onResult?.Invoke();
            });
        });

        return true;
    }
}
