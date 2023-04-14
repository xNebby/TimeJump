using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : SingletonClass<PlayerStateManager>
{

    public bool PlayerIsCrouching = false;
    public bool PlayerIsJumping = false;
    public bool PlayerIsOnGround = false;
    public bool PlayerIsSprint = false;
    public bool PlayerIsTouchWall = false;
    public bool PlayerIsWallSlide = false;
    public bool PlayerIsOnRamp = false;
    public float TimeScale = 1f;

    void OnEnable()
    {
        EventManager.StartListening("IM_StartSprint", SprintTrue);
        EventManager.StartListening("IM_StopSprint", SprintFalse);

        EventManager.StartListening("CD_TouchGround", GroundTouched);
        EventManager.StartListening("CD_LeaveGround", GroundLeft);

        EventManager.StartListening("CD_TouchWall", WallTouched);
        EventManager.StartListening("CD_LeaveWall", WallLeft);

        EventManager.StartListening("CD_StartWSlide", WallSlideStart);
        EventManager.StartListening("CD_StopWSlide", WallSlideEnd);

        EventManager.StartListening("CD_OnRamp", OnRamp);
        EventManager.StartListening("CD_OffRamp", OffRamp);
    }
    void OnDisable()
    {
        EventManager.StopListening("IM_StartSprint", SprintTrue);
        EventManager.StopListening("IM_StopSprint", SprintFalse);

        EventManager.StopListening("CD_TouchGround", GroundTouched);
        EventManager.StopListening("CD_LeaveGround", GroundLeft);

        EventManager.StopListening("CD_TouchWall", WallTouched);
        EventManager.StopListening("CD_LeaveWall", WallLeft);

        EventManager.StopListening("CD_StartWSlide", WallSlideStart);
        EventManager.StopListening("CD_StopWSlide", WallSlideEnd);

        EventManager.StopListening("CD_OnRamp", OnRamp);
        EventManager.StopListening("CD_OffRamp", OffRamp);
    }

    void OnRamp()
    {
        PlayerIsOnRamp = true;
        GroundStateCheck();
    }
    void OffRamp()
    {
        PlayerIsOnRamp = false;
        GroundStateCheck();
    }

    void WallSlideStart()
    {
        PlayerIsWallSlide = true;
        WallStateCheck();
    }
    void WallSlideEnd()
    {
        PlayerIsWallSlide = false;
        WallStateCheck();
    }

    void SprintTrue()
    {
        PlayerIsSprint = true;
        SprintStateCheck();
    }
    void SprintFalse()
    {
        PlayerIsSprint = false;
        SprintStateCheck();
    }

    void GroundTouched()
    {
        PlayerIsOnGround = true;
        GroundStateCheck();
    }
    void GroundLeft()
    {
        PlayerIsOnGround = false;
        GroundStateCheck();
    }

    void WallTouched()
    {
        PlayerIsTouchWall = true;
        WallStateCheck();
    }
    void WallLeft()
    {
        PlayerIsTouchWall = false;
        WallStateCheck();
    }

    public void StateCheckAll()
    {
        GroundStateCheck();
        WallStateCheck();
        SprintStateCheck();
    }
    public void GroundStateCheck()
    {
        if (PlayerIsOnGround == false)
        {
            PlayerIsCrouching = false;
        } else
        {
            PlayerIsOnRamp = false;
        }
            
    }
    public void SprintStateCheck()
    {
        if (PlayerIsSprint)
        {
            PlayerIsCrouching = false;
        }
    }
    public void WallStateCheck()
    {   
        if (PlayerIsWallSlide)
        {
            PlayerIsTouchWall = true;
        }
        if (PlayerIsTouchWall)
        {
            PlayerIsJumping = false;
        }
    }

}
