

public partial class FrontForm : FormEx
{
    public FrontForm()
    {
        Logger.SetForm(this);

        InitializeComponent();

        InitializeComponentMapping();
    }

    public override void InitializeComponentMapping()
    {
        _serverListView         = this.listViewServer;
        _consoleListBox         = this.listBoxConsole;

        _logWrite               = this.logWrite;
        _logPause               = this.logPause;
        _logSystemPacket        = this.logSystemPacket;
        _logPacket              = this.logPacket;

        _exceptionTextBox       = this.exception;
        _criticalTextBox        = this.critical;
        _errorTextBox           = this.error;
        _failTextBox            = this.fail;

        _sendBytes              = this.sendBytes;
        _sendCount              = this.sendCount;
        _recvBytes              = this.recvBytes;
        _recvCount              = this.recvCount;
        _sessionCount           = this.SessionCount;

        _localMode              = this.localMode;
        _localVersion           = this.localVersion;
        _localId                = this.localId;

        _jobThreadCount         = this.jobThreadCount;
        _jobSlowThreadCount     = this.jobSlowThreadCount;
        _jobSingleThreadCount   = this.jobSingleThreadCount;

        _jobCount               = this.jobCount;
        _jobSlowCount           = this.jobSlowCount;
        _jobSingleCount         = this.jobSingleCount;
    }

    protected override void OnShown(EventArgs e)
    {
        this.Text += $" ( {ServerConfig.BuildDateTime()} )   ★  {ServerConfig.GetServiceMode()}  ★   {ServerConfig.GetGroup()} ";

        ServerConfig.Name = this.Text;

        Program.Initialize();

        // 타이머 시작.
        timer1000.Enabled = true;

        this.logSystemPacket.Checked    = false;
        this.logPacket.Checked          = ServerConfig.IsMode(MODE.DEBUG);
        this.logWrite.Checked           = ServerConfig.IsMode(MODE.DEBUG);
    }

    private void timer1000_Tick(object sender, EventArgs e)
    {
        // 로그 업데이트.
        UpdateLogComponent();

        // 잡 업데이트.
        UpdateJobComponent();

        // 네트워크 정보 업데이트.
        UpdateNetworkComponent(Network.GetServerConnectedCount());
    }

    private void ClearLog_Click(object sender, EventArgs e)
    {
        HNET.LOG.Reset();

        // 로그 업데이트.
        UpdateLogComponent();
    }
}