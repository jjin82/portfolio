

using System.Collections.Concurrent;

public partial class VersionHubForm : FormEx
{
    public VersionHubForm()
    {
        Logger.SetForm(this);

        InitializeComponent();

        InitializeComponentMapping();
    }

    public override void InitializeComponentMapping()
    {
        _versionListView = this.listViewServer;
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

        _localMode = this.localMode;
        _localVersion = this.localVersion;
        _localId = this.localId;

        _jobThreadCount = this.jobThreadCount;
        _jobSlowThreadCount = this.jobSlowThreadCount;
        _jobSingleThreadCount = this.jobSingleThreadCount;

        _jobCount = this.jobCount;
        _jobSlowCount = this.jobSlowCount;
        _jobSingleCount = this.jobSingleCount;
    }

    protected override void OnShown(EventArgs e)
    {
        this.Text += $" - ■ {ServerConfig.GetMode()} ■  ( {ServerConfig.BuildDateTime()} )";
        ServerConfig.Name = this.Text;

        Program.Initialize();

        // 타이머 시작.
        timer1000.Enabled = true;

        this.logSystemPacket.Checked = false;
        this.logPacket.Checked = ServerConfig.IsMode(MODE.DEBUG);
        this.logWrite.Checked = ServerConfig.IsMode(MODE.DEBUG);
    }

    private void timer1000_Tick(object sender, EventArgs e)
    {
        // 로그 업데이트.
        UpdateLogComponent();

        // 잡 업데이트.
        UpdateJobComponent();

        // 네트워크 정보 업데이트.
        UpdateNetworkComponent();
    }

    private void ClearLog_Click(object sender, EventArgs e)
    {
        HNET.LOG.Reset();

        // 로그 업데이트.
        UpdateLogComponent();
    }

    private void VersionHubForm_Load(object sender, EventArgs e)
    {

    }

    private void Reload_Click(object sender, EventArgs e)
    {
        DialogResult result1 = MessageBox.Show("[▶ ▶ ▶ RELOAD ◀ ◀ ◀]\n\n Are you sure?", "■   WARNING   ■", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        if (result1 == DialogResult.Cancel) return;

        // 버전 정보 로드.
        VersionManager.LoadExcel();
    }
}