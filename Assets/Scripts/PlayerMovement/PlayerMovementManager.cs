using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementManager : SingletonClass<PlayerMovementManager>
{
    public Vector2 IM_PlayerVector = Vector2.zero;
    public Vector2 PSM_StatusVector = Vector2.zero;
    public float PSM_PlayerVectorMultiplier = 1f;
    public Vector2 ParallelVector;
    public Vector2 PlayerNormal;
    public Vector2 FinalVector;
    public float PlayerMovementMultiplier = 4f;
    public float SprintMultiplier = 1.5f;

    public override void Awake()
    {
        PSM_StatusVector = Vector2.zero;
        IM_PlayerVector = Vector2.zero;
        PSM_PlayerVectorMultiplier = 1f;
        ParallelVector = Vector2.zero;
        PlayerNormal = Vector2.zero;
        FinalVector = Vector2.zero;
        PlayerMovementMultiplier = 4f;
        base.Awake();
    }
    public void IM_UpdateVelocity(Vector2 v_Velocity)
    {
        //Debug.Log(v_Velocity);
        IM_PlayerVector = v_Velocity;
    }

    public void PSM_UpdateVelocity(Vector2 v_Velocity)
    {
        PSM_StatusVector = v_Velocity;
    }
    public void PSM_UpdateMultiplier(float v_Multiplier)
    {
        PSM_PlayerVectorMultiplier = v_Multiplier;
    }

    

    void FixedUpdate()
    {
        FinalVector = (IM_PlayerVector * PSM_PlayerVectorMultiplier * PlayerMovementMultiplier) + PSM_StatusVector;
        PlayerNormal = Vector2Extensions.rotateDeg(PlayerStateManager.Instance.PlayerGravity, PlayerInfo.Instance.PlayerRigidbody.rotation);
        ParallelVector = (Vector2.Perpendicular(PlayerNormal) * FinalVector.x);

        //CollisionDetection.Instance.CheckMovement(gameObject, PlayerInfo.Instance.PlayerRigidbody, FinalVector, CollisionDetection.Instance.PlayerGravity, ParallelVector);
        CollisionDetection.Instance.CheckCollision(ParallelVector);
        PlayerInfo.Instance.PlayerRigidbody.velocity = (ParallelVector * PlayerStateManager.Instance.TimeScale);
    
    
    }

}
