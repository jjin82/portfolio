

using CommonStruct;

public class PushInfo
{
    public PushInfo(long pushId, long cid, long executeTime)
    {
        this.pushId         = pushId;
        this.cid            = cid;
        this.executeTime    = executeTime;
    }

    public long pushId      = 0;
    public long cid         = 0;
    public long executeTime = 0;
}

public class PushParam
{
    public PushParam()
    {

    }

    public PushParam(long roomId, long receiverUid, long senderCid, string title, string content, DateTime executeDateTimeUtc)
    {
        this.receiverUid        = receiverUid;
        this.senderCid          = senderCid;
        this.roomId             = roomId;
        this.title              = title;
        this.content            = content;
        this.executeDateTimeUtc = executeDateTimeUtc;
    }   

    public PushParam(long receiverUid, long senderCid, string title, string content, DateTime executeDateTimeUtc)
    {
        this.receiverUid        = receiverUid;
        this.senderCid          = senderCid;
        this.title              = title;
        this.content            = content;
        this.executeDateTimeUtc = executeDateTimeUtc;
    } 

    public PushParam(DMInfo dmInfo, long receiverUid, string title, DateTime executeDateTimeUtc)
    {
        this.receiverUid        = receiverUid;
        this.title              = title;
        this.executeDateTimeUtc = executeDateTimeUtc;

        this.senderCid          = dmInfo.sendCid;
        this.dmId               = dmInfo.dmId;
        this.content            = dmInfo.msg;
    }
    
    public bool IsValid()
    {
        if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content))
        {
            return false;
        }

        return true;
    }

    public long  receiverUid = 0;
    public long  senderCid   = 0;
    public long  roomId      = 0;
    public long  dmId        = 0;

    public string title     = "";
    public string content   = "";

    public DateTime executeDateTimeUtc = DateTime.UtcNow;
}


