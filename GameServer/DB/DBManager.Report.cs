

using CommonStruct;
using Microsoft.VisualBasic.ApplicationServices;
using System.Collections.Concurrent;
using System.Security.Cryptography;

public partial class DBManager : BaseManager<DBManager>
{
    public static void AddReport(User user, string kind, string reason, Action<bool> action)
    {
        if (null == user)
            return;

        PostSlow(user.GetJobId(), () =>
        {
            bool result = true;

            using (var con = new DB.MySql(DB_KIND.GF_COMMON))
            {
                con.PREPARE("INSERT INTO t_report(pid, uid, kind, reason) VALUES(@1,@2,@3,@4)");
                {
                    con.SET_PARAM("@1", user.pid);
                    con.SET_PARAM("@2", user.uid);
                    con.SET_PARAM("@3", kind);
                    con.SET_PARAM("@4", reason);
                }

                if (0 == con.EXECUTE_UPDATE())
                {
                    result = false;

                    Logger.CRITICAL(user, $"failed to t_report()... pid( {user.pid} ), uid( {user.uid} ), kind( {kind} ), reason( {reason} )");
                }
            }

            action.Invoke(result);
        });
    }
}
