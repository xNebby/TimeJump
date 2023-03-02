using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprint : MonoBehaviour
{
    public float SprintMoveMult = 1.5f;


    void OnEnable()
    {
        EventManager.StartListening("IM_StartSprint", SprintStart);
        EventManager.StartListening("IM_StopSprint", SprintStop);
    }

    void OnDisable()
    {
        EventManager.StopListening("IM_StartSprint", SprintStart);
        EventManager.StopListening("IM_StopSprint", SprintStop);
    }


    void SprintStart()
    {
        Debug.Log("added!");
        MovementStatusManager.Instance.AddMovementEffect("Sprint", SprintMoveMult);
    }

    void SprintStop()
    {
        Debug.Log("removed!");
        MovementStatusManager.Instance.RemoveEffect("Sprint");
    }
}
