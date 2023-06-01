using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimerManager : MonoBehaviour
{
    private Dictionary<string, EventWrapper> TimerDictionary;
    private Dictionary<string, float> TimerRemainingTime;
    private Dictionary<string, UnityAction> TimerMethods;

    private static TimerManager timerManager;
    private static TimerManager instance
    {
        get
        {
            if (!timerManager)
            {
                timerManager = FindObjectOfType(typeof(TimerManager)) as TimerManager;
                if (!timerManager)
                {
                    Debug.LogError("There needs to be one active TimerManager object in the scene.");
                }
                else
                {
                    //Debug.Log("Init Requested");
                    timerManager.Init();
                }
            }

            return timerManager;
        }
    }

    void Init()
    {
        //LogSystem.Log("TimerManager", "Init Called");
        if (TimerDictionary == null)
        {
            //LogSystem.Log("TimerManager", "Initialised TimerDictionaries");
            TimerDictionary = new Dictionary<string, EventWrapper>();
            TimerRemainingTime = new Dictionary<string, float>();
            TimerMethods = new Dictionary<string, UnityAction>();
        }
    }

    public static void AddTimer(string TimerName, float Timer, UnityAction Listener)
    {
        EventWrapper thisEvent = null;
        if (instance.TimerDictionary.TryGetValue(TimerName, out thisEvent))
        {
            // If value exists, set thisevent to it and Add listener.
            thisEvent.AddListener(Listener);
        } 
        else
        {
            //Debug.Log(Listener);
            // if value does NOT exist, create a new 
            thisEvent = new EventWrapper();
            thisEvent.instantiate();
            thisEvent.AddListener(Listener);
            instance.TimerDictionary.Add(TimerName, thisEvent);
        }
        if (instance.TimerRemainingTime.ContainsKey(TimerName))
        {
            instance.TimerRemainingTime[TimerName] = Timer;
        }
        else
        {
            instance.TimerRemainingTime.Add(TimerName, Timer);
        }
        if (instance.TimerMethods.ContainsKey(TimerName))
        {
            instance.TimerMethods[TimerName] = Listener;
        }
        else
        {
            instance.TimerMethods.Add(TimerName, Listener);
        }
    }

    public static void RemoveTimer(string TimerName)
    {
        if (timerManager == null) return;
        EventWrapper thisEvent = null;
        if (instance.TimerDictionary.TryGetValue(TimerName, out thisEvent))
        {
            UnityAction listener = instance.TimerMethods[TimerName];
            thisEvent.RemoveListener(listener);
        }
        if (instance.TimerDictionary.ContainsKey(TimerName))
        {
            instance.TimerDictionary.Remove(TimerName);
        }
        if (instance.TimerMethods.ContainsKey(TimerName))
        {
            instance.TimerMethods.Remove(TimerName);
        }
        if (instance.TimerRemainingTime.ContainsKey(TimerName))
        {
            instance.TimerRemainingTime.Remove(TimerName);
        }
    }

    public static void RemoveTimer(string TimerName, UnityAction listener)
    {
        LogSystem.Log(instance.gameObject, "Got this guy");
        if (timerManager == null) return;
        Debug.Log(timerManager);
        EventWrapper thisEvent = null;
        if (instance.TimerDictionary.TryGetValue(TimerName, out thisEvent))
        {
            string temp = ("Removing " + TimerName + " From Listener");
            LogSystem.Log(instance.gameObject, temp);
            thisEvent.RemoveListener(listener);
        }
        if (instance.TimerDictionary.ContainsKey(TimerName))
        {
            instance.TimerDictionary.Remove(TimerName);
        }
        if (instance.TimerMethods.ContainsKey(TimerName))
        {
            instance.TimerMethods.Remove(TimerName);
        }
        if (instance.TimerRemainingTime.ContainsKey(TimerName))
        {
            instance.TimerRemainingTime.Remove(TimerName);
        }
    }

    public static void TriggerTimer(string TimerName)
    {
        EventWrapper thisEvent = null;
        if (instance.TimerDictionary.TryGetValue(TimerName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }

    public void Update()
    {
        if (instance.TimerRemainingTime.Count > 0)
        {
            Dictionary<string, float> TimerRemainingTimeClone = new Dictionary<string, float>(instance.TimerRemainingTime);
            foreach (string TimerKey in TimerRemainingTimeClone.Keys)
            {
                //Debug.Log(TimerKey);
                instance.TimerRemainingTime[TimerKey] -= Time.deltaTime;
                //Debug.Log(TimerRemainingTime[TimerKey]);
                if (instance.TimerRemainingTime[TimerKey] <= 0)
                {
                    //LogSystem.Log("TimerManager", "Timer Removed");
                    TriggerTimer(TimerKey);
                    RemoveTimer(TimerKey);
                }
            }
        }
    }
}

public class EventWrapper
{
    public UnityEvent c_Event;
    public int c_ListenerCount;

    public void instantiate(UnityEvent v_Event)
    {
        c_Event = v_Event;
        c_ListenerCount = 0;
    }
    public void instantiate()
    {
        c_Event = new UnityEvent();
        c_ListenerCount = 0;
    }
    public void AddListener(UnityAction Listener)
    {
        c_Event.AddListener(Listener);
        c_ListenerCount++;
    }
    public void RemoveListener(UnityAction Listener)
    {
        c_Event.RemoveListener(Listener);
        c_ListenerCount--;
    }
    public void Invoke()
    {
        c_Event.Invoke();
    }
    public int GetListenerCount()
    {
        return c_ListenerCount;
    }
    public UnityEvent GetEvent()
    {
        return c_Event;
    }
}