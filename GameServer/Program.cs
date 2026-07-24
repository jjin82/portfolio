
using Microsoft.VisualBasic.ApplicationServices;
using NerdFox.http;
using NerdFox.OpenAI.define;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

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
        Application.Run(_form = new GameForm());
    }

    public static void Initialize()
    {
        try
        {
            Logger.INFO_PRINT($"■ Start ■");

            // 서버 정보 출력.
            Logger.INFO_PRINT($"[ Network ] - Client to Game( C2G ), host( {ServerConfig.C2G.host} ), port( {ServerConfig.C2G.port} )");
            Logger.INFO_PRINT($"[ Network ] - Game to Front( G2F ), host( {ServerConfig.G2F.host} ), port( {ServerConfig.G2F.port} )");

            // 느려서 최초 로그를 쌓는 정보는 쓰레드로 뺌.
            Task.Run(() =>
            {
                // 서버 성능.
                Logger.INFO_PRINT($"[ Performance ] cpu( {Performance.Cpu()} ), memory( {Performance.Memory()} ), disk( {Performance.Disk()} )");
            });

            // I/O 쓰레드는 프로세스의 3배로 세팅.
            HNET.JOB_SLOW.SetThreadCount((Environment.ProcessorCount * 3));

            // 게임 데이터 로드.
            GameData.Excel.LoadAllGameData("GameData");

            // 메니져 초기화.
            {
                if (!StringManager.Get.Initialize())        { Logger.CRITICAL_PRINT($"failed to initialize string manager.");       return; } // 문자열 관리자 초기화.
                if (!DBManager.Get.Initialize())            { Logger.CRITICAL_PRINT($"failed to initialize db manager.");           return; } // DB 관리자 초기화.
                if (!UserManager.Get.Initialize())          { Logger.CRITICAL_PRINT($"failed to initialize user manager.");         return; } // 유저 관리자 초기화.
                if (!ShopManager.Get.Initialize())          { Logger.CRITICAL_PRINT($"failed to initialize shop manager.");         return; } // 상점 관리자 초기화.
                if (!PushManager.Get.Initialize())          { Logger.CRITICAL_PRINT($"failed to initialize push manager.");         return; } // 알람 이벤트 관리자 초기화(푸시 알람).
                if (!TimeEventManager.Get.Initialize())     { Logger.CRITICAL_PRINT($"failed to initialize time event manager.");   return; } // 시간별 이벤트 관리자 초기화.
                if (!WebManager.Get.Initialize())           { Logger.CRITICAL_PRINT($"failed to initialize web manager.");          return; } // 웹 메니져.
                if (!GameManager.Get.Initialize())          { Logger.CRITICAL_PRINT($"failed to initialize game manager.");         return; } // 게임 메니져.
                if (!RankingManager.Get.Initialize())       { Logger.CRITICAL_PRINT($"failed to initialize ranking manager.");      return; } // 랭킹 초기화.                


                ForbiddenWord.Get().Load(); // 금칙어.
            }

            // 네트워크 초기화.
            if (false == Network.Initialize())
                return;

            // 로컬 정보 세팅.
            _form?.UpdateLocalInfoComponent();
        }
        catch (Exception ex)
        {
            Logger.EXCEPTION(ex);
            return;
        }
    }

    public static MODEL_TYPE GetLLM()
    {
        return s_LLM;
    }

    public static MODEL_TYPE GetDefaultLLM()
    {
        return s_defaultLLM;
    }

    public static MODEL_TYPE GetFreeLLM()
    {
        return MODEL_TYPE.GPT_4O_MINI;
    }

    public static bool IsWebLogin()
    {
        return _form?.webLogin?.Checked ?? true;
    }

    public static bool IsChatHistory()
    {
        return _form?.chatHistory?.Checked ?? true;
    }

    public static bool IsRemainIAP()
    {
        return _form?.remainIAP?.Checked ?? true;
    }

    public static bool IsScenario()
    {
        return _form?.scenario?.Checked ?? true;
    }

    public static bool IsLogSystemPacket()
    {
        return _form?.IsLogSystemPacket() ?? true;
    }

    public static bool IsLogPacket(ushort type)
    {
        return _form?.IsLogPacket(type) ?? true;
    }

    public static bool IsOpenService()
    {
        return s_serviceOpen;
    }

    public static bool IsMakeScenario()
    {
        return s_makeScenario;
    }

    public static bool IsMakeSmallTalk()
    {
        return s_makeSmallTalk;
    }


    private static GameForm?    _form           = null;
    public static bool          s_serviceOpen   = true;
    public static MODEL_TYPE    s_LLM           = MODEL_TYPE.NONE;
    public static MODEL_TYPE    s_defaultLLM    = MODEL_TYPE.NONE;
    public static bool          s_makeScenario  = false;
    public static bool          s_makeSmallTalk = false;
}