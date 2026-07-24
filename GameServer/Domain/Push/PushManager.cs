

using CommonStruct;
using HNET;
using Microsoft.VisualBasic.ApplicationServices;
using NerdFox.http;
using NerdFox.http.define;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;

public partial class PushManager : BaseManager<PushManager>
{
    public override void OnUpdate()
    {
        lock (this)
        {
            long nowTimeUtc = DateTime.UtcNow.ToFileTime();

            while (0 < _pushMessageList.Count)
            {
                if (false == _pushMessageList.TryPeek(out var pushMessage, out var executeTime))
                {
                    continue;
                }

                // 첫 번째 작업의 시간이 아직 도달하지 않았으면 나머지도 확인할 필요 없음
                if (pushMessage.executeTime > nowTimeUtc)
                {
                    break;
                }

                // 작업 실행
                ExecutePushMessage(_pushMessageList.Dequeue());
            }
        }
    }
}


public partial class PushManager : BaseManager<PushManager>
{
    public override bool Initialize()
    {
        lock (this)
        {
            // db 로드.
            DBManager.LoadPushMessage(out _pushMessageList);
        }


        return true;
    }

    public void AddPushMessage(PushParam param, Action<long>? onResult = null)
    {
        if (false == param.IsValid())
        {
            Logger.FAIL($"failed to push message...  ( {JsonConvert.SerializeObject(param)} )");
            return;
        }

        // 같은 시간 푸시 메시지 체크
        if (IsExistCachePushMessage(param.senderCid, param.executeDateTimeUtc.ToFileTime()))
        {
            Logger.FAIL($"failed to duplicate push message...  ( {JsonConvert.SerializeObject(param)} )");
            return;
        }

        // DB에 기록.
        DBManager.AddPushMessage(param, (pushId) =>
        {
            lock (this)
            {
                var pushMessage = new PushInfo(pushId, param.senderCid, param.executeDateTimeUtc.ToFileTime());
                if (null == pushMessage) return;

                _pushMessageList.Enqueue(pushMessage, pushMessage.executeTime);

                AddCachePushMessage(pushMessage);
            }

            onResult?.Invoke(pushId);
        });
    }

    public void DelPushMessage(long pushId, User? user = null, Action? action = null)
    {
        if (null == user)
            return;

        DBManager.DelPushMessage(pushId, user, (executeTime) => 
        {
            DelCachePushMessage(user.cid, executeTime);

            action?.Invoke();
        });
    }

    public void ExecutePushMessage(PushInfo pushMessage)
    {
        Logger.DEBUG($"★ ★ execute push message... pushId( {pushMessage.pushId} ), cid( {pushMessage.cid} ), execute time( {DateTime.FromFileTime(pushMessage.executeTime)} )");

        DBManager.GetPushMessage(pushMessage.pushId, (title, content, roomId, dmId, pushToken) => 
        {
            var jsonMessage = "";
            {
                try
                {
                    var subMessage = new PushSubMessage
                    {
                        cid     = pushMessage.cid.ToString(),
                        message = content,
                        roomId  = roomId,
                        dmId    = dmId,
                    };
                    jsonMessage = JsonConvert.SerializeObject(subMessage);
                }
                catch (Exception ex) 
                {
                    Logger.EXCEPTION(ex, "failed to ExecutePushMessage()... ");
                    return;
                }
            }

            var connector = NerdFoxConnector.Create();
            connector.SetGameType(NERDFOX_GAME_TYPE.GF);

            NerdFoxPacket? recvPacket = connector.SendPush(title, content, pushToken, jsonMessage);
            if (recvPacket?.IsError() ?? true)
            {
                Logger.ERROR($"failed to push message... errorCode( {recvPacket?.GetErrorCode()} ), errorMessage( {recvPacket?.GetErrorMessage()} )");
                return;
            }

            // 캐싱 삭제
            DelCachePushMessage(pushMessage.cid, pushMessage.executeTime);

            Logger.DEBUG($"★ ★ del push message... pushId( {pushMessage.pushId} ), cid( {pushMessage.cid} ), execute time( {DateTime.FromFileTime(pushMessage.executeTime)} )");
        });
    }

    private void AddCachePushMessage(PushInfo pushMessage)
    {
        lock (_cacheList)
        {
            // 캐릭터의 푸시 등록리스트 확보.
            if (false == _cacheList.TryGetValue(pushMessage.cid, out var pushList))
            {
                pushList = new Dictionary<long, long>();
                _cacheList.Add(pushMessage.cid, pushList);
            }

            if (pushList.ContainsKey(pushMessage.executeTime))
            {
                Logger.CRITICAL($"failed to duplicate push message... pushId( {pushMessage.pushId} ), cid( {pushMessage.cid} ), execute time( {pushMessage.executeTime} )");
                return;
            }

            pushList.Add(pushMessage.executeTime, pushMessage.pushId);
        }
    }

    public void DelCachePushMessage(long cid, long executeTime)
    {
        lock (_cacheList)
        {
            if (false == _cacheList.TryGetValue(cid, out var pushList))
                return;

            pushList.Remove(executeTime);
        }
    }

    public bool IsExistCachePushMessage(long cid, long executeTime)
    {
        lock (_cacheList)
        {
            if (false == _cacheList.TryGetValue(cid, out var pushList))
                return false;

            return pushList.ContainsKey(executeTime);
        }
    }

    public int Count()
    {
        return _pushMessageList.Count;
    }


    // ===================================================================================================================
    //
    //
    // 시간이 작은 순대로 이벤트 관리.
    private PriorityQueue<PushInfo, long> _pushMessageList = new PriorityQueue<PushInfo, long>();

    // 캐싱 정보.
    private Dictionary<long, Dictionary<long, long>> _cacheList = new Dictionary<long, Dictionary<long, long>>(); // <cid, <fileTime, fileTime>>
}
