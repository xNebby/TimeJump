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
    public Transform FollowTarget, UpperCamera, LowerCamera;
    public Vector3 FollowVector;
    // Start is called before the first frame update
    void OnEnable()
    {
        EventManager.StartListening("CAM_UpdateFollow", UpdateFollow);
        EventManager.StartListening("CAM_PanUp", PanUp);
        EventManager.StartListening("CAM_PanDown", PanDown);
        EventManager.StartListening("CAM_PanCancel", PanCancel);

    }
    void OnDisable()
    {
        EventManager.StopListening("CAM_UpdateFollow", UpdateFollow);
        EventManager.StopListening("CAM_PanUp", PanUp);
        EventManager.StopListening("CAM_PanDown", PanDown);
        EventManager.StopListening("CAM_PanCancel", PanCancel);
    }
    void UpdateFollow()
    {
        FollowTarget = GameObject.FindWithTag("CameraAnchor").transform;
        if (CineLockOn)
        {
            VCam = GetComponent<CinemachineVirtualCamera>();
            //VCam.LookAt = FollowTarget;
            VCam.Follow = FollowTarget;
        } 
    }
    void PanUp()
    {
        if (CineLockOn) 
        {
            if (PlayerStateManager.Instance.PlayerIsAlive & PlayerStateManager.Instance.PlayerIsOnGround)
            {
                if (VCam == null)
                {
                    UpdateFollow();
                }
                if (UpperCamera == null)
                {
                    UpperCamera = GameObject.FindWithTag("UpperCamera").transform;
                }
                VCam.Follow = UpperCamera;
            }
        }
    }
    void PanDown()
    {
        if (CineLockOn)
        {
            if (PlayerStateManager.Instance.PlayerIsAlive & PlayerStateManager.Instance.PlayerIsOnGround)
            {
                if (VCam == null)
                {
                    UpdateFollow();
                }
                if (LowerCamera == null)
                {
                    LowerCamera = GameObject.FindWithTag("LowerCamera").transform;
                }
                VCam.Follow = LowerCamera;
            }
        }
    }
    void PanCancel()
    {
        if (CineLockOn)
        {
            if (VCam == null)
            {
                UpdateFollow();
            }
            VCam.Follow = FollowTarget;
        }
    }

    void Update()
    {
        if (PlayerStateManager.Instance.PlayerIsAlive == false || PlayerStateManager.Instance.PlayerIsOnGround == false)
        {
            PanCancel();
        }
        if (CineLockOn == false)
        {
            // TO ADD: a thing that makes it so the camera is set a certain distance away, which means that it doesnt instantly follow. 
            gameObject.transform.position = new Vector3(FollowTarget.position.x, FollowTarget.position.y, -10) + FollowVector;


        }
    }

}
