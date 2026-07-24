

using Common;
using CommonStruct;
using Microsoft.VisualBasic.ApplicationServices;
using System.Collections.Concurrent;

public partial class DBManager : BaseManager<DBManager>
{
    public static bool UpdateAd(User user, Ad ad)
    {
        PostSlow(user.GetJobId(), () =>
        {
            using (var con = new DB.MySql(user.gameDB))
            {
                con.PREPARE("INSERT INTO t_user_ad(uid, type, next_time, reward_key, count) VALUES(@1, @2, @3, @4, @5) AS new ON DUPLICATE KEY UPDATE next_time = new.next_time, reward_key = new.reward_key, count = new.count");
                {
                    con.SET_PARAM("@1", user.uid);
                    con.SET_PARAM("@2", (int)ad.type);
                    con.SET_PARAM("@3", ad.nextTime);
                    con.SET_PARAM("@4", ad.rewardToken);
                    con.SET_PARAM("@5", ad.count);
                }

                if (0 == con.EXECUTE_UPDATE())
                {
                    Logger.CRITICAL(user, $"UpdateAd. type( {ad.type} ), nextTime( {ad.nextTime} ), rewardToken( {ad.rewardToken} ), count( {ad.count} )");
                }
            }
        });

        return true;
    }
}
