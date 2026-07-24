

using Common;
using CommonStruct;
using Microsoft.VisualBasic.ApplicationServices;
using System.Collections.Concurrent;

public partial class DBManager : BaseManager<DBManager>
{
    public static bool ConsumeIAP(User user, string orderId, string productId)
    {
        var nowUtc = DateTime.UtcNow;

        using (var con = new DB.MySql(DB_KIND.GF_COMMON))
        {
            con.PREPARE("INSERT INTO t_iap(pid, uid, nickname, order_id, product_id, buy_filetime, buy_datetime_utc, buy_datetime_local) VALUES(@1, @2 , @3, @4, @5, @6, @7, @8); INSERT INTO t_iap_log(product_id, count) VALUES(@5, 1) ON DUPLICATE KEY UPDATE count=count+1");
            {
                con.SET_PARAM("@1", user.pid);
                con.SET_PARAM("@2", user.uid);
                con.SET_PARAM("@3", user.nickname);
                con.SET_PARAM("@4", orderId);
                con.SET_PARAM("@5", productId);
                con.SET_PARAM("@6", nowUtc.ToFileTime());
                con.SET_PARAM("@7", nowUtc);
                con.SET_PARAM("@8", nowUtc.ToLocalTime());
            }

            if (0 == con.EXECUTE_UPDATE())
            {
                return false;
            }
        }

        return true;
    }

    public static bool IsConsumeIAP(string orderId)
    {
        using (var con = new DB.MySql(DB_KIND.GF_COMMON))
        {
            con.PREPARE("SELECT pid FROM t_iap WHERE order_id=@1");
            {
                con.SET_PARAM("@1", orderId);
            }

            if (false == con.EXECUTE())
            {
                return true;
            }

            if (false == con.FETCH())
            {
                return false;
            }
        }

        // DB에서 결과를 못 얻으면 무조건 컨슘 처리로 판단.        
        return true;
    }
}
