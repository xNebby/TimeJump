using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Start");
        EventManager.TriggerEvent("CAM_UpdateFollow");
    }
    void OnEnable()
    {
        //Debug.Log("OnEnable");
        EventManager.TriggerEvent("CAM_UpdateFollow");
    }
    void Awake()
    {
        //Debug.Log("Awake");
        EventManager.TriggerEvent("CAM_UpdateFollow");
    }

}
