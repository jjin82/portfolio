

public class Mail
{
    public Mail()
    {
        
    }

    public T_MailData           data            => T_MailData.Get(_tid); // 테이블 정보.
    public long                 id              => _id;
    public int                  tid             => _tid;
    public bool                 open            => _open;
    public string               title           => data.Title;
    public string               content         => data.Content;
    public long                 createFileTime  => _createFileTime;
    public REWARD_TYPE          rewardType      => data?.RewardType ?? 0;
    
    // ===================================================================================================================
    //
    //
    public long     _id             = 0;
    public int      _tid            = 0;
    public bool     _open           = false;
    public long     _createFileTime = 0;
}
