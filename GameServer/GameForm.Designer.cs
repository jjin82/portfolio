partial class GameForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
        GameDataHash = new TextBox();
        timer1000 = new System.Windows.Forms.Timer(components);
        groupBox1 = new GroupBox();
        fail = new TextBox();
        error = new TextBox();
        critical = new TextBox();
        ClearLog = new Button();
        exception = new TextBox();
        label4 = new Label();
        label3 = new Label();
        label2 = new Label();
        label1 = new Label();
        GameDataGroup = new GroupBox();
        LoadGameData = new Button();
        groupBox2 = new GroupBox();
        groupBox13 = new GroupBox();
        label26 = new Label();
        PushMsgCount = new TextBox();
        label25 = new Label();
        SquareChatCount = new TextBox();
        label24 = new Label();
        AIChatCount = new TextBox();
        groupBox11 = new GroupBox();
        CumulativeGameCount = new TextBox();
        label21 = new Label();
        gameCount = new TextBox();
        jobSingleCount = new TextBox();
        jobSlowCount = new TextBox();
        jobCount = new TextBox();
        jobSingleThreadCount = new TextBox();
        label20 = new Label();
        groupBox9 = new GroupBox();
        label28 = new Label();
        tingRoomCount = new TextBox();
        label27 = new Label();
        scenarioRoomCount = new TextBox();
        label18 = new Label();
        chatRoomCount = new TextBox();
        groupBox7 = new GroupBox();
        label19 = new Label();
        CumulativeLogin = new TextBox();
        label5 = new Label();
        userCount = new TextBox();
        jobSlowThreadCount = new TextBox();
        label17 = new Label();
        jobThreadCount = new TextBox();
        label16 = new Label();
        localId = new TextBox();
        label8 = new Label();
        localVersion = new TextBox();
        label7 = new Label();
        localMode = new TextBox();
        label6 = new Label();
        groupBox12 = new GroupBox();
        pvpRankingExpireTime = new TextBox();
        weekRankingExpireTime = new TextBox();
        dailyRankingExpireTime = new TextBox();
        pvpRankingCount = new TextBox();
        weekRankingCount = new TextBox();
        dailyRankingCount = new TextBox();
        pvpRankingSeason = new TextBox();
        weekRankingSeason = new TextBox();
        label9 = new Label();
        label22 = new Label();
        label23 = new Label();
        dailyRankingSeason = new TextBox();
        recvCount = new TextBox();
        recvBytes = new TextBox();
        sendCount = new TextBox();
        sendBytes = new TextBox();
        timer100 = new System.Windows.Forms.Timer(components);
        timer5000 = new System.Windows.Forms.Timer(components);
        groupBox3 = new GroupBox();
        groupBox6 = new GroupBox();
        SessionCount = new TextBox();
        groupBox5 = new GroupBox();
        groupBox4 = new GroupBox();
        listBoxConsole = new ListBox();
        logPause = new CheckBox();
        logWrite = new CheckBox();
        groupBox8 = new GroupBox();
        logPacket = new CheckBox();
        logSystemPacket = new CheckBox();
        groupBox10 = new GroupBox();
        LoadRanking = new Button();
        remainIAP = new CheckBox();
        MakeScenario = new Button();
        scenario = new CheckBox();
        chatHistory = new CheckBox();
        SendNotice = new Button();
        OpenServer = new Button();
        CloseServer = new Button();
        TruncateDatabase = new Button();
        webLogin = new CheckBox();
        LLMComboBox = new ComboBox();
        LLMGroup = new GroupBox();
        LLMTextBox = new TextBox();
        TestButton = new Button();
        groupBox14 = new GroupBox();
        DefaultLLMTextBox = new TextBox();
        DefaultLLMComboBox = new ComboBox();
        groupBox1.SuspendLayout();
        GameDataGroup.SuspendLayout();
        groupBox2.SuspendLayout();
        groupBox13.SuspendLayout();
        groupBox11.SuspendLayout();
        groupBox9.SuspendLayout();
        groupBox7.SuspendLayout();
        groupBox12.SuspendLayout();
        groupBox3.SuspendLayout();
        groupBox6.SuspendLayout();
        groupBox5.SuspendLayout();
        groupBox4.SuspendLayout();
        groupBox8.SuspendLayout();
        groupBox10.SuspendLayout();
        LLMGroup.SuspendLayout();
        groupBox14.SuspendLayout();
        SuspendLayout();
        // 
        // GameDataHash
        // 
        GameDataHash.Location = new Point(16, 32);
        GameDataHash.Name = "GameDataHash";
        GameDataHash.Size = new Size(294, 23);
        GameDataHash.TabIndex = 3;
        GameDataHash.TextAlign = HorizontalAlignment.Center;
        // 
        // timer1000
        // 
        timer1000.Interval = 1000;
        timer1000.Tick += timer1000_Tick;
        // 
        // groupBox1
        // 
        groupBox1.BackColor = SystemColors.ScrollBar;
        groupBox1.Controls.Add(fail);
        groupBox1.Controls.Add(error);
        groupBox1.Controls.Add(critical);
        groupBox1.Controls.Add(ClearLog);
        groupBox1.Controls.Add(exception);
        groupBox1.Controls.Add(label4);
        groupBox1.Controls.Add(label3);
        groupBox1.Controls.Add(label2);
        groupBox1.Controls.Add(label1);
        groupBox1.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        groupBox1.Location = new Point(1421, 754);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new Size(195, 140);
        groupBox1.TabIndex = 4;
        groupBox1.TabStop = false;
        groupBox1.Text = "log";
        // 
        // fail
        // 
        fail.Location = new Point(82, 109);
        fail.Name = "fail";
        fail.ReadOnly = true;
        fail.Size = new Size(93, 23);
        fail.TabIndex = 9;
        fail.TextAlign = HorizontalAlignment.Right;
        // 
        // error
        // 
        error.Location = new Point(82, 82);
        error.Name = "error";
        error.ReadOnly = true;
        error.Size = new Size(93, 23);
        error.TabIndex = 8;
        error.TextAlign = HorizontalAlignment.Right;
        // 
        // critical
        // 
        critical.Location = new Point(82, 55);
        critical.Name = "critical";
        critical.ReadOnly = true;
        critical.Size = new Size(93, 23);
        critical.TabIndex = 7;
        critical.TextAlign = HorizontalAlignment.Right;
        // 
        // ClearLog
        // 
        ClearLog.BackColor = Color.RosyBrown;
        ClearLog.Location = new Point(172, 0);
        ClearLog.Name = "ClearLog";
        ClearLog.Size = new Size(24, 24);
        ClearLog.TabIndex = 10;
        ClearLog.Text = "*";
        ClearLog.UseVisualStyleBackColor = false;
        ClearLog.Click += ClearLog_Click;
        // 
        // exception
        // 
        exception.Location = new Point(82, 28);
        exception.Name = "exception";
        exception.ReadOnly = true;
        exception.Size = new Size(93, 23);
        exception.TabIndex = 6;
        exception.TextAlign = HorizontalAlignment.Right;
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Location = new Point(17, 113);
        label4.Name = "label4";
        label4.Size = new Size(24, 15);
        label4.TabIndex = 3;
        label4.Text = "fail";
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Location = new Point(17, 86);
        label3.Name = "label3";
        label3.Size = new Size(36, 15);
        label3.TabIndex = 2;
        label3.Text = "error";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(17, 59);
        label2.Name = "label2";
        label2.Size = new Size(45, 15);
        label2.TabIndex = 1;
        label2.Text = "critical";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(17, 32);
        label1.Name = "label1";
        label1.Size = new Size(63, 15);
        label1.TabIndex = 0;
        label1.Text = "exception";
        // 
        // GameDataGroup
        // 
        GameDataGroup.BackColor = SystemColors.ScrollBar;
        GameDataGroup.Controls.Add(LoadGameData);
        GameDataGroup.Controls.Add(GameDataHash);
        GameDataGroup.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        GameDataGroup.Location = new Point(1239, 508);
        GameDataGroup.Name = "GameDataGroup";
        GameDataGroup.Size = new Size(377, 70);
        GameDataGroup.TabIndex = 11;
        GameDataGroup.TabStop = false;
        GameDataGroup.Text = "[ game data ]";
        // 
        // LoadGameData
        // 
        LoadGameData.Location = new Point(316, 30);
        LoadGameData.Name = "LoadGameData";
        LoadGameData.Size = new Size(54, 26);
        LoadGameData.TabIndex = 4;
        LoadGameData.Text = "load";
        LoadGameData.UseVisualStyleBackColor = true;
        LoadGameData.Click += LoadGameData_Click;
        // 
        // groupBox2
        // 
        groupBox2.BackColor = SystemColors.ScrollBar;
        groupBox2.Controls.Add(groupBox13);
        groupBox2.Controls.Add(groupBox11);
        groupBox2.Controls.Add(jobSingleCount);
        groupBox2.Controls.Add(jobSlowCount);
        groupBox2.Controls.Add(jobCount);
        groupBox2.Controls.Add(jobSingleThreadCount);
        groupBox2.Controls.Add(label20);
        groupBox2.Controls.Add(groupBox9);
        groupBox2.Controls.Add(groupBox7);
        groupBox2.Controls.Add(jobSlowThreadCount);
        groupBox2.Controls.Add(label17);
        groupBox2.Controls.Add(jobThreadCount);
        groupBox2.Controls.Add(label16);
        groupBox2.Controls.Add(localId);
        groupBox2.Controls.Add(label8);
        groupBox2.Controls.Add(localVersion);
        groupBox2.Controls.Add(label7);
        groupBox2.Controls.Add(localMode);
        groupBox2.Controls.Add(label6);
        groupBox2.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        groupBox2.Location = new Point(12, 508);
        groupBox2.Name = "groupBox2";
        groupBox2.Size = new Size(511, 386);
        groupBox2.TabIndex = 12;
        groupBox2.TabStop = false;
        groupBox2.Text = "info";
        // 
        // groupBox13
        // 
        groupBox13.Controls.Add(label26);
        groupBox13.Controls.Add(PushMsgCount);
        groupBox13.Controls.Add(label25);
        groupBox13.Controls.Add(SquareChatCount);
        groupBox13.Controls.Add(label24);
        groupBox13.Controls.Add(AIChatCount);
        groupBox13.Location = new Point(17, 246);
        groupBox13.Name = "groupBox13";
        groupBox13.Size = new Size(220, 128);
        groupBox13.TabIndex = 36;
        groupBox13.TabStop = false;
        groupBox13.Text = "chat";
        // 
        // label26
        // 
        label26.AutoSize = true;
        label26.Location = new Point(9, 93);
        label26.Name = "label26";
        label26.Size = new Size(64, 15);
        label26.TabIndex = 37;
        label26.Text = "push msg";
        // 
        // PushMsgCount
        // 
        PushMsgCount.Location = new Point(93, 89);
        PushMsgCount.Name = "PushMsgCount";
        PushMsgCount.ReadOnly = true;
        PushMsgCount.Size = new Size(116, 23);
        PushMsgCount.TabIndex = 36;
        PushMsgCount.TextAlign = HorizontalAlignment.Center;
        // 
        // label25
        // 
        label25.AutoSize = true;
        label25.Location = new Point(9, 64);
        label25.Name = "label25";
        label25.Size = new Size(76, 15);
        label25.TabIndex = 35;
        label25.Text = "square chat";
        // 
        // SquareChatCount
        // 
        SquareChatCount.Location = new Point(93, 60);
        SquareChatCount.Name = "SquareChatCount";
        SquareChatCount.ReadOnly = true;
        SquareChatCount.Size = new Size(116, 23);
        SquareChatCount.TabIndex = 34;
        SquareChatCount.TextAlign = HorizontalAlignment.Center;
        // 
        // label24
        // 
        label24.AutoSize = true;
        label24.Location = new Point(9, 35);
        label24.Name = "label24";
        label24.Size = new Size(46, 15);
        label24.TabIndex = 33;
        label24.Text = "ai chat";
        // 
        // AIChatCount
        // 
        AIChatCount.Location = new Point(93, 31);
        AIChatCount.Name = "AIChatCount";
        AIChatCount.ReadOnly = true;
        AIChatCount.Size = new Size(116, 23);
        AIChatCount.TabIndex = 19;
        AIChatCount.TextAlign = HorizontalAlignment.Center;
        // 
        // groupBox11
        // 
        groupBox11.Controls.Add(CumulativeGameCount);
        groupBox11.Controls.Add(label21);
        groupBox11.Controls.Add(gameCount);
        groupBox11.Location = new Point(253, 246);
        groupBox11.Name = "groupBox11";
        groupBox11.Size = new Size(245, 128);
        groupBox11.TabIndex = 35;
        groupBox11.TabStop = false;
        groupBox11.Text = "Game";
        // 
        // CumulativeGameCount
        // 
        CumulativeGameCount.Location = new Point(125, 31);
        CumulativeGameCount.Name = "CumulativeGameCount";
        CumulativeGameCount.ReadOnly = true;
        CumulativeGameCount.Size = new Size(108, 23);
        CumulativeGameCount.TabIndex = 34;
        CumulativeGameCount.TextAlign = HorizontalAlignment.Center;
        // 
        // label21
        // 
        label21.AutoSize = true;
        label21.Location = new Point(14, 35);
        label21.Name = "label21";
        label21.Size = new Size(39, 15);
        label21.TabIndex = 33;
        label21.Text = "count";
        // 
        // gameCount
        // 
        gameCount.Location = new Point(58, 31);
        gameCount.Name = "gameCount";
        gameCount.ReadOnly = true;
        gameCount.Size = new Size(61, 23);
        gameCount.TabIndex = 19;
        gameCount.TextAlign = HorizontalAlignment.Center;
        // 
        // jobSingleCount
        // 
        jobSingleCount.BackColor = SystemColors.Control;
        jobSingleCount.Location = new Point(357, 80);
        jobSingleCount.Name = "jobSingleCount";
        jobSingleCount.ReadOnly = true;
        jobSingleCount.Size = new Size(99, 23);
        jobSingleCount.TabIndex = 39;
        jobSingleCount.TextAlign = HorizontalAlignment.Center;
        // 
        // jobSlowCount
        // 
        jobSlowCount.BackColor = SystemColors.Control;
        jobSlowCount.Location = new Point(357, 51);
        jobSlowCount.Name = "jobSlowCount";
        jobSlowCount.ReadOnly = true;
        jobSlowCount.Size = new Size(99, 23);
        jobSlowCount.TabIndex = 38;
        jobSlowCount.TextAlign = HorizontalAlignment.Center;
        // 
        // jobCount
        // 
        jobCount.BackColor = SystemColors.Control;
        jobCount.Location = new Point(357, 24);
        jobCount.Name = "jobCount";
        jobCount.ReadOnly = true;
        jobCount.Size = new Size(99, 23);
        jobCount.TabIndex = 37;
        jobCount.TextAlign = HorizontalAlignment.Center;
        // 
        // jobSingleThreadCount
        // 
        jobSingleThreadCount.BackColor = SystemColors.ControlDark;
        jobSingleThreadCount.Location = new Point(462, 80);
        jobSingleThreadCount.Name = "jobSingleThreadCount";
        jobSingleThreadCount.ReadOnly = true;
        jobSingleThreadCount.Size = new Size(36, 23);
        jobSingleThreadCount.TabIndex = 36;
        jobSingleThreadCount.TextAlign = HorizontalAlignment.Center;
        // 
        // label20
        // 
        label20.AutoSize = true;
        label20.Location = new Point(278, 84);
        label20.Name = "label20";
        label20.Size = new Size(63, 15);
        label20.TabIndex = 35;
        label20.Text = "job single";
        // 
        // groupBox9
        // 
        groupBox9.Controls.Add(label28);
        groupBox9.Controls.Add(tingRoomCount);
        groupBox9.Controls.Add(label27);
        groupBox9.Controls.Add(scenarioRoomCount);
        groupBox9.Controls.Add(label18);
        groupBox9.Controls.Add(chatRoomCount);
        groupBox9.Location = new Point(253, 119);
        groupBox9.Name = "groupBox9";
        groupBox9.Size = new Size(245, 109);
        groupBox9.TabIndex = 34;
        groupBox9.TabStop = false;
        groupBox9.Text = "room";
        // 
        // label28
        // 
        label28.AutoSize = true;
        label28.Location = new Point(20, 81);
        label28.Name = "label28";
        label28.Size = new Size(30, 15);
        label28.TabIndex = 37;
        label28.Text = "ting";
        // 
        // tingRoomCount
        // 
        tingRoomCount.Location = new Point(93, 77);
        tingRoomCount.Name = "tingRoomCount";
        tingRoomCount.ReadOnly = true;
        tingRoomCount.Size = new Size(116, 23);
        tingRoomCount.TabIndex = 36;
        tingRoomCount.TextAlign = HorizontalAlignment.Center;
        // 
        // label27
        // 
        label27.AutoSize = true;
        label27.Location = new Point(20, 55);
        label27.Name = "label27";
        label27.Size = new Size(55, 15);
        label27.TabIndex = 35;
        label27.Text = "scenario";
        // 
        // scenarioRoomCount
        // 
        scenarioRoomCount.Location = new Point(93, 51);
        scenarioRoomCount.Name = "scenarioRoomCount";
        scenarioRoomCount.ReadOnly = true;
        scenarioRoomCount.Size = new Size(116, 23);
        scenarioRoomCount.TabIndex = 34;
        scenarioRoomCount.TextAlign = HorizontalAlignment.Center;
        // 
        // label18
        // 
        label18.AutoSize = true;
        label18.Location = new Point(20, 29);
        label18.Name = "label18";
        label18.Size = new Size(32, 15);
        label18.TabIndex = 33;
        label18.Text = "chat";
        // 
        // chatRoomCount
        // 
        chatRoomCount.Location = new Point(93, 25);
        chatRoomCount.Name = "chatRoomCount";
        chatRoomCount.ReadOnly = true;
        chatRoomCount.Size = new Size(116, 23);
        chatRoomCount.TabIndex = 19;
        chatRoomCount.TextAlign = HorizontalAlignment.Center;
        // 
        // groupBox7
        // 
        groupBox7.Controls.Add(label19);
        groupBox7.Controls.Add(CumulativeLogin);
        groupBox7.Controls.Add(label5);
        groupBox7.Controls.Add(userCount);
        groupBox7.Location = new Point(17, 119);
        groupBox7.Name = "groupBox7";
        groupBox7.Size = new Size(220, 109);
        groupBox7.TabIndex = 32;
        groupBox7.TabStop = false;
        groupBox7.Text = "user";
        // 
        // label19
        // 
        label19.AutoSize = true;
        label19.Location = new Point(9, 64);
        label19.Name = "label19";
        label19.Size = new Size(69, 15);
        label19.TabIndex = 35;
        label19.Text = "cumulative";
        // 
        // CumulativeLogin
        // 
        CumulativeLogin.Location = new Point(93, 60);
        CumulativeLogin.Name = "CumulativeLogin";
        CumulativeLogin.ReadOnly = true;
        CumulativeLogin.Size = new Size(116, 23);
        CumulativeLogin.TabIndex = 34;
        CumulativeLogin.TextAlign = HorizontalAlignment.Center;
        // 
        // label5
        // 
        label5.AutoSize = true;
        label5.Location = new Point(9, 35);
        label5.Name = "label5";
        label5.Size = new Size(26, 15);
        label5.TabIndex = 33;
        label5.Text = "ccu";
        // 
        // userCount
        // 
        userCount.Location = new Point(93, 31);
        userCount.Name = "userCount";
        userCount.ReadOnly = true;
        userCount.Size = new Size(116, 23);
        userCount.TabIndex = 19;
        userCount.TextAlign = HorizontalAlignment.Center;
        // 
        // jobSlowThreadCount
        // 
        jobSlowThreadCount.BackColor = SystemColors.ControlDark;
        jobSlowThreadCount.Location = new Point(462, 51);
        jobSlowThreadCount.Name = "jobSlowThreadCount";
        jobSlowThreadCount.ReadOnly = true;
        jobSlowThreadCount.Size = new Size(36, 23);
        jobSlowThreadCount.TabIndex = 31;
        jobSlowThreadCount.TextAlign = HorizontalAlignment.Center;
        // 
        // label17
        // 
        label17.AutoSize = true;
        label17.Location = new Point(278, 55);
        label17.Name = "label17";
        label17.Size = new Size(55, 15);
        label17.TabIndex = 30;
        label17.Text = "job slow";
        // 
        // jobThreadCount
        // 
        jobThreadCount.BackColor = SystemColors.ControlDark;
        jobThreadCount.Location = new Point(462, 24);
        jobThreadCount.Name = "jobThreadCount";
        jobThreadCount.ReadOnly = true;
        jobThreadCount.Size = new Size(36, 23);
        jobThreadCount.TabIndex = 29;
        jobThreadCount.TextAlign = HorizontalAlignment.Center;
        // 
        // label16
        // 
        label16.AutoSize = true;
        label16.Location = new Point(278, 28);
        label16.Name = "label16";
        label16.Size = new Size(25, 15);
        label16.TabIndex = 28;
        label16.Text = "job";
        // 
        // localId
        // 
        localId.Location = new Point(82, 78);
        localId.Name = "localId";
        localId.ReadOnly = true;
        localId.Size = new Size(93, 23);
        localId.TabIndex = 27;
        localId.TextAlign = HorizontalAlignment.Center;
        // 
        // label8
        // 
        label8.AutoSize = true;
        label8.Location = new Point(17, 28);
        label8.Name = "label8";
        label8.Size = new Size(40, 15);
        label8.TabIndex = 22;
        label8.Text = "mode";
        // 
        // localVersion
        // 
        localVersion.Location = new Point(82, 51);
        localVersion.Name = "localVersion";
        localVersion.ReadOnly = true;
        localVersion.Size = new Size(93, 23);
        localVersion.TabIndex = 26;
        localVersion.TextAlign = HorizontalAlignment.Center;
        // 
        // label7
        // 
        label7.AutoSize = true;
        label7.Location = new Point(17, 55);
        label7.Name = "label7";
        label7.Size = new Size(48, 15);
        label7.TabIndex = 23;
        label7.Text = "version";
        // 
        // localMode
        // 
        localMode.Location = new Point(82, 24);
        localMode.Name = "localMode";
        localMode.ReadOnly = true;
        localMode.Size = new Size(93, 23);
        localMode.TabIndex = 25;
        localMode.TextAlign = HorizontalAlignment.Center;
        // 
        // label6
        // 
        label6.AutoSize = true;
        label6.Location = new Point(17, 82);
        label6.Name = "label6";
        label6.Size = new Size(18, 15);
        label6.TabIndex = 24;
        label6.Text = "id";
        // 
        // groupBox12
        // 
        groupBox12.BackColor = SystemColors.ScrollBar;
        groupBox12.Controls.Add(pvpRankingExpireTime);
        groupBox12.Controls.Add(weekRankingExpireTime);
        groupBox12.Controls.Add(dailyRankingExpireTime);
        groupBox12.Controls.Add(pvpRankingCount);
        groupBox12.Controls.Add(weekRankingCount);
        groupBox12.Controls.Add(dailyRankingCount);
        groupBox12.Controls.Add(pvpRankingSeason);
        groupBox12.Controls.Add(weekRankingSeason);
        groupBox12.Controls.Add(label9);
        groupBox12.Controls.Add(label22);
        groupBox12.Controls.Add(label23);
        groupBox12.Controls.Add(dailyRankingSeason);
        groupBox12.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        groupBox12.Location = new Point(529, 508);
        groupBox12.Name = "groupBox12";
        groupBox12.Size = new Size(274, 386);
        groupBox12.TabIndex = 36;
        groupBox12.TabStop = false;
        groupBox12.Text = "ranking";
        // 
        // pvpRankingExpireTime
        // 
        pvpRankingExpireTime.Location = new Point(64, 288);
        pvpRankingExpireTime.Name = "pvpRankingExpireTime";
        pvpRankingExpireTime.ReadOnly = true;
        pvpRankingExpireTime.Size = new Size(200, 23);
        pvpRankingExpireTime.TabIndex = 44;
        pvpRankingExpireTime.Text = "000000";
        pvpRankingExpireTime.TextAlign = HorizontalAlignment.Center;
        // 
        // weekRankingExpireTime
        // 
        weekRankingExpireTime.Location = new Point(64, 173);
        weekRankingExpireTime.Name = "weekRankingExpireTime";
        weekRankingExpireTime.ReadOnly = true;
        weekRankingExpireTime.Size = new Size(200, 23);
        weekRankingExpireTime.TabIndex = 43;
        weekRankingExpireTime.Text = "000000";
        weekRankingExpireTime.TextAlign = HorizontalAlignment.Center;
        // 
        // dailyRankingExpireTime
        // 
        dailyRankingExpireTime.Location = new Point(64, 57);
        dailyRankingExpireTime.Name = "dailyRankingExpireTime";
        dailyRankingExpireTime.ReadOnly = true;
        dailyRankingExpireTime.Size = new Size(200, 23);
        dailyRankingExpireTime.TabIndex = 42;
        dailyRankingExpireTime.Text = "000000";
        dailyRankingExpireTime.TextAlign = HorizontalAlignment.Center;
        // 
        // pvpRankingCount
        // 
        pvpRankingCount.Location = new Point(118, 261);
        pvpRankingCount.Name = "pvpRankingCount";
        pvpRankingCount.ReadOnly = true;
        pvpRankingCount.Size = new Size(59, 23);
        pvpRankingCount.TabIndex = 41;
        pvpRankingCount.TextAlign = HorizontalAlignment.Center;
        // 
        // weekRankingCount
        // 
        weekRankingCount.Location = new Point(118, 146);
        weekRankingCount.Name = "weekRankingCount";
        weekRankingCount.ReadOnly = true;
        weekRankingCount.Size = new Size(59, 23);
        weekRankingCount.TabIndex = 40;
        weekRankingCount.TextAlign = HorizontalAlignment.Center;
        // 
        // dailyRankingCount
        // 
        dailyRankingCount.Location = new Point(118, 31);
        dailyRankingCount.Name = "dailyRankingCount";
        dailyRankingCount.ReadOnly = true;
        dailyRankingCount.Size = new Size(59, 23);
        dailyRankingCount.TabIndex = 39;
        dailyRankingCount.Text = "000000";
        dailyRankingCount.TextAlign = HorizontalAlignment.Center;
        // 
        // pvpRankingSeason
        // 
        pvpRankingSeason.Location = new Point(64, 261);
        pvpRankingSeason.Name = "pvpRankingSeason";
        pvpRankingSeason.ReadOnly = true;
        pvpRankingSeason.Size = new Size(48, 23);
        pvpRankingSeason.TabIndex = 38;
        pvpRankingSeason.TextAlign = HorizontalAlignment.Center;
        // 
        // weekRankingSeason
        // 
        weekRankingSeason.Location = new Point(64, 146);
        weekRankingSeason.Name = "weekRankingSeason";
        weekRankingSeason.ReadOnly = true;
        weekRankingSeason.Size = new Size(48, 23);
        weekRankingSeason.TabIndex = 37;
        weekRankingSeason.TextAlign = HorizontalAlignment.Center;
        // 
        // label9
        // 
        label9.AutoSize = true;
        label9.Location = new Point(28, 265);
        label9.Name = "label9";
        label9.Size = new Size(29, 15);
        label9.TabIndex = 36;
        label9.Text = "pvp";
        // 
        // label22
        // 
        label22.AutoSize = true;
        label22.Location = new Point(21, 150);
        label22.Name = "label22";
        label22.Size = new Size(38, 15);
        label22.TabIndex = 35;
        label22.Text = "week";
        // 
        // label23
        // 
        label23.AutoSize = true;
        label23.Location = new Point(21, 35);
        label23.Name = "label23";
        label23.Size = new Size(34, 15);
        label23.TabIndex = 33;
        label23.Text = "daily";
        // 
        // dailyRankingSeason
        // 
        dailyRankingSeason.Location = new Point(64, 31);
        dailyRankingSeason.Name = "dailyRankingSeason";
        dailyRankingSeason.ReadOnly = true;
        dailyRankingSeason.Size = new Size(48, 23);
        dailyRankingSeason.TabIndex = 19;
        dailyRankingSeason.Text = "0000";
        dailyRankingSeason.TextAlign = HorizontalAlignment.Center;
        // 
        // recvCount
        // 
        recvCount.Location = new Point(20, 56);
        recvCount.Name = "recvCount";
        recvCount.ReadOnly = true;
        recvCount.Size = new Size(144, 23);
        recvCount.TabIndex = 20;
        recvCount.TextAlign = HorizontalAlignment.Right;
        // 
        // recvBytes
        // 
        recvBytes.Location = new Point(20, 30);
        recvBytes.Name = "recvBytes";
        recvBytes.ReadOnly = true;
        recvBytes.Size = new Size(144, 23);
        recvBytes.TabIndex = 19;
        recvBytes.TextAlign = HorizontalAlignment.Right;
        // 
        // sendCount
        // 
        sendCount.Location = new Point(18, 54);
        sendCount.Name = "sendCount";
        sendCount.ReadOnly = true;
        sendCount.Size = new Size(144, 23);
        sendCount.TabIndex = 18;
        sendCount.TextAlign = HorizontalAlignment.Right;
        // 
        // sendBytes
        // 
        sendBytes.Location = new Point(18, 28);
        sendBytes.Name = "sendBytes";
        sendBytes.ReadOnly = true;
        sendBytes.Size = new Size(144, 23);
        sendBytes.TabIndex = 17;
        sendBytes.TextAlign = HorizontalAlignment.Right;
        // 
        // timer100
        // 
        timer100.Tick += timer100_Tick;
        // 
        // timer5000
        // 
        timer5000.Interval = 5000;
        timer5000.Tick += timer5000_Tick;
        // 
        // groupBox3
        // 
        groupBox3.BackColor = SystemColors.ScrollBar;
        groupBox3.Controls.Add(groupBox6);
        groupBox3.Controls.Add(groupBox5);
        groupBox3.Controls.Add(groupBox4);
        groupBox3.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        groupBox3.Location = new Point(809, 508);
        groupBox3.Name = "groupBox3";
        groupBox3.Size = new Size(214, 386);
        groupBox3.TabIndex = 21;
        groupBox3.TabStop = false;
        groupBox3.Text = "network  ( client acceptor )";
        // 
        // groupBox6
        // 
        groupBox6.Controls.Add(SessionCount);
        groupBox6.Location = new Point(17, 265);
        groupBox6.Name = "groupBox6";
        groupBox6.Size = new Size(182, 95);
        groupBox6.TabIndex = 23;
        groupBox6.TabStop = false;
        groupBox6.Text = "session";
        // 
        // SessionCount
        // 
        SessionCount.Location = new Point(20, 30);
        SessionCount.Name = "SessionCount";
        SessionCount.ReadOnly = true;
        SessionCount.Size = new Size(144, 23);
        SessionCount.TabIndex = 19;
        SessionCount.TextAlign = HorizontalAlignment.Right;
        // 
        // groupBox5
        // 
        groupBox5.Controls.Add(recvCount);
        groupBox5.Controls.Add(recvBytes);
        groupBox5.Location = new Point(17, 147);
        groupBox5.Name = "groupBox5";
        groupBox5.Size = new Size(182, 95);
        groupBox5.TabIndex = 22;
        groupBox5.TabStop = false;
        groupBox5.Text = "recv  ( bytes )";
        // 
        // groupBox4
        // 
        groupBox4.Controls.Add(sendCount);
        groupBox4.Controls.Add(sendBytes);
        groupBox4.Location = new Point(17, 33);
        groupBox4.Name = "groupBox4";
        groupBox4.Size = new Size(182, 95);
        groupBox4.TabIndex = 21;
        groupBox4.TabStop = false;
        groupBox4.Text = "send  ( bytes )";
        // 
        // listBoxConsole
        // 
        listBoxConsole.FormattingEnabled = true;
        listBoxConsole.HorizontalScrollbar = true;
        listBoxConsole.ItemHeight = 15;
        listBoxConsole.Location = new Point(6, 4);
        listBoxConsole.Name = "listBoxConsole";
        listBoxConsole.Size = new Size(1592, 484);
        listBoxConsole.TabIndex = 2;
        // 
        // logPause
        // 
        logPause.AutoSize = true;
        logPause.BackColor = SystemColors.Control;
        logPause.Location = new Point(1477, 9);
        logPause.Name = "logPause";
        logPause.Size = new Size(57, 19);
        logPause.TabIndex = 33;
        logPause.Text = "pause";
        logPause.UseVisualStyleBackColor = false;
        // 
        // logWrite
        // 
        logWrite.AutoSize = true;
        logWrite.BackColor = SystemColors.Control;
        logWrite.Checked = true;
        logWrite.CheckState = CheckState.Checked;
        logWrite.Location = new Point(1540, 9);
        logWrite.Name = "logWrite";
        logWrite.Size = new Size(52, 19);
        logWrite.TabIndex = 34;
        logWrite.Text = "write";
        logWrite.UseVisualStyleBackColor = false;
        // 
        // groupBox8
        // 
        groupBox8.BackColor = SystemColors.ControlDark;
        groupBox8.Controls.Add(logPacket);
        groupBox8.Controls.Add(logSystemPacket);
        groupBox8.Controls.Add(logWrite);
        groupBox8.Controls.Add(logPause);
        groupBox8.Controls.Add(listBoxConsole);
        groupBox8.Location = new Point(12, 9);
        groupBox8.Name = "groupBox8";
        groupBox8.Size = new Size(1604, 493);
        groupBox8.TabIndex = 34;
        groupBox8.TabStop = false;
        // 
        // logPacket
        // 
        logPacket.AutoSize = true;
        logPacket.BackColor = SystemColors.Control;
        logPacket.Location = new Point(1302, 9);
        logPacket.Name = "logPacket";
        logPacket.Size = new Size(61, 19);
        logPacket.TabIndex = 35;
        logPacket.Text = "packet";
        logPacket.UseVisualStyleBackColor = false;
        // 
        // logSystemPacket
        // 
        logSystemPacket.AutoSize = true;
        logSystemPacket.BackColor = SystemColors.Control;
        logSystemPacket.Location = new Point(1369, 9);
        logSystemPacket.Name = "logSystemPacket";
        logSystemPacket.Size = new Size(102, 19);
        logSystemPacket.TabIndex = 12;
        logSystemPacket.Text = "system packet";
        logSystemPacket.UseVisualStyleBackColor = false;
        // 
        // groupBox10
        // 
        groupBox10.BackColor = SystemColors.ScrollBar;
        groupBox10.Controls.Add(LoadRanking);
        groupBox10.Controls.Add(remainIAP);
        groupBox10.Controls.Add(MakeScenario);
        groupBox10.Controls.Add(scenario);
        groupBox10.Controls.Add(chatHistory);
        groupBox10.Controls.Add(SendNotice);
        groupBox10.Controls.Add(OpenServer);
        groupBox10.Controls.Add(CloseServer);
        groupBox10.Controls.Add(TruncateDatabase);
        groupBox10.Controls.Add(webLogin);
        groupBox10.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        groupBox10.Location = new Point(1029, 508);
        groupBox10.Name = "groupBox10";
        groupBox10.Size = new Size(204, 382);
        groupBox10.TabIndex = 35;
        groupBox10.TabStop = false;
        groupBox10.Text = "option";
        // 
        // LoadRanking
        // 
        LoadRanking.BackColor = Color.RosyBrown;
        LoadRanking.Font = new Font("맑은 고딕", 8F, FontStyle.Bold);
        LoadRanking.Location = new Point(6, 229);
        LoadRanking.Name = "LoadRanking";
        LoadRanking.Size = new Size(192, 34);
        LoadRanking.TabIndex = 39;
        LoadRanking.Text = "load ranking";
        LoadRanking.UseVisualStyleBackColor = false;
        LoadRanking.Click += LoadRanking_Click;
        // 
        // remainIAP
        // 
        remainIAP.AutoSize = true;
        remainIAP.Location = new Point(17, 103);
        remainIAP.Name = "remainIAP";
        remainIAP.Size = new Size(157, 19);
        remainIAP.TabIndex = 17;
        remainIAP.Text = "remain InAppPurchase";
        remainIAP.UseVisualStyleBackColor = true;
        // 
        // MakeScenario
        // 
        MakeScenario.BackColor = Color.LightCoral;
        MakeScenario.Font = new Font("맑은 고딕", 8F, FontStyle.Bold);
        MakeScenario.Location = new Point(17, 137);
        MakeScenario.Name = "MakeScenario";
        MakeScenario.Size = new Size(176, 48);
        MakeScenario.TabIndex = 18;
        MakeScenario.Text = "make scenario";
        MakeScenario.UseVisualStyleBackColor = false;
        MakeScenario.Click += MakeScenario_Click;
        // 
        // scenario
        // 
        scenario.AutoSize = true;
        scenario.Location = new Point(17, 78);
        scenario.Name = "scenario";
        scenario.Size = new Size(74, 19);
        scenario.TabIndex = 16;
        scenario.Text = "scenario";
        scenario.UseVisualStyleBackColor = true;
        // 
        // chatHistory
        // 
        chatHistory.AutoSize = true;
        chatHistory.Checked = true;
        chatHistory.CheckState = CheckState.Checked;
        chatHistory.Location = new Point(17, 55);
        chatHistory.Name = "chatHistory";
        chatHistory.Size = new Size(94, 19);
        chatHistory.TabIndex = 15;
        chatHistory.Text = "chat history";
        chatHistory.UseVisualStyleBackColor = true;
        // 
        // SendNotice
        // 
        SendNotice.BackColor = Color.Gray;
        SendNotice.Font = new Font("맑은 고딕", 8F, FontStyle.Bold);
        SendNotice.Location = new Point(6, 269);
        SendNotice.Name = "SendNotice";
        SendNotice.Size = new Size(192, 34);
        SendNotice.TabIndex = 14;
        SendNotice.Text = "send notice";
        SendNotice.UseVisualStyleBackColor = false;
        SendNotice.Click += SendNotice_Click;
        // 
        // OpenServer
        // 
        OpenServer.BackColor = Color.Gray;
        OpenServer.Font = new Font("맑은 고딕", 8F, FontStyle.Bold);
        OpenServer.Location = new Point(6, 305);
        OpenServer.Name = "OpenServer";
        OpenServer.Size = new Size(98, 34);
        OpenServer.TabIndex = 13;
        OpenServer.Text = "open server";
        OpenServer.UseVisualStyleBackColor = false;
        OpenServer.Click += OpenServer_Click;
        // 
        // CloseServer
        // 
        CloseServer.BackColor = Color.Gray;
        CloseServer.Font = new Font("맑은 고딕", 8F, FontStyle.Bold);
        CloseServer.Location = new Point(109, 305);
        CloseServer.Name = "CloseServer";
        CloseServer.Size = new Size(89, 34);
        CloseServer.TabIndex = 12;
        CloseServer.Text = "close server";
        CloseServer.UseVisualStyleBackColor = false;
        CloseServer.Click += CloseServer_Click;
        // 
        // TruncateDatabase
        // 
        TruncateDatabase.BackColor = Color.Gray;
        TruncateDatabase.Location = new Point(6, 342);
        TruncateDatabase.Name = "TruncateDatabase";
        TruncateDatabase.Size = new Size(192, 34);
        TruncateDatabase.TabIndex = 11;
        TruncateDatabase.Text = "truncate database";
        TruncateDatabase.UseVisualStyleBackColor = false;
        TruncateDatabase.Click += TruncateDatabase_Click;
        // 
        // webLogin
        // 
        webLogin.AutoSize = true;
        webLogin.Checked = true;
        webLogin.CheckState = CheckState.Checked;
        webLogin.Location = new Point(17, 33);
        webLogin.Name = "webLogin";
        webLogin.Size = new Size(83, 19);
        webLogin.TabIndex = 0;
        webLogin.Text = "web login";
        webLogin.UseVisualStyleBackColor = true;
        // 
        // LLMComboBox
        // 
        LLMComboBox.BackColor = SystemColors.ScrollBar;
        LLMComboBox.FormattingEnabled = true;
        LLMComboBox.Location = new Point(199, 25);
        LLMComboBox.Name = "LLMComboBox";
        LLMComboBox.Size = new Size(163, 23);
        LLMComboBox.TabIndex = 5;
        LLMComboBox.SelectedIndexChanged += LLMComboBox_SelectedIndexChanged;
        // 
        // LLMGroup
        // 
        LLMGroup.BackColor = Color.DarkSeaGreen;
        LLMGroup.Controls.Add(LLMTextBox);
        LLMGroup.Controls.Add(LLMComboBox);
        LLMGroup.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        LLMGroup.Location = new Point(1239, 597);
        LLMGroup.Name = "LLMGroup";
        LLMGroup.Size = new Size(377, 55);
        LLMGroup.TabIndex = 37;
        LLMGroup.TabStop = false;
        LLMGroup.Text = "LLM ( Large Language Model )";
        // 
        // LLMTextBox
        // 
        LLMTextBox.BackColor = Color.LightGreen;
        LLMTextBox.Location = new Point(16, 25);
        LLMTextBox.Name = "LLMTextBox";
        LLMTextBox.Size = new Size(163, 23);
        LLMTextBox.TabIndex = 5;
        LLMTextBox.TextAlign = HorizontalAlignment.Center;
        // 
        // TestButton
        // 
        TestButton.BackColor = Color.Gray;
        TestButton.Font = new Font("맑은 고딕", 8F, FontStyle.Bold);
        TestButton.Location = new Point(1329, 863);
        TestButton.Name = "TestButton";
        TestButton.Size = new Size(89, 34);
        TestButton.TabIndex = 38;
        TestButton.Text = "test button";
        TestButton.UseVisualStyleBackColor = false;
        TestButton.Click += button1_Click;
        // 
        // groupBox14
        // 
        groupBox14.BackColor = Color.BurlyWood;
        groupBox14.Controls.Add(DefaultLLMTextBox);
        groupBox14.Controls.Add(DefaultLLMComboBox);
        groupBox14.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        groupBox14.Location = new Point(1240, 667);
        groupBox14.Name = "groupBox14";
        groupBox14.Size = new Size(377, 56);
        groupBox14.TabIndex = 38;
        groupBox14.TabStop = false;
        groupBox14.Text = "Default LLM ( Large Language Model )";
        // 
        // DefaultLLMTextBox
        // 
        DefaultLLMTextBox.BackColor = Color.LightGreen;
        DefaultLLMTextBox.Location = new Point(16, 23);
        DefaultLLMTextBox.Name = "DefaultLLMTextBox";
        DefaultLLMTextBox.Size = new Size(163, 23);
        DefaultLLMTextBox.TabIndex = 5;
        DefaultLLMTextBox.TextAlign = HorizontalAlignment.Center;
        // 
        // DefaultLLMComboBox
        // 
        DefaultLLMComboBox.BackColor = SystemColors.ScrollBar;
        DefaultLLMComboBox.FormattingEnabled = true;
        DefaultLLMComboBox.Location = new Point(199, 23);
        DefaultLLMComboBox.Name = "DefaultLLMComboBox";
        DefaultLLMComboBox.Size = new Size(163, 23);
        DefaultLLMComboBox.TabIndex = 5;
        DefaultLLMComboBox.SelectedIndexChanged += DefaultLLMComboBox_SelectedIndexChanged;
        // 
        // GameForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = SystemColors.Control;
        ClientSize = new Size(1628, 904);
        Controls.Add(groupBox14);
        Controls.Add(TestButton);
        Controls.Add(LLMGroup);
        Controls.Add(groupBox12);
        Controls.Add(groupBox10);
        Controls.Add(groupBox8);
        Controls.Add(groupBox3);
        Controls.Add(groupBox2);
        Controls.Add(GameDataGroup);
        Controls.Add(groupBox1);
        Icon = (Icon)resources.GetObject("$this.Icon");
        Name = "GameForm";
        Text = "Game Server";
        groupBox1.ResumeLayout(false);
        groupBox1.PerformLayout();
        GameDataGroup.ResumeLayout(false);
        GameDataGroup.PerformLayout();
        groupBox2.ResumeLayout(false);
        groupBox2.PerformLayout();
        groupBox13.ResumeLayout(false);
        groupBox13.PerformLayout();
        groupBox11.ResumeLayout(false);
        groupBox11.PerformLayout();
        groupBox9.ResumeLayout(false);
        groupBox9.PerformLayout();
        groupBox7.ResumeLayout(false);
        groupBox7.PerformLayout();
        groupBox12.ResumeLayout(false);
        groupBox12.PerformLayout();
        groupBox3.ResumeLayout(false);
        groupBox6.ResumeLayout(false);
        groupBox6.PerformLayout();
        groupBox5.ResumeLayout(false);
        groupBox5.PerformLayout();
        groupBox4.ResumeLayout(false);
        groupBox4.PerformLayout();
        groupBox8.ResumeLayout(false);
        groupBox8.PerformLayout();
        groupBox10.ResumeLayout(false);
        groupBox10.PerformLayout();
        LLMGroup.ResumeLayout(false);
        LLMGroup.PerformLayout();
        groupBox14.ResumeLayout(false);
        groupBox14.PerformLayout();
        ResumeLayout(false);
    }

    #endregion
    private TextBox GameDataHash;
    private GroupBox groupBox1;
    private Label label4;
    private Label label3;
    private Label label2;
    private Label label1;
    private TextBox fail;
    private TextBox error;
    private TextBox critical;
    private TextBox exception;
    private Button ClearLog;
    private GroupBox GameDataGroup;
    private Button LoadGameData;
    private GroupBox groupBox2;
    public System.Windows.Forms.Timer timer100;
    public System.Windows.Forms.Timer timer1000;
    private System.Windows.Forms.Timer timer5000;
    private Label label12;
    private Label label13;
    private Label label14;
    private Label label15;
    private Label label11;
    private Label label10;
    private Label label9;
    private Label label5;
    private TextBox textBox8;
    private TextBox textBox9;
    private TextBox textBox10;
    private TextBox textBox11;
    private TextBox dailyRankingCount;
    private TextBox recvCount;
    private TextBox recvBytes;
    private TextBox sendCount;
    private TextBox sendBytes;
    private GroupBox groupBox3;
    private GroupBox groupBox4;
    private GroupBox groupBox5;
    private TextBox localId;
    private Label label8;
    private TextBox localVersion;
    private Label label7;
    private TextBox localMode;
    private Label label6;
    private TextBox jobSlowThreadCount;
    private Label label17;
    private TextBox jobThreadCount;
    private Label label16;
    private GroupBox groupBox6;
    private TextBox SessionCount;
    private GroupBox groupBox7;
    private TextBox userCount;
    public ListBox listBoxConsole;
    public CheckBox logPause;
    public CheckBox logWrite;
    private GroupBox groupBox8;
    private GroupBox groupBox9;
    private Label label18;
    private TextBox chatRoomCount;
    private GroupBox groupBox10;
    public CheckBox webLogin;
    private Button TruncateDatabase;
    public CheckBox logSystemPacket;
    private Label label19;
    private TextBox CumulativeLogin;
    private TextBox jobSingleThreadCount;
    private Label label20;
    private TextBox jobSingleCount;
    private TextBox jobSlowCount;
    private TextBox jobCount;
    private GroupBox groupBox11;
    private Label label21;
    private TextBox gameCount;
    private GroupBox groupBox12;
    private Label label22;
    private Label label23;
    private TextBox dailyRankingSeason;
    private TextBox pvpRankingSeason;
    private TextBox weekRankingSeason;
    private TextBox pvpRankingCount;
    private TextBox weekRankingCount;
    private TextBox pvpRankingExpireTime;
    private TextBox weekRankingExpireTime;
    private TextBox dailyRankingExpireTime;
    private Button CloseServer;
    private Button OpenServer;
    private Button SendNotice;
    public CheckBox logPacket;
    private GroupBox groupBox13;
    private Label label25;
    private TextBox SquareChatCount;
    private Label label24;
    private TextBox AIChatCount;
    private Label label26;
    private TextBox PushMsgCount;
    public CheckBox chatHistory;
    public CheckBox scenario;
    public CheckBox remainIAP;
    private Label label28;
    private TextBox tingRoomCount;
    private Label label27;
    private TextBox scenarioRoomCount;
    private GroupBox LLMGroup;
    public ComboBox LLMComboBox;
    public TextBox LLMTextBox;
    private Button TestButton;
    private GroupBox groupBox14;
    public TextBox DefaultLLMTextBox;
    public ComboBox DefaultLLMComboBox;
    private Button MakeScenario;
    private Button LoadRanking;
    private TextBox CumulativeGameCount;
}