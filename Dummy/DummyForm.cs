using NerdFox.http.define;
using NerdFox.http;
using NerdFox.OpenAI.define;
using NerdFox.OpenAI.http;
using NerdFox.OpenAI.vo;
using NerdFox.GF.vo;
using NerdFox;
using System.Text.RegularExpressions;
using System.Text;
using HNET;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using Newtonsoft.Json;
using CommonStruct;
using System.Reflection.Metadata;

public partial class DummyForm : FormEx
{
    public DummyForm()
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
        _roomCount = this.roomCount;
    }

    protected override void OnShown(EventArgs e)
    {
        this.Text += $" ( {ServerConfig.BuildDateTime()} )   ★  {ServerConfig.GetServiceMode()}  ★   {ServerConfig.GetGroup()} ";

        ServerConfig.Name = this.Text;

        Program.Initialize();

        this.logSystemPacket.Checked = false;
        this.logPacket.Checked = false;

        // 타이머 시작.
        timer100.Enabled = true;
        timer1000.Enabled = true;

        // 치트 명령어 추가.
        foreach (var v in Program.GetCheat())
        {
            CheatComboBox.Items.Add(v);
        }
        CheatComboBox.SelectedIndex = 0;  // 첫 번째 항목 선택
        CheatComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
    }

    private string? SelectCheat()
    {
        string selectedCheat = CheatComboBox?.SelectedItem?.ToString() ?? "";

        // 선택된 항목 출력
        if (selectedCheat != null)
        {
            Logger.INFO($"＞＞＞ send cheat ＞＞＞ ( {selectedCheat} )");
        }
        else
        {
            MessageBox.Show("선택된 항목이 없습니다.");
        }

        return selectedCheat;
    }

    private void SendCheat_Click(object sender, EventArgs e)
    {
        var cheat = SelectCheat();

        if (string.IsNullOrEmpty(cheat))
            return;

        GameConnector.Command_Cheat(cheat, CmdParam1.Text, CmdParam2.Text, CmdParam3.Text, CmdParam4.Text, CmdParam5.Text);
    }

    private void timer100_Tick(object sender, EventArgs e)
    {

    }

    private void timer1000_Tick(object sender, EventArgs e)
    {
        tokenSeq.Text = GameConnector.GetTokenSeq().ToString();
        token.Text = GameConnector.GetToken();

        // 유저 정보.
        UpdateUserComponent(GameConnector.GetUserCount(), 0);

        // 로그 업데이트.
        UpdateLogComponent();

        // 잡 업데이트.
        UpdateJobComponent();

        // 네트워크 정보 업데이트.
        UpdateNetworkComponent(GameConnector.GetSessionCount());
    }

    private void timer10000_Tick(object sender, EventArgs e)
    {
        // 유저 업데이트.
        GameConnector.Update();
    }

    private void ClearLog_Click(object sender, EventArgs e)
    {
        HNET.LOG.Reset();

        // 로그 업데이트.
        UpdateLogComponent();
    }

    private void connect_Click(object sender, EventArgs e)
    {
        GameConnector.ConnectEx((int)tryConnect.Value);
    }

    private void disconnect_Click(object sender, EventArgs e)
    {
        GameConnector.DisconnectEx();
    }

    private void connect_1_Click(object sender, EventArgs e)
    {
        GameConnector.ConnectEx(1);
    }

    private void connect_10_Click(object sender, EventArgs e)
    {
        GameConnector.ConnectEx(10);
    }

    private void connect_100_Click(object sender, EventArgs e)
    {
        GameConnector.ConnectEx(100);
    }

    private void connect_1000_Click(object sender, EventArgs e)
    {
        GameConnector.ConnectEx(1000);
    }

    private void resetAccount_Click(object sender, EventArgs e)
    {
        GameConnector.ResetTokenSeq();
    }

    private void tokenSeq_ValueChanged(object sender, EventArgs e)
    {
        GameConnector.s_tokenSeq = (int)tokenSeq.Value;
    }

    private void ChatMsg_KeyDown(object sender, KeyEventArgs e)
    {
        if (Keys.Enter != e.KeyCode)
        {
            return;
        }

        //GameConnector.Command_AIChat(AIChatMsg.Text);

        AIChatMsg.Text = "";
    }

    private void PlayerChatMsg_KeyDown(object sender, KeyEventArgs e)
    {
        if (Keys.Enter != e.KeyCode)
        {
            return;
        }

        //GameConnector.Command_PlayerChat(PlayerChatMsg.Text);

        PlayerChatMsg.Text = "";
    }

    private void SquareChatMsg_KeyDown(object sender, KeyEventArgs e)
    {
        if (Keys.Enter != e.KeyCode)
        {
            return;
        }

        GameConnector.Command_SquareChat(SquareChatMsg.Text);

        SquareChatMsg.Text = "";
    }

    private void BtnChaneName_Click(object sender, EventArgs e)
    {

        CmdParam1.Text = "";
    }

    private void TestChat_Click(object sender, EventArgs e)
    {

    }

    public class Root
    {
        public string Payload { get; set; } = "";
        public string Store { get; set; } = "";
        public string TransactionID { get; set; } = "";
    }

    public class Payload
    {
        public string json { get; set; } = "";
        public string signature { get; set; } = "";
        public List<string> skuDetails { get; set; } = new List<string>();
    }

    public class PayloadData
    {
        public string orderId { get; set; } = "";
        public string packageName { get; set; } = "";
        public string productId { get; set; } = "";
        public long purchaseTime { get; set; } = 0;
        public int purchaseState { get; set; } = 0;
        public string purchaseToken { get; set; } = "";
        public int quantity { get; set; } = 0;
        public bool acknowledged { get; set; } = false;
    }

    private void TestIap_Click(object sender, EventArgs e)
    {
        Root root2 = JsonConvert.DeserializeObject<Root>("") ?? new Root();

        var receipt = "{\"Payload\":\"{\\\"json\\\":\\\"{\\\\\\\"orderId\\\\\\\":\\\\\\\"GPA.3348-1520-5963-78476\\\\\\\",\\\\\\\"packageName\\\\\\\":\\\\\\\"com.nerdfox.girlfriends\\\\\\\",\\\\\\\"productId\\\\\\\":\\\\\\\"com.nerdfox.girlfriend.aos.test\\\\\\\",\\\\\\\"purchaseTime\\\\\\\":1727161825967,\\\\\\\"purchaseState\\\\\\\":0,\\\\\\\"purchaseToken\\\\\\\":\\\\\\\"amdkfcmmcpgfenfmbbaebboh.AO-J1OwqLTGb1YyNel6K-BamcdXb9eQX-vKfIW8vYyqZeu4aasuSZMIf6oPW9SSlSouebqJW7OWSCleqCLM4F2Pyoi-ADf5ZtWFdNLeDk2T-WMS7h8reghc\\\\\\\",\\\\\\\"quantity\\\\\\\":1,\\\\\\\"acknowledged\\\\\\\":false}\\\",\\\"signature\\\":\\\"FE8whaKufxy2l2eNM6xGH9qaiPoh2wY6sO1PkmH6QWBuN6+B0g7jB3NXsO7fI4etjeNunkDd/0x3ywxuFDtT8EXijae7chMp8GMF+zdLjQUE1gNRP7LgArcpWlXf7ySGPSDJiAsEAhedBb9VL80c0n8MmCTU5W6OWUmloa/ANbpEiDEAp18yB1SohtzxZlvietExQ8nY++ij+aluBb4beY6ZJaBNubzwY9sFOG5KEAx8DgPOWLqf7RXViKb+MhxzFcJjKmY0A1FI4b3oCo4Ne+pBa8/XNhZCTZWigyvo+QvEwiGxoVJUnjQzveKG+L1ZLJEsPslS4od0Tv7D+j8tUQ==\\\",\\\"skuDetails\\\":[\\\"{\\\\\\\"productId\\\\\\\":\\\\\\\"com.nerdfox.girlfriend.aos.test\\\\\\\",\\\\\\\"type\\\\\\\":\\\\\\\"inapp\\\\\\\",\\\\\\\"title\\\\\\\":\\\\\\\"\\\\\\\\ub370\\\\\\\\uc774\\\\\\\\ud130 \\\\\\\\ucda9\\\\\\\\uc804(\\\\\\\\ud14c\\\\\\\\uc2a4\\\\\\\\ud2b8) (\\\\\\\\uac78\\\\\\\\ud504\\\\\\\\ub80c\\\\\\\\ub4dc : FirstTime)\\\\\\\",\\\\\\\"name\\\\\\\":\\\\\\\"\\\\\\\\ub370\\\\\\\\uc774\\\\\\\\ud130 \\\\\\\\ucda9\\\\\\\\uc804(\\\\\\\\ud14c\\\\\\\\uc2a4\\\\\\\\ud2b8)\\\\\\\",\\\\\\\"description\\\\\\\":\\\\\\\"\\\\\\\\ub370\\\\\\\\uc774\\\\\\\\ud130 \\\\\\\\ucda9\\\\\\\\uc804\\\\\\\",\\\\\\\"price\\\\\\\":\\\\\\\"\\\\\\\\u20a91,000\\\\\\\",\\\\\\\"price_amount_micros\\\\\\\":1000000000,\\\\\\\"price_currency_code\\\\\\\":\\\\\\\"KRW\\\\\\\"}\\\"]}\",\"Store\":\"GooglePlay\",\"TransactionID\":\"amdkfcmmcpgfenfmbbaebboh.AO-J1OwqLTGb1YyNel6K-BamcdXb9eQX-vKfIW8vYyqZeu4aasuSZMIf6oPW9SSlSouebqJW7OWSCleqCLM4F2Pyoi-ADf5ZtWFdNLeDk2T-WMS7h8reghc\"}";

        // 루트 객체를 역직렬화
        Root root = JsonConvert.DeserializeObject<Root>(receipt) ?? new Root();

        // Payload를 역직렬화
        var payload = JsonConvert.DeserializeObject<Payload>(root.Payload);

        // payload의 json 필드를 역직렬화
        var payloadData = JsonConvert.DeserializeObject<PayloadData>(payload?.json ?? "");
    }

    public struct PushSubMessage
    {
        public string cid;
        public string message;
    }

    private void PushMessage_Click(object sender, EventArgs e)
    {
        string pushToken = CmdParam1.Text;

        pushToken = "dByZe090SAyzAz_ukz_d5F:APA91bGaOw-Dxt45ZOmPO458-emgbuUuYMo_X8VaGpoh6R4Xf0eMblaE5PxyX6ZRN5c4-Ay0fZWXR-J14TlWaGwsA-SescHMS5Lis-YU3NW2IuOVpcDlMvA";

        var subMessage = new PushSubMessage();
        subMessage.cid = "777";
        subMessage.message = "되라 잘되라";
        var subJson = JsonConvert.SerializeObject(subMessage);

        var connector = NerdFoxConnector.Create();
        connector.SetGameType(NERDFOX_GAME_TYPE.GF);
        connector.SendPush("걸프랜드", "기다리자나 짜식아!!", pushToken, subJson);
    }

    private void Withdraw_Click(object sender, EventArgs e)
    {
        var reply = "1. 스포트라이트\n2. 그랜드 부다페스트 호텔\n3. 올드보이\n4. 라이프 오브 파이\n5. 패신저스\n6. 해리포터와 마법사의 돌\n7. 안나 카레니나\n8. 포커스\n9. 엣지 오브 투모로우\n10. 덩케르크";

        var s0 = HNET.API.NewlineAfterChar(reply);
        var s1 = HNET.API.NewlineAfterChar("아, '행'. 이름 말하는 거구나! 정말 귀여운 이름이지? 다른 캐릭터 이름도 생각해봤어?");
        var s2 = HNET.API.NewlineAfterChar("아, '행'. 이름 말하는 거구나! 정말 귀여운 이름이지? 다른 캐릭터 이름도 생각해봤어?");

        return;

        GameConnector.Command_Withdraw();
    }

    private void GiftMobileData_Click(object sender, EventArgs e)
    {
        GameConnector.Command_GiftMobileData();
    }

    private void InviteRespond_Click(object sender, EventArgs e)
    {
        GameConnector.Command_GiftMobileData();
    }
}
