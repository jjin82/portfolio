

using Common;
using CommonStruct;
using Microsoft.VisualBasic.ApplicationServices;
using System.Collections.Concurrent;

public partial class DBManager : BaseManager<DBManager>
{
    public static void AddDM(long uid, DB_KIND gameDB, ref DMInfo info, Action? action = null)
    {
        info.dmId = AllocId();

        using (var con = new DB.MySql(gameDB))
        {
            con.PREPARE("INSERT INTO t_user_dm(uid, id, parent_id, send_cid, send_nickname, send_profile_tid, recv_cid, recv_nickname, recv_profile_tid, msg, filetime) VALUES(@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11)");
            {
                con.SET_PARAM("@1",     uid);
                con.SET_PARAM("@2",     info.dmId);
                con.SET_PARAM("@3",     info.parentDmId);
                con.SET_PARAM("@4",     info.sendCid);
                con.SET_PARAM("@5",     info.sendNickname);
                con.SET_PARAM("@6",     info.sendProfileTid);
                con.SET_PARAM("@7",     info.recvCid);
                con.SET_PARAM("@8",     info.recvNickname);
                con.SET_PARAM("@9",     info.recvProfileTid);
                con.SET_PARAM("@10",    info.msg);
                con.SET_PARAM("@11",    info.fileTime);
            }

            if (0 == con.EXECUTE_UPDATE())
            {
                Logger.CRITICAL($"AddDM... uid( {uid} ), send nickname( {info.sendNickname} ), recv nickname( {info.recvNickname} ), msg( {info.msg} )");
                return;
            }
        }

        action?.Invoke();
    }

    public static bool AddDM(User sendUser, long recvUid, DB_KIND recvDB, ref DMInfo info)
    {
        info.dmId = AllocId();

        using (var con = new DB.MySql(sendUser.gameDB))
        {
            con.PREPARE("INSERT INTO t_user_dm(uid, id, parent_id, send_cid, send_nickname, send_profile_tid, recv_cid, recv_nickname, recv_profile_tid, msg, filetime) VALUES(@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11)");
            {
                con.SET_PARAM("@1",  sendUser.uid);
                con.SET_PARAM("@2",  info.dmId);
                con.SET_PARAM("@3",  info.parentDmId);
                con.SET_PARAM("@4",  info.sendCid);
                con.SET_PARAM("@5",  info.sendNickname);
                con.SET_PARAM("@6",  info.sendProfileTid);
                con.SET_PARAM("@7",  info.recvCid);
                con.SET_PARAM("@8",  info.recvNickname);
                con.SET_PARAM("@9",  info.recvProfileTid);
                con.SET_PARAM("@10", info.msg);
                con.SET_PARAM("@11", info.fileTime);
            }

            if (0 == con.EXECUTE_UPDATE())
            {
                Logger.CRITICAL(sendUser, $"1. AddDM... sender uid( {sendUser.uid} ), nickname( {info.sendNickname} ), msg( {info.msg} )");
                return false;
            }
        }

        using (var con = new DB.MySql(recvDB))
        {
            con.PREPARE("INSERT INTO t_user_dm(uid, id, parent_id, send_cid, send_nickname, send_profile_tid, recv_cid, recv_nickname, recv_profile_tid, msg, filetime) VALUES(@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11)");
            {
                con.SET_PARAM("@1",  recvUid);
                con.SET_PARAM("@2",  info.dmId);
                con.SET_PARAM("@3",  info.parentDmId);
                con.SET_PARAM("@4",  info.sendCid);
                con.SET_PARAM("@5",  info.sendNickname);
                con.SET_PARAM("@6",  info.sendProfileTid);
                con.SET_PARAM("@7",  info.recvCid);
                con.SET_PARAM("@8",  info.recvNickname);
                con.SET_PARAM("@9",  info.recvProfileTid);
                con.SET_PARAM("@10", info.msg);
                con.SET_PARAM("@11", info.fileTime);
            }

            if (0 == con.EXECUTE_UPDATE())
            {
                Logger.CRITICAL(sendUser, $"2. AddDM... receiver uid( {recvUid} ), nickname( {info.recvNickname} ), msg( {info.msg} )");
                return false;
            }
        }

        return true;
    }
}
