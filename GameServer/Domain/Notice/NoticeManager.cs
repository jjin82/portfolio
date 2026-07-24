

using Common;
using CommonStruct;
using Microsoft.VisualBasic.ApplicationServices;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;

public partial class NoticeManager : BaseManager<NoticeManager>
{
    public override bool Initialize()
    {
        return true;
    }

    public void Notify()
    {
        UserManager.Get.SendAll((user) =>
        {
            user.Post(() => 
            {
                SendList(user);
            
            }, UTIL.GetRandom(5000));
        });
    }

    public void SendList(User user)
    {
        var packet = new C2G.RS_NOTICE_LIST();
        foreach (var e in T_NoticeData.GetAll())
        {
            if (false == e.Usable)
                continue;

            if (false == DateTime.TryParseExact(e.Date, "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
            {
                Logger.CRITICAL($"Failed to parse the notice time... notice tid( {e.TID} ), date( {e.Date} )");
                continue;
            }

            packet.Add(e.TID, e.Title, e.Content, parsedDate.ToFileTimeUtc());
        }
        user?.Send(packet);
    }
}
