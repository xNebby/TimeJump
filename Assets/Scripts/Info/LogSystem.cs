using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogSystem : SingletonClass<LogSystem>
{
    [Header("Print to Unity console")]
    public bool DebugLog = true;
    [Header("Save Log at end of session")]
    public bool SaveAuto = false;

    public Dictionary<int, Log> LogStore;

    void OnEnable()
    {
        Instance.LogStore = new Dictionary<int, Log>();
    }
    void OnDisable()
    {
        // This is only ever called when the log is disabled, as it *should not happen*.
        Debug.LogError("[LogSystem] LogSystem was Disabled!");
        if (SaveAuto == true)
        {
            SaveLogs();
        }
    }
    void SaveLogs()
    {
        // Save the Dictionary in a format which makes sense
    }
    public static void Log(GameObject v_Obj, string v_LogString)
    {
        string t_ObjName = v_Obj.name;
        LogSystem.Instance.LocalLog(t_ObjName, v_LogString);
    }
    public static void Log(string v_ObjName, string v_LogString)
    {
        LogSystem.Instance.LocalLog(v_ObjName, v_LogString);
    }
    void LocalLog(string v_ObjName, string v_LogString)
    {
        var ConstructorLog = new Log();
        if (ConstructorLog.Init(LogStore.Count, v_ObjName, v_LogString))
        {
            if (DebugLog == true)
            {
                Debug.Log("[" + v_ObjName + "] " + v_LogString);
            }
            Instance.LogStore.Add((Instance.LogStore.Count), ConstructorLog);
        } else
        {
            Debug.LogError("[LogSystem] Log Error from " + v_ObjName + " with the log of " + v_LogString);
        }
    }

}

public class Log
{
    public int LogNumber;
    public string ObjectName;
    public string LogString;

    public bool Init(int v_LogNum, string v_ObjName, string v_LogString)
    {
        LogNumber = v_LogNum;
        ObjectName = v_ObjName;
        LogString = v_LogString;
        return true;
    }
}
