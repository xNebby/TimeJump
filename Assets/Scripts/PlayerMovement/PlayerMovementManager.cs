using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementManager : SingletonClass<PlayerMovementManager>
{
    public Vector2 IM_PlayerVector = Vector2.zero;
    public Vector2 PSM_StatusVector = Vector2.zero;
    public float PSM_PlayerVectorMultiplier = 1f;
    public Vector2 ParallelVector;
    public Vector2 FinalVector;
    public Rigidbody2D RB;
    public float PlayerMovementMultiplier = 4f;

    public override void Awake()
    {
        PSM_StatusVector = Vector2.zero;
        IM_PlayerVector = Vector2.zero;
        PSM_PlayerVectorMultiplier = 1f;
        ParallelVector = Vector2.zero;
        FinalVector = Vector2.zero;
        RB = gameObject.GetComponent<Rigidbody2D>();
        PlayerMovementMultiplier = 4f;
        base.Awake();
    }

    public void IM_UpdateVelocity(Vector2 v_Velocity)
    {
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
        ParallelVector = (Vector2.Perpendicular(GravityManager.Instance.PlayerGravity) * FinalVector.x);
        //CollisionDetection.Instance.CheckMovement(gameObject, RB, FinalVector, GravityManager.Instance.PlayerGravity, ParallelVector);
        RB.velocity = (ParallelVector * PlayerStateManager.Instance.TimeScale);
    
    
    }

}
