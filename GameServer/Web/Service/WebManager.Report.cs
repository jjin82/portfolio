
using Common;
using NerdFox.http;
using NerdFox.http.define;
using Org.BouncyCastle.Tls;

public partial class WebManager : BaseManager<WebManager>
{
    private string GAME_TYPE => "02"; // 디어 마이 프렌즈.
    private string TEST_BED => ServerConfig.IsLive() ? "00" : "99"; // ANAL_PING 파라미터 test_bed - 운영구분 00:운영 99:테스트

    private Dictionary<string, object> GetParam(string view, string pid, long uid, string nickname)
    {
        var param = new Dictionary<string, object>();
        param["game_type"]  = GAME_TYPE;
        param["test_bed"]   = TEST_BED;
        param["view"]       = view;
        param["pid"]        = pid;
        param["uid"]        = uid;
        param["nickname"]   = nickname; 

        return param;
    }

    public void ReportLogin(string pid, long uid, string nickname)
    {
        HNET.JOB_SLOW.Post(() =>
        {
            NerdFoxConnector connector = NerdFoxConnector.Create();

            var param = GetParam("login", pid, uid, nickname);
            connector.Analize(NERDFOX_API.ANAL_PING, param);
        });
    }

    public void ReportLogout(string pid, long uid, string nickname)
    {
        HNET.JOB_SLOW.Post(() =>
        {
            NerdFoxConnector connector = NerdFoxConnector.Create();

            var param = GetParam("logout", pid, uid, nickname);
            connector.Analize(NERDFOX_API.ANAL_PING, param);
        });
    }

    public void ReportNewbie(string pid, long uid, string nickname)
    {
        HNET.JOB_SLOW.Post(() =>
        {
            NerdFoxConnector connector = NerdFoxConnector.Create();

            var param = GetParam("newbie", pid, uid, nickname);
            connector.Analize(NERDFOX_API.ANAL_PING, param);
        });
    }

    public void ReportUseItem(string pid, long uid, string nickname, int itemTid)
    {
        HNET.JOB_SLOW.Post(() =>
        {
            NerdFoxConnector connector = NerdFoxConnector.Create();

            var param = GetParam("use_item", pid, uid, nickname);
            param["item_tid"] = itemTid;
            connector.Analize(NERDFOX_API.ANAL_PING, param);
        });
    }

    public void ReportGiftMoblieData(string pid, long uid, string nickname, int mailTid)
    {
        HNET.JOB_SLOW.Post(() =>
        {
            NerdFoxConnector connector = NerdFoxConnector.Create();

            var param = GetParam("gift_mail", pid, uid, nickname);
            param["mail_tid"] = mailTid;
            connector.Analize(NERDFOX_API.ANAL_PING, param);
        });
    }

    public void ReportExecuteAd(string pid, long uid, string nickname, AD_TYPE type)
    {
        HNET.JOB_SLOW.Post(() =>
        {
            NerdFoxConnector connector = NerdFoxConnector.Create();

            var param = GetParam("execute_ad", pid, uid, nickname);
            param["ad_type"] = type.ToString();
            connector.Analize(NERDFOX_API.ANAL_PING, param);
        });
    }
}
