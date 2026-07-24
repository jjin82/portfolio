

using Common;
using System.Reflection.Metadata.Ecma335;

public abstract partial class Character : Entity
{
    public class Param
    {
        public Param() 
        {
        
        }

        public Param(long cid, string nickname, int profileTid, FRIEND_STATE friendState) 
        {
            this.cid            = cid;
            this.charType       = CHAR_TYPE.PC;
            this.nickname       = nickname;
            this.profileTid     = profileTid;
            this.friendState    = friendState;
        }

        public Param(CHAR_TYPE charType, int tid, FRIEND_STATE friendState) 
        {
            this.charType       = charType;
            this.tid            = tid;
            this.friendState    = friendState;
        }

        public Param(CHAR_TYPE charType, int tid)
        {
            this.charType   = charType;
            this.tid        = tid;
        }

        public bool IsNewCharacter()
        {
            return (0 >= cid);
        }

        public long         cid                 = Interlocked.Decrement(ref s_tempCid);
        public CHAR_TYPE    charType            = CHAR_TYPE.NONE;
        public int          tid                 = 0;
        public string       nickname            = "";
        public int          profileTid          = 0;
        public FRIEND_STATE friendState         = FRIEND_STATE.NONE;
        public int          lovePoint           = 0;
        public long         chatOpenerFiletime  = 0;
    };

    public static Character? Alloc(User user, Param param)
    {
        Character? character = null;
        switch (param.charType)
        {
            case CHAR_TYPE.PC:      character = new NpcHeroine();   break;
            case CHAR_TYPE.NPC:     character = new Npc();          break;
            default:
                {
                    Logger.CRITICAL($"not exist character type... type( {param.charType} )");
                }
                break;
        }

        if (null == character)
            return null;

        character.Initialize(user, param);

        return character;
    }

    public static Character? Alloc(int charTid)
    {
        var charData = T_CharacterData.Get(charTid);
        if (null == charData)
        {
            Logger.CRITICAL($"T_CharacterData is null... tid( {charTid} )");
            return null;
        }
        
        return Alloc(null, new Param(charData.CharType, charData.TID));
    }
}