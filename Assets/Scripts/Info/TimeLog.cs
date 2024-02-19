using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLog : MonoBehaviour
{
    private int TimerArraySize = 20;
    private float TimerResolution = 0.5f;
    public List<TimePos> Timeline;
    private float TimerTick = 0f;
    public bool PausedTime = false;
    public bool RewindTime = false;
    private Rigidbody2D RB;

    public void OnEnable()
    {
        RB = GetComponent<Rigidbody2D>();
        Timeline = new List<TimePos>();
    }

    public void FixedUpdate()
    {
        TimerTick += Time.fixedDeltaTime;
        if (TimerTick >= TimerResolution) {
            TimerTick = 0f;
            if (PausedTime || RewindTime)
            {
                if (RewindTime)
                {
                    if (Timeline.Count > 0)
                    {
                        // Add the velocity change from the timeline count position to the rigidbody
                        Debug.Log(Timeline.Count);
                        RB.isKinematic = true;
                        RB.velocity = Timeline[Timeline.Count - 1].LogPos - gameObject.transform.position;
                        Timeline.RemoveAt((Timeline.Count - 1));
                    } else if (Timeline.Count == 0)
                    {
                        // trigger the end of the rewind time visible effect
                        RewindTime = false;
                    }
                }
            }
            else
            {
                if (Timeline.Count > 0)
                {
                    if (!(Timeline[Timeline.Count - 1].LogPos == gameObject.transform.position))
                    {
                        RB.isKinematic = false;
                        // Add another timeslice to the timeline.  
                        if (Timeline.Count >= TimerArraySize)
                        {
                            Timeline.RemoveAt(0);
                        }
                        TimePos t_TimePos = new TimePos();
                        t_TimePos.Constructor(gameObject.transform.position, gameObject.transform.rotation);
                        Timeline.Add(t_TimePos);
                    }
                }
                else
                {

                    RB.isKinematic = false;
                    // Add another timeslice to the timeline.  
                    if (Timeline.Count >= TimerArraySize)
                    {
                        Timeline.RemoveAt(0);
                    }
                    TimePos t_TimePos = new TimePos();
                    t_TimePos.Constructor(gameObject.transform.position, gameObject.transform.rotation);
                    Timeline.Add(t_TimePos);
                }
            }
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