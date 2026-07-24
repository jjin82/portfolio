using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Collections.Generic;

// 날짜 + 시간 구간
public class DateTimeRange
{
    public DateTimeRange(string startDateTime, string endDateTime)
    {
        _start = DateTime.Parse(startDateTime); // 예: "2025-12-25 00:00"
        _end = DateTime.Parse(endDateTime);     // 예: "2025-12-25 23:59"
    }

    public bool IsInRange(DateTime now)
    {
        return _start <= now && now <= _end;
    }

    public DateTime _start { get; }
    public DateTime _end { get; }
}

// 매일 반복되는 시간대 (날짜 없이 시간만 사용)
public class DailyTimeRange
{
    public DailyTimeRange(string start, string end)
    {
        _start = TimeSpan.Parse(start); // 예: "08:30"
        _end = TimeSpan.Parse(end);     // 예: "09:30"
    }

    public bool IsInRange(TimeSpan now)
    {
        return _start <= now && now <= _end;
    }

    public TimeSpan _start { get; }
    public TimeSpan _end { get; }
}

public class TimeEventManager : BaseManager<TimeEventManager>
{
    public override bool Initialize()
    {
        lock (this)
        {
            dailyEventTable.Clear();
            dateEventTable.Clear();

        }
        
        return true;
    }

    // 매일 반복 이벤트 추가
    public void AddDailyEvent(TIME_EVENT_TYPE type, string startTime, string endTime)
    {
        lock (this)
        {
            if (!dailyEventTable.ContainsKey(type))
            {
                dailyEventTable[type] = new List<DailyTimeRange>();
            }

            dailyEventTable[type].Add(new DailyTimeRange(startTime, endTime));
        }
    }

    // 특정 날짜 이벤트 추가
    public void AddDateEvent(TIME_EVENT_TYPE type, string startDateTime, string endDateTime)
    {
        lock (this)
        {
            if (!dateEventTable.ContainsKey(type))
            {
                dateEventTable[type] = new List<DateTimeRange>();
            }

            dateEventTable[type].Add(new DateTimeRange(startDateTime, endDateTime));
        }
    }

    // 현재 이벤트가 활성화되었는지 여부 확인
    public bool IsEventActive(TIME_EVENT_TYPE type)
    {
        lock (this)
        {
            DateTime now = DateTime.Now;
            TimeSpan nowTime = now.TimeOfDay;

            // 1. 특정 날짜 이벤트 확인
            if (dateEventTable.TryGetValue(type, out var dateRanges))
            {
                foreach (var range in dateRanges)
                {
                    if (range.IsInRange(now))
                        return true;
                }
            }

            // 2. 반복 시간 이벤트 확인
            if (dailyEventTable.TryGetValue(type, out var dailyRanges))
            {
                foreach (var range in dailyRanges)
                {
                    if (range.IsInRange(nowTime))
                        return true;
                }
            }
        }

        return false;
    }

    public void SendTimeEventList(User? user = null)
    {
        lock (this)
        {
            var sendPacket = new C2G.RS_TIME_EVENT_LIST();
            foreach (var group in dailyEventTable)
            {
                foreach(var e in group.Value)
                {
                    sendPacket.Set(group.Key, e._start, e._end);
                }
            }
            user?.Send(sendPacket);
        }
    }

    private readonly Dictionary<TIME_EVENT_TYPE, List<DailyTimeRange>>    dailyEventTable = new();
    private readonly Dictionary<TIME_EVENT_TYPE, List<DateTimeRange>>     dateEventTable  = new();
}
