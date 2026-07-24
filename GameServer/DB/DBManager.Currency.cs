

using CommonStruct;
using Microsoft.VisualBasic.ApplicationServices;
using System.Collections.Concurrent;

public partial class DBManager : BaseManager<DBManager>
{
    public static bool UpdateCurrency(User user, Currency currency)
    {
        PostSlow(user.GetJobId(), () =>
        {
            using (var con = new DB.MySql(user.gameDB))
            {
                con.PREPARE("INSERT INTO t_user_currency(uid, type, count) VALUES(@1, @2, @3) AS new ON DUPLICATE KEY UPDATE count = new.count");
                {
                    con.SET_PARAM("@1", user.uid);
                    con.SET_PARAM("@2", (int)currency.type);
                    con.SET_PARAM("@3", currency.count);
                }

                if (0 == con.EXECUTE_UPDATE())
                {
                    Logger.CRITICAL(user, $"failed to UpdateCurrency()... currency type( {currency.type} ), count( {currency._count} )");
                }
            }
        });

        return true;
    }
}
