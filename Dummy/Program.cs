using Microsoft.VisualBasic.ApplicationServices;
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
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(_form = new DummyForm());
    }

    public static void Initialize() 
    {
        try
        {
            Logger.INFO_PRINT($"■ Start ■");

            // 게임 데이터 로드.
            GameData.Excel.LoadAllGameData("GameData");

            // 로컬 정보 세팅.
            _form?.UpdateLocalInfoComponent();
        }
        catch (Exception ex)
        {
            Logger.EXCEPTION(ex);
            return;
        }
    }

    public static string[] GetCheat()
    {
        return new string[]
        {
            "room_create",
            "room_chat",
            "friend_add",
            "friend_request",
            "friend_accept",
            "friend_cancel",
            "character_add",
            "character_del",
            "character_collection_buy",
            "currency_add",
            "item_set",
            "item_use",
            "mail_add",
            "mail_open",
            "product_buy",
            "withdraw",
            "set_iap",
            "gift_mobile_data"
        };
    }

    public static int Port()
    {
        return Convert.ToInt32(_form?.port.Text);
    }

    public static bool IsHistoryEnglish()
    {
        return _form?.historyEnglish?.Checked ?? true;
    }

    public static bool IsLogSystemPacket()
    {
        return _form?.logSystemPacket?.Checked ?? true;
    }

    public static bool IsLogPacket(ushort type)
    {
        return _form?.IsLogPacket(type) ?? true;
    }

    private static DummyForm? _form = null;
}