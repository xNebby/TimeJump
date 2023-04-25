using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SetFollowTarget : MonoBehaviour
{
    [Header("Use Cinemachines Lock on")]
    public bool CineLockOn = false;
    [Space(10)]
    [Header("Cam Properties")]
    public CinemachineVirtualCamera VCam;
    public Transform FollowTarget;
    public Vector3 FollowVector;
    // Start is called before the first frame update
    void OnEnable()
    {
        EventManager.StartListening("CAM_UpdateFollow", UpdateFollow);
    }
    void OnDisable()
    {
        EventManager.StopListening("CAM_UpdateFollow", UpdateFollow);
    }
    void UpdateFollow()
    {
        FollowTarget = GameObject.FindWithTag("CameraAnchor").transform;
        if (CineLockOn)
        {
            VCam = GetComponent<CinemachineVirtualCamera>();
            VCam.LookAt = FollowTarget;
            VCam.Follow = FollowTarget;
        } 
    }

    void Update()
    {
        if (CineLockOn == false)
        {
            // TO ADD: a thing that makes it so the camera is set a certain distance away, which means that it doesnt instantly follow. 
            gameObject.transform.position = new Vector3(FollowTarget.position.x, FollowTarget.position.y, -10) + FollowVector;


        }
    }

}
