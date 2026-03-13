using System;
using System.Collections.Generic;
using System.Reflection;

public class Test4_2018 : BaseTest
{
    Dictionary<int, Guard> GuardIdMap = new Dictionary<int, Guard>();

    public override void Initialise()
    {
        Year = 2018;
        TestID = 4;
    }

    public override void Execute()
    {
        List<(DateTime, string)> eventList = new List<(DateTime, string)>();

        foreach (string line in m_dataFileContents)
        {
            int index = line.IndexOf(']');
            string dateTimeString = line.Substring(1, index - 1);
            string eventInfo = line.Substring(index + 1);
            eventList.Add((DateTime.Parse(dateTimeString), eventInfo));
        }

        eventList.Sort((a, b) => a.Item1.CompareTo(b.Item1));

        // ok have all events ordered.

        Guard currentGuard = null;
        foreach (var eventPair in eventList)
        {
            if (eventPair.Item2.Contains("begins shift"))
            {
                string[] tokens = eventPair.Item2.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                int id = int.Parse(tokens[1].Replace("#", ""));
                if (!GuardIdMap.TryGetValue(id, out Guard guard))
                {
                    guard = new Guard();
                    guard.Id = id;
                    GuardIdMap.Add(id, guard);
                }

                // if (currentGuard != null && currentGuard.Id != guard.Id)
                // {
                //     currentGuard.Events.Add((eventPair.Item1, false));
                // }
                currentGuard = guard;
                //currentGuard.Events.Add((eventPair.Item1, true));
                // force the shift to start midnight of the following day?
                DateTime adjustedDate = eventPair.Item1.AddHours(2).Date;
                currentGuard.Events.Add((adjustedDate, true));
            }
            else if (eventPair.Item2.Contains("falls asleep"))
            {
                currentGuard.Events.Add((eventPair.Item1, false));
            }
            else if (eventPair.Item2.Contains("wakes up"))
            {
                currentGuard.Events.Add((eventPair.Item1, true));
            }
        }

        foreach (Guard guard in GuardIdMap.Values)
        {
            guard.CalculateMinutesPerDay();
        }


        if (IsPart1)
        {
            Guard sleepiestGuard = null;
            foreach (Guard guard in GuardIdMap.Values)
            {
                if (sleepiestGuard == null || guard.TotalMinutes() > sleepiestGuard.TotalMinutes())
                {
                    sleepiestGuard = guard;
                }
            }

            DebugOutput($"Guard {sleepiestGuard.Id}  sleepiest at minute {sleepiestGuard.CalculateSleepiestMinute()}");

            DebugOutput($"Result : {sleepiestGuard.Id * sleepiestGuard.CalculateSleepiestMinute()}");
        }
        else
        {
            Guard sleepiestGuard = null;
            int mostFrequentMinute = 0;
            int mostFrequentMinuteValue = 0;

            foreach (Guard guard in GuardIdMap.Values)
            {
                guard.CalculateSleepiestMinute();
            }

            for (int i = 0; i < 60; i++)
            {
                foreach (Guard guard in GuardIdMap.Values)
                {
                    if (sleepiestGuard == null || guard.SleepiestMinutes[i] > mostFrequentMinuteValue)
                    {
                        sleepiestGuard = guard;
                        mostFrequentMinute = i;
                        mostFrequentMinuteValue = guard.SleepiestMinutes[i];
                    }

                }
            }
            
            DebugOutput($"Guard {sleepiestGuard.Id}  sleepiest at minute {mostFrequentMinute}");

            DebugOutput($"Result : {sleepiestGuard.Id * mostFrequentMinute}");

        }
    }
}

public class Guard
{
    public int Id;

    public List<(DateTime, bool)> Events = new List<(DateTime, bool)>();
    public Dictionary<DateTime, int> MinutesAsleep = new Dictionary<DateTime, int>();
    public Dictionary<int, int> SleepiestMinutes = new Dictionary<int, int>();

    public bool OnDuty(DateTime date)
    {
        foreach (var guardEvent in Events)
        {
            if (guardEvent.Item1.Date == date.Date)
            {
                return true;
            }
        }

        return false;
    }

    public int CalculateSleepiestMinute()
    {
        int sleepiestMinute = -1;
        int sleepiestMinuteCount = -1;

        for (int i = 0; i < 60; i++)
        {
            if (!SleepiestMinutes.ContainsKey(i))
            {
                SleepiestMinutes[i] = 0;
            }

            int minuteCount = 0;
            DateTime firstDay = Events[0].Item1.Date;
            DateTime lastDay = Events[Events.Count - 1].Item1.Date;

            DateTime currentDay = firstDay;
            bool keepGoing = true;
            while (keepGoing)
            {
                if (OnDuty(currentDay))
                {
                    currentDay = currentDay.AddMinutes(i);

                    if (!AwakeAtTime(currentDay))
                    {
                        minuteCount++;
                        SleepiestMinutes[i] += 1;
                    }

                    if (minuteCount >= sleepiestMinuteCount)
                    {
                        sleepiestMinute = i;
                        sleepiestMinuteCount = minuteCount;
                    }
                }

                currentDay = currentDay.Date.AddDays(1);
                if (currentDay > lastDay)
                {
                    keepGoing = false;
                }
            }
        }

        return sleepiestMinute;
    }

    public void CalculateMinutesPerDay()
    {
        DateTime firstDay = Events[0].Item1.Date;
        DateTime lastDay = Events[Events.Count - 1].Item1.Date;

        DateTime currentDay = firstDay;
        bool keepGoing = true;
        while (keepGoing)
        {
            if (OnDuty(currentDay))
            {
                int minuteCount = 0;

                for (int i = 0; i < 60; i++)
                {
                    if (!AwakeAtTime(currentDay))
                    {
                        minuteCount++;
                    }

                    currentDay = currentDay.AddMinutes(1);
                }

                {
                    MinutesAsleep[currentDay] = minuteCount;
                }
            }

            currentDay = currentDay.Date.AddDays(1);
            if (currentDay > lastDay)
            {
                keepGoing = false;
            }
        }
    }

    public int TotalMinutes()
    {
        int total = 0;
        foreach (int count in MinutesAsleep.Values)
        {
            total += count;
        }

        return total;
    }

    public bool AwakeAtTime(DateTime time)
    {
        for (int i = 0; i < Events.Count; i++)
        {
            if (time >= Events[i].Item1 && (i == Events.Count - 1 || time < Events[i + 1].Item1))
            {
                return Events[i].Item2;
            }
        }

        return false;
    }

    public override string ToString()
    {
        return "" + Id;
    }
}