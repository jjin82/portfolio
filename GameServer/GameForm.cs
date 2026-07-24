

using Common;
using Microsoft.VisualBasic.ApplicationServices;
using NerdFox.OpenAI.define;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

public partial class GameForm : FormEx
{
    public GameForm()
    {
        Logger.SetForm(this);

        InitializeComponent();

        InitializeComponentMapping();
    }

    public override void InitializeComponentMapping()
    {
        _consoleListBox = this.listBoxConsole;

        _logWrite = this.logWrite;
        _logPause = this.logPause;
        _logSystemPacket = this.logSystemPacket;
        _logPacket = this.logPacket;

        _exceptionTextBox = this.exception;
        _criticalTextBox = this.critical;
        _errorTextBox = this.error;
        _failTextBox = this.fail;

        _sendBytes = this.sendBytes;
        _sendCount = this.sendCount;
        _recvBytes = this.recvBytes;
        _recvCount = this.recvCount;
        _sessionCount = this.SessionCount;

        _localMode = this.localMode;
        _localVersion = this.localVersion;
        _localId = this.localId;

        _jobThreadCount = this.jobThreadCount;
        _jobSlowThreadCount = this.jobSlowThreadCount;
        _jobSingleThreadCount = this.jobSingleThreadCount;

        _jobCount = this.jobCount;
        _jobSlowCount = this.jobSlowCount;
        _jobSingleCount = this.jobSingleCount;

        _userCount = this.userCount;
        _roomCount = this.chatRoomCount;
    }

    protected override void OnShown(EventArgs e)
    {
        this.Text += $" ( {ServerConfig.BuildDateTime()} )   ★  {ServerConfig.GetServiceMode()}  ★   {ServerConfig.GetGroup()} ";

        ServerConfig.Name = this.Text;

        Program.Initialize();

        // 타이머 시작.
        timer100.Enabled = true;
        timer1000.Enabled = true;
        timer5000.Enabled = true;

        this.logSystemPacket.Checked = false;
        this.logPacket.Checked = false == ServerConfig.IsLive();
        this.logWrite.Checked = ServerConfig.IsMode(MODE.DEBUG);

        // 라이브 버전 설정.
        if (ServerConfig.IsLive())
        {
            this.logWrite.Enabled = false;
            this.TestButton.Enabled = false;
        }

        InitializeLLM();
    }

    private void timer100_Tick(object sender, EventArgs e)
    {
        //var packet = new G2F.RQ_RELAY_EX();
        //for(int i = 0; i < 1000; ++i)
        //{
        //    packet.In("1234567890");
        //}

        //Network.Send(packet);
    }

    private void timer1000_Tick(object sender, EventArgs e)
    {
        localVersion.Text = T_GlobalValueData.Get(GLOBAL_VALUE_TYPE.DATA_VERSION).ValueString;

        GameDataGroup.Text = $"[ game data ]  ( {GameData.Excel.LastModifiedTime} )";
        GameDataHash.Text = GameData.Excel.Hash;

        // 유저 로그인 누적수.
        CumulativeLogin.Text = UserManager.Get.GetAccumulatedLogin().ToString();

        // 방 수.
        chatRoomCount.Text = "0";

        // 시나리오 방 수
        scenarioRoomCount.Text = "0";

        // 팅방 수.
        tingRoomCount.Text = "0";

        // 게임 수 (실시간, 누적)
        gameCount.Text = GameManager.Get.GetCount().ToString();
        CumulativeGameCount.Text = Game.s_playCount.ToString(); 

        // 채팅 수.
        AIChatCount.Text = "none";
        SquareChatCount.Text = "0";

        // 푸시 메시지 수.
        PushMsgCount.Text = PushManager.Get.Count().ToString();

        // 유저 정보.
        UpdateUserComponent(UserManager.Get.GetCCU(), UserManager.Get.GetMCU());

        // 로그 업데이트.
        UpdateLogComponent();

        // 잡 업데이트.
        UpdateJobComponent();

        // 네트워크 정보 업데이트.
        UpdateNetworkComponent(Network.GetAccpetedCount());
    }

    private void timer5000_Tick(object sender, EventArgs e)
    {
        // front server.
        Network.Send(new G2F.RQ_HEARTBEAT());
    }

    private void ClearLog_Click(object sender, EventArgs e)
    {
        HNET.LOG.Reset();

        // 로그 업데이트.
        UpdateLogComponent();
    }

    private void LoadGameData_Click(object sender, EventArgs e)
    {
        var prevHash = GameData.Excel.Hash;

        // 게임 데이터 로드.
        GameData.Excel.LoadAllGameData("GameData");

        // 상점 업데이트.
        ShopManager.Get.UpdateProductList();

        // 문자열 초기화.
        StringManager.Get.Initialize();

        // 시간별 초기화.
        TimeEventManager.Get.Initialize();

        Logger.INFO_PRINT($"▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼");
        Logger.INFO_PRINT("");
        Logger.INFO_PRINT($"[ Gane Data ]  - ( prev: {prevHash} ] --▶ [ next: {GameData.Excel.Hash} )");
        Logger.INFO_PRINT("");
        Logger.INFO_PRINT($"▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲");
    }

    private void TruncateDatabase_Click(object sender, EventArgs e)
    {
        DialogResult result1 = MessageBox.Show("[▶ ▶ ▶ DRUNCATE DB ◀ ◀ ◀]\n\n Are you sure? \n\nWould you like to delete all data in the database???", "■   WARNING   ■", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        if (result1 == DialogResult.Cancel) return;

        // db 삭제.
        DB.MySql.Truncate();
    }

    private void OpenServer_Click(object sender, EventArgs e)
    {
        Logger.INFO_PRINT($"▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼");
        Logger.INFO_PRINT("");
        Logger.INFO_PRINT($" open server!!!");
        Logger.INFO_PRINT("");
        Logger.INFO_PRINT($"▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲");

        Program.s_serviceOpen = true;
    }

    private void CloseServer_Click(object sender, EventArgs e)
    {
        Logger.INFO_PRINT($"▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼  ▼");
        Logger.INFO_PRINT("");
        Logger.INFO_PRINT($" close server...");
        Logger.INFO_PRINT("");
        Logger.INFO_PRINT($"▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲");

        Program.s_serviceOpen = false;
    }

    private void SendNotice_Click(object sender, EventArgs e)
    {
        NoticeManager.Get.Notify();
    }


    private void InitializeLLM()
    {
        // ▶  LLM 선택  ◀
        MODEL_TYPE LLM          = MODEL_TYPE.GPT_5O_MINI;
        MODEL_TYPE defaultLLM   = MODEL_TYPE.GPT_5O_MINI;
        if (!ServerConfig.IsLive())
        {
            LLM = defaultLLM;
        }

        {
            // LLM 선택지 추가.
            int selectIndex = 0;
            foreach (var v in Enum.GetValues(typeof(MODEL_TYPE)))
            {
                LLMComboBox.Items.Add(v);

                if (v.Equals(LLM))
                {
                    selectIndex = (LLMComboBox.Items.Count - 1);
                }
            }
            LLMComboBox.SelectedIndex = selectIndex;
            LLMComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        {
            // Default LLM 선택지 추가.
            int selectIndex = 0;
            foreach (var v in Enum.GetValues(typeof(MODEL_TYPE)))
            {
                DefaultLLMComboBox.Items.Add(v);

                if (v.Equals(defaultLLM))
                {
                    selectIndex = (DefaultLLMComboBox.Items.Count - 1);
                }
            }
            DefaultLLMComboBox.SelectedIndex = selectIndex;
            DefaultLLMComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }
    }

    private void LLMComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        Program.s_LLM = (MODEL_TYPE)LLMComboBox.SelectedIndex;

        LLMTextBox.Text = Program.s_LLM.ToString();
    }

    private void DefaultLLMComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        Program.s_defaultLLM = (MODEL_TYPE)DefaultLLMComboBox.SelectedIndex;

        DefaultLLMTextBox.Text = Program.s_defaultLLM.ToString();
    }

    private void button1_Click(object sender, EventArgs e)
    {
    }

    private void MakeScenario_Click(object sender, EventArgs e)
    {
        Logger.INFO($" - - - - - - - -  scenario generation is \"TURN ON\"  - - - - - - - - - - - -");

        Program.s_makeScenario = !Program.s_makeScenario;

        if (Program.s_makeScenario)
        {
            MakeScenario.BackColor = Color.Green;
        }
        else
        {
            MakeScenario.BackColor = Color.LightCoral;
        }
    }

    private void LoadRanking_Click(object sender, EventArgs e)
    {
        Logger.INFO($" - - - - - - - -  small talk generation is \"TURN ON\"  - - - - - - - - - - - -");

        RankingManager.Get.LoadDB();
        return;
        

        LoadRanking.BackColor = Color.Gray;

        Program.s_makeSmallTalk = true;

        var user = new User();
        user.GiveProduct(4, 0);

        // 생성형 스몰토크 리셋.
        //ChatOpenerManager.Get.ResetMakeSmallTalk();

        string receipt = "{\"Payload\":\"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\",\"Store\":\"AppleAppStore\",\"TransactionID\":\"2000001044088992\"}";
        ShopManager.Get.AppleInAppPurchase(null, receipt, "", (shopTid) =>
        {

        });
    }
}
