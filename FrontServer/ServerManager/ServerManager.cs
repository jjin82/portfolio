using System.Collections.Concurrent;

public class ServerManager
{
    private static readonly Lazy<ServerManager> _instance = new Lazy<ServerManager>(() => new ServerManager());
    public static ServerManager Instance { get { return _instance.Value; } }

    public ServerManager()
    {
    }

    public void Set(int netId, ServerInfo info)
    {
        info._netId     = netId;
        info._connected = true;

        _serverList.AddOrUpdate(info._Id, info, (key, oldValue) => oldValue = info);
    }

    public bool Del(int netId, out ServerInfo info)
    {
        info = new ServerInfo();

        lock (_serverList)
        {
            var list = _serverList.Where(x => x.Value._netId == netId);
            if (0 == list.Count()) return false;

            foreach (var e in list)
            {
                // 접속 종료 서버 정보.
                info = e.Value;

                // 접속 종료.
                info._connected = false;
            }
        }

        return true;
    }

    public bool Del(int netId, out List<ServerInfo> infos)
    {
        infos = new List<ServerInfo>();

        lock (_serverList)
        {
            var list = _serverList.Where(x => x.Value._netId == netId);
            if (0 == list.Count()) return false;

            foreach (var e in list)
            {
                // 접속 종료.
                e.Value._connected = false;

                // 접속 종료 서버 리스트 업.
                infos.Add(e.Value);
            }
        }

        return true;
    }

    public ServerInfo? GetConnectGameServerInfo()
    {
        ServerInfo? selectGameServer = null;
        int userCount = int.MaxValue;

        foreach (var e in _serverList)
        {
            if (false == e.Value._connected)
                continue;

            if (userCount <= e.Value._userCount)
                continue;

            // 유저수 기록.
            userCount = e.Value._userCount;

            // 선택 될 가능성이 있는 게임 서버.
            selectGameServer = e.Value;
        }

        return selectGameServer;
    }

    public void UpdateUserCount(int serverId, int userCount)
    {
        var serverInfo = FindServer(serverId);
        if (serverInfo == null) return;

        serverInfo._userCount = userCount;
    }

    public ServerInfo? FindServer(int serverId)
    {
        if (false == _serverList.TryGetValue(serverId, out ServerInfo? gameServerInfo))
            return null;

        return gameServerInfo;
    }

    public List<ServerInfo> GetServerList()
    {
        return _serverList.Select(x => x.Value).ToList();
    }

    public int GetTotalUserCount()
    {
        int sumCount = _serverList.Sum(x => x.Value._userCount);
        _maxUserCount = Math.Max(sumCount, _maxUserCount);

        return sumCount;
    }

    public int GetMaxUserCount()
    {
        return _maxUserCount;
    }

    private int _maxUserCount = 0;
    protected ConcurrentDictionary<int, ServerInfo> _serverList = new ConcurrentDictionary<int, ServerInfo>();
}