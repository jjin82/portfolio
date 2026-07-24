using Microsoft.VisualBasic.Devices;
using System.Windows.Forms;

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
        Application.Run(_form = new FrontForm());
    }

    public static void Initialize()
    {
        try
        {
            Logger.INFO_PRINT($"■ Start ■");

            // 서버 정보 출력.
            Logger.INFO_PRINT($"[ Network ] - [ Client to Front ] ( C2F ), host( {ServerConfig.C2F.host} ), port( {ServerConfig.C2F.port} )");
            Logger.INFO_PRINT($"[ Network ] - [ Game to Front ] ( G2F ), host( {ServerConfig.G2F.host} ), port( {ServerConfig.G2F.port} )");

            // 네트워크 초기화.
            if (false == Network.Initialize())
            {
                return;
            }

            // 로컬 정보 세팅.
            _form?.UpdateLocalInfoComponent();
        }
        catch (Exception ex)
        {
            Logger.EXCEPTION(ex);
            return;
        }
    }

    public static void UpdateServerInfo(ServerInfo serverInfo)
    {
        _form?.UpdateServerInfoComponent(serverInfo);
    }

    public static void UpdateServerUserCount(int serverId, int userCount)
    {
        _form?.UpdateServerUserCountComponent(serverId, userCount);
    }

    public static void UpdateServerGameDataHash(int serverId, string hash)
    {
        _form?.UpdateServerGameDataHashComponent(serverId, hash);
    }

    public static bool IsLogSystemPacket()
    {
        return _form?.IsLogSystemPacket() ?? true;
    }

    public static bool IsLogPacket(ushort type)
    {
        return _form?.IsLogPacket(type) ?? true;
    }

    private static FrontForm? _form = null;
}
