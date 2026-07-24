

using Common;
using CommonStruct;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;

public partial class GameConnector : HNET.Connector
{
    void RS_RESULT_CODE(C2G.RS_RESULT_CODE packet)
    {
        Logger.PacketLog("--◀", packet);
    }

    void RS_LOGIN(C2G.RS_LOGIN packet)
    {
        Logger.PacketLog("--◀", packet);

        packet.Get(out bool newbie, out var cid, out var nickname, out var profileTid, out var hasMadeIAP, out  var lastLogoutFileTime, out var currencyList);

        _cid            = cid; 
        _nickname       = nickname;
        _profileImgTid  = profileTid;
        _currencyList   = currencyList;

        // static.
        s_userList.TryAdd(_cid, _cid);

        Send(new C2G.RQ_LOGIN_OK());

        _login = true;
    }

    void RS_RELOGIN(C2G.RS_RELOGIN packet)
    {
        Logger.PacketLog("--◀", packet);

        // 유저 정보.
        ulong userKey = 0;
        packet.Out(ref userKey);

        s_userList.TryAdd(_cid, _cid);
    }

    void RS_USER_PROFILE(C2G.RS_USER_PROFILE packet)
    {
        Logger.PacketLog("--◀", packet);
    }

    void RS_USER_PUSH_TOKEN_GET(C2G.RS_USER_PUSH_TOKEN_GET packet)
    {
        Logger.PacketLog("--◀", packet);
    }

    void RS_CHARACTER_LIST(C2G.RS_CHARACTER_LIST packet)
    {
        Logger.PacketLog("--◀", packet);

        packet.Get(out var characterList);

        _characterList = characterList;

        foreach(var e in characterList)
        {
            // 캐릭터 정보 출력
            Logger.INFO($"[ ▣ ▣ ▣ ]  character info. cid( {e.cid} ), tid( {e.tid} ), nickname( {e.nickname} )");
        }
    }

    void RS_CHARACTER_ADD(C2G.RS_CHARACTER_ADD packet)
    {
        Logger.PacketLog("--◀", packet);

        packet.Get(out var characterInfo);
        _characterList.Add(characterInfo);

        // 캐릭터 정보 출력
        Logger.INFO($"[ ▣ ▣ ▣ ]  add character. cid( {characterInfo.cid} ), tid( {characterInfo.tid} ), nickname( {characterInfo.nickname} )");
    }

    void RS_CHARACTER_UPDATE(C2G.RS_CHARACTER_UPDATE packet)
    {
        Logger.PacketLog("--◀", packet);

        packet.Get(out var cid, out var state);

        _characterList.Add(new CommonStruct.CharacterInfo
        { 
            cid         = cid,
            state       = state,
        });

        // 캐릭터 정보 출력
        Logger.INFO($"[ ▣ ▣ ▣ ]  update character. cid( {cid} ), state( {state} )");
    }

    void RS_CHARACTER_PUSH_MESSAGE_SCHEDULE(C2G.RS_CHARACTER_PUSH_MESSAGE_SCHEDULE packet)
    {
        Logger.PacketLog("--◀", packet);

        packet.Get(out long pushId, out long cid, out string message, out long executeFileTime, out var roomId);

        var utc = DateTime.FromFileTimeUtc(executeFileTime);
    }
}