partial class FrontForm
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
        ColumnHeader serverId;
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrontForm));
        listViewServer = new ListView();
        version = new ColumnHeader();
        host = new ColumnHeader();
        port = new ColumnHeader();
        hash = new ColumnHeader();
        userCount = new ColumnHeader();
        log = new ColumnHeader();
        resourceMonitor = new ColumnHeader();
        timer1000 = new System.Windows.Forms.Timer(components);
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
        localId = new TextBox();
        jobSlowCount = new TextBox();
        localVersion = new TextBox();
        jobCount = new TextBox();
        jobSingleThreadCount = new TextBox();
        localMode = new TextBox();
        label20 = new Label();
        label6 = new Label();
        jobSlowThreadCount = new TextBox();
        label7 = new Label();
        label17 = new Label();
        label8 = new Label();
        jobThreadCount = new TextBox();
        label16 = new Label();
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
        groupBox8 = new GroupBox();
        logSystemPacket = new CheckBox();
        logWrite = new CheckBox();
        logPause = new CheckBox();
        listBoxConsole = new ListBox();
        groupBox10 = new GroupBox();
        logPacket = new CheckBox();
        serverId = new ColumnHeader();
        groupBox3.SuspendLayout();
        groupBox6.SuspendLayout();
        groupBox5.SuspendLayout();
        groupBox4.SuspendLayout();
        groupBox2.SuspendLayout();
        groupBox1.SuspendLayout();
        groupBox8.SuspendLayout();
        SuspendLayout();
        // 
        // serverId
        // 
        serverId.Text = "serverId";
        // 
        // listViewServer
        // 
        listViewServer.BackColor = Color.DarkGray;
        listViewServer.Columns.AddRange(new ColumnHeader[] { serverId, version, host, port, hash, userCount, log, resourceMonitor });
        listViewServer.GridLines = true;
        listViewServer.Location = new Point(12, 371);
        listViewServer.Name = "listViewServer";
        listViewServer.Size = new Size(1384, 162);
        listViewServer.TabIndex = 0;
        listViewServer.UseCompatibleStateImageBehavior = false;
        listViewServer.View = View.Details;
        // 
        // version
        // 
        version.Text = "version";
        version.TextAlign = HorizontalAlignment.Center;
        // 
        // host
        // 
        host.Text = "host";
        host.TextAlign = HorizontalAlignment.Center;
        host.Width = 120;
        // 
        // port
        // 
        port.Text = "port";
        port.TextAlign = HorizontalAlignment.Center;
        // 
        // hash
        // 
        hash.Text = "hash";
        hash.TextAlign = HorizontalAlignment.Center;
        hash.Width = 260;
        // 
        // userCount
        // 
        userCount.Text = "user count";
        userCount.TextAlign = HorizontalAlignment.Center;
        userCount.Width = 80;
        // 
        // log
        // 
        log.Text = "log";
        log.Width = 200;
        // 
        // resourceMonitor
        // 
        resourceMonitor.Text = "resource monitor";
        resourceMonitor.Width = 300;
        // 
        // timer1000
        // 
        timer1000.Interval = 1000;
        timer1000.Tick += timer1000_Tick;
        // 
        // groupBox3
        // 
        groupBox3.BackColor = SystemColors.ScrollBar;
        groupBox3.Controls.Add(groupBox6);
        groupBox3.Controls.Add(groupBox5);
        groupBox3.Controls.Add(groupBox4);
        groupBox3.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        groupBox3.Location = new Point(567, 539);
        groupBox3.Name = "groupBox3";
        groupBox3.Size = new Size(214, 366);
        groupBox3.TabIndex = 24;
        groupBox3.TabStop = false;
        groupBox3.Text = "network  ( game acceptor )";
        // 
        // groupBox6
        // 
        groupBox6.Controls.Add(SessionCount);
        groupBox6.Location = new Point(17, 265);
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
        groupBox5.Location = new Point(17, 151);
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
        groupBox4.Location = new Point(17, 33);
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
        groupBox2.Controls.Add(localId);
        groupBox2.Controls.Add(jobSlowCount);
        groupBox2.Controls.Add(localVersion);
        groupBox2.Controls.Add(jobCount);
        groupBox2.Controls.Add(jobSingleThreadCount);
        groupBox2.Controls.Add(localMode);
        groupBox2.Controls.Add(label20);
        groupBox2.Controls.Add(label6);
        groupBox2.Controls.Add(jobSlowThreadCount);
        groupBox2.Controls.Add(label7);
        groupBox2.Controls.Add(label17);
        groupBox2.Controls.Add(label8);
        groupBox2.Controls.Add(jobThreadCount);
        groupBox2.Controls.Add(label16);
        groupBox2.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        groupBox2.Location = new Point(12, 539);
        groupBox2.Name = "groupBox2";
        groupBox2.Size = new Size(541, 366);
        groupBox2.TabIndex = 23;
        groupBox2.TabStop = false;
        groupBox2.Text = "info";
        // 
        // jobSingleCount
        // 
        jobSingleCount.BackColor = SystemColors.Control;
        jobSingleCount.Location = new Point(383, 81);
        jobSingleCount.Name = "jobSingleCount";
        jobSingleCount.ReadOnly = true;
        jobSingleCount.Size = new Size(99, 23);
        jobSingleCount.TabIndex = 48;
        jobSingleCount.TextAlign = HorizontalAlignment.Center;
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
        // jobSlowCount
        // 
        jobSlowCount.BackColor = SystemColors.Control;
        jobSlowCount.Location = new Point(383, 52);
        jobSlowCount.Name = "jobSlowCount";
        jobSlowCount.ReadOnly = true;
        jobSlowCount.Size = new Size(99, 23);
        jobSlowCount.TabIndex = 47;
        jobSlowCount.TextAlign = HorizontalAlignment.Center;
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
        // jobCount
        // 
        jobCount.BackColor = SystemColors.Control;
        jobCount.Location = new Point(383, 25);
        jobCount.Name = "jobCount";
        jobCount.ReadOnly = true;
        jobCount.Size = new Size(99, 23);
        jobCount.TabIndex = 46;
        jobCount.TextAlign = HorizontalAlignment.Center;
        // 
        // jobSingleThreadCount
        // 
        jobSingleThreadCount.BackColor = SystemColors.ControlDark;
        jobSingleThreadCount.Location = new Point(488, 81);
        jobSingleThreadCount.Name = "jobSingleThreadCount";
        jobSingleThreadCount.ReadOnly = true;
        jobSingleThreadCount.Size = new Size(36, 23);
        jobSingleThreadCount.TabIndex = 45;
        jobSingleThreadCount.TextAlign = HorizontalAlignment.Center;
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
        // label20
        // 
        label20.AutoSize = true;
        label20.Location = new Point(304, 85);
        label20.Name = "label20";
        label20.Size = new Size(63, 15);
        label20.TabIndex = 44;
        label20.Text = "job single";
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
        // jobSlowThreadCount
        // 
        jobSlowThreadCount.BackColor = SystemColors.ControlDark;
        jobSlowThreadCount.Location = new Point(488, 52);
        jobSlowThreadCount.Name = "jobSlowThreadCount";
        jobSlowThreadCount.ReadOnly = true;
        jobSlowThreadCount.Size = new Size(36, 23);
        jobSlowThreadCount.TabIndex = 43;
        jobSlowThreadCount.TextAlign = HorizontalAlignment.Center;
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
        // label17
        // 
        label17.AutoSize = true;
        label17.Location = new Point(304, 56);
        label17.Name = "label17";
        label17.Size = new Size(55, 15);
        label17.TabIndex = 42;
        label17.Text = "job slow";
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
        // jobThreadCount
        // 
        jobThreadCount.BackColor = SystemColors.ControlDark;
        jobThreadCount.Location = new Point(488, 25);
        jobThreadCount.Name = "jobThreadCount";
        jobThreadCount.ReadOnly = true;
        jobThreadCount.Size = new Size(36, 23);
        jobThreadCount.TabIndex = 41;
        jobThreadCount.TextAlign = HorizontalAlignment.Center;
        // 
        // label16
        // 
        label16.AutoSize = true;
        label16.Location = new Point(304, 29);
        label16.Name = "label16";
        label16.Size = new Size(25, 15);
        label16.TabIndex = 40;
        label16.Text = "job";
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
        groupBox1.Location = new Point(1201, 765);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new Size(195, 140);
        groupBox1.TabIndex = 22;
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
        // groupBox8
        // 
        groupBox8.BackColor = SystemColors.ControlDark;
        groupBox8.Controls.Add(logPacket);
        groupBox8.Controls.Add(logSystemPacket);
        groupBox8.Controls.Add(logWrite);
        groupBox8.Controls.Add(logPause);
        groupBox8.Controls.Add(listBoxConsole);
        groupBox8.Location = new Point(3, 5);
        groupBox8.Name = "groupBox8";
        groupBox8.Size = new Size(1401, 360);
        groupBox8.TabIndex = 35;
        groupBox8.TabStop = false;
        // 
        // logSystemPacket
        // 
        logSystemPacket.AutoSize = true;
        logSystemPacket.BackColor = SystemColors.Control;
        logSystemPacket.Location = new Point(1167, 9);
        logSystemPacket.Name = "logSystemPacket";
        logSystemPacket.Size = new Size(102, 19);
        logSystemPacket.TabIndex = 0;
        logSystemPacket.Text = "system packet";
        logSystemPacket.UseVisualStyleBackColor = false;
        // 
        // logWrite
        // 
        logWrite.AutoSize = true;
        logWrite.BackColor = SystemColors.Control;
        logWrite.Checked = true;
        logWrite.CheckState = CheckState.Checked;
        logWrite.Location = new Point(1338, 9);
        logWrite.Name = "logWrite";
        logWrite.Size = new Size(52, 19);
        logWrite.TabIndex = 34;
        logWrite.Text = "write";
        logWrite.UseVisualStyleBackColor = false;
        // 
        // logPause
        // 
        logPause.AutoSize = true;
        logPause.BackColor = SystemColors.Control;
        logPause.Location = new Point(1275, 9);
        logPause.Name = "logPause";
        logPause.Size = new Size(57, 19);
        logPause.TabIndex = 33;
        logPause.Text = "pause";
        logPause.UseVisualStyleBackColor = false;
        // 
        // listBoxConsole
        // 
        listBoxConsole.FormattingEnabled = true;
        listBoxConsole.HorizontalScrollbar = true;
        listBoxConsole.ItemHeight = 15;
        listBoxConsole.Location = new Point(7, 5);
        listBoxConsole.Name = "listBoxConsole";
        listBoxConsole.Size = new Size(1387, 349);
        listBoxConsole.TabIndex = 2;
        // 
        // groupBox10
        // 
        groupBox10.BackColor = SystemColors.ScrollBar;
        groupBox10.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        groupBox10.Location = new Point(793, 539);
        groupBox10.Name = "groupBox10";
        groupBox10.Size = new Size(186, 366);
        groupBox10.TabIndex = 36;
        groupBox10.TabStop = false;
        groupBox10.Text = "option";
        // 
        // logPacket
        // 
        logPacket.AutoSize = true;
        logPacket.BackColor = SystemColors.Control;
        logPacket.Location = new Point(1100, 9);
        logPacket.Name = "logPacket";
        logPacket.Size = new Size(61, 19);
        logPacket.TabIndex = 36;
        logPacket.Text = "packet";
        logPacket.UseVisualStyleBackColor = false;
        // 
        // FrontForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = SystemColors.Control;
        ClientSize = new Size(1408, 917);
        Controls.Add(groupBox10);
        Controls.Add(groupBox8);
        Controls.Add(groupBox3);
        Controls.Add(groupBox2);
        Controls.Add(groupBox1);
        Controls.Add(listViewServer);
        Icon = (Icon)resources.GetObject("$this.Icon");
        Name = "FrontForm";
        Text = "Front Server";
        groupBox3.ResumeLayout(false);
        groupBox6.ResumeLayout(false);
        groupBox6.PerformLayout();
        groupBox5.ResumeLayout(false);
        groupBox5.PerformLayout();
        groupBox4.ResumeLayout(false);
        groupBox4.PerformLayout();
        groupBox2.ResumeLayout(false);
        groupBox2.PerformLayout();
        groupBox1.ResumeLayout(false);
        groupBox1.PerformLayout();
        groupBox8.ResumeLayout(false);
        groupBox8.PerformLayout();
        ResumeLayout(false);
    }

    #endregion
    private ColumnHeader version;
    private ColumnHeader host;
    private System.Windows.Forms.Timer timer1000;
    public ListView listViewServer;
    private ColumnHeader port;
    private ColumnHeader hash;
    private ColumnHeader userCount;
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
    private GroupBox groupBox1;
    private TextBox fail;
    private TextBox error;
    private TextBox critical;
    private TextBox exception;
    private Label label4;
    private Label label3;
    private Label label2;
    private Label label1;
    private Button ClearLog;
    private ColumnHeader log;
    private ColumnHeader resourceMonitor;
    private GroupBox groupBox6;
    private TextBox SessionCount;
    private GroupBox groupBox8;
    public CheckBox logWrite;
    public CheckBox logPause;
    public ListBox listBoxConsole;
    private GroupBox groupBox10;
    public CheckBox logSystemPacket;
    private TextBox jobSingleCount;
    private TextBox jobSlowCount;
    private TextBox jobCount;
    private TextBox jobSingleThreadCount;
    private Label label20;
    private TextBox jobSlowThreadCount;
    private Label label17;
    private TextBox jobThreadCount;
    private Label label16;
    public CheckBox logPacket;
}