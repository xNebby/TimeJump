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

    public void Awake()
    {
        MSM_StatusVector = Vector2.zero;
        IM_PlayerVector = Vector2.zero;
        MSM_PlayerVectorMultiplier = 1f;
        ParallelVector = Vector2.zero;
        PlayerNormal = Vector2.zero;
        FinalVector = Vector2.zero;
        PlayerMovementMultiplier = 4f;
        PlayerRB = GetComponent<Rigidbody2D>();
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

    

    void FixedUpdate()
    {
        FinalVector = (IM_PlayerVector * MSM_PlayerVectorMultiplier * PlayerMovementMultiplier) + MSM_StatusVector;
        PlayerNormal = Vector2Extensions.rotateDeg(PlayerStateManager.Instance.PlayerGravity, PlayerRB.rotation);
        ParallelVector = (Vector2.Perpendicular(PlayerNormal) * FinalVector.x);

        //CollisionDetection.Instance.CheckMovement(gameObject, PlayerManager.Instance.PlayerRB, FinalVector, CollisionDetection.Instance.PlayerGravity, ParallelVector);
        CollisionDetection.Instance.CheckCollision(ParallelVector);
        //FireflyFollow.Instance.PlayerMove(ParallelVector * PlayerStateManager.Instance.TimeScale);
        PlayerRB.velocity = (ParallelVector * PlayerStateManager.Instance.TimeScale);
    
    
    }

}
