using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
public static class Timers
{
    private static List<Timer> _timers = new List<Timer>();
    /// <summary>
    /// Schedules the specified action to be executed after a given time interval has elapsed.
    /// </summary>
    /// <param name="time">The delay, in seconds, before the action is invoked. If the value is less than or equal to zero, the action is
    /// executed immediately.</param>
    /// <param name="doThis">The action to execute after the specified delay. If null, no action is performed.</param>
    public static Timer After(float time, Action doThis)
    {
        // actions must be () => delegates not simple method references
        if (time <= 0f)
        {
            doThis?.Invoke();
            return null;
        }
        Timer timer = new Timer(time, doThis);
        _timers.Add(timer);
        return timer;
    }
    /// <summary>
    /// Schedules the specified action to be executed every frame for the given duration.
    /// </summary>
    /// <remarks>If <paramref name="time"/> is less than or equal to zero, the action is not scheduled and the
    /// method returns immediately. This method is typically used to perform repeated operations over a set period, such
    /// as animations or timed effects.</remarks>
    /// <param name="time">The duration, in seconds, for which the action will be executed. Must be greater than zero.</param>
    /// <param name="doThis">The action to execute each frame during the specified time interval.</param>
    public static Timer For(float time, Action doThis)
    {
        if (time <= 0f)
        {
            return null;
        }
        //doThis is run every frame for duration of time
        Timer timer = new Timer(time, doThis, null);
        _timers.Add(new Timer(time, doThis, null));
        return timer;
    }
    /// <summary>
    /// Schedules an action to be repeatedly executed for a specified duration, then executes a follow-up action once
    /// the duration has elapsed.
    /// </summary>
    /// <param name="time">The duration, in seconds, for which <paramref name="doThisUntil"/> will be repeatedly invoked. Must be greater
    /// than zero to schedule the timer.</param>
    /// <param name="doThisUntil">The action to invoke repeatedly until the specified duration has elapsed. This delegate is called during the
    /// timer's active period.</param>
    /// <param name="doThisAfter">The action to invoke once the duration has elapsed or immediately if <paramref name="time"/> is less than or
    /// equal to zero. This delegate is called after the timer completes.</param>
    public static Timer UntilThen(float time, Action doThisUntil, Action doThisAfter)
    {
        if (time <= 0f)
        {
            doThisAfter?.Invoke();
            return null;
        }
        Timer timer = new Timer(time, doThisUntil, doThisAfter);
        _timers.Add(timer);
        return timer;
    }
    public static void Clear()
    {
        _timers.Clear();
    }
    public static void RunTimers()
    {
        for (int i = _timers.Count - 1; i >= 0; i--)
        {
            Timer timer = _timers[i];
            if (timer == null)
            {
                _timers.RemoveAt(i);
                continue;
            }
            timer.Update();
            if (timer.IsFinished())
            {
                timer.OnTimerComplete?.Invoke();
                _timers.RemoveAt(i);
            }
        }
    }
    public static void Remove(Timer timer)
    {
        if (timer == null) return;
        timer.Delete();
        _timers.Remove(timer);
    }
    public static void Add(Timer timer)
    {
        if (timer == null) return;
        _timers.Add(timer);
    }
}

public class Timer
{
    public Action OnTimerComplete;
    public Action OnUpdate;
    private float _startTime;
    private float _duration;
    private bool _isPaused;
    public Timer(float duration, Action onComplete)
    {
        _duration = duration;
        _startTime = Time.time;
        OnTimerComplete = onComplete;
    }
    public Timer(float duration, Action onUpdate, Action onComplete)
    {
        _duration = duration;
        _startTime = Time.time;
        OnUpdate = onUpdate;
        OnTimerComplete = onComplete;
    }
    public void Update()
    {
        if(_isPaused) return;
        OnUpdate?.Invoke();
    }
    public bool IsFinished()
    {
        return Time.time - _startTime >= _duration;
    }
    public void Reset()
    {
        _startTime = Time.time;
    }
    public void Delete()
    {
        OnTimerComplete = null;
        OnUpdate = null;
    }
    public void Pause(bool state)
    {
        _isPaused = state;
    }
    public void PauseFor(float seconds)
    {
        _isPaused = true;
        Timers.After(seconds, () => _isPaused = false);
    }
}