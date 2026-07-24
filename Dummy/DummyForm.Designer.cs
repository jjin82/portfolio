partial class DummyForm
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
        groupBox1 = new GroupBox();
        ClearLog = new Button();
        fail = new TextBox();
        error = new TextBox();
        critical = new TextBox();
        exception = new TextBox();
        label4 = new Label();
        label3 = new Label();
        label2 = new Label();
        label1 = new Label();
        groupBox3 = new GroupBox();
        groupBox6 = new GroupBox();
        SessionCount = new TextBox();
        groupBox5 = new GroupBox();
        recvCount = new TextBox();
        recvBytes = new TextBox();
        groupBox4 = new GroupBox();
        sendCount = new TextBox();
        sendBytes = new TextBox();
        groupBox2 = new GroupBox();
        jobSingleCount = new TextBox();
        groupBox12 = new GroupBox();
        label18 = new Label();
        roomCount = new TextBox();
        jobSlowCount = new TextBox();
        groupBox7 = new GroupBox();
        label5 = new Label();
        userCount = new TextBox();
        jobCount = new TextBox();
        jobSingleThreadCount = new TextBox();
        localId = new TextBox();
        label20 = new Label();
        localVersion = new TextBox();
        jobSlowThreadCount = new TextBox();
        localMode = new TextBox();
        label17 = new Label();
        label6 = new Label();
        jobThreadCount = new TextBox();
        label7 = new Label();
        label16 = new Label();
        label8 = new Label();
        listBoxConsole = new ListBox();
        timer100 = new System.Windows.Forms.Timer(components);
        timer1000 = new System.Windows.Forms.Timer(components);
        groupBox8 = new GroupBox();
        groupBox10 = new GroupBox();
        tokenSeq = new NumericUpDown();
        resetToken = new Button();
        token = new TextBox();
        groupBox9 = new GroupBox();
        connect_1000 = new Button();
        connect_100 = new Button();
        connect_10 = new Button();
        connect_1 = new Button();
        disconnect = new Button();
        tryConnect = new NumericUpDown();
        connect = new Button();
        numericUpDown1 = new NumericUpDown();
        groupBox11 = new GroupBox();
        port = new NumericUpDown();
        label9 = new Label();
        logWrite = new CheckBox();
        logPause = new CheckBox();
        groupBox13 = new GroupBox();
        groupBox18 = new GroupBox();
        PlayerChatMsg = new TextBox();
        groupBox16 = new GroupBox();
        SquareChatMsg = new TextBox();
        groupBox17 = new GroupBox();
        label14 = new Label();
        label13 = new Label();
        label12 = new Label();
        label11 = new Label();
        label10 = new Label();
        CmdParam5 = new TextBox();
        CmdParam4 = new TextBox();
        SendCheat = new Button();
        CheatComboBox = new ComboBox();
        InviteRespond = new Button();
        GiftMobileData = new Button();
        Withdraw = new Button();
        CmdParam3 = new TextBox();
        CmdParam2 = new TextBox();
        CmdParam1 = new TextBox();
        groupBox15 = new GroupBox();
        AIChatMsg = new TextBox();
        groupBox14 = new GroupBox();
        TestIap = new Button();
        historyEnglish = new CheckBox();
        PushMessage = new Button();
        TestChat = new Button();
        logSystemPacket = new CheckBox();
        logPacket = new CheckBox();
        timer10000 = new System.Windows.Forms.Timer(components);
        groupBox1.SuspendLayout();
        groupBox3.SuspendLayout();
        groupBox6.SuspendLayout();
        groupBox5.SuspendLayout();
        groupBox4.SuspendLayout();
        groupBox2.SuspendLayout();
        groupBox12.SuspendLayout();
        groupBox7.SuspendLayout();
        groupBox8.SuspendLayout();
        groupBox10.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)tokenSeq).BeginInit();
        groupBox9.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)tryConnect).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
        groupBox11.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)port).BeginInit();
        groupBox13.SuspendLayout();
        groupBox18.SuspendLayout();
        groupBox16.SuspendLayout();
        groupBox17.SuspendLayout();
        groupBox15.SuspendLayout();
        groupBox14.SuspendLayout();
        SuspendLayout();
        // 
        // groupBox1
        // 
        groupBox1.BackColor = SystemColors.ScrollBar;
        groupBox1.Controls.Add(ClearLog);
        groupBox1.Controls.Add(fail);
        groupBox1.Controls.Add(error);
        groupBox1.Controls.Add(critical);
        groupBox1.Controls.Add(exception);
        groupBox1.Controls.Add(label4);
        groupBox1.Controls.Add(label3);
        groupBox1.Controls.Add(label2);
        groupBox1.Controls.Add(label1);
        groupBox1.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        groupBox1.Location = new Point(1434, 708);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new Size(195, 140);
        groupBox1.TabIndex = 23;
        groupBox1.TabStop = false;
        groupBox1.Text = "log";
        // 
        // ClearLog
        // 
        ClearLog.BackColor = Color.RosyBrown;
        ClearLog.Location = new Point(171, 0);
        ClearLog.Name = "ClearLog";
        ClearLog.Size = new Size(24, 24);
        ClearLog.TabIndex = 11;
        ClearLog.Text = "*";
        ClearLog.UseVisualStyleBackColor = false;
        ClearLog.Click += ClearLog_Click;
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
        // groupBox3
        // 
        groupBox3.BackColor = SystemColors.ScrollBar;
        groupBox3.Controls.Add(groupBox6);
        groupBox3.Controls.Add(groupBox5);
        groupBox3.Controls.Add(groupBox4);
        groupBox3.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        groupBox3.Location = new Point(553, 475);
        groupBox3.Name = "groupBox3";
        groupBox3.Size = new Size(214, 373);
        groupBox3.TabIndex = 26;
        groupBox3.TabStop = false;
        groupBox3.Text = "network  ( client connector )";
        // 
        // groupBox6
        // 
        groupBox6.Controls.Add(SessionCount);
        groupBox6.Location = new Point(17, 261);
        groupBox6.Name = "groupBox6";
        groupBox6.Size = new Size(182, 95);
        groupBox6.TabIndex = 24;
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
        groupBox5.Location = new Point(17, 143);
        groupBox5.Name = "groupBox5";
        groupBox5.Size = new Size(182, 95);
        groupBox5.TabIndex = 22;
        groupBox5.TabStop = false;
        groupBox5.Text = "recv  ( bytes )";
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
        // groupBox4
        // 
        groupBox4.Controls.Add(sendCount);
        groupBox4.Controls.Add(sendBytes);
        groupBox4.Location = new Point(17, 25);
        groupBox4.Name = "groupBox4";
        groupBox4.Size = new Size(182, 95);
        groupBox4.TabIndex = 21;
        groupBox4.TabStop = false;
        groupBox4.Text = "send  ( bytes )";
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
        // groupBox2
        // 
        groupBox2.BackColor = SystemColors.ScrollBar;
        groupBox2.Controls.Add(jobSingleCount);
        groupBox2.Controls.Add(groupBox12);
        groupBox2.Controls.Add(jobSlowCount);
        groupBox2.Controls.Add(groupBox7);
        groupBox2.Controls.Add(jobCount);
        groupBox2.Controls.Add(jobSingleThreadCount);
        groupBox2.Controls.Add(localId);
        groupBox2.Controls.Add(label20);
        groupBox2.Controls.Add(localVersion);
        groupBox2.Controls.Add(jobSlowThreadCount);
        groupBox2.Controls.Add(localMode);
        groupBox2.Controls.Add(label17);
        groupBox2.Controls.Add(label6);
        groupBox2.Controls.Add(jobThreadCount);
        groupBox2.Controls.Add(label7);
        groupBox2.Controls.Add(label16);
        groupBox2.Controls.Add(label8);
        groupBox2.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        groupBox2.Location = new Point(6, 475);
        groupBox2.Name = "groupBox2";
        groupBox2.Size = new Size(541, 373);
        groupBox2.TabIndex = 25;
        groupBox2.TabStop = false;
        groupBox2.Text = "info";
        // 
        // jobSingleCount
        // 
        jobSingleCount.BackColor = SystemColors.Control;
        jobSingleCount.Location = new Point(379, 81);
        jobSingleCount.Name = "jobSingleCount";
        jobSingleCount.ReadOnly = true;
        jobSingleCount.Size = new Size(99, 23);
        jobSingleCount.TabIndex = 48;
        jobSingleCount.TextAlign = HorizontalAlignment.Center;
        // 
        // groupBox12
        // 
        groupBox12.Controls.Add(label18);
        groupBox12.Controls.Add(roomCount);
        groupBox12.Location = new Point(300, 162);
        groupBox12.Name = "groupBox12";
        groupBox12.Size = new Size(220, 199);
        groupBox12.TabIndex = 35;
        groupBox12.TabStop = false;
        groupBox12.Text = "room";
        // 
        // label18
        // 
        label18.AutoSize = true;
        label18.Location = new Point(20, 45);
        label18.Name = "label18";
        label18.Size = new Size(39, 15);
        label18.TabIndex = 33;
        label18.Text = "count";
        // 
        // roomCount
        // 
        roomCount.Location = new Point(93, 42);
        roomCount.Name = "roomCount";
        roomCount.ReadOnly = true;
        roomCount.Size = new Size(116, 23);
        roomCount.TabIndex = 19;
        roomCount.TextAlign = HorizontalAlignment.Center;
        // 
        // jobSlowCount
        // 
        jobSlowCount.BackColor = SystemColors.Control;
        jobSlowCount.Location = new Point(379, 52);
        jobSlowCount.Name = "jobSlowCount";
        jobSlowCount.ReadOnly = true;
        jobSlowCount.Size = new Size(99, 23);
        jobSlowCount.TabIndex = 47;
        jobSlowCount.TextAlign = HorizontalAlignment.Center;
        // 
        // groupBox7
        // 
        groupBox7.Controls.Add(label5);
        groupBox7.Controls.Add(userCount);
        groupBox7.Location = new Point(17, 162);
        groupBox7.Name = "groupBox7";
        groupBox7.Size = new Size(220, 199);
        groupBox7.TabIndex = 36;
        groupBox7.TabStop = false;
        groupBox7.Text = "user";
        // 
        // label5
        // 
        label5.AutoSize = true;
        label5.Location = new Point(20, 45);
        label5.Name = "label5";
        label5.Size = new Size(39, 15);
        label5.TabIndex = 33;
        label5.Text = "count";
        // 
        // userCount
        // 
        userCount.Location = new Point(87, 42);
        userCount.Name = "userCount";
        userCount.ReadOnly = true;
        userCount.Size = new Size(116, 23);
        userCount.TabIndex = 19;
        userCount.Text = "0";
        userCount.TextAlign = HorizontalAlignment.Center;
        // 
        // jobCount
        // 
        jobCount.BackColor = SystemColors.Control;
        jobCount.Location = new Point(379, 25);
        jobCount.Name = "jobCount";
        jobCount.ReadOnly = true;
        jobCount.Size = new Size(99, 23);
        jobCount.TabIndex = 46;
        jobCount.TextAlign = HorizontalAlignment.Center;
        // 
        // jobSingleThreadCount
        // 
        jobSingleThreadCount.BackColor = SystemColors.ControlDark;
        jobSingleThreadCount.Location = new Point(484, 81);
        jobSingleThreadCount.Name = "jobSingleThreadCount";
        jobSingleThreadCount.ReadOnly = true;
        jobSingleThreadCount.Size = new Size(36, 23);
        jobSingleThreadCount.TabIndex = 45;
        jobSingleThreadCount.TextAlign = HorizontalAlignment.Center;
        // 
        // localId
        // 
        localId.Location = new Point(82, 78);
        localId.Name = "localId";
        localId.ReadOnly = true;
        localId.Size = new Size(93, 23);
        localId.TabIndex = 8;
        localId.TextAlign = HorizontalAlignment.Center;
        // 
        // label20
        // 
        label20.AutoSize = true;
        label20.Location = new Point(300, 85);
        label20.Name = "label20";
        label20.Size = new Size(63, 15);
        label20.TabIndex = 44;
        label20.Text = "job single";
        // 
        // localVersion
        // 
        localVersion.Location = new Point(82, 51);
        localVersion.Name = "localVersion";
        localVersion.ReadOnly = true;
        localVersion.Size = new Size(93, 23);
        localVersion.TabIndex = 7;
        localVersion.TextAlign = HorizontalAlignment.Center;
        // 
        // jobSlowThreadCount
        // 
        jobSlowThreadCount.BackColor = SystemColors.ControlDark;
        jobSlowThreadCount.Location = new Point(484, 52);
        jobSlowThreadCount.Name = "jobSlowThreadCount";
        jobSlowThreadCount.ReadOnly = true;
        jobSlowThreadCount.Size = new Size(36, 23);
        jobSlowThreadCount.TabIndex = 43;
        jobSlowThreadCount.TextAlign = HorizontalAlignment.Center;
        // 
        // localMode
        // 
        localMode.Location = new Point(82, 24);
        localMode.Name = "localMode";
        localMode.ReadOnly = true;
        localMode.Size = new Size(93, 23);
        localMode.TabIndex = 6;
        localMode.TextAlign = HorizontalAlignment.Center;
        // 
        // label17
        // 
        label17.AutoSize = true;
        label17.Location = new Point(300, 56);
        label17.Name = "label17";
        label17.Size = new Size(55, 15);
        label17.TabIndex = 42;
        label17.Text = "job slow";
        // 
        // label6
        // 
        label6.AutoSize = true;
        label6.Location = new Point(17, 82);
        label6.Name = "label6";
        label6.Size = new Size(18, 15);
        label6.TabIndex = 2;
        label6.Text = "id";
        // 
        // jobThreadCount
        // 
        jobThreadCount.BackColor = SystemColors.ControlDark;
        jobThreadCount.Location = new Point(484, 25);
        jobThreadCount.Name = "jobThreadCount";
        jobThreadCount.ReadOnly = true;
        jobThreadCount.Size = new Size(36, 23);
        jobThreadCount.TabIndex = 41;
        jobThreadCount.TextAlign = HorizontalAlignment.Center;
        // 
        // label7
        // 
        label7.AutoSize = true;
        label7.Location = new Point(17, 55);
        label7.Name = "label7";
        label7.Size = new Size(48, 15);
        label7.TabIndex = 1;
        label7.Text = "version";
        // 
        // label16
        // 
        label16.AutoSize = true;
        label16.Location = new Point(300, 29);
        label16.Name = "label16";
        label16.Size = new Size(25, 15);
        label16.TabIndex = 40;
        label16.Text = "job";
        // 
        // label8
        // 
        label8.AutoSize = true;
        label8.Location = new Point(17, 28);
        label8.Name = "label8";
        label8.Size = new Size(40, 15);
        label8.TabIndex = 0;
        label8.Text = "mode";
        // 
        // listBoxConsole
        // 
        listBoxConsole.FormattingEnabled = true;
        listBoxConsole.HorizontalScrollbar = true;
        listBoxConsole.ItemHeight = 15;
        listBoxConsole.Location = new Point(8, 12);
        listBoxConsole.Name = "listBoxConsole";
        listBoxConsole.Size = new Size(1042, 454);
        listBoxConsole.TabIndex = 27;
        // 
        // timer100
        // 
        timer100.Tick += timer100_Tick;
        // 
        // timer1000
        // 
        timer1000.Interval = 1000;
        timer1000.Tick += timer1000_Tick;
        // 
        // groupBox8
        // 
        groupBox8.BackColor = SystemColors.AppWorkspace;
        groupBox8.Controls.Add(groupBox10);
        groupBox8.Controls.Add(groupBox9);
        groupBox8.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        groupBox8.Location = new Point(1056, 87);
        groupBox8.Name = "groupBox8";
        groupBox8.Size = new Size(573, 379);
        groupBox8.TabIndex = 28;
        groupBox8.TabStop = false;
        groupBox8.Text = "connection";
        // 
        // groupBox10
        // 
        groupBox10.Controls.Add(tokenSeq);
        groupBox10.Controls.Add(resetToken);
        groupBox10.Controls.Add(token);
        groupBox10.Location = new Point(23, 22);
        groupBox10.Name = "groupBox10";
        groupBox10.Size = new Size(174, 118);
        groupBox10.TabIndex = 1;
        groupBox10.TabStop = false;
        groupBox10.Text = "Token  ( pid )";
        // 
        // tokenSeq
        // 
        tokenSeq.Location = new Point(15, 74);
        tokenSeq.Maximum = new decimal(new int[] { 50000, 0, 0, 0 });
        tokenSeq.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        tokenSeq.Name = "tokenSeq";
        tokenSeq.Size = new Size(71, 23);
        tokenSeq.TabIndex = 7;
        tokenSeq.TextAlign = HorizontalAlignment.Center;
        tokenSeq.Value = new decimal(new int[] { 1, 0, 0, 0 });
        tokenSeq.ValueChanged += tokenSeq_ValueChanged;
        // 
        // resetToken
        // 
        resetToken.Location = new Point(90, 71);
        resetToken.Name = "resetToken";
        resetToken.Size = new Size(71, 29);
        resetToken.TabIndex = 7;
        resetToken.Text = "reset";
        resetToken.UseVisualStyleBackColor = true;
        resetToken.Click += resetAccount_Click;
        // 
        // token
        // 
        token.Font = new Font("맑은 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
        token.Location = new Point(15, 29);
        token.Name = "token";
        token.ReadOnly = true;
        token.Size = new Size(146, 29);
        token.TabIndex = 0;
        token.TextAlign = HorizontalAlignment.Center;
        // 
        // groupBox9
        // 
        groupBox9.Controls.Add(connect_1000);
        groupBox9.Controls.Add(connect_100);
        groupBox9.Controls.Add(connect_10);
        groupBox9.Controls.Add(connect_1);
        groupBox9.Controls.Add(disconnect);
        groupBox9.Controls.Add(tryConnect);
        groupBox9.Controls.Add(connect);
        groupBox9.Controls.Add(numericUpDown1);
        groupBox9.Location = new Point(215, 22);
        groupBox9.Name = "groupBox9";
        groupBox9.Size = new Size(338, 118);
        groupBox9.TabIndex = 0;
        groupBox9.TabStop = false;
        groupBox9.Text = "client";
        // 
        // connect_1000
        // 
        connect_1000.Location = new Point(232, 71);
        connect_1000.Name = "connect_1000";
        connect_1000.Size = new Size(88, 29);
        connect_1000.TabIndex = 6;
        connect_1000.Text = "1000";
        connect_1000.UseVisualStyleBackColor = true;
        connect_1000.Click += connect_1000_Click;
        // 
        // connect_100
        // 
        connect_100.Location = new Point(149, 71);
        connect_100.Name = "connect_100";
        connect_100.Size = new Size(76, 29);
        connect_100.TabIndex = 5;
        connect_100.Text = "100";
        connect_100.UseVisualStyleBackColor = true;
        connect_100.Click += connect_100_Click;
        // 
        // connect_10
        // 
        connect_10.Location = new Point(82, 71);
        connect_10.Name = "connect_10";
        connect_10.Size = new Size(60, 29);
        connect_10.TabIndex = 4;
        connect_10.Text = "10";
        connect_10.UseVisualStyleBackColor = true;
        connect_10.Click += connect_10_Click;
        // 
        // connect_1
        // 
        connect_1.Location = new Point(25, 71);
        connect_1.Name = "connect_1";
        connect_1.Size = new Size(50, 29);
        connect_1.TabIndex = 3;
        connect_1.Text = "1";
        connect_1.UseVisualStyleBackColor = true;
        connect_1.Click += connect_1_Click;
        // 
        // disconnect
        // 
        disconnect.Location = new Point(239, 29);
        disconnect.Name = "disconnect";
        disconnect.Size = new Size(81, 29);
        disconnect.TabIndex = 2;
        disconnect.Text = "disconnect";
        disconnect.UseVisualStyleBackColor = true;
        disconnect.Click += disconnect_Click;
        // 
        // tryConnect
        // 
        tryConnect.Location = new Point(25, 32);
        tryConnect.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        tryConnect.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        tryConnect.Name = "tryConnect";
        tryConnect.Size = new Size(120, 23);
        tryConnect.TabIndex = 0;
        tryConnect.TextAlign = HorizontalAlignment.Center;
        tryConnect.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // connect
        // 
        connect.Location = new Point(150, 29);
        connect.Name = "connect";
        connect.Size = new Size(81, 29);
        connect.TabIndex = 1;
        connect.Text = "connect";
        connect.UseVisualStyleBackColor = true;
        connect.Click += connect_Click;
        // 
        // numericUpDown1
        // 
        numericUpDown1.Location = new Point(25, 32);
        numericUpDown1.Name = "numericUpDown1";
        numericUpDown1.Size = new Size(120, 23);
        numericUpDown1.TabIndex = 0;
        // 
        // groupBox11
        // 
        groupBox11.BackColor = SystemColors.ControlDark;
        groupBox11.Controls.Add(port);
        groupBox11.Controls.Add(label9);
        groupBox11.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        groupBox11.Location = new Point(1056, 12);
        groupBox11.Name = "groupBox11";
        groupBox11.Size = new Size(573, 58);
        groupBox11.TabIndex = 8;
        groupBox11.TabStop = false;
        groupBox11.Text = "server info";
        // 
        // port
        // 
        port.Location = new Point(77, 21);
        port.Maximum = new decimal(new int[] { 65000, 0, 0, 0 });
        port.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        port.Name = "port";
        port.Size = new Size(120, 23);
        port.TabIndex = 7;
        port.TextAlign = HorizontalAlignment.Center;
        port.Value = new decimal(new int[] { 30002, 0, 0, 0 });
        // 
        // label9
        // 
        label9.AutoSize = true;
        label9.Location = new Point(23, 25);
        label9.Name = "label9";
        label9.Size = new Size(32, 15);
        label9.TabIndex = 37;
        label9.Text = "port";
        // 
        // logWrite
        // 
        logWrite.AutoSize = true;
        logWrite.Checked = true;
        logWrite.CheckState = CheckState.Checked;
        logWrite.Location = new Point(993, 16);
        logWrite.Name = "logWrite";
        logWrite.Size = new Size(52, 19);
        logWrite.TabIndex = 36;
        logWrite.Text = "write";
        logWrite.UseVisualStyleBackColor = true;
        // 
        // logPause
        // 
        logPause.AutoSize = true;
        logPause.Location = new Point(930, 16);
        logPause.Name = "logPause";
        logPause.Size = new Size(57, 19);
        logPause.TabIndex = 35;
        logPause.Text = "pause";
        logPause.UseVisualStyleBackColor = true;
        // 
        // groupBox13
        // 
        groupBox13.BackColor = SystemColors.WindowFrame;
        groupBox13.Controls.Add(groupBox18);
        groupBox13.Controls.Add(groupBox16);
        groupBox13.Controls.Add(groupBox17);
        groupBox13.Controls.Add(groupBox15);
        groupBox13.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        groupBox13.Location = new Point(773, 471);
        groupBox13.Name = "groupBox13";
        groupBox13.Size = new Size(655, 377);
        groupBox13.TabIndex = 37;
        groupBox13.TabStop = false;
        groupBox13.Text = "command";
        // 
        // groupBox18
        // 
        groupBox18.Controls.Add(PlayerChatMsg);
        groupBox18.ForeColor = Color.NavajoWhite;
        groupBox18.Location = new Point(243, 31);
        groupBox18.Name = "groupBox18";
        groupBox18.Size = new Size(189, 73);
        groupBox18.TabIndex = 15;
        groupBox18.TabStop = false;
        groupBox18.Text = "player chat";
        // 
        // PlayerChatMsg
        // 
        PlayerChatMsg.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 129);
        PlayerChatMsg.Location = new Point(9, 30);
        PlayerChatMsg.Name = "PlayerChatMsg";
        PlayerChatMsg.Size = new Size(172, 25);
        PlayerChatMsg.TabIndex = 8;
        PlayerChatMsg.KeyDown += PlayerChatMsg_KeyDown;
        // 
        // groupBox16
        // 
        groupBox16.Controls.Add(SquareChatMsg);
        groupBox16.ForeColor = Color.NavajoWhite;
        groupBox16.Location = new Point(461, 31);
        groupBox16.Name = "groupBox16";
        groupBox16.Size = new Size(189, 73);
        groupBox16.TabIndex = 15;
        groupBox16.TabStop = false;
        groupBox16.Text = "square chat";
        // 
        // SquareChatMsg
        // 
        SquareChatMsg.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 129);
        SquareChatMsg.Location = new Point(11, 30);
        SquareChatMsg.Name = "SquareChatMsg";
        SquareChatMsg.Size = new Size(172, 25);
        SquareChatMsg.TabIndex = 8;
        SquareChatMsg.KeyDown += SquareChatMsg_KeyDown;
        // 
        // groupBox17
        // 
        groupBox17.Controls.Add(label14);
        groupBox17.Controls.Add(label13);
        groupBox17.Controls.Add(label12);
        groupBox17.Controls.Add(label11);
        groupBox17.Controls.Add(label10);
        groupBox17.Controls.Add(CmdParam5);
        groupBox17.Controls.Add(CmdParam4);
        groupBox17.Controls.Add(SendCheat);
        groupBox17.Controls.Add(CheatComboBox);
        groupBox17.Controls.Add(InviteRespond);
        groupBox17.Controls.Add(GiftMobileData);
        groupBox17.Controls.Add(Withdraw);
        groupBox17.Controls.Add(CmdParam3);
        groupBox17.Controls.Add(CmdParam2);
        groupBox17.Controls.Add(CmdParam1);
        groupBox17.Location = new Point(25, 119);
        groupBox17.Name = "groupBox17";
        groupBox17.Size = new Size(613, 246);
        groupBox17.TabIndex = 16;
        groupBox17.TabStop = false;
        // 
        // label14
        // 
        label14.AutoSize = true;
        label14.ForeColor = SystemColors.Control;
        label14.Location = new Point(9, 160);
        label14.Name = "label14";
        label14.Size = new Size(52, 15);
        label14.TabIndex = 53;
        label14.Text = "param5";
        // 
        // label13
        // 
        label13.AutoSize = true;
        label13.ForeColor = SystemColors.Control;
        label13.Location = new Point(9, 133);
        label13.Name = "label13";
        label13.Size = new Size(52, 15);
        label13.TabIndex = 52;
        label13.Text = "param4";
        // 
        // label12
        // 
        label12.AutoSize = true;
        label12.ForeColor = SystemColors.Control;
        label12.Location = new Point(9, 106);
        label12.Name = "label12";
        label12.Size = new Size(52, 15);
        label12.TabIndex = 51;
        label12.Text = "param3";
        // 
        // label11
        // 
        label11.AutoSize = true;
        label11.ForeColor = SystemColors.Control;
        label11.Location = new Point(9, 79);
        label11.Name = "label11";
        label11.Size = new Size(52, 15);
        label11.TabIndex = 50;
        label11.Text = "param2";
        // 
        // label10
        // 
        label10.AutoSize = true;
        label10.ForeColor = SystemColors.Control;
        label10.Location = new Point(9, 52);
        label10.Name = "label10";
        label10.Size = new Size(52, 15);
        label10.TabIndex = 49;
        label10.Text = "param1";
        // 
        // CmdParam5
        // 
        CmdParam5.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 129);
        CmdParam5.Location = new Point(89, 155);
        CmdParam5.Name = "CmdParam5";
        CmdParam5.Size = new Size(182, 25);
        CmdParam5.TabIndex = 32;
        // 
        // CmdParam4
        // 
        CmdParam4.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 129);
        CmdParam4.Location = new Point(89, 128);
        CmdParam4.Name = "CmdParam4";
        CmdParam4.Size = new Size(182, 25);
        CmdParam4.TabIndex = 31;
        // 
        // SendCheat
        // 
        SendCheat.BackColor = Color.Peru;
        SendCheat.Location = new Point(9, 188);
        SendCheat.Name = "SendCheat";
        SendCheat.Size = new Size(264, 46);
        SendCheat.TabIndex = 30;
        SendCheat.Text = "send";
        SendCheat.UseVisualStyleBackColor = false;
        SendCheat.Click += SendCheat_Click;
        // 
        // CheatComboBox
        // 
        CheatComboBox.FormattingEnabled = true;
        CheatComboBox.Location = new Point(9, 18);
        CheatComboBox.Name = "CheatComboBox";
        CheatComboBox.Size = new Size(264, 23);
        CheatComboBox.TabIndex = 29;
        // 
        // InviteRespond
        // 
        InviteRespond.Location = new Point(436, 123);
        InviteRespond.Name = "InviteRespond";
        InviteRespond.Size = new Size(145, 29);
        InviteRespond.TabIndex = 28;
        InviteRespond.Text = "invite respond";
        InviteRespond.UseVisualStyleBackColor = true;
        InviteRespond.Click += InviteRespond_Click;
        // 
        // GiftMobileData
        // 
        GiftMobileData.Location = new Point(436, 53);
        GiftMobileData.Name = "GiftMobileData";
        GiftMobileData.Size = new Size(145, 29);
        GiftMobileData.TabIndex = 27;
        GiftMobileData.Text = "gift mobile data";
        GiftMobileData.UseVisualStyleBackColor = true;
        GiftMobileData.Click += GiftMobileData_Click;
        // 
        // Withdraw
        // 
        Withdraw.Location = new Point(447, 163);
        Withdraw.Name = "Withdraw";
        Withdraw.Size = new Size(145, 29);
        Withdraw.TabIndex = 26;
        Withdraw.Text = "withdraw";
        Withdraw.UseVisualStyleBackColor = true;
        Withdraw.Click += Withdraw_Click;
        // 
        // CmdParam3
        // 
        CmdParam3.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 129);
        CmdParam3.Location = new Point(89, 101);
        CmdParam3.Name = "CmdParam3";
        CmdParam3.Size = new Size(182, 25);
        CmdParam3.TabIndex = 22;
        // 
        // CmdParam2
        // 
        CmdParam2.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 129);
        CmdParam2.Location = new Point(89, 74);
        CmdParam2.Name = "CmdParam2";
        CmdParam2.Size = new Size(182, 25);
        CmdParam2.TabIndex = 18;
        // 
        // CmdParam1
        // 
        CmdParam1.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 129);
        CmdParam1.Location = new Point(89, 47);
        CmdParam1.Name = "CmdParam1";
        CmdParam1.Size = new Size(182, 25);
        CmdParam1.TabIndex = 9;
        // 
        // groupBox15
        // 
        groupBox15.Controls.Add(AIChatMsg);
        groupBox15.ForeColor = Color.NavajoWhite;
        groupBox15.Location = new Point(25, 31);
        groupBox15.Name = "groupBox15";
        groupBox15.Size = new Size(189, 73);
        groupBox15.TabIndex = 14;
        groupBox15.TabStop = false;
        groupBox15.Text = "ai chat";
        // 
        // AIChatMsg
        // 
        AIChatMsg.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 129);
        AIChatMsg.Location = new Point(9, 30);
        AIChatMsg.Name = "AIChatMsg";
        AIChatMsg.Size = new Size(172, 25);
        AIChatMsg.TabIndex = 8;
        AIChatMsg.KeyDown += ChatMsg_KeyDown;
        // 
        // groupBox14
        // 
        groupBox14.BackColor = SystemColors.ScrollBar;
        groupBox14.Controls.Add(TestIap);
        groupBox14.Controls.Add(historyEnglish);
        groupBox14.Controls.Add(PushMessage);
        groupBox14.Controls.Add(TestChat);
        groupBox14.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        groupBox14.Location = new Point(1434, 467);
        groupBox14.Name = "groupBox14";
        groupBox14.Size = new Size(195, 230);
        groupBox14.TabIndex = 38;
        groupBox14.TabStop = false;
        groupBox14.Text = "option";
        // 
        // TestIap
        // 
        TestIap.Location = new Point(17, 187);
        TestIap.Name = "TestIap";
        TestIap.Size = new Size(158, 29);
        TestIap.TabIndex = 29;
        TestIap.Text = "test iap";
        TestIap.UseVisualStyleBackColor = true;
        TestIap.Click += TestIap_Click;
        // 
        // historyEnglish
        // 
        historyEnglish.AutoSize = true;
        historyEnglish.Checked = true;
        historyEnglish.CheckState = CheckState.Checked;
        historyEnglish.Location = new Point(17, 33);
        historyEnglish.Name = "historyEnglish";
        historyEnglish.Size = new Size(110, 19);
        historyEnglish.TabIndex = 28;
        historyEnglish.Text = "history english";
        historyEnglish.UseVisualStyleBackColor = true;
        // 
        // PushMessage
        // 
        PushMessage.Location = new Point(17, 117);
        PushMessage.Name = "PushMessage";
        PushMessage.Size = new Size(158, 29);
        PushMessage.TabIndex = 27;
        PushMessage.Text = "push message";
        PushMessage.UseVisualStyleBackColor = true;
        PushMessage.Click += PushMessage_Click;
        // 
        // TestChat
        // 
        TestChat.Location = new Point(17, 152);
        TestChat.Name = "TestChat";
        TestChat.Size = new Size(158, 29);
        TestChat.TabIndex = 25;
        TestChat.Text = "test chat";
        TestChat.UseVisualStyleBackColor = true;
        TestChat.Click += TestChat_Click;
        // 
        // logSystemPacket
        // 
        logSystemPacket.AutoSize = true;
        logSystemPacket.Checked = true;
        logSystemPacket.CheckState = CheckState.Checked;
        logSystemPacket.Location = new Point(822, 16);
        logSystemPacket.Name = "logSystemPacket";
        logSystemPacket.Size = new Size(102, 19);
        logSystemPacket.TabIndex = 0;
        logSystemPacket.Text = "system packet";
        logSystemPacket.UseVisualStyleBackColor = true;
        // 
        // logPacket
        // 
        logPacket.AutoSize = true;
        logPacket.BackColor = SystemColors.Control;
        logPacket.Location = new Point(755, 16);
        logPacket.Name = "logPacket";
        logPacket.Size = new Size(61, 19);
        logPacket.TabIndex = 39;
        logPacket.Text = "packet";
        logPacket.UseVisualStyleBackColor = false;
        // 
        // timer10000
        // 
        timer10000.Interval = 10000;
        timer10000.Tick += timer10000_Tick;
        // 
        // DummyForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1641, 853);
        Controls.Add(logPacket);
        Controls.Add(logSystemPacket);
        Controls.Add(groupBox14);
        Controls.Add(groupBox13);
        Controls.Add(logWrite);
        Controls.Add(logPause);
        Controls.Add(groupBox11);
        Controls.Add(groupBox8);
        Controls.Add(listBoxConsole);
        Controls.Add(groupBox3);
        Controls.Add(groupBox2);
        Controls.Add(groupBox1);
        Name = "DummyForm";
        Text = "Dummy";
        groupBox1.ResumeLayout(false);
        groupBox1.PerformLayout();
        groupBox3.ResumeLayout(false);
        groupBox6.ResumeLayout(false);
        groupBox6.PerformLayout();
        groupBox5.ResumeLayout(false);
        groupBox5.PerformLayout();
        groupBox4.ResumeLayout(false);
        groupBox4.PerformLayout();
        groupBox2.ResumeLayout(false);
        groupBox2.PerformLayout();
        groupBox12.ResumeLayout(false);
        groupBox12.PerformLayout();
        groupBox7.ResumeLayout(false);
        groupBox7.PerformLayout();
        groupBox8.ResumeLayout(false);
        groupBox10.ResumeLayout(false);
        groupBox10.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)tokenSeq).EndInit();
        groupBox9.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)tryConnect).EndInit();
        ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
        groupBox11.ResumeLayout(false);
        groupBox11.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)port).EndInit();
        groupBox13.ResumeLayout(false);
        groupBox18.ResumeLayout(false);
        groupBox18.PerformLayout();
        groupBox16.ResumeLayout(false);
        groupBox16.PerformLayout();
        groupBox17.ResumeLayout(false);
        groupBox17.PerformLayout();
        groupBox15.ResumeLayout(false);
        groupBox15.PerformLayout();
        groupBox14.ResumeLayout(false);
        groupBox14.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private GroupBox groupBox1;
    private Button ClearLog;
    private TextBox fail;
    private TextBox error;
    private TextBox critical;
    private TextBox exception;
    private Label label4;
    private Label label3;
    private Label label2;
    private Label label1;
    private GroupBox groupBox3;
    private GroupBox groupBox5;
    private TextBox recvCount;
    private TextBox recvBytes;
    private GroupBox groupBox4;
    private TextBox sendCount;
    private TextBox sendBytes;
    private GroupBox groupBox2;
    private TextBox localId;
    private TextBox localVersion;
    private TextBox localMode;
    private Label label6;
    private Label label7;
    private Label label8;
    public ListBox listBoxConsole;
    public System.Windows.Forms.Timer timer100;
    public System.Windows.Forms.Timer timer1000;
    private System.Windows.Forms.Timer timer1;
    private GroupBox groupBox6;
    private TextBox SessionCount;
    private GroupBox groupBox7;
    private Label label5;
    private TextBox userCount;
    private GroupBox groupBox8;
    private GroupBox groupBox9;
    private Button connect_1000;
    private Button connect_100;
    private Button connect_10;
    private Button disconnect;
    private Button connect;
    private NumericUpDown numericUpDown1;
    private Button connect_1;
    private GroupBox groupBox10;
    private Button resetToken;
    private GroupBox groupBox11;
    private Label label9;
    private NumericUpDown tryConnect;
    public NumericUpDown port;
    public CheckBox logWrite;
    public CheckBox logPause;
    private GroupBox groupBox12;
    private Label label18;
    private TextBox roomCount;
    private GroupBox groupBox13;
    public TextBox token;
    private GroupBox groupBox14;
    public CheckBox logSystemPacket;
    public NumericUpDown tokenSeq;
    private TextBox jobSingleCount;
    private TextBox jobSlowCount;
    private TextBox jobCount;
    private TextBox jobSingleThreadCount;
    private Label label20;
    private TextBox jobSlowThreadCount;
    private Label label17;
    private TextBox jobThreadCount;
    private Label label16;
    public TextBox AIChatMsg;
    private GroupBox groupBox15;
    private GroupBox groupBox17;
    public TextBox CmdParam1;
    public TextBox CmdParam2;
    private GroupBox groupBox16;
    public TextBox SquareChatMsg;
    private Button BuyItem;
    public TextBox CmdParam3;
    private Button TestChat;
    private Button PushMessage;
    public CheckBox logPacket;
    private Button Withdraw;
    public CheckBox historyEnglish;
    private GroupBox groupBox18;
    public TextBox PlayerChatMsg;
    private Button GiftMobileData;
    private Button InviteRespond;
    private Button TestIap;
    private ComboBox CheatComboBox;
    private Button SendCheat;
    public TextBox CmdParam5;
    public TextBox CmdParam4;
    private Label label14;
    private Label label13;
    private Label label12;
    private Label label11;
    private Label label10;
    public System.Windows.Forms.Timer timer10000;
}
