using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : SingletonClass<GravityManager>
{
    public Vector2 WorldGravity = Vector2.down;
    public Vector2 PlayerGravity = Vector2.down;
    public float WorldGravityScale = 10f;
    public float PlayerGravityScale = 10f;
    public float CurrentGravityMult = 0f;
    public float TerminalMult = 60f;

    private Rigidbody2D PlayerRB;

    void Start()
    {
        PlayerRB = PlayerMovementManager.Instance.RB;
    }
    void OnEnable()
    {
        PlayerRB = PlayerMovementManager.Instance.RB;
    }

    void ApplyGravity()
    {
        if (PlayerStateManager.Instance.PlayerIsOnGround == false & PlayerStateManager.Instance.PlayerIsOnRamp == false & PlayerStateManager.Instance.PlayerIsWallSlide == false)
        {
            if (CurrentGravityMult < TerminalMult)
            {
                CurrentGravityMult += PlayerGravityScale * Time.fixedDeltaTime;
            } else 
            {
                CurrentGravityMult = TerminalMult;
            }
            PlayerRB.velocity += (PlayerGravity * CurrentGravityMult);
        } else
        {
            CurrentGravityMult = PlayerGravityScale * Time.fixedDeltaTime;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ApplyGravity();
    }
}
