using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : SingletonClass<TimeStop> 
{
    public bool CanStopTime;
    public bool TimeStopped;
    public float ObjectSpeed = 0.5f;

    private void OnEnable()
    {
        CanStopTime = true;
        EventManager.StartListening("ID_StopTime", StopTime);
        EventManager.StartListening("ID_RewindTime", RewindTime);
        EventManager.StartListening("ID_JumpTime", JumpTime);

    }
    private void OnDisable()
    {
        EventManager.StopListening("ID_StopTime", StopTime);
        EventManager.StopListening("ID_RewindTime", RewindTime);
        EventManager.StopListening("ID_JumpTime", JumpTime);
    }
    void StopTime()
    {
        // set the global time to 0, send out a event to tell all affected objects to pause physics updates. 
        // Objects that are paused should allow for the storing of time updates when they are being moved--
        // Any object that has the player colliding with them should update. 
        // Objects that are touching the player should move at a different speed to normal to show theyre still paused.
        if (CanStopTime)
        {
            if (TimeStopped)
            {
                EventManager.TriggerEvent("TS_ResumeTime");
            } else
            {
                EventManager.TriggerEvent("TS_StopTime");
            }
        }

    }
    void RewindTime()
    {

    }
    void JumpTime()
    {

    }
}
