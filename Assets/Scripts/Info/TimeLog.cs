using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLog : MonoBehaviour
{
    static int TimerArraySize = 50;
    private float Timer;
    private Dictionary<int, TimePos> LogArray;
    private int CurrentInt;
    private int LowestInt;
    
    void OnEnable()
    {
        Timer = 0;
        CurrentInt = 0;
        LowestInt = 0;
        LogArray = new Dictionary<int, TimePos>();
    }

    void FixedUpdate()
    {
        Timer += Time.fixedDeltaTime;
        if (Timer >= (1 / PlayerStateManager.Instance.TimeScale))
        {
            if ((gameObject.transform.position == LogArray[CurrentInt].LogPos) & (gameObject.transform.rotation == LogArray[CurrentInt].LogRot))
            {
                LogSystem.Log(gameObject, "Rejecting Timelog: hasnt moved.");
            } else
            {
                LogSystem.Log(gameObject, "New time log added.");
                TimePos TempLog = new TimePos();
                TempLog.Constructor(gameObject.transform.position, gameObject.transform.rotation);
                Timer = 0;
                CurrentInt++;
                LogArray.Add(CurrentInt, TempLog);
                if (CurrentInt - LowestInt >= TimerArraySize)
                {
                    LogArray.Remove(LowestInt);
                    LowestInt++;
                }
            }
        }
    }

    TimePos GrabPos()
    {
        TimePos CurrentLog = LogArray[CurrentInt];
        LogArray.Remove(CurrentInt);
        CurrentInt--;
        if (CurrentInt == LowestInt)
        {
            CurrentInt = 0;
            LowestInt = 0;
        }
        return CurrentLog;
    }

}

public class TimePos
{
    public Vector3 LogPos;
    public Quaternion LogRot;
    public void Constructor(Vector3 v_Position, Quaternion v_Rotation) 
    {
        LogPos = v_Position;
        LogRot = v_Rotation;
    }
}