

using Common;
using CommonStruct;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using static Character;

public partial class DBManager : BaseManager<DBManager>
{
    public static void Withdraw(User user)
    {
        PostSlow(user.GetJobId(), () =>
        {
            var withdrawTime = DateTime.Now;

            using (var con = new DB.MySql(DB_KIND.GF_ACCOUNT))
            {
                con.PREPARE("UPDATE t_account SET pid=@2, nickname=@3, deleted=@4 WHERE uid=@1");
                {
                    con.SET_PARAM("@1", user.uid);
                    con.SET_PARAM("@2", user.pid        + $"_({withdrawTime})");
                    con.SET_PARAM("@3", user.nickname   + $"_({withdrawTime.ToFileTime()})");
                    con.SET_PARAM("@4", true);
                }

                if (0 == con.EXECUTE_UPDATE())
                {
                    Logger.CRITICAL(user, $"<0> Withdraw...");
                }
            }

            using (var con = new DB.MySql(user.gameDB))
            {
                con.PREPARE("UPDATE t_user SET pid=@2, nickname=@3 WHERE uid=@1");
                {
                    con.SET_PARAM("@1", user.uid);
                    con.SET_PARAM("@2", user.pid        + $"_({withdrawTime})");
                    con.SET_PARAM("@3", user.nickname   + $"_({withdrawTime.ToFileTime()})");
                }

                if (0 == con.EXECUTE_UPDATE())
                {
                    Logger.CRITICAL(user, $"<1> Withdraw...");
                }
            }
        });
    }

    public static void Login(LOGIN_TYPE loginType, int netId, string pid, string email, string accessToken)
    {
        PostSlow(() =>
        {
            // ===================================================================================================================
            // 
            // 유저의 기본 정보를 로드하는 코드
            //
            var user = LoadUserBasicInfo(loginType, netId, pid, email, accessToken);
            if (null == user) return;


            // ===================================================================================================================
            // 
            // 유저의 아이템, 캐릭터, 이메일 등의 디테일한 정보를 로드하는 코드
            //
            if (false == LoadUserDetailInfo(user))
            {
                Network.SendResultCode(netId, RESULT_CODE.FAIL_LOGIN);
                return;
            }

            user.Create();
        });
    }

    // ===================================================================================================================
    //
    public static User? LoadUserBasicInfo(LOGIN_TYPE loginType, int netId, string pid, string email, string accessToken)
    {
        RESULT_CODE resultCode = RESULT_CODE.SUCCESS;

        // pid 기본 검증.
         if (string.IsNullOrEmpty(pid))
        {
            Logger.CRITICAL($"failed to DB Login... netId( {netId} ), pid( {pid} ), email( {email} ), accessToken( {accessToken} )");
            return null;
        }

        long     uid                = 0;                            // DB 아이디
        string   nickname           = "";                           // 닉네임.   
        sbyte    gender             = 0;                            // 성별
        int      gameDBKind         = 0;                            // 유저 정보가 기록되어 있는 game db.
        int      adminLevel         = 0;                            // 유저 권한 정보.
        string   pushToken          = "";
        DateTime lastLogoutTime     = DateTime.Now;

        long     cid                = 0;                            // 유저의 캐릭터 아이디        
        int      profileTid         = 0;                            // 프로필 tid.
        sbyte    hasMadeIAP         = 0;                            // 결제 여부.
        long     chatOpenerCharId   = 0;                            // 첫 대화를 보낸 캐릭터.
        long     chatOpenerFiletime = 0;                            // 첫 대화를 보낸 시간.


        // pid 기준 유저 정보 획득.
        using (var con = new DB.MySql(DB_KIND.GF_ACCOUNT))
        {
            con.PREPARE("SELECT uid, game_db, admin_level, push_token, logout_datetime_local FROM t_account WHERE pid=@1 and deleted=0");
            {
                con.SET_PARAM("@1", pid);
            }

            if (false == con.EXECUTE())
            {
                Network.SendResultCode(netId, RESULT_CODE.FAIL_LOGIN);
                return null;
            }

            if (con.FETCH())
            {
                con.GET_DATA("uid",                     out uid);
                con.GET_DATA("game_db",                 out gameDBKind);
                con.GET_DATA("admin_level",             out adminLevel);
                con.GET_DATA("push_token",              out pushToken);
                con.GET_DATA("logout_datetime_local",   out lastLogoutTime); 
            }
            else
            {
                // pid가 없는 유저.
                resultCode = RESULT_CODE.SUCCESS_USER_CREATE;
            }
        }

        // 탈퇴 유저 확인. (탈퇴 후 가입 어뷰징 처리)
        if (resultCode.Equals(RESULT_CODE.SUCCESS_USER_CREATE))
        {
            // pid 기준 유저 정보 획득.
            using (var con = new DB.MySql(DB_KIND.GF_ACCOUNT))
            {
                con.PREPARE("SELECT logout_datetime_local FROM t_account WHERE email=@1 and deleted=1 ORDER BY logout_datetime_local DESC LIMIT 1");
                {
                    con.SET_PARAM("@1", email);
                }

                if (false == con.EXECUTE())
                {
                    Network.SendResultCode(netId, RESULT_CODE.FAIL_LOGIN);
                    return null;
                }

                if (con.FETCH())
                {
                    con.GET_DATA("logout_datetime_local", out lastLogoutTime);

                    // live 에서만 동작.
                    if (ServerConfig.IsLive())
                    {
                        // 탈퇴 기록이 있으면 7일이 지났는지 확인.
                        if (7 > (DateTime.Now - lastLogoutTime).TotalDays)
                        {
                            Network.SendResultCode(netId, RESULT_CODE.FAIL_USER_REJOIN_RESTRICTED, lastLogoutTime.AddDays(7).ToString("yyyy-MM-dd HH:mm:ss"));
                            return null;
                        }
                    }
                }
            }
        }

        // 신규 유저 생성.
        if (resultCode.Equals(RESULT_CODE.SUCCESS_USER_CREATE))
        {
            // 캐릭터 아이디 발급.
            cid = AllocId();

            // game db 할당.
            gameDBKind = (int)AllocGameDBKind();

            // 닉네임 생성.
            if (loginType.Equals(LOGIN_TYPE.GUEST) || email.Contains("@nerdfox.com"))
            {
                nickname = $"Friend#{cid:D3}";
            }
            else
            {
                int index = email.IndexOf('@');
                nickname = (index > 0) ? email.Substring(0, index) : email;
            }

            using (var con = new DB.MySql(DB_KIND.GF_ACCOUNT))
            {
                con.PREPARE("INSERT INTO t_account(pid, cid, game_db, nickname, email, login_type, create_datetime_utc, create_datetime_local) VALUES(@1,@2,@3,@4,@5,@6,@7,@8); SELECT LAST_INSERT_ID() AS uid");
                {
                    con.SET_PARAM("@1", pid);
                    con.SET_PARAM("@2", cid);
                    con.SET_PARAM("@3", gameDBKind);
                    con.SET_PARAM("@4", nickname);
                    con.SET_PARAM("@5", email);
                    con.SET_PARAM("@6", loginType.ToString()); 
                    con.SET_PARAM("@7", DateTime.UtcNow);
                    con.SET_PARAM("@8", DateTime.Now);
                }

                if (false == con.EXECUTE())
                {
                    Network.SendResultCode(netId, RESULT_CODE.FAIL_USER_SIGNUP);
                    return null;
                }

                UInt64 newUID = 0; // LAST_INSERT_ID()가 리턴값이 ulong이다.
                if (con.FETCH())
                {
                    con.GET_DATA("uid", out newUID);
                    uid = (long)newUID;
                }
                else
                {
                    Logger.CRITICAL($"failed to create user - pid= {pid}");
                    return null;
                }
            }

            // 게임 DB에 유저 정보 등록.
            using (var con = new DB.MySql(gameDBKind))
            {
                con.PREPARE("INSERT INTO t_user(uid, pid, cid, nickname) VALUES(@1,@2,@3,@4)");
                {
                    con.SET_PARAM("@1", uid);
                    con.SET_PARAM("@2", pid);
                    con.SET_PARAM("@3", cid);
                    con.SET_PARAM("@4", nickname);
                }

                if (false == con.EXECUTE())
                {
                    using (var deleteCon = new DB.MySql(DB_KIND.GF_ACCOUNT))
                    {
                        deleteCon.PREPARE("DELETE FROM t_account WHERE pid=@1;");
                        {
                            deleteCon.SET_PARAM("@1", pid);
                        }

                        deleteCon.EXECUTE();
                    }

                    Network.SendResultCode(netId, RESULT_CODE.FAIL_USER_SIGNUP);
                    return null;
                }
            }
        }
        else if (0 < uid) // 가입된 유저.
        {
            using (var con = new DB.MySql(gameDBKind))
            {
                con.PREPARE("SELECT cid, nickname, gender, profile_tid, has_made_iap, chat_opener_char_id, chat_opener_filetime FROM t_user WHERE uid=@1");
                {
                    con.SET_PARAM("@1", uid);
                }

                if (false == con.EXECUTE())
                {
                    Network.SendResultCode(netId, RESULT_CODE.FAIL_LOGIN);
                    return null;
                }

                if (con.FETCH())
                {
                    con.GET_DATA("cid",                     out cid); 
                    con.GET_DATA("nickname",                out nickname);
                    con.GET_DATA("gender",                  out gender); 
                    con.GET_DATA("profile_tid",             out profileTid);
                    con.GET_DATA("has_made_iap",            out hasMadeIAP);
                    con.GET_DATA("chat_opener_char_id",     out chatOpenerCharId);
                    con.GET_DATA("chat_opener_filetime",    out chatOpenerFiletime);
                }
                else
                {
                    Network.SendResultCode(netId, RESULT_CODE.FAIL_LOGIN);
                    return null;
                }
            }
        }
        else
        {
            Logger.CRITICAL($"failed to login... netId( {netId} ), pid( {pid} ), uid( {uid} ), cid( {cid} ), adminLevel( {adminLevel} ), DBKind( {gameDBKind} ), nickname( {nickname} ), gender( {gender} ), email( {email} )");

            Network.SendResultCode(netId, RESULT_CODE.FAIL_LOGIN);
            return null;
        }

        var user = new User(netId, pid, uid, cid, adminLevel, gameDBKind, nickname, profileTid, (0 != hasMadeIAP), chatOpenerCharId, chatOpenerFiletime, accessToken, pushToken, lastLogoutTime);
        if (user != null) 
        {
            Logger.INFO($"■ login db. pid( {pid} )");
        }

        return user;
    }

    // ===================================================================================================================
    //
    public class QueryUserDetailInfo
    {
        private static readonly Lazy<QueryUserDetailInfo> s_instance = new Lazy<QueryUserDetailInfo>(() => new QueryUserDetailInfo());

        QueryUserDetailInfo()
        {
            _loginQuery = new StringBuilder(32768);
            _loginQuery.Append("SELECT type, count FROM t_user_currency WHERE uid=@1;");

            // character.
            _loginQuery.Append($"SELECT c.cid, c.char_type, c.tid, c.state, c.love_point, COALESCE(u.nickname, '') AS nickname, COALESCE(u.profile_tid, 0) AS profileTid FROM t_user_character c LEFT JOIN t_user u ON c.char_type={(int)CHAR_TYPE.PC} AND c.cid = u.cid WHERE c.uid=@1 AND state!=0;");     // 캐릭터 정보.

            // item.
            _loginQuery.Append("SELECT tid, count FROM t_user_item WHERE uid=@1;");

            // mail. (메일 정보)
            _loginQuery.Append("SELECT mail_id, tid, open, create_filetime FROM t_user_mail WHERE uid=@1 AND open=0;");

            // ad.
            _loginQuery.Append("SELECT type, next_time, reward_key, count FROM t_user_ad WHERE uid=@1;");

            // dm.
            _loginQuery.Append("SELECT uid, id, parent_id, send_cid, send_nickname, send_profile_tid, recv_cid, recv_nickname, recv_profile_tid, msg, filetime FROM  t_user_dm WHERE uid=@1;");

            // mission.
            _loginQuery.Append("SELECT tid, param1, param2, resetTime, is_reward FROM t_user_mission WHERE uid = @1;");
        }

        public static string GetQuery()
        {
            return s_instance?.Value?._loginQuery?.ToString() ?? "";
        }

        private StringBuilder? _loginQuery = null;
    }

    public static bool LoadUserDetailInfo(User user)
    {
        var currencyList            = new Dictionary<CURRENCY_TYPE, Currency>();
        var itemList                = new List<Item>();
        var characterList           = new Dictionary<long, Character>();
        var mailList                = new List<Mail>();
        var adList                  = new List<Ad>();
        var dmList                  = new List<DMInfo>();
        var missionList             = new List<Mission>();


        using (var con = new DB.MySql(user.gameDB))
        {
            con.PREPARE(QueryUserDetailInfo.GetQuery());
            {
                con.SET_PARAM("@1", user.uid);
            }

            if (false == con.EXECUTE())
            {
                user.SendResultCode(RESULT_CODE.FAIL_LOGIN);
                return false;
            }

            // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            //
            // t_user_currency
            //
            while (con.FETCH())
            {
                int type  = 0;
                int count = 0;

                var currency = new Currency();
                con.GET_DATA("type",  out type);  currency._type = (CURRENCY_TYPE)type;
                con.GET_DATA("count", out count); currency._count = count;

                currencyList.Add(currency.type, currency);
            }

            // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            //
            // t_user_character
            //
            if (con.NEXT_FETCH())
            {
                while (con.FETCH())
                {
                    Character.Param param = new Character.Param();

                    // 캐릭터 기본 정보.
                    con.GET_DATA("cid",                     out         param.cid);
                    con.GET_DATA("char_type",               out sbyte   tempCharType);              param.charType      = (CHAR_TYPE)tempCharType;
                    con.GET_DATA("tid",                     out         param.tid);
                    con.GET_DATA("state",                   out sbyte   tempState);                 param.friendState   = (FRIEND_STATE)tempState;
                    con.GET_DATA("love_point",              out         param.lovePoint);

                    // 유저도 캐릭터이기 때문에 유저의 경우엔 아래 정보 확보.
                    con.GET_DATA("nickname",                out param.nickname);
                    con.GET_DATA("profileTid",              out long    tempProfileTid);            param.profileTid    = (int)tempProfileTid;


                    var character = Character.Alloc(user, param);
                    if(null == character) continue;
                    
                    characterList.Add(character.cid, character);
                }
            }

            // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            //
            // t_user_item
            //
            if (con.NEXT_FETCH())
            {
                while (con.FETCH())
                {
                    var item = new Item();
                    con.GET_DATA("tid",     out item._tid);
                    con.GET_DATA("count",   out item._count);

                    itemList.Add(item);
                }
            }
            else
            {
                user.SendResultCode(RESULT_CODE.FAIL_LOGIN);
                return false;
            }

            // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            //
            // t_user_mail
            //
            if (con.NEXT_FETCH())
            {
                while (con.FETCH())
                {
                    con.GET_DATA("mail_id",         out long  mailId);
                    con.GET_DATA("tid",             out int   tid);
                    con.GET_DATA("open",            out sbyte open);
                    con.GET_DATA("create_filetime", out long  createFiletime); 

                    mailList.Add(new Mail { _id = mailId, _tid = tid, _open = (0 != open), _createFileTime = createFiletime });
                }
            }
            else
            {
                user.SendResultCode(RESULT_CODE.FAIL_LOGIN);
                return false;
            }

            // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            //
            // t_user_ad
            //
            if (con.NEXT_FETCH())
            {
                while (con.FETCH())
                {
                    var ad = new Ad();
                    
                    int type = 0;
                    con.GET_DATA("type",        out type); ad.type = (AD_TYPE)type;
                    con.GET_DATA("next_time",   out ad.nextTime);
                    con.GET_DATA("reward_key",  out ad.rewardToken);
                    con.GET_DATA("count",       out ad.count);

                    adList.Add(ad);
                }
            }
            else
            {
                user.SendResultCode(RESULT_CODE.FAIL_LOGIN);
                return default;
            }

            // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            //
            // t_user_dm
            //
            if (con.NEXT_FETCH())
            {
                while (con.FETCH())
                {
                    var dmInfo = new DMInfo();

                    con.GET_DATA("id",                 out dmInfo.dmId);
                    con.GET_DATA("parent_id",          out dmInfo.parentDmId);
                    con.GET_DATA("send_cid",           out dmInfo.sendCid);
                    con.GET_DATA("send_nickname",      out dmInfo.sendNickname);
                    con.GET_DATA("send_profile_tid",   out dmInfo.sendProfileTid);
                    con.GET_DATA("recv_cid",           out dmInfo.recvCid);
                    con.GET_DATA("recv_nickname",      out dmInfo.recvNickname);
                    con.GET_DATA("recv_profile_tid",   out dmInfo.recvProfileTid);
                    con.GET_DATA("msg",                out dmInfo.msg);
                    con.GET_DATA("filetime",           out dmInfo.fileTime);

                    dmList.Add(dmInfo);
                }
            }
            else
            {
                user.SendResultCode(RESULT_CODE.FAIL_LOGIN);
                return default;
            }

            // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            //
            // t_user_mission
            //
            if (con.NEXT_FETCH())
            {
                while (con.FETCH())
                {
                    var info = new Mission();
                    con.GET_DATA("tid",         out info._tid);
                    con.GET_DATA("param1",      out info._param1);
                    con.GET_DATA("param2",      out info._param2);
                    con.GET_DATA("resetTime",   out info._resetTime);
                    con.GET_DATA("is_reward",   out sbyte tempIsReward); info._isReward = (0 != tempIsReward);

                    missionList.Add(info);
                }
            }
            else
            {
                user.SendResultCode(RESULT_CODE.FAIL_LOGIN);
                return default;
            }
        }

        // 유저 기타 정보 로드.(로드되는 정보에 순서도 의미가 있다. ex> 시나리오는 히로인 정보가 필요하여 마지막에 로드되게 코드 작성)
        user.LoadDB(currencyList, characterList, itemList, mailList, adList, dmList, missionList);
        
        return true;
    }

    public static void Logout(User user)
    {
        if (null == user)
            return;

        PostSlow(user.GetJobId(), () =>
        {
            using (var con = new DB.MySql(DB_KIND.GF_ACCOUNT))
            {
                con.PREPARE("UPDATE t_account SET logout_datetime_local=@2, login_count=login_count + 1 WHERE uid=@1");
                {
                    con.SET_PARAM("@1", user.uid);
                    con.SET_PARAM("@2", DateTime.Now);
                }

                if (0 == con.EXECUTE_UPDATE())
                {
                    Logger.CRITICAL(user, $"failed to logout..");
                }
            }
        });
    }


    public static void UpdateProfile(User user, string? nickname, int profileTid, Action action)
    {
        if (null == user)
            return;

        PostSlow(user.GetJobId(), () =>
        {
            // 새로운 닉네임의 경우 중복 체크.
            if (false == user.nickname.Equals(nickname))
            {
                using (var con = new DB.MySql(DB_KIND.GF_ACCOUNT))
                {
                    con.PREPARE("SELECT uid FROM t_account WHERE nickname=@1");
                    {
                        con.SET_PARAM("@1", nickname);
                    }

                    if (false == con.EXECUTE())
                    {
                        Network.SendResultCode(user?.netId ?? 0, RESULT_CODE.FAIL_NICKNAME_CHANGE);
                        return;
                    }

                    if (con.FETCH())
                    {
                        Network.SendResultCode(user?.netId ?? 0, RESULT_CODE.FAIL_NICKNAME_ALREADY_USE_ANOTHER_USER);
                        return;
                    }
                }
            }

            using (var con = new DB.MySql(DB_KIND.GF_ACCOUNT))
            {
                con.PREPARE("UPDATE t_account SET nickname=@2 WHERE uid=@1");
                {
                    con.SET_PARAM("@1", user.uid);
                    con.SET_PARAM("@2", nickname);
                }

                if (0 == con.EXECUTE_UPDATE())
                {
                    Logger.CRITICAL(user, $"failed to update nickname... DB( {DB_KIND.GF_ACCOUNT} )");
                    return;
                }
            }

            using (var con = new DB.MySql(user.gameDB))
            {
                con.PREPARE("UPDATE t_user SET nickname=@2,profile_tid=@3 WHERE uid=@1");
                {
                    con.SET_PARAM("@1", user.uid);
                    con.SET_PARAM("@2", nickname);
                    con.SET_PARAM("@3", profileTid);
                }

                if (0 == con.EXECUTE_UPDATE())
                {
                    Logger.CRITICAL(user, $"failed to update nickname... DB( {user.gameDB} ), nickname( {nickname} ), profile tid( {profileTid} )");
                }
            }

            user?.Post(() => 
            {
                action?.Invoke();
            });
        });
    }

    public static void UpdatePushToken(User user, string? pushToken)
    {
        if (null == user || string.IsNullOrEmpty(pushToken))
            return;

        PostSlow(user.GetJobId(), () =>
        {
            using (var con = new DB.MySql(DB_KIND.GF_ACCOUNT))
            {
                con.PREPARE("UPDATE t_account SET push_token=@2 WHERE uid=@1");
                {
                    con.SET_PARAM("@1", user.uid);
                    con.SET_PARAM("@2", pushToken);
                }

                if (0 == con.EXECUTE_UPDATE())
                {
                    Logger.CRITICAL(user, $"failed to update push token... DB( {user.gameDB} ), push token( {pushToken} )");
                }
            }
        });
    }

    public static void UpdateUserInfo(User user)
    {
        if (null == user)
            return;

        PostSlow(user.GetJobId(), () =>
        {
            using (var con = new DB.MySql(user.gameDB))
            {
                con.PREPARE("UPDATE t_user SET has_made_iap=@3 WHERE uid=@1");
                {
                    con.SET_PARAM("@1", user.uid);
                    con.SET_PARAM("@3", user.IsHasMadeIAP());
                }

                if (0 == con.EXECUTE_UPDATE())
                {
                    Logger.CRITICAL(user, $"failed to update user info... has made InAppPurchase( {user.IsHasMadeIAP()} )");
                }
            }
        });
    }

    public static void UpdateUserChatOpener(User user, Character character)
    {
        if (null == user)
            return;

        if (null == character)
            return;

        PostSlow(user.GetJobId(), () =>
        {
            using (var con = new DB.MySql(user.gameDB))
            {
                con.PREPARE("UPDATE t_user SET chat_opener_char_id=@2,chat_opener_filetime=@3 WHERE uid=@1");
                {
                    con.SET_PARAM("@1", user.uid);
                    con.SET_PARAM("@2", character.tid);
                    con.SET_PARAM("@3", DateTime.UtcNow.ToFileTime());
                }

                if (0 == con.EXECUTE_UPDATE())
                {
                    Logger.CRITICAL(user, $"failed to UpdateUserChatOpener()...  character tid( {character.tid} )");
                }
            }
        });
    }
}


