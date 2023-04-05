using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : SingletonClass<PlayerStateManager>
{
    [Header("Player Game status")]
    public bool PlayerIsAlive = true;
    public bool PlayerCanRespawn = true;
    [Space(10)]
    [Header("Physics States")]
    public bool PlayerIsCrouching = false;
    public bool PlayerIsJumping = false;
    public bool PlayerIsOnGround = false;
    public bool PlayerIsSprint = false;
    public bool PlayerIsTouchWall = false;
    public bool PlayerIsWallSlide = false;
    public bool PlayerIsOnRamp = false;
    [Space(10)]
    [Header("World States")]
    public float TimeScale = 1f;
    public Vector2 WorldGravity = Vector2.down;
    public Vector2 PlayerGravity = Vector2.down;

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

        EventManager.StartListening("CD_TouchRamp", OnRamp);
        EventManager.StartListening("CD_LeaveRamp", OffRamp);

        EventManager.StartListening("PM_KillPlayer", KillPlayer);
        EventManager.StartListening("PM_RespawnPlayer", RespawnPlayer);

        EventManager.StartListening("PM_CanRespawn", EnablePlayerRespawn);
        EventManager.StartListening("PM_CannotRespawn", DisablePlayerRespawn);
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

        EventManager.StopListening("CD_TouchRamp", OnRamp);
        EventManager.StopListening("CD_LeaveRamp", OffRamp);

        EventManager.StopListening("PM_KillPlayer", KillPlayer);
        EventManager.StopListening("PM_RespawnPlayer", RespawnPlayer);

        EventManager.StopListening("PM_CanRespawn", EnablePlayerRespawn);
        EventManager.StopListening("PM_CannotRespawn", DisablePlayerRespawn);
    }
    void KillPlayer()
    {
        PlayerIsAlive = false;
        DisablePhysicalStates();
    }
    void RespawnPlayer()
    {
        if (PlayerCanRespawn == true)
        {
            PlayerIsAlive = true;
        }
    }

    void EnablePlayerRespawn()
    {
        PlayerCanRespawn = true;
    }
    void DisablePlayerRespawn()
    {
        PlayerCanRespawn = false;
    }

    void OnRamp()
    {
        if (PlayerIsOnRamp == false)
        {
            PlayerIsOnRamp = true;
            GroundStateCheck();
        }
    }
    void OffRamp()
    {
        if (PlayerIsOnRamp)
        {
            PlayerIsOnRamp = false;
            GroundStateCheck();
        }
    }

    void WallSlideStart()
    {
        if (PlayerIsWallSlide == false)
        {
            PlayerIsWallSlide = true;
            WallStateCheck();
        }
    }
    void WallSlideEnd()
    {
        if (PlayerIsWallSlide)
        {
            PlayerIsWallSlide = false;
            WallStateCheck();
        }
    }

    void SprintTrue()
    {
        if (PlayerIsSprint == false)
        {
            PlayerIsSprint = true;
            SprintStateCheck();
        }
    }
    void SprintFalse()
    {
        if (PlayerIsSprint)
        {
            PlayerIsSprint = false;
            SprintStateCheck();
        }
    }

    void GroundTouched()
    {
        if (PlayerIsOnGround == false)
        {
            PlayerIsOnGround = true;
            GroundStateCheck();
        }
    }
    void GroundLeft()
    {
        if (PlayerIsOnGround == true)
        {
            PlayerIsOnGround = false;
            GroundStateCheck();
        }
    }

    void WallTouched()
    {
        if (PlayerIsTouchWall == false)
        {
            PlayerIsTouchWall = true;
            WallStateCheck();
        }
    }
    void WallLeft()
    {
        if (PlayerIsTouchWall)
        {
            PlayerIsTouchWall = false;
            WallStateCheck();
        }
    }

    void DisablePhysicalStates()
    {
        PlayerIsCrouching = false;
        PlayerIsJumping = false;
        PlayerIsOnGround = false;
        PlayerIsSprint = false;
        PlayerIsTouchWall = false;
        PlayerIsWallSlide = false;
        PlayerIsOnRamp = false;
        StateCheckAll();
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
