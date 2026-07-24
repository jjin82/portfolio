

// ===================================================================================================================
//
// 시나리오.
//
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using NerdFox;
using NerdFox.GF.vo;
using NerdFox.OpenAI.define;
using NerdFox.OpenAI.http;
using Newtonsoft.Json;


public partial class Npc : Character
{
    public override bool IsNpc()
    {
        return true;
    }

    // ===================================================================================================================
    //
    //
    public override CHAR_TYPE charType => CHAR_TYPE.NPC;
}





