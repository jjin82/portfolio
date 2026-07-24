
using Common;
using NerdFox.http;
using NerdFox.http.define;


public partial class WebManager : BaseManager<WebManager>
{
    public void RequestLogin(int netId, string transactionToken, string advertisingId, LOGIN_TYPE loginType)
    {
        Logger.DEBUG($"■■■■■■■■■■■■■■■■■ advertisingId( {advertisingId} ) ■■■■■■■■■■■■■■■■■");

        // 웹 로그인 검증 처리 요청.(id token 검증)
        PostEx(() =>
        { 
            string pid              = "";
            string nickname 	    = "";
            string email            = "";
            string accessToken      = "";

            // 더미 인증이 아닌 경우는 transaction token으로 웹 로그인 처리.
            if (!DummyAuth(netId, transactionToken, ref pid, ref nickname, ref email))
            {
                Logger.DEBUG($"access token. nerfox api type( {NERDFOX_API.GET_PID} ), login type( {loginType} ), transaction token( {transactionToken} )");

                // 웹 커넥션. (개발 or 라이브 분기)
                var connector = NerdFoxConnector.Create();
                if (null != connector) 
                {
                    connector.CtxPath = ServerConfig.GetNerdFoxHost();
                }

                // web 인증 처리.
                NerdFoxPacket? recvPacket = connector.GetPID(transactionToken, advertisingId, loginType.ToString().ToLower()) ?? null;
                if (recvPacket?.IsError() ?? true)
                {
                    Logger.ERROR($"failed to login... errorCode( {recvPacket?.GetErrorCode()} ), errorMessage( {recvPacket?.GetErrorMessage()} )");

                    if (false == ServerConfig.IsDev())
                    {
                        Network.Disconnect(netId);
                        return;
                    }
                }

                pid                 = recvPacket.GetResponseString("pid");
                nickname            = recvPacket.GetResponseString("nick_name");
                email               = recvPacket.GetResponseString("email");
                accessToken         = recvPacket.GetResponseString("access_token");

                // 기본 정보 확인.
                if (string.IsNullOrEmpty(pid) || string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(accessToken))
                {
                    Logger.CRITICAL_PRINT($"failed to login... pid( {pid} ), nickname( {nickname} ), email( {email} ), accessToken( {accessToken} ), transactionToken( {transactionToken} )");
                    Network.Disconnect(netId);
                    return;
                }
            }

            var user = UserManager.Get.FindFromPID(pid);
            if(null == user)
            {
                UserManager.Get.PrepareLogin(loginType, netId, pid, email, accessToken);
            }
            else
            {
                user.Post(() => 
                {
                    user.SendResultCode(RESULT_CODE.FAIL_DUPLICATE_LOGIN);

                    var disconnectNetId = user.netId;

                    if (false == UserManager.Get.Relogin(netId, user))
                    {
                        Network.Disconnect(netId);
                    }

                    // 기존 세션은 제거.
                    Network.Disconnect(disconnectNetId);
                });
            }
        });
    }

    private bool DummyAuth(int netId, string token, ref string pid, ref string nickname, ref string email)
    {
        // 라이브 or 웹 로그인 처리를 진행하는지 여부.
        if (ServerConfig.IsLive() || Program.IsWebLogin())
            return false;

        pid      = token.Substring(Math.Max(0, token.Length - 10));
        nickname = pid;
        email    = pid + "@gmail.com";

        return true;
    }
}
