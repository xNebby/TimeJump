using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        //Debug.Log("OnEnable");
        EventManager.TriggerEvent("CAM_UpdateFollow");
    }
}
