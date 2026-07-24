

public class Item : Entity
{
    public Item() : base(false)
    {
    }

    public void Use(int useCount = 1)
    {
        _count      = Math.Max(0, _count - useCount);
        _cooltime   = DateTime.Now.AddMilliseconds(skillData?.CoolTime ?? 0).ToFileTime();
    }

    public void ResetCooltime()
    {
        _cooltime = 0;
    }

    public T_ItemData   data            => T_ItemData.Get(_tid);                                                                     // 테이블 정보.
    public T_SkillData  skillData       => T_SkillData.Get(data?.SkillTID ?? 0);                                                     // 스킬 테이블 정보.
    public int          remainCooltime  => Math.Max(0, (int)(DateTime.FromFileTime(_cooltime) - DateTime.Now).TotalMilliseconds);    // 남은 시간.(밀리)

    public int  tid      => _tid;
    public int  count    => _count;
    public long cooltime => _cooltime;

    public int  _tid      = 0;
    public int  _count    = 0;
    public long _cooltime = 0;
}