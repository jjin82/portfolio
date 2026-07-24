

using Common;
using CommonStruct;

public partial class User : Entity
{
    private void LoadDBMail(List<Mail> mailList)
    {
        foreach (var v in mailList)
        {
            _mailList.Add(v.id, v);
        }
    }

    public bool AddMail(int mailTid)
    {
        var mailData = T_MailData.Get(mailTid);
        if (null == mailData)
        {
            Logger.CRITICAL(this, $"not exist T_MailData... mail tid( {mailTid} )");
            return false;
        }

        // DB 처리.
        DBManager.AddMail(this, mailTid, (mailId, createFileTime) =>
        {
            var newMail = new Mail 
            { 
                _id             = mailId, 
                _tid            = mailTid,
                _createFileTime = createFileTime,
            };
            _mailList.Add(mailId, newMail);

            // 클라에 알림.
            Send(new C2G.RS_MAIL_UPDATE(newMail));
        });

        return true;
    }

    public bool UpdateMail(Mail mail)
    {
        if (null == mail)
        {
            Logger.CRITICAL(this, $"not exist mail...");
            return false;
        }

        // DB 처리.
        DBManager.UpdateMail(this, mail, () => 
        {
            // 클라에 알림.
            Send(new C2G.RS_MAIL_UPDATE(mail));
        });

        return true;
    }

    public bool UpdateMail(long id, bool open)
    {
        var item = FindMail(id);
        if (null == item) return false;
        
        item._open = open;

        return UpdateMail(item);
    }

    public bool OpenMail(long id)
    {
        var mail = FindMail(id);
        if (null == mail) return false;

        // 오픈 확인
        if (mail.open)
            return false;

        // 보상 지급.
        if (!GiveReward(mail.rewardType))
            return false;

        // 오픈 세팅.
        mail._open = true;

        // DB 처리.
        DBManager.UpdateMail(this, mail, () => 
        {
            Send(new C2G.RS_MAIL_OPEN
            {
                mailId = mail.id,
            });
        });

        return true;
    }

    public Mail? FindMail(long id)
    {
        if (false == _mailList.TryGetValue(id, out var item))
            return null;

        return item;
    }

    public void SendMailList()
    {
        if (0 >= _mailList.Count)
            return;

        Send(new C2G.RS_MAIL_LIST(_mailList));
    }

    Dictionary<long, Mail> _mailList = new Dictionary<long, Mail>();
}
