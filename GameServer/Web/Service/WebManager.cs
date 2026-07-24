
using G2F;
using NerdFox.http;
using System.Collections.Concurrent;


public partial class WebManager : BaseManager<WebManager>
{
    public WebManager()
    {
        // 20개의 스레드를 생성하고 대기열에 추가
        for (int i = 0; i < 20; i++)
        {
            Thread thread = new Thread(new ThreadStart(ThreadWork));
            thread.IsBackground = true; // 프로그램 종료 시 백그라운드 스레드도 종료되도록 설정
            _availableThreads.Enqueue(thread);
            thread.Start();
        }
    }

    public override bool Initialize()
    {
        _pingTimer = new System.Threading.Timer((object? state) =>
                    {
                        // 웹 커넥션. (개발 or 라이브 분기)
                        var connector = NerdFoxConnector.Create();
                        if (null != connector)
                        {
                            connector.CtxPath = ServerConfig.GetNerdFoxHost();
                        }

                        // web 핑 처리.
                        var result = connector.Ping();
                        if (!result.Contains("Hello"))
                        {
                            Logger.CRITICAL_PRINT("Ping request failed.");
                        }

                    }, null, 3000, _pingInterval);

        return true;
    }

    public void PostEx(Action action)
    {
        if (null == action)
            return;

        _taskQueue.Add(() =>
        {
            action();
        });
    }

    // 스레드 작업
    private void ThreadWork()
    {
        while (true)
        {
            try
            {
                // 작업 큐에서 작업을 가져옴 (없으면 대기)
                Action task = _taskQueue.Take();
                task?.Invoke();
            }
            catch (Exception e)
            {
                Logger.EXCEPTION(e);
            }
        }
    }

    private ConcurrentQueue<Thread>    _availableThreads = new ConcurrentQueue<Thread>();
    private BlockingCollection<Action> _taskQueue        = new BlockingCollection<Action>();


    private System.Threading.Timer? _pingTimer      = null;
    private const int               _pingInterval   = 60000; // 60초.
}
