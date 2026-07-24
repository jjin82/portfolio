

// ===================================================================================================================
//
// 시나리오.
//
using NerdFox.GF.vo;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class NpcHeroine : Npc
{
    public NpcHeroine()
    {
        
    }

    public override void Initialize(User? owner, Param param)
    {
        base.Initialize(owner, param);

        _friendState = Common.FRIEND_STATE.FRIENDS;
    }

    public override bool IsHeroine()
    {
        return true;
    }

    public override CHAR_TYPE charType => CHAR_TYPE.NONE;
}