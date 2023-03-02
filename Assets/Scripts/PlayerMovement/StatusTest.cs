using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusTest : MonoBehaviour
{
    public string Name = "test";
    public bool velocity = false;
    public bool multiplier = false;
    public bool timer = false;
    public Vector2 velocityVar = Vector2.zero;
    public float multiplierVar = 0f;
    public int timevar = 100;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("h"))
        {
            MovementStatusManager.Instance.AddNamedEffect(Name);
            if (velocity == true)
            {
                MovementStatusManager.Instance.AddVelocityEffect(Name, velocityVar);
            }
            if (multiplier == true)
            {
                MovementStatusManager.Instance.AddMultiplierEffect(Name, multiplierVar);
            }
            if (timer == true)
            {
                MovementStatusManager.Instance.AddTimer(Name, timevar);
            }
        }

    }
}
