


using Common;
using Google.Protobuf.WellKnownTypes;

internal partial class ClientAcceptor : HNET.Acceptor
{
    void RQ_CHEAT(int netId, User? user, C2G.RQ_CHEAT packet)
    {
        try
        {
            if (ServerConfig.IsLive())
                return;

            switch (packet.cmd)
            {
                case "character_add":
                    {
                        foreach (var e in T_CharacterData.GetAll())
                        {
                            if (false == e.Enable)
                                continue;

                            user?.AddCharacter(e);
                        }
                    }
                    break;
                case "currency_add":
                    {
                        var type  = int.Parse(packet.param1);
                        var count = int.Parse(packet.param2);

                        user?.AddCurrency((CURRENCY_TYPE)type, count); 
                    }
                    break;
                case "item_set":
                    {
                        var tid   = int.Parse(packet.param1);
                        var count = int.Parse(packet.param2);

                        user?.UpdateItem(tid, count);
                    }
                    break;
                case "item_use":
                    {
                        var tid = int.Parse(packet.param1);

                        user?.UseItem(tid);
                    }
                    break;
                case "mail_add":
                    {
                        user?.AddMail(1);
                    }
                    break;
                case "mail_open":
                    {
                        var mailId = long.Parse(packet.param1);

                        user?.OpenMail(mailId);
                    }
                    break;
                case "product_buy":
                    {
                        var shopTid = int.Parse(packet.param1);

                        user?.BuyProduct(shopTid, 1);
                    }
                    break;
                case "withdraw":
                    {
                        user?.Withdraw();
                    }
                    break;
                case "set_iap":
                    {
                        user?.SetHasMadeIAP();
                    }
                    break;
                case "reset_ad":
                    {
                        user.ResetActiveAd();
                    }
                    break;
                case "time_reward":
                    {
                        user.CheatTimeReward();
                    }
                    break;
                case "ms":
                    {
                        var type = (MISSION_TYPE)int.Parse(packet.param1);
                        int.TryParse(Convert.ToString(packet.param2), out int cond1);
                        int.TryParse(Convert.ToString(packet.param3), out int cond2);

                        user.CheatExecuteMission(type, cond1, cond2);
                    }
                    break;
                case "ms_c":
                    {
                        user.CheatClearMission();
                    }
                    break;
            }
        }
        catch (Exception e)
        {
            Logger.EXCEPTION(e, $"failed to cheat... cmd( {packet.cmd} )");
        }
    }
}
 