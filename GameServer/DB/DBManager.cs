

using Microsoft.VisualBasic.ApplicationServices;
using System.Collections.Concurrent;
using static Google.Protobuf.WellKnownTypes.Field.Types;


public partial class DBManager : BaseManager<DBManager>
{
    public override bool Initialize()
    {
        if (false == DB.MySql.LoadConfigDB("Config/ConfigDB.ini"))
            return false;

        return true; ;
    }

    public static DB_KIND AllocGameDBKind()
    {
        // 100회 중 한번 DB 선택 동작.
#if !DEBUG
        if (0 != (s_allocCount++) % 100)
#endif
        {
            long minUserCount = long.MaxValue;

            foreach (DB_KIND kind in Enum.GetValues(typeof(DB_KIND)))
            {
                // game db만 
                if (false == kind.ToString().Contains("GAME"))
                    continue;

                // 사용 가능한 DB 인지.
                if (false == DB.MySql.IsValid(kind))
                    continue;

                using (var con = new DB.MySql(kind))
                {
                    con.PREPARE("SELECT COUNT(uid) userCount FROM t_user");

                    con.EXECUTE();

                    if (con.FETCH())
                    {
                        con.GET_DATA("userCount", out long userCount);

                        if (userCount < minUserCount)
                        {
                            minUserCount = userCount;
                            
                            // 유저가 적은 DB로 결정.
                            s_gameDBKind = kind;
                        }
                    }
                }
            }
        }

        return s_gameDBKind;
    }

    private static long AllocId()
    {
        using (var con = new DB.MySql(DB_KIND.GF_COMMON))
        {
            con.PREPARE("ALTER TABLE t_alloc_id AUTO_INCREMENT = 1000; INSERT INTO t_alloc_id() VALUES(); SELECT LAST_INSERT_ID() AS id;");

            if (false == con.EXECUTE())
            {
                return 0;
            }

            if (false == con.FETCH())
                return 0;

            UInt64 newId = 0; // LAST_INSERT_ID()가 리턴값이 ulong이다.            
            con.GET_DATA("id", out newId);

            return (long)newId;
        }
    }

    private static DB_KIND s_gameDBKind = DB_KIND.GF_GAME01;
    private static short   s_allocCount = 0;
}
