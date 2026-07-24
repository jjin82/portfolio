

using Common;
using CommonStruct;
using System;

public partial class User : Entity
{
    private void LoadDBCharacter(Dictionary<long, Character> characterList)
    {
        _characterList = characterList;
    }

    private void AddCharacter(Character.Param param, bool isMutual = true, Action<Character?>? onResult = null)
    {
        // DB에 캐릭터 추가 후 처리. (DB에서 cid 획득)
        DBManager.AddCharacter(this, param, (result, targetGameDB, targetUid) =>
        {
            Character? newCharacter = null;
            try
            {
                if (false == result)
                    return;

                // 1. 나에게 초대 대상자 추가.
                newCharacter = Character.Alloc(this, param);
                if (null == newCharacter)
                {
                    Logger.CRITICAL($"failed to alloc character... cid( {param.cid} ), nickname( {param.nickname} )");
                    return;
                }

                if (false == _characterList.TryAdd(param.cid, newCharacter))
                {
                    Logger.CRITICAL($"failed to add character... cid( {param.cid} ), nickname( {param.nickname} )");
                    return;
                }
                Send(new C2G.RS_CHARACTER_ADD(newCharacter));

                // 2. 상대 유저에게 나를 초대자로 추가.
                if (isMutual && newCharacter.IsOtherUser())
                {
                    var targetUser = UserManager.Get.FindFromCid(newCharacter.cid);
                    if (null == targetUser)
                    {
                        DBManager.AddCharacter(GetJobId(), targetGameDB, targetUid, new Character.Param(cid, nickname, profileTid, FRIEND_STATE.INVITED));
                    }
                    else
                    {
                        targetUser.Post(() => 
                        { 
                            targetUser.AddCharacter(new Character.Param(cid, nickname, profileTid, FRIEND_STATE.INVITED), false);

                            // 상대가 친구 초대를 보냈을을 알림.
                            targetUser.SendResultCode(RESULT_CODE.NOTIFY_SENT_FRIEND_INVITATION, nickname);
                        });
                    }
                }
            }
            finally 
            {
                onResult?.Invoke(newCharacter);
            }
        });
    }

    public void AddCharacter(T_CharacterData charData, Action<Character?>? onResult = null)
    {
        if (null == charData)
        {
            Logger.CRITICAL($"character data is null... AddCharacter()...");
            return;
        }

        var character = FindCharacter(charData.TID);
        if (null != character)
        {
            Logger.CRITICAL($"character data is not null... AddCharacter()...");
            return;
        }

        AddCharacter(new Character.Param(charData.CharType, charData.TID, FRIEND_STATE.FRIENDS), false, onResult);
    }

    public void AddCharacter(long cid, string nickname, int profileTid, FRIEND_STATE friendState, Action<Character?>? onResult = null)
    {
        if (0 == cid)
            return;

        if (string.IsNullOrEmpty(nickname)) 
            return;

        AddCharacter(new Character.Param(cid, nickname, profileTid, friendState), true, onResult);
    }

    public void DelCharacter(long targetCid, bool isMutual = true)
    {
        DBManager.DelCharacter(this, targetCid, (targetUid, targetGameDB) =>
        {
            var character = FindCharacter(targetCid);
            if (null == character) return;

            _characterList.Remove(targetCid);

            Send(new C2G.RS_CHARACTER_DEL(targetCid));

            // 상대방도 삭제.(상호적 처리)
            if (isMutual)
            {
                var targetUser = UserManager.Get.FindFromCid(targetCid);
                if (null == targetUser)
                {
                    DBManager.DelCharacter((uint)DateTime.Now.Ticks, targetGameDB, targetUid, cid);
                }
                else
                {
                    targetUser.DelCharacter(cid, false);
                }
            }
        });
    }

    public void DelPlayerCharacter()
    {
        foreach (var e in _characterList.Values)
        {
            if (false == e.IsOtherUser())
                continue;

            DBManager.DelCharacter(this, e.cid, (targetUid, targetGameDB) =>
            {
                var targetUser = UserManager.Get.FindFromCid(e.cid);
                if (null == targetUser)
                {
                    DBManager.DelCharacter((uint)DateTime.Now.Ticks, targetGameDB, targetUid, cid);
                }
                else
                {
                    targetUser.DelCharacter(cid, false);
                }
            });
        }
    }

    public void UpdateCharacter(Character? character)
    {
        if (null == character) 
            return;

        SendCharacter(character);

        // DB 처리.
        DBManager.UpdateCharacter(this, character);
    }

    public Character? FindCharacter(long cid)
    {
        if (false == _characterList.TryGetValue(cid, out var character))
        {
            return null;
        }
        
        return character;
    }

    public Character? FindCharacter(int tid)
    {
        return _characterList.Values.FirstOrDefault(character => character.tid == tid);
    }

    public Character? FindCharacter(string nickname)
    {
        return _characterList.Values.FirstOrDefault(c => c.nickname.Equals( nickname));
    }

    public Character? GetRandomCharacter()
    {
        return _characterList.OrderBy(x => UTIL.GetRandom()).FirstOrDefault().Value;
    }

    public int GetCharacterCount()
    {
        return _characterList.Count;
    }

    public void SendCharacterList()
    {
        // 캐릭터 리스트.
        Send(new C2G.RS_CHARACTER_LIST(_characterList));
    }

    public void SendCharacter(Character? character)
    {
        if (null == character)
            return;

        Send(new C2G.RS_CHARACTER_UPDATE(character));
    }


    public long cid => _cid;


    Dictionary<long, Character> _characterList = new Dictionary<long, Character>();
    public long _cid = 0;
}
