

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows.Forms;

public partial class GameConnector : HNET.Connector
{
    public static void Command_Cheat(string cheat, string param1, string param2, string param3, string param4, string param5)
    {
        foreach (var v in s_clientList.Values)
        {
            v.RequestCheat(cheat, param1, param2, param3, param4, param5);
            break;
        }
    }

    public static void Command_Use_Item()
    {
        foreach (var v in s_clientList.Values)
        {
            v.RequestUseItem();
            break;
        }
    }

    public static void Command_SquareChat(string msg)
    {
        Logger.INFO_PRINT($" ¢º square chat( {msg} )");

        foreach (var v in s_clientList.Values)
        {
            //v.RequestSquareChat(0, msg);
            break;
        }
    }

    public static void Command_Withdraw()
    {
        foreach (var v in s_clientList.Values)
        {
            v.RequestWithdraw();
            break;
        }
    }

    public static void Command_GiftMobileData()
    {
        foreach (var v in s_clientList.Values)
        {
            break;
        }
    }

    public static void Command_InviteRespond()
    {
        foreach (var v in s_clientList.Values)
        {
            break;
        }
    }
}