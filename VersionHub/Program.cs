using ClosedXML.Excel;
using Microsoft.VisualBasic.Devices;
using System.Collections.Concurrent;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // 덤프 초기화.
        MiniDumper.Initialize();

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(_form = new VersionHubForm());
    }

    public static void Initialize()
    {
        try
        {
            Logger.INFO_PRINT($"■ Start ■");

            // 서버 정보 출력.
            Logger.INFO_PRINT($"[ Network ] - [ Client to Version ] ( C2V ), host( {ServerConfig.C2V.host} ), port( {ServerConfig.C2V.port} )");

            // 네트워크 초기화.
            if (false == Network.Initialize())
            {
                return;
            }

            // 로컬 정보 세팅.
            _form?.UpdateLocalInfoComponent();

            // 버전 정보 로드.
            VersionManager.LoadExcel();
        }
        catch (Exception ex)
        {
            Logger.EXCEPTION(ex);
            return;
        }
    }

    public static void UpdateVersionInfo(Dictionary<string, VersionInfo> versionInfos)
    {
        _form?.UpdateVersionInfoComponent(versionInfos);
    }

    public static bool IsLogSystemPacket()
    {
        return _form?.IsLogSystemPacket() ?? true;
    }

    public static bool IsLogPacket(ushort type)
    {
        return _form?.IsLogPacket(type) ?? true;
    }

    private static VersionHubForm? _form = null;
}
