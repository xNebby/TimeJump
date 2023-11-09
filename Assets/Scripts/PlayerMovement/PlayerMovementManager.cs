using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementManager : MonoBehaviour
{
    public Vector2 IM_PlayerVector = Vector2.zero;
    public Vector2 MSM_StatusVector = Vector2.zero;
    public float MSM_PlayerVectorMultiplier = 1f;
    public Vector2 ParallelVector;
    public Vector2 PlayerNormal;
    public Vector2 FinalVector;
    public float PlayerMovementMultiplier = 4f;
    public float SprintMultiplier = 1.5f;
    public Rigidbody2D PlayerRB;
    public Vector2 ResultantVector;

    [Space(10)]
    [Header("Gravity Values")]
    public bool EnableGravity = true;
    [Space(5)]
    public float WorldGravityScale = 10f;
    public float PlayerGravityScale = 10f;
    public float CurrentGravityMult = 0f;
    public float TerminalMult = 40f;
    public bool Coyote = false;

    public Vector2 JumpVector;

    void OnEnable()
    {
        MSM_StatusVector = Vector2.zero;
        IM_PlayerVector = Vector2.zero;
        MSM_PlayerVectorMultiplier = 1f;
        ParallelVector = Vector2.zero;
        PlayerNormal = Vector2.zero;
        FinalVector = Vector2.zero;
        JumpVector = Vector2.zero;
        PlayerMovementMultiplier = 4f;
        PlayerRB = GetComponent<Rigidbody2D>();

        EventManager.StartListening("PM_RespawnPlayer", RespawnActions);
        EventManager.StartListening("PJ_CoyoteTimeStart", delegate { CoyoteTime(true); });
        EventManager.StartListening("PJ_CoyoteTimeEnd", delegate { CoyoteTime(false); });
    }
    void OnDisable()
    {
        EventManager.StopListening("PM_RespawnPlayer", RespawnActions);
        EventManager.StopListening("PJ_CoyoteTimeStart", delegate { CoyoteTime(true); });
        EventManager.StopListening("PJ_CoyoteTimeEnd", delegate { CoyoteTime(false); });
    }


    public void IM_UpdateVelocity(Vector2 v_Velocity)
    {
        //Debug.Log(v_Velocity);
        IM_PlayerVector = v_Velocity;
    }
    public void MSM_UpdateVelocity(Vector2 v_Velocity)
    {
        MSM_StatusVector = v_Velocity;
    }
    public void MSM_UpdateMultiplier(float v_Multiplier)
    {
        MSM_PlayerVectorMultiplier = v_Multiplier;
    }

    private Vector2 GravityVector()
    {
        CurrentGravityMult += PlayerGravityScale * Time.fixedDeltaTime;
        return (PlayerStateManager.Instance.PlayerGravity * CurrentGravityMult);
    }


    public void AddJumpVector(Vector2 v_JumpVector)
    {
        JumpVector = v_JumpVector;
    }

    void CoyoteTime(bool Var)
    {
        Coyote = Var;
    }

    private void RespawnActions()
    {
        CurrentGravityMult = 0;
    }

    void FixedUpdate()
    {
        FinalVector = (IM_PlayerVector * MSM_PlayerVectorMultiplier * PlayerMovementMultiplier) + MSM_StatusVector;
        PlayerNormal = Vector2Extensions.rotateDeg(PlayerStateManager.Instance.PlayerGravity, PlayerRB.rotation);
        ParallelVector = (Vector2.Perpendicular(PlayerNormal) * FinalVector.x);
        
        //CollisionDetection.Instance.CheckMovement(gameObject, PlayerManager.Instance.PlayerRB, FinalVector, CollisionDetection.Instance.PlayerGravity, ParallelVector);
        CollisionDetection.Instance.CheckCollision(ParallelVector);
        //FireflyFollow.Instance.PlayerMove(ParallelVector * PlayerStateManager.Instance.TimeScale);

        if (Coyote == true & PlayerStateManager.Instance.PlayerIsOnGround == false)
        {
            CurrentGravityMult = 0;
        }
        if (PlayerStateManager.Instance.PlayerIsOnGround == false)
        {
            ResultantVector = (ParallelVector + JumpVector + GravityVector()) * PlayerStateManager.Instance.TimeScale; 
        } else {
            ResultantVector = (ParallelVector + JumpVector) * PlayerStateManager.Instance.TimeScale;
            CurrentGravityMult = 0;
        }

        PlayerRB.velocity = (ResultantVector);

    }

}
