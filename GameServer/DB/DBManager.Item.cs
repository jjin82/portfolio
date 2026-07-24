

using CommonStruct;
using Microsoft.VisualBasic.ApplicationServices;
using System.Collections.Concurrent;

public partial class DBManager : BaseManager<DBManager>
{
    public static bool UpdateItem(User user, Item item)
    {
        PostSlow(user.GetJobId(), () =>
        {
            using (var con = new DB.MySql(user.gameDB))
            {
                con.PREPARE("INSERT INTO t_user_item(uid, tid, count) VALUES(@1, @2, @3) AS new ON DUPLICATE KEY UPDATE count = new.count");
                {
                    con.SET_PARAM("@1", user.uid);
                    con.SET_PARAM("@2", item._tid);
                    con.SET_PARAM("@3", item._count);
                }

                if (0 == con.EXECUTE_UPDATE())
                {
                    Logger.CRITICAL(user, $"failed to UpdateItem()... item tid( {item.tid} ), count( {item.count} )");
                }
            }
        });

        return true;
    }
}
