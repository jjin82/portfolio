using System.Diagnostics;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;

public static class Config
{
    public static string ToString(string fileName, string section, string key)
    {
        IniFile iniFile = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName));
        if (null == iniFile) return "";

        return iniFile.ReadIniFile(section, key);
    }

    public static int ToInt(string fileName, string section, string key, bool isSataic = false)
    {
        try
        {
            return Convert.ToInt32(ToString(fileName, section, key));
        }
        catch (Exception e)
        {
            Logger.WARNING($"Section ( {section} ), Key ( {key} ), message ( {e.Message} )");
        }

        return 0;
    }

    public static bool ToBool(string fileName, string section, string key, bool isSataic = false)
    {
        try
        {
            return Convert.ToBoolean(ToString(fileName, section, key));
        }
        catch (Exception e)
        {
            Logger.WARNING($"Section ( {section} ), Key ( {key} ), message ( {e.Message} )");
        }

        return false;
    }

    public static string[] GetSectionNames(string path)
    {
        IniFile iniFile = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path));
        if (null == iniFile) return new string[0];

        return iniFile.GetSectionNames();
    }

    public static bool GetSection(string fileName, string section, string key)
    {
        IniFile iniFile = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName));
        if (null == iniFile) return false;

        return iniFile.GetSection(section, key);
    }
}
 
public partial class UTIL
{ 
    public class WEB
    {
        public static string AuthURL()
        {
            return "https://gamepot.apigw.ntruss.com/gpapps/v1/loginauth";
        }

        public static async Task<T> Post<T>(object o, T result)
        {
            var url = AuthURL();

            try
            {
                var httpClient = new HttpClient();

                var content = JsonConvert.SerializeObject(o);
                var request = new StringContent(content);
                request.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                var response = await httpClient.PostAsync(url, request);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Logger.INFO($"Rest api( {url} ), request( {content} ), response( {responseContent} )");

                    return JsonConvert.DeserializeObject<T>(responseContent);
                }
                else
                {
                    Logger.CRITICAL($"Request failed with status code {response.StatusCode}");
                }
            }
            catch (Exception e)
            {
                Logger.CRITICAL($"Rest api( {url} ), exception( {e} )");
            }

            return default;
        }

        public static T Post<T>(string url)
        {
            // 예시>
            //T_ORDER.ResultModelRoomInfo r = UTIL.WebPost<T_ORDER.ResultModelRoomInfo>("https://dev-apigw.torder.co.kr/table-game/app/partner-games/rooms/2857/players/4648/test?isUpdate=false");

            try
            {
                Logger.INFO($"BEGIN Rest api( {url} )");

                //var httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
                //httpWebRequest.Headers.Add("X-AUTH-TOKEN", "8da8599980764201ac68d8129be4d656");
                //httpWebRequest.ContentType = "application/json";
                //httpWebRequest.Method = "PUT";

                //using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                //{
                //    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                //    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                //    {
                //        var result = streamReader.ReadToEnd();

                //        Logger.INFO($"END Rest api( {url} ), result( {result} )");

                //        return JsonConvert.DeserializeObject<T>(result);
                //    }
                //}
            }
            catch (Exception e)
            {
                Logger.CRITICAL($"Rest api( {url} ), exception( {e} )");
            }

            return default(T);
        }
    }
}

public partial class UTIL
{
    public static bool IsInHourRange(int[] range)
    {
        if (2 != range.Length)
            return false;

        int hour = DateTime.Now.Hour;

        // 시간 범위 시작과 끝
        int start   = range[0];
        int end     = range[1];

        if (start <= end)
        {
            // 같은 날 범위 (예: 1시 ~ 2시)
            return hour >= start && hour <= end;
        }
        else
        {
            // 다음 날로 넘어가는 범위 (예: 23시 ~ 1시)
            return hour >= start || hour <= end;
        }
    }

    public static DateTime NextMonthMidnight()
    {
        DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0, DateTimeKind.Local);   // 오늘 시작 시간.

        // 다음 달 1일.
        return today.AddMonths(1);
    }

    public static DateTime NextMonthMidnightUtc()
    {
        DateTime today = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc);   // 오늘 시작 시간.

        // 다음 달 1일.
        return today.AddMonths(1);
    }

    public static DateTime NextWeekMidnight()
    {
        DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, DateTimeKind.Local);   // 오늘 시작 시간.

        // 현재 요일부터 다음 주 월요일까지의 일수 계산
        int daysUntilMonday = ((7 - (int)today.DayOfWeek) % 7) + 1;

        // 다음 주 월요일 시간.
        return today.AddDays(daysUntilMonday);
    }

    public static DateTime NextWeekMidnightUtc()
    {
        DateTime today = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 0, 0, 0, DateTimeKind.Utc);   // 오늘 시작 시간.

        // 현재 요일부터 다음 주 월요일까지의 일수 계산
        int daysUntilMonday = ((7 - (int)today.DayOfWeek) % 7) + 1;

        // 다음 주 월요일 시간.
        return today.AddDays(daysUntilMonday);
    }

    public static int NextWeekMidnightUtcRemainSeconds()
    {
        return (int)(DateTime.FromFileTimeUtc(NextWeekMidnightUtc().ToFileTime()) - DateTime.UtcNow).TotalSeconds;
    }

    public static DateTime NextDayMidnight()
    {
        return NextDayMidnight(1);
    }

    public static DateTime NextDayMidnight(DateTime time)
    {
        return NextDayMidnight(1, time);
    }

    public static DateTime NextDayMidnight(int nextDay, DateTime time = default)
    {
        if (time.Equals(default))
        {
            time = DateTime.Now;
        }

        return new DateTime(time.Year, time.Month, time.Day, 0, 0, 0, DateTimeKind.Local).AddDays(nextDay);
    }

    public static DateTime NextDayMidnightUtc()
    {
        return NextMidnightUtc(1);
    }

    public static int NextDayMidnightUtcRemainSeconds()
    {
        return (int)(DateTime.FromFileTimeUtc(NextDayMidnightUtc().ToFileTime()) - DateTime.UtcNow).TotalSeconds;
    }

    public static DateTime NextMidnightUtc(DateTime time)
    {
        return NextMidnightUtc(1, time);
    }

    public static DateTime NextMidnightUtc(int nextDay, DateTime time = default)
    {
        if (time.Equals(default))
        {
            time = DateTime.UtcNow;
        }

        return new DateTime(time.Year, time.Month, time.Day, 0, 0, 0, DateTimeKind.Utc).AddDays(nextDay);
    }

    public static DateTime NextFullHourUtc()
    {
        return new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, DateTime.UtcNow.Hour, 0, 0, DateTimeKind.Utc).AddHours(1);
    }

    public static ulong MakeUniqueKey(int serverId, uint key)
    {
        return ((ulong)(serverId & 0xFFFFFFFF) << 32) | (key & 0xFFFFFFFF);
    }

    private static ThreadLocal<Random> tls_random = new ThreadLocal<Random>(() =>
    {
        return new Random(Guid.NewGuid().GetHashCode());
    });

    private static ThreadLocal<StringBuilder> tls_StringBuilder = new ThreadLocal<StringBuilder>(() =>
    {
        return new StringBuilder();
    });

    public static StringBuilder? GetStringBuilder(string v = "")
    {
        tls_StringBuilder?.Value?.Clear();
        tls_StringBuilder?.Value?.Append(v);
        return tls_StringBuilder?.Value;
    }

    public static int GetRandom(int max)
    {
        return tls_random.Value.Next(max);
    }

    public static int GetRandomRange(int min, int max)
    {
        return GetRandom(min, (max + 1));
    }

    public static int GetRandom(int min, int max)
    {
        if (min == max)
            return max;

        return tls_random.Value.Next(min, Math.Max(min, max));
    }

    public static int GetRandom()
    {
        return tls_random.Value.Next();
    }

    public static int GetRateCount(List<int> minValue, List<int> maxValue, List<int> rate)
    {
        if (minValue.Count != maxValue.Count)
            return 0;

        if (minValue.Count != rate.Count)
            return 0;
        
        var totalRate = rate.Sum();

        int randRate = GetRandom(totalRate);
        int sumRate = 0;

        for (int i = 0; i < rate.Count; ++i)
        {
            sumRate += rate[i];

            if (randRate < sumRate)
            {
                return UTIL.GetRandom(minValue[i], maxValue[i]);
            }
        }

        return 0;
    }

    public static bool GetRateValue<T>(List<T> value, List<int> rate, ref T t)
    {
        int totalRate = 0;

        if (value.Count == rate.Count)
        {
            totalRate = rate.Sum();
        }
        else
        {
            for (int i = 0; i < value.Count; ++i)
            {
                totalRate += rate[i];
            }
        }

        int randRate = GetRandom(totalRate);
        int sumRate = 0;

        for (int i = 0; i < value.Count; ++i)
        {
            sumRate += rate[i];

            if (randRate < sumRate)
            {
                t = value[i];
                return true;
            }
        }

        return false;
    }

    public static T GetRateValue<T>(List<T> value, List<int> rate)
    {
        int totalRate = 0;

        if (value.Count == rate.Count)
        {
            totalRate = rate.Sum();
        }
        else
        {
            for (int i = 0; i < value.Count; ++i)
            {
                totalRate += rate[i];
            }
        }

        int randRate = GetRandom(totalRate);
        int sumRate = 0;

        for (int i = 0; i < value.Count; ++i)
        {
            sumRate += rate[i];

            if (randRate < sumRate)
            {
                return value[i];
            }
        }

        return default(T);
    }

    public static int GetRateIndex(List<int> rate)
    {
        List<int> value = new List<int>();
        for (int i = 0; i < rate.Count; ++i)
        {
            value.Add(i);
        }

        return GetRateValue(value, rate);
    }

    public static decimal TruncateDecimal(decimal value, int precision)
    {
        decimal step = (decimal)Math.Pow(10, precision);
        decimal tmp = Math.Truncate(step * value);
        return tmp / step;
    }

    public static string Func
    {
        get
        {
            StackTrace stackTrace = new StackTrace();
            return stackTrace.GetFrame(1).GetMethod().Name;
        }
    }

    public static string BatchFile(string fileName)
    {
        try
        {
            // Redirect the output stream of the child process.
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = fileName;

            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardOutput = true;

            bool result = p.Start();
            p.WaitForExit();

            string output = p.StandardOutput.ReadToEnd();
            string error = p.StandardError.ReadToEnd();
            // Read the output stream first and then wait.
            return output;
        }
        catch (Exception e)
        {
            Logger.EXCEPTION(e);
            return "";
        }
    }
}

public class Performance
{
    public static float Cpu()
    {
        try { return s_cpu.NextValue(); } // 사용율.
        catch { return 0.0f; }
    }

    public static float Memory()
    {
        try { return s_memory.NextValue(); } // 사용 가능 메모리
        catch { return 0.0f; }
    }

    public static float Disk()
    {
        if (null == s_driveInfo)
        {
            string[] split = Directory.GetCurrentDirectory().Split(new char[] { '/', '\\' });
            s_driveInfo = new DriveInfo(split[0]);
        }

        try { return (s_driveInfo.AvailableFreeSpace / (float)s_driveInfo.TotalSize) * 100; } // 남은량
        catch { return 0.0f; }
    }

    public static long DBLatencyWarningCount    { get; set; }
    public static long MaxDBLatencyWarningCount { get; set; }
    public static long MaxDBLatency             { get; set; }

    private static PerformanceCounter   s_cpu           = new PerformanceCounter("Process", "% Processor Time", Process.GetCurrentProcess().ProcessName);
    private static PerformanceCounter   s_memory        = new PerformanceCounter("Memory", "Available MBytes");
    private static PerformanceCounter   s_totalMemory   = new PerformanceCounter("Memory", "Available MBytes", "_Total");
    private static DriveInfo            s_driveInfo     = null;
}

public partial class Logger
{
    static FormEx? _form = null;

    public static void SetForm(FormEx? form)
    {
        _form = form;
    }

    public static void DEBUG(string s)
    {
        _form?.ConsolePrintComponent($"【 DEBUG 】 {s}");

        HNET.LOG.DEBUG(s);
    }

    public static void INFO(string s)
    {
        _form?.ConsolePrintComponent($"【 INFO 】 {s}");
        
        HNET.LOG.INFO(s);
    }

    public static void INFO_PRINT(string s)
    {
        _form?.ConsolePrintComponent($"【 INFO 】 {s}", true);
        
        HNET.LOG.INFO(s);
    }

    public static void ERROR(string s)
    {
        _form?.ConsolePrintComponent($"【 ERROR 】 {s}");
        
        HNET.LOG.ERROR(s);
    }

    public static void FAIL(string s)
    {
        _form?.ConsolePrintComponent($"【 FAIL 】 {s}");

        HNET.LOG.FAIL(s);
    }

    public static void WARNING(string s)
    {
        HNET.LOG.WARNING(s);
    }

    public static void WARNING_PRINT(string s)
    {
        _form?.ConsolePrintComponent($"【 WARNING 】 {s}");

        HNET.LOG.WARNING(s);
    }

    public static void CRITICAL(string s, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
    {
        var r = $"{s}, ** File( {Path.GetFileName(filePath)} ), ** Method( {memberName} ), ** Line( {lineNumber} ) **";

        _form?.ConsolePrintComponent($"【 CRITICAL 】 {r}");
        
        HNET.LOG.CRITICAL(r);
    }

    public static void CRITICAL_PRINT(string s, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
    {
        var r = $"{s}, ** File( {Path.GetFileName(filePath)} ), ** Method( {memberName} ), ** Line( {lineNumber} ) **";

        _form?.ConsolePrintComponent($"【 CRITICAL 】 {r}", true);

        HNET.LOG.CRITICAL(r);
    }

    public static void EXCEPTION(Exception ex)
    {
        _form?.ConsolePrintComponent($"【 EXCEPTION 】 {ex}");

        HNET.LOG.EXCEPTION(ex);
    }

    public static void EXCEPTION(Exception ex, string s)
    {
        _form?.ConsolePrintComponent($"【 EXCEPTION 】 {ex}");

        HNET.LOG.EXCEPTION(ex, s);
    }

    public static void PacketLog(string dir, HNET.Packet packet, int netId = 0)
    {
        if (null == packet)
            return;

        if (false == (_form?.IsLogPacket(packet.Type()) ?? true))
            return;

        var text = UTIL.GetStringBuilder($"{dir}: {packet.ToString().Replace('_', ' ')}");
        if (null == text) return;

        if (false == ServerConfig.IsLive())
        {
            text.Append($", size( {packet.Size()} ), body( {JsonConvert.SerializeObject(packet)} )");
        }

        if (0 < netId)
        {
            text.Append($", netId( {netId} )");
        }

        Logger.INFO_PRINT(text.ToString());
    }
}

#if USER_LOG
public partial class Logger
{
    public static void DEBUG(User? user, string s)
    {
        DEBUG($"{s}{user?.LogDefault ?? ""}");
    }

    public static void INFO(User? user, string s)
    {
        INFO($"{s}{user?.LogDefault ?? ""}");
    }

    public static void INFO_PRINT(User? user, string s)
    {
        INFO_PRINT($"{s}{user?.LogDefault ?? ""}");
    }

    public static void ERROR(User? user, string s)
    {
        ERROR($"{s}{user?.LogDefault ?? ""}");
    }

    public static void FAIL(User? user, string s)
    {
        FAIL($"{s}{user?.LogDefault ?? ""}");
    }

    public static void WARNING(User? user, string s)
    {
        WARNING($"{s}{user?.LogDefault ?? ""}");
    }

    public static void CRITICAL(User? user, string s)
    {
        CRITICAL($"{s}{user?.LogDefault ?? ""}");
    }

    public static void EXCEPTION(User? user, Exception e, string s)
    {
        EXCEPTION(e, $"{s}{user?.LogDefault ?? ""}");
    }

    public static void EXCEPTION(User? user, Exception e)
    {
        EXCEPTION(e, $"{user?.LogDefault ?? ""}");
    }
}
#endif

public class FormEx : Form
{
    protected ListBox?  _consoleListBox             = null;
    protected ListView? _serverListView             = null;
    protected ListView? _versionListView            = null;

    protected CheckBox? _logWrite                   = null;
    protected CheckBox? _logPause                   = null;
    protected CheckBox? _logSystemPacket            = null;
    protected CheckBox? _logPacket                  = null;

    protected TextBox?  _exceptionTextBox           = null;
    protected TextBox?  _criticalTextBox            = null;
    protected TextBox?  _errorTextBox               = null;
    protected TextBox?  _failTextBox                = null;
    
    protected TextBox?  _recvBytes                  = null;
    protected TextBox?  _recvMaxBytes               = null;
    protected TextBox?  _recvCount                  = null;
    protected TextBox?  _recvMaxCount               = null;
    protected TextBox?  _sendBytes                  = null;
    protected TextBox?  _sendCount                  = null;
    protected TextBox?  _sessionCount               = null;

    protected TextBox?  _localMode                  = null;
    protected TextBox?  _localVersion               = null;
    protected TextBox?  _localId                    = null;

    protected TextBox?  _jobThreadCount             = null;
    protected TextBox?  _jobSlowThreadCount         = null;
    protected TextBox?  _jobSingleThreadCount       = null; 

    protected TextBox?  _jobCount                   = null;
    protected TextBox?  _jobSlowCount               = null;
    protected TextBox?  _jobSingleCount             = null; 

    protected TextBox?  _userCount                  = null;
    protected TextBox?  _roomCount                  = null;

    private bool _logAlert = false;

    public virtual void InitializeComponentMapping()
    {

    }

    public bool IsConsolePrint()
    {
        return _logWrite?.Checked ?? true;
    }

    public bool IsConsolePause()
    {
        return _logPause?.Checked ?? true;
    }

    public bool IsLogSystemPacket()
    {
        return _logSystemPacket?.Checked ?? true;
    }

    public bool IsLogPacket(ushort type)
    {
        // 시스템 패킷 비노출 상태면 타입 확인.
        if (false == IsLogSystemPacket() && (100 > type))
        {
            return false;
        }

        return _logPacket?.Checked ?? true;
    }

    public void ConsolePrintComponent(string text, bool force = false)
    {
        if (false == force)
        {
            if (false == IsConsolePrint())
                return;
        }

        if (null == _consoleListBox)
            return;

        string log = $"[{DateTime.Now}] <{Thread.CurrentThread.ManagedThreadId.ToString().PadLeft(2)}> {text}";

        BeginInvoke(() =>
        {
            _consoleListBox.Items.Add(log);

            // 포커스 이동 여부.
            if (false == IsConsolePause())
            {
                _consoleListBox.TopIndex = (_consoleListBox.Items.Count - 1);
            }
            
        });
    }

    public void UpdateUserComponent(int ccu, int mcu)
    {
        BeginInvoke(() =>
        {
            _userCount.Text = $"{ccu} / {mcu}";
        });
    }

    public void UpdateRoomComponent(int ccu, int mcu)
    {
        BeginInvoke(() =>
        {
            _roomCount.Text = $"{ccu} / {mcu}";
        });
    }

    public void UpdateLocalInfoComponent(int userCount = 0)
    {
        BeginInvoke(() =>
        {
            _localMode.Text             = ServerConfig.GetMode().ToString(); _localMode.BackColor = ServerConfig.IsLive() ? Color.LawnGreen : Color.Orange;
            _localVersion.Text          = ServerConfig.Version();
            _localId.Text               = ServerConfig.ServerId().ToString();
        });
    }

    public void UpdateJobComponent()
    {
        BeginInvoke(() =>
        {
            _jobThreadCount.Text        = HNET.JOB.GetThreadCount().ToString();
            _jobSlowThreadCount.Text    = HNET.JOB_SLOW.GetThreadCount().ToString();
            _jobSingleThreadCount.Text  = HNET.JOB_SINGLE.GetThreadCount().ToString();

            _jobCount.Text              = HNET.JOB.Count().ToString();
            _jobSlowCount.Text          = HNET.JOB_SLOW.Count().ToString();
            _jobSingleCount.Text        = HNET.JOB_SINGLE.Count().ToString();
        });
    }

    public void UpdateLogComponent()
    {
        BeginInvoke(() =>
        {
            _exceptionTextBox.Text  = HNET.LOG.ExceptionCount.ToString("#,##0");
            _criticalTextBox.Text   = HNET.LOG.CriticalCount.ToString("#,##0");
            _errorTextBox.Text      = HNET.LOG.ErrorCount.ToString("#,##0");
            _failTextBox.Text       = HNET.LOG.FailCount.ToString("#,##0");

            _exceptionTextBox.BackColor = (0 == HNET.LOG.ExceptionCount) ? Color.White : (_logAlert ? Color.Red : Color.White);
            _criticalTextBox.BackColor  = (0 == HNET.LOG.CriticalCount)  ? Color.White : (_logAlert ? Color.Red : Color.White);

            _logAlert = !_logAlert;
        });
    }

    public void UpdateNetworkComponent()
    {
        BeginInvoke(() =>
        {
            HNET.Performance.NextRecvInfo(out var recvIOCount, out var recvCount, out var recvAvgCount, out var recvMaxCount, out var recvBytes, out var recvAvgBytes, out var recvMaxBytes);
            _recvBytes.Text     = $"{recvBytes} / {recvMaxBytes}";
            _recvCount.Text     = $"{recvCount} / {recvMaxCount}"; 

            HNET.Performance.NextSendInfo(out var sendIOCount, out var sendCount, out var sendAvgCount, out var sendMaxCount, out var sendBytes, out var sendAvgBytes, out var sendMaxBytes);
            _sendBytes.Text     = $"{sendBytes} / {sendMaxBytes}";
            _sendCount.Text     = $"{sendCount} / {sendMaxCount}";
        });
    }

    public void UpdateNetworkComponent(int sessionCount)
    {
        BeginInvoke(() =>
        {
            HNET.Performance.NextRecvInfo(out var recvIOCount, out var recvCount, out var recvAvgCount, out var recvMaxCount, out var recvBytes, out var recvAvgBytes, out var recvMaxBytes);
            _recvBytes.Text     = $"{recvBytes} / {recvMaxBytes}";
            _recvCount.Text     = $"{recvCount} / {recvMaxCount}"; 

            HNET.Performance.NextSendInfo(out var sendIOCount, out var sendCount, out var sendAvgCount, out var sendMaxCount, out var sendBytes, out var sendAvgBytes, out var sendMaxBytes);
            _sendBytes.Text     = $"{sendBytes} / {sendMaxBytes}";
            _sendCount.Text     = $"{sendCount} / {sendMaxCount}";

            _sessionCount.Text  = sessionCount.ToString();
        });
    }

    public void UpdateServerInfoComponent(ServerInfo serverInfo)
    {
        BeginInvoke(() =>
        {
            var item = new ListViewItem();
            item.BackColor = (serverInfo._connected ? Color.LightGreen : Color.Red);
            item.Text = (0 == serverInfo._Id ? "..." : serverInfo._Id.ToString());
            item.SubItems.Add(serverInfo._version);
            item.SubItems.Add(serverInfo._host);
            item.SubItems.Add(serverInfo._port.ToString());
            item.SubItems.Add(serverInfo._hash);
            item.SubItems.Add(serverInfo._userCount.ToString());

            int serverIndex = -1;
            foreach (ColumnHeader e in _serverListView.Columns)
            {
                ++serverIndex;

                if (e.Text.Equals("serverId")) break;
            }

            bool isUpdate = false;
            for (int i = 0; i < _serverListView.Items.Count; i++)
            {
                if (Convert.ToUInt32(_serverListView.Items[i].SubItems[serverIndex].Text) == serverInfo._Id)
                {
                    //정보 갱신
                    _serverListView.Items[i] = item;
                    isUpdate = true;
                    break;
                }
            }

            // 갱신된 정보가 없으면 추가.
            if (!isUpdate)
            {
                _serverListView.Items.Add(item);
            }
        });
    }

    public void UpdateServerUserCountComponent(int serverId, int userCount)
    {
        BeginInvoke(() =>
        {
            for (int i = 0; i < _serverListView?.Items.Count; i++)
            {
                if (_serverListView.Items[i].Text.Equals(serverId.ToString()))
                {
                    //정보 갱신
                    _serverListView.Items[i].SubItems[5].Text = userCount.ToString();
                    break;
                }
            }
        });
    }

    public void UpdateServerGameDataHashComponent(int serverId, string hash)
    {
        BeginInvoke(() =>
        {
            for (int i = 0; i < _serverListView?.Items.Count; i++)
            {
                if (_serverListView.Items[i].Text.Equals(serverId.ToString()))
                {
                    //정보 갱신
                    _serverListView.Items[i].SubItems[4].Text = hash;
                    break;
                }
            }
        });
    }

    public void UpdateVersionInfoComponent(Dictionary<string, VersionInfo> infos)
    {
        if (null == _versionListView)
            return;

        BeginInvoke(() =>
        {
            for (int i = 0; i < _versionListView.Items.Count; ++i)
            {
                _versionListView.Items[i].BackColor = Color.Red;
            }

            foreach (var info in infos.Values)
            {
                var item = new ListViewItem();
                item.BackColor = (info.live ? Color.LightGreen : Color.LightGray);
                item.Text = info.version;
                item.SubItems.Add(info.host);
                item.SubItems.Add(info.port.ToString());
                item.SubItems.Add(info.live.ToString());

                int serverIndex = -1;
                foreach (ColumnHeader e in _versionListView.Columns)
                {
                    ++serverIndex;

                    if (e.Text.Equals("version")) break;
                }

                bool isUpdate = false;
                for (int i = 0; i < _versionListView.Items.Count; i++)
                {
                    if (_versionListView.Items[i].SubItems[serverIndex].Text.Equals(info.version))
                    {
                        //정보 갱신
                        _versionListView.Items[i] = item;
                        isUpdate = true;
                        break;
                    }
                }

                // 갱신된 정보가 없으면 추가.
                if (!isUpdate)
                {
                    _versionListView.Items.Add(item);
                }
            }
        });
    }
}