

using Common;
using CommonStruct;
using Microsoft.VisualBasic.ApplicationServices;
using System.Collections.Concurrent;

public abstract partial class BaseManager<T> where T : new()
{
    private static readonly Lazy<T> _instance = new Lazy<T>(() => new T());
    public static T Get => _instance.Value;

    public BaseManager()
    {
        Logger.INFO_PRINT($"[ Manager ] class name( {this} )");

        HNET.JOB_SINGLE.Post(_Timer);
    }

    private void _Timer()
    {
        OnUpdate();

        HNET.JOB_SINGLE.Post(_Timer, 1000);
    }

    public virtual bool Initialize() { return true; }       // 무조건 초기화를 정의 하게 한다.
    public virtual void OnUpdate()   { }                    // 무조건 업데이트를 정의 하게 한다.
}


public partial class BaseManager<T> where T : new()
{
    public static void Post(Action action)
    {
        HNET.JOB_SINGLE.Post(() => { action.Invoke(); });
    }

    public static void PostSlow(Action action)
    {
        HNET.JOB_SLOW.Post(() => { action.Invoke(); });
    }

    public static void PostSlow(uint jobId, Action action)
    {
        HNET.JOB_SLOW.Post(jobId, () => { action.Invoke(); });
    }

    public static void PostSlow(long jobId, Action action)
    {
        HNET.JOB_SLOW.Post(jobId, () => { action.Invoke(); });
    }
}