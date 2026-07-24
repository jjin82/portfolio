using Common;
using CommonStruct;
using NerdFox.OpenAI.define;
using Org.BouncyCastle.Utilities.Zlib;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

public partial class User : Entity
{
    public User()
    {

    }
   public User( int netId, string pid, long uid, long cid, int adminLevel, int gameDB, string nickname, int profileTid, bool hasMadeIAP, long chatOpenerCharId, long chatOpenerFiletime,
                string accessToken, string pushToken, DateTime lastLogoutTime)
        : base(true)
   {
        _netId              = netId;
        _pid                = pid;
        _uid                = uid;
        _nickname           = nickname;
        _profileTid         = profileTid;

        _adminLevel         = adminLevel;
        _gameDB             = gameDB;
        
        _hasMadeIAP         = hasMadeIAP;
        _accessToken        = accessToken;
        _pushToken          = pushToken;
        _lastLogoutTime     = lastLogoutTime;

        _isFirstLoginDaily  = !_lastLogoutTime.Day.Equals(DateTime.Now.Day);
    }

    protected override void OnCreate()
    {
        // 1. 메니져 로그인 처리.
        if (false == UserManager.Get.Login(this))
        {
            Destroy();
            return;
        }

        // 로그인.
        LoginNotify(new C2G.RS_LOGIN());


        if (IsNewbie())
        {
            // [REPORT] 신규 유저.
            WebManager.Get.ReportNewbie(pid, uid, nickname);
        }
        // [REPORT] 로그인.
        WebManager.Get.ReportLogin(pid, uid, nickname);
    }

    protected override void OnDestroy()
    {
        // 광고 리셋 처리.
        ResetAd();

        // 로그아웃 처리.
        UserManager.Get.Logout(this);

        // 게임방 로그아웃 처리.
        GameManager.Get.Logout(this);

        // DB에 유저 로그아웃 처리.
        DBManager.Logout(this);

        // [REPORT] 로그 아웃.
        WebManager.Get.ReportLogout(pid, uid, nickname);
    }

    protected override void OnUpdate()
    {
        // 펜딩(셧다운.. 등등) 중이면 리턴.
        if (_pendingShutdown)
            return;

        UpdateAd();                 // 광고 처리.
        UpdateTimeReward();         // 시간 보상.

        // 서버가 셧다운 됨.
        if (false == Program.IsOpenService())
        {
            _pendingShutdown = true;

            // 유저에게 셧다운 알림.
            SendResultCode(RESULT_CODE.FAIL_GAME_SERVER_CLOSE);

            Post(() =>
            {
                Destroy();

            }, UTIL.GetRandom(1000, 5000)); // 1 ~ 5초 동안 분포해서 유저가 로그아웃 되도록 함.
        }
    }

    protected override void OnUpdate5Second()
    {
        UpdateMission(); // 미션.
    }

    protected override void OnUpdateMinute()
    {

    }

    public void LoadDB( Dictionary<CURRENCY_TYPE, Currency>     currencyList,
                        Dictionary<long, Character>             characterList,
                        List<Item>                              itemList,
                        List<Mail>                              mailList,
                        List<Ad>                                adList,
                        List<DMInfo>                            dmList,
                        List<Mission>                           missionList
                    )
    {
        // 생명 주기 세팅.
        SetLifeTime();

        LoadDBCurrency(currencyList);                               // 재화.
        LoadDBCharacter(characterList);                             // 캐릭터.
        LoadDBItem(itemList);                                       // 아이템.
        LoadDBMail(mailList);                                       // 메일.
        LoadAd(adList);                                             // 광고.
        LoadDM(dmList);                                             // DM.
        LoadDBMission(missionList);                                 // 미션.
    }

    public void Withdraw()
    {
        // 유저 캐릭터만 모두 삭제.
        DelPlayerCharacter();

        // 탈퇴.
        DBManager.Withdraw(this);

        Send(new C2G.RS_WITHDRAW_ACCOUNT { success = true });
    }

    public void LoginOK()
    {
        // 미션.
        ExecuteMission(MISSION_TYPE.LOGIN_DAILY);
        ExecuteMission(MISSION_TYPE.LOGIN_STREAK_TIMES);

        if (string.IsNullOrEmpty(_pushToken))
        {
            Send(new C2G.RS_USER_PUSH_TOKEN_GET());
        }

        // 신규 유저 처리. (최초 설정을 요청)
        if (IsNewbie())
        {
            _isSeeYouCharTid = int.MaxValue; // 0이 아닌값을 넣는건 설정을 하라는 조건 설정.

            NewbieTimeReward();
        }

        // 오늘 첫 로그인.
        if (!_isFirstLoginDaily)
        {
            ResetTimeReward();
        }
    }

    public void LoginNotify(C2G.RS_LOGIN packet)
    {
        // RS_LOGIN
        packet.Set(IsNewbie(), _cid, _nickname, _profileTid, _hasMadeIAP, _lastLogoutTime.ToFileTime(), _currencyList);
        Send(packet);

        //------------------------------------------------------------------------------------------------------
        {
            SendItemList();                     // 아이템 리스트.
            SendCharacterList();                // 캐릭터 리스트.
            SendMailList();                     // 메일 정보.
            SendAdList();                       // 광고 정보.
            SendDMList();                       // DM 정보.
            SendMissionList();                  // 미션 정보.
            
            ShopManager.Get.SendProductList(this);// 상점 정보.
            NoticeManager.Get.SendList(this);// 공지 사항.

            // 시간 이벤트 정보.
            //TimeEventManager.Get.SendTimeEventList(this);
        }
        //------------------------------------------------------------------------------------------------------

        // 로그인 완료.
        Send(new C2G.RS_LOGIN_COMPLETED()); 
    }

    public void Relogin(int netId)
    {
        Logger.DEBUG(this, $"relogin user netId. netId( {_netId} -> {netId} ) ");

        // netId 변경.
        _netId = netId;

        // 생명 주기 복구.
        SetLifeTime();

        // 재 로그인.
        LoginNotify(new C2G.RS_RELOGIN());
    }

    public void SetPushToken(string? pushToken)
    {
        if (string.IsNullOrEmpty(pushToken))
        {
            Logger.ERROR(this, $"invalid push token...");
            return;
        }

        _pushToken = pushToken;   

        DBManager.UpdatePushToken(this, pushToken);

        Logger.DEBUG(this, $"▣ set push token( {pushToken} )");
    }

    public void ChangeProfile(string? changeNickname, int changeProfileTid)
    {
        // 닉네임 확인.
        if (string.IsNullOrEmpty(changeNickname))
            return;

        // 변경 내용이 없음.
        if (nickname.Equals(changeNickname) && profileTid.Equals(changeProfileTid))
            return;

        // 닉네임 욕설 필터.
        if (ForbiddenWord.Get().Check(changeNickname))
        {
            SendResultCode(RESULT_CODE.FAIL_NICKNAME_BAD);
            return;
        }

        DBManager.UpdateProfile(this, changeNickname, changeProfileTid, () => 
        {
            // 미션.
            if(!_nickname.Equals(changeNickname ?? ""))
            {
                ExecuteMission(MISSION_TYPE.CHANGE_NICKNAME);
            }

            _nickname    = changeNickname ?? _nickname;
            _profileTid  = changeProfileTid;

            Send(new C2G.RS_USER_PROFILE
            {
                nickname   = _nickname,
                profileTid = _profileTid,
            });
        });
    }

    public void SetLifeTime(double second = (Define.HEART_BEAT_SECOND * 2))
    {
        _lifeTime = DateTime.Now.AddSeconds(second).ToFileTime();
    }

    public void UpdateInfo()
    {
        Send(new C2G.RS_USER_INFO_UPDATE(this));

        // 유저 정보 업데이트.
        DBManager.UpdateUserInfo(this);
    }

    public void NewbieSupport()
    {
        // 신규 유저 지원 여부.
        if (false == IsNewbie())
            return;

        /////////////////////////////////////////////////////////////////////////////////////////////
        ///////   BETA   ///////////   BETA   /////////   BETA   ///////////   BETA   ///////////////
        //
        // 베타 서비스 가입 보상.
        //
        AddMail(T_GlobalValueData.Get(GLOBAL_VALUE_TYPE.BETA_SERVICE_REWARD_MAIL).ValueInt);
        //
        ///////   BETA   ///////////   BETA   /////////   BETA   ///////////   BETA   ///////////////
        /////////////////////////////////////////////////////////////////////////////////////////////            
    }

    public void SetSeeYouPushMsg(int charTid)
    {
        if (0 == _isSeeYouCharTid)
            return;

        _isSeeYouCharTid = charTid;
    }

    public override bool IsValid()
    {
        if (false == base.IsValid())
            return false;

        // 유저 수명 체크.
        if (_lifeTime < DateTime.Now.ToFileTime())
        {
            return false;
        }

        return true;
    }

    public bool IsNewbie()
    {
        return false;
    }

    public void SetHasMadeIAP()
    {
        // 이미 인앱 구매를 했으면 리턴.
        if (_hasMadeIAP)
            return;

        _hasMadeIAP = true;

        UpdateInfo();
    }

    public bool IsHasMadeIAP()
    {
        return _hasMadeIAP;
    }

    public void AddForbiddenWordCount()
    {
        ++_forbiddenWordCount;
    }

    public bool IsBanSquareChat()
    {
        return (_forbiddenWordCount >= T_GlobalValueData.Get(GLOBAL_VALUE_TYPE.CHAT_BANNED_FORBIDDEN_WORD_LIMIT).ValueInt);
    }

    public void SendResultCode(RESULT_CODE code, RESULT_POPUP_TYPE popupType, string param = "")
    {
        Network.Send(_netId, new C2G.RS_RESULT_CODE(code, popupType, param));
    }

    public void SendResultCode(RESULT_CODE code, string param = "")
    {
        Network.Send(_netId, new C2G.RS_RESULT_CODE(code, RESULT_POPUP_TYPE.POPUP_SIMPLE_MESSAGE, param));
    }

    public void Send(C2GPacket packet)
    {
        Network.Send(_netId, packet);
    }

    public void Send(C2GPacketEx packet)
    {
        Network.Send(_netId, packet);
    }

    public string LogDefault
    {
        get { return $"    【 netId( {_netId} ), pid( {_pid} ), uid( {_uid} ), nickname( {_nickname} ), cid( {_cid} ), gameDB( {_gameDB} ) 】"; }
    }

    
    // ===================================================================================================================
    //
    public int                  netId               => _netId;
    public DB_KIND              gameDB              => (DB_KIND)_gameDB;
    public string               pid                 => _pid;
    public long                 uid                 => _uid;
    public string               accessToken         => _accessToken;
    public string               pushToken           => _pushToken;
    public int                  forbiddenWordCount  => _forbiddenWordCount;
    public string               nickname            => _nickname;
    public int                  profileTid          => _profileTid;


    // ===================================================================================================================
    //
    private int         _netId              = 0;                    // 세션 ID
    private int         _adminLevel         = 0;                    // 관리자 레벨.
    private int         _gameDB             = 0;                    // database kind (유저 정보가 저장되어 있는 종류)

    private string      _pid                = "";                   // 계정 ID
    private long        _uid                = 0;                    // 유저의 DB ID
    public string       _nickname           = "";
    public int          _profileTid         = 0;

    private bool        _hasMadeIAP         = false;                // 결제 한 경험이 있다.
    private string      _accessToken        = "";                   // 엑세스 토큰.
    private string      _pushToken          = "";                   // 푸시 토큰.
    private DateTime    _lastLogoutTime     = DateTime.MinValue;    // 마지막 로그아웃 시간
    private bool        _isFirstLoginDaily  = false;                // 오늘 첫 로그인.
    private int         _isSeeYouCharTid    = 0;                    // "다시 만나" 푸시 알림 처리. (retention)

    private long        _lifeTime           = 0;                    // 생명 주기
    private bool        _pendingShutdown    = false;                // 셧다운 처리중.
    private int         _forbiddenWordCount = 0;                    // 금지어 경고 수.
}


