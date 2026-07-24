using System;
using System.Threading;

public abstract partial class Entity
{
    protected virtual void OnCreate()           { }
    protected virtual void OnDestroy()          { }
    protected virtual void OnUpdate()           { Logger.CRITICAL($"must override the Update method...  Type( {GetType().Name} )"); }
    protected virtual void OnUpdate5Second()    { }
    protected virtual void OnUpdateMinute()     { }

    public virtual bool IsValid()               { return (0 == _destroyTime) ? true : (_destroyTime > DateTime.Now.ToFileTime()); }
    public virtual bool IsDestroy()             { return (0 != _destroyTime); }
    public virtual uint GetJobId()              { return key; }
}


public abstract partial class Entity
{
    protected Entity(bool update = true)
    {
        _key        = AllocKey();
        _update     = update;
        _allocTime  = DateTime.Now.ToFileTime();
    }

    public void Create()
    {
        if (false == _create)
        {
            _create = true;

            Post(OnCreate);
        }

        // 업데이트 수행 시작.
        if (_update)
        {
            Post(Update);

            Interlocked.Increment(ref s_jobCount);
        }
    }

    public void Destroy(int delayInMilliseconds = 0)
    {
        if (0 != _destroyTime)
            return;
        
        _destroyTime = DateTime.Now.AddMilliseconds(delayInMilliseconds).ToFileTime();

        // 업데이트 수행 종료.
        if (_update)
        {
            Interlocked.Decrement(ref s_jobCount);

            // 업데이트를 사용하는데 생명 주기가 짧은건 의심 필요.
            double seconds = (DateTime.Now - DateTime.FromFileTime(_allocTime)).TotalSeconds;
            if (2 >= seconds)
            {
                Logger.CRITICAL($"please check the lifecycle of class name( {GetType().Name} )");
            }
        }

        Post(OnDestroy, delayInMilliseconds);
    }

    private void Update()
    {
        if (false == _create)
        {
            // 생성 처리 후 동작한다고 알림.
            Logger.CRITICAL($"the entity's Update method is not working. Please execute the Create() function first...  Type( {GetType().Name} )");
            
            Post(Update, 1000); // 1초 뒤 다시 호출.
            return;
        }

        if (false == _update)
            return;

        if (IsDestroy())
            return;

        if (false == IsValid())
        {
            Destroy();
            return;
        }

        OnUpdate();

        // 5초당 업데이트
        if (DateTime.UtcNow.ToFileTime() > _secondUpdateTime)
        {
            _secondUpdateTime = DateTime.UtcNow.AddSeconds(5).ToFileTime();

            OnUpdate5Second();
        }
        
        // 분당 업데이트
        if (DateTime.UtcNow.ToFileTime() > _minuteUpdateTime)
        {
            _minuteUpdateTime = DateTime.UtcNow.AddMinutes(1).ToFileTime();

            OnUpdateMinute();
        }

        Post(Update, s_updateTime);
    }
    
    public void Post(Action action)
    {
        HNET.JOB.Post(GetJobId(), action);
    }

    public void Post(Action action, int millisecond)
    {
        HNET.JOB.Post(GetJobId(), action, millisecond);
    }

    public void ReplaceKey()
    {
        _key = AllocKey();
    }

    public void Resume()
    {
        if (false == _create)
        {
            // 생성 처리 후 동작한다고 알림.
            Logger.CRITICAL($"the Resume() function cannot be executed at this time. Please execute the Create() function first...  Type( {GetType().Name} )");
            return;
        }

        _update = true;

        Post(Update);

        Interlocked.Increment(ref s_jobCount);
    }

    public void Suspend()
    {
        _update = false;
    }


    public uint key => _key;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// valiable
    protected uint _key;

    private bool _create        = false;
    private bool _update        = true;
    private long _allocTime     = 0;
    private long _destroyTime   = 0;

    private long _secondUpdateTime = DateTime.UtcNow.AddMicroseconds(1).ToFileTime();
    private long _minuteUpdateTime = DateTime.UtcNow.AddMinutes(1).ToFileTime();
}

// ===================================================================================================================
//
// static valiable
//
public partial class Entity
{
    public static uint AllocKey()
    {
        int newKey = Interlocked.Increment(ref s_sequenceKey);
        while (0 == newKey)
        {
            newKey = Interlocked.Increment(ref s_sequenceKey);
        }

        return (uint)newKey;
    }

    public static int JobCount()
    {
        return s_jobCount;
    }

    private static int s_updateTime  = 200;
    private static int s_sequenceKey = Environment.TickCount;
    private static int s_jobCount    = 0;
}








// ===================================================================================================================
// ===================================================================================================================
// ===================================================================================================================
//
// mathod mapping. (엄청 느리다. 100배 이상은 느리니까 이런게 있다고 참고만하고 1회성 코드 정도에서만 고민해보자..)
// 
public abstract partial class Entity
{
    //    // 함수만 파라미터로 받아 메서드를 등록하는 함수
    //    protected void RegisterMethod(Delegate method)
    //    {
    //        // 각 파라미터의 본인 클래스(자신)와 하위 클래스(자식 클래스) 모두를 포함하여 키를 생성.
    //        var methodKeys = GetMethodKeysWithSelfAndDescendants(method);
    //        if (0 >= methodKeys.Length)
    //        {
    //            Logger.CRITICAL_PRINT($"the key generation for mapping the method failed... method( {method} )");
    //            return;
    //        }

    //        foreach (var key in methodKeys)
    //        {
    //            if (false == _methodMap.ContainsKey(key))
    //            {
    //                _methodMap[key] = method;
    //            }
    //            else
    //            {
    //                Logger.CRITICAL_PRINT($"Method '{key}' is already registered.");
    //            }
    //        }
    //    }

    //    // 메서드 이름과 매개변수를 사용하여 메서드를 호출.
    //    public void Post(string methodName, params object[] parameters)
    //    {
    //        string methodKey = GetMethodKey(methodName, parameters);

    //        if (false == _methodMap.TryGetValue(methodKey, out var method))
    //        {
    //            Logger.CRITICAL_PRINT($"no mapped method found... methd name( {methodName} ), method key( {methodKey} )");
    //            return;
    //        }

    //        HNET.JOB.Post(() =>
    //        {
    //            method.DynamicInvoke(parameters);
    //        });
    //    }

    //    // 고유한 키를 생성하는 함수 (메서드 이름 + 매개변수 타입)
    //    private string GetMethodKey(string methodName, object[] parameters)
    //    {
    //        string paramTypes = string.Join(",", Array.ConvertAll(parameters, p => p.GetType().Name));
    //        return $"{methodName}({paramTypes})";
    //    }

    //    private string[] GetMethodKeysWithSelfAndDescendants(Delegate method)
    //    {
    //        var methodName = method.Method.Name;
    //        var parameterTypes = method.Method.GetParameters();

    //        // 각 파라미터에 대해 본인 클래스와 하위(자식) 클래스의 키 생성
    //        var methodKeys = parameterTypes.Select(p =>
    //            GetSelfAndDescendantTypeKeys(p.ParameterType) // 본인 클래스와 자식 클래스 키 생성
    //        ).ToList();

    //        return CombineKeys(methodName, methodKeys);
    //    }

    //    // 본인 클래스와 자식 클래스를 포함하여 반환
    //    private string[] GetSelfAndDescendantTypeKeys(Type baseType)
    //    {
    //        // 본인 클래스 먼저 추가
    //        var types = new List<string> { baseType.Name };

    //        // 모든 어셈블리에서 자식 클래스 탐색
    //        var descendantTypes = AppDomain.CurrentDomain.GetAssemblies()
    //            .SelectMany(a => a.GetTypes())
    //            .Where(t => t.IsSubclassOf(baseType))
    //            .Select(t => t.Name);

    //        types.AddRange(descendantTypes); // 본인 클래스에 자식 클래스들 추가

    //        return types.ToArray();
    //    }

    //    // 모든 파라미터 키의 조합을 생성
    //    private string[] CombineKeys(string methodName, List<string[]> methodKeys)
    //    {
    //        var results = new List<string>();
    //        CombineRecursive(methodKeys, 0, "", methodName, results);
    //        return results.ToArray();
    //    }

    //    private void CombineRecursive(List<string[]> methodKeys, int index, string currentCombination, string methodName, List<string> results)
    //    {
    //        if (index == methodKeys.Count)
    //        {
    //            results.Add($"{methodName}({currentCombination.TrimEnd(',')})");
    //            return;
    //        }

    //        foreach (var key in methodKeys[index])
    //        {
    //            CombineRecursive(methodKeys, (index + 1), $"{currentCombination}{key},", methodName, results);
    //        }
    //    }


    //// 메서드명을 키로 하는 델리게이트 사전
    //private readonly Dictionary<string, Delegate> _methodMap = new Dictionary<string, Delegate>();
}