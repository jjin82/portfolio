

using Microsoft.VisualBasic.ApplicationServices;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using static Google.Protobuf.WellKnownTypes.Field.Types;


public partial class DBManager : BaseManager<DBManager>
{
    private static bool GetAccountFromCid(long cid, out DB_KIND gameDB, out long uid, out string nickname, out int profileTid)
    {
        gameDB = 0;
        uid = 0;
        nickname = "";
        profileTid = 0;

        // uid 기준 유저 정보 획득.
        using (var con = new DB.MySql(DB_KIND.GF_ACCOUNT))
        {
            con.PREPARE("SELECT game_db, uid, nickname, push_token FROM t_account WHERE cid=@1");
            {
                con.SET_PARAM("@1", cid);
            }

            if (false == con.EXECUTE())
            {
                return false;
            }

            if (con.FETCH())
            {
                con.GET_DATA("game_db", out int outGameDB); gameDB = (DB_KIND)outGameDB;
                con.GET_DATA("uid", out uid);
                con.GET_DATA("nickname", out nickname);
            }
            else
            {
                // uid가 없는 유저.
                return false;
            }
        }

        // uid 기준 유저 정보 획득.
        using (var con = new DB.MySql(gameDB))
        {
            con.PREPARE("SELECT profile_tid FROM t_user WHERE uid=@1");
            {
                con.SET_PARAM("@1", uid);
            }

            if (false == con.EXECUTE())
            {
                return false;
            }

            if (con.FETCH())
            {
                con.GET_DATA("profile_tid", out profileTid);
            }
            else
            {
                // uid가 없는 유저.
                return false;
            }

        }

        return true;
    }

    public static void GetAccountFromCid(long cid, Action<long, DB_KIND, string, int> onResult)
    {
        PostSlow(() =>
        {
            if (false == GetAccountFromCid(cid, out DB_KIND gameDB, out long uid, out string nickname, out int profileTid))
                return;

            onResult?.Invoke(uid, gameDB, nickname, profileTid);
        });
    }

    private static bool GetAccountFromNickname(string nickname, out DB_KIND gameDB, out long uid, out long cid, out int profileTid)
    {
        gameDB = 0;
        uid = 0;
        cid = 0;
        profileTid = 0;

        // uid 기준 유저 정보 획득.
        using (var con = new DB.MySql(DB_KIND.GF_ACCOUNT))
        {
            con.PREPARE("SELECT game_db, uid, cid, push_token FROM t_account WHERE nickname=@1 AND deleted=0");
            {
                con.SET_PARAM("@1", nickname);
            }

            if (false == con.EXECUTE())
            {
                return false;
            }

            if (con.FETCH())
            {
                con.GET_DATA("game_db", out int outGameDB); gameDB = (DB_KIND)outGameDB;
                con.GET_DATA("uid", out uid);
                con.GET_DATA("cid", out cid);
            }
            else
            {
                // uid가 없는 유저.
                return false;
            }
        }

        // uid 기준 유저 정보 획득.
        using (var con = new DB.MySql(gameDB))
        {
            con.PREPARE("SELECT profile_tid FROM t_user WHERE uid=@1");
            {
                con.SET_PARAM("@1", uid);
            }

            if (false == con.EXECUTE())
            {
                return false;
            }

            if (con.FETCH())
            {
                con.GET_DATA("profile_tid", out profileTid);
            }
            else
            {
                // uid가 없는 유저.
                return false;
            }

        }

        return true;
    }

    public static void GetAccountFromNickname(string nickname, Action<bool, DB_KIND, long, long, int> onResult)
    {
        PostSlow(() =>
        {
            var reuslt = GetAccountFromNickname(nickname, out DB_KIND gameDB, out long uid, out long cid, out int profileTid);

            onResult?.Invoke(reuslt, gameDB, uid, cid, profileTid);
        });
    }
}
