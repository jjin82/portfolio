

// ===================================================================================================================
//
// 대화.
//
using System;
using System.Text;
using NerdFox.OpenAI.define;
using NerdFox.OpenAI.http;
using NerdFox.GF.vo;
using Newtonsoft.Json;
using NerdFox.OpenAI.vo;
using Microsoft.VisualBasic.ApplicationServices;



public partial class Npc : Character
{
    public override bool AddPushMessage(long roomId, string time, string message)
    {
        if(false == base.AddPushMessage(roomId, time, message))
            return false;

        if (null == owner)
            return false;

        // 시간 보정.(gpt에서 제공 받은 시간 오류 보정)
        if (false == _AdjustPushTime(time, out var outTime))
            return false;

        // UTC로 변경.
        var executeTimeUtc = outTime.ToUniversalTime();

        // 푸시 메시지 추가.
        PushManager.Get.AddPushMessage(new PushParam(roomId, owner.uid, cid, nickname, message, executeTimeUtc), (pushId) =>
        {
            //owner.Send(new C2G.RS_CHARACTER_PUSH_MESSAGE_SCHEDULE(pushId, cid, message, executeTimeUtc.ToFileTimeUtc(), roomId));
        });

        return true;
    }
}

public partial class Npc : Character
{
    private bool _AdjustPushTime(string inputTime, out DateTime executeTime)
    {
        if (false == DateTime.TryParse(inputTime, out executeTime))
            return false;

        DateTime now = DateTime.Now;
        DateTime compareTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);

        executeTime = new DateTime(now.Year, now.Month, now.Day, executeTime.Hour, executeTime.Minute, 0);

        // 현재 시간보다 작을 경우 보정
        while (executeTime < compareTime)
        {
            // 시간 보정: 오전/오후 변경
            if (executeTime.Hour < 12)
            {
                // 오전일 경우 오후로 보정
                executeTime = executeTime.AddHours(12);
            }
            else
            {
                // 오후일 경우 오전으로 보정
                executeTime = executeTime.AddHours(-12);

                // 보정한 시간이 여전히 현재 시간보다 작다면 날짜를 하루 뒤로 변경
                if (executeTime <= compareTime)
                {
                    executeTime = executeTime.AddDays(1);
                }
            }
        }

        Logger.DEBUG($"▶ now( {now} ) ◀ ■ ■ ■      ( {inputTime} ) ◆ ( {executeTime} )");

        return true;
    }
}

