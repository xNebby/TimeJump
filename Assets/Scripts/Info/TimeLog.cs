using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLog : MonoBehaviour
{
    static int TimerArraySize = 15;
    public int CurrentIndex = 0;
    public List<TimePos> Timeline;
    private float TimerTick = 0f;

    public void FixedUpdate()
    {
        TimerTick += Time.fixedDeltaTime;
        if (TimerTick >= 1f;) {
            // Add another timeslice to the timeline.
        }
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