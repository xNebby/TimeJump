using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Space(10)]
    [Header("Jump Values")]
    //private float JumpSpeed = 1f;
    private float JumpHeight = 1.5f;
    private float NormalJumpHeight = 1f;
    private float CrouchJumpHeight = 1.5f;
    private float JumpAirConstant = 600f;
    private float JumpInitialConstant = 800f;
    public float JumpConstant = 0f;
    private float JumpTime = 0.4f;
    private float CrouchJumpTime = 0.5f;
    private float NormalJumpTime = 0.2f;
    private float CoyoteTime = 0.15f;
    private Rigidbody2D RB;
    public bool TimerActive = false;
    public bool Coyote = false;
    public bool BounceBack = false;

    // Start is called before the first frame update
    void OnEnable()
    {
        RB = GetComponent<Rigidbody2D>();
        EventManager.StartListening("IM_StartJump", HoldingJump);
        EventManager.StartListening("IM_StopJump", ReleaseJumpButton);

        EventManager.StartListening("PC_CrouchJumpCharged", CrouchJumpCharged);
        EventManager.StartListening("PC_CrouchJumpCancelled", CrouchJumpCancelled);
    }
    void OnDisable()
    {
        EventManager.StopListening("IM_StartJump", HoldingJump);
        EventManager.StopListening("IM_StopJump", ReleaseJumpButton);

        EventManager.StopListening("PC_CrouchJumpCharged", CrouchJumpCharged);
        EventManager.StopListening("PC_CrouchJumpCancelled", CrouchJumpCancelled);
    }

    void HoldingJump()
    {
        // if is able to jump, jump
        // if player is on the ground, there can be either a crouch jump upwards, or a normal jump (up or diagonally)
        LogSystem.Log(gameObject, "Jump command received by PJ module.");
        if (PlayerStateManager.Instance.PlayerIsOnGround)
        {
            TimerManager.AddTimer("PJ_JumpStarted", JumpTime, ReleaseJumpTimer);
            TimerActive = true;
            EventManager.TriggerEvent("PJ_JumpStarted");
        } else
        {
            LogSystem.Log(gameObject, "Jump cannot start- not on ground.");
        }
    }

    void ReleaseJumpButton()
    {
        if (Coyote == false)
        {
            if (TimerActive == true)
            {
                TimerManager.RemoveTimer("PJ_JumpStarted", ReleaseJumpTimer);
                TimerActive = false;
            }
            LogSystem.Log(gameObject, "Jump stopped via button.");//
            ReleaseJump();
        }
    }
    void ReleaseJumpTimer()
    {
        if (Coyote == false)
        {
            LogSystem.Log(gameObject, "Jump stopped via time.");
            TimerActive = false;
            ReleaseJump();
        }
    }
    void ReleaseJump()
    {
        if (Coyote == false)
        {
            CrouchJumpCancelled();
            if (PlayerStateManager.Instance.PlayerIsOnGround)
            {
            }
            else
            {
                TimerManager.AddTimer("PJ_CoyoteTime", CoyoteTime, CoyoteRelease);
                Coyote = true;
                LogSystem.Log(gameObject, "Coyote timer added.");//
            }
        }
    }

    void CoyoteRelease()
    {
        LogSystem.Log(gameObject, "Coyote time is finished."); 
        Coyote = false;
        EventManager.TriggerEvent("PJ_JumpStopped");
    }

    void CrouchJumpCharged()
    {
        JumpHeight = CrouchJumpHeight;
        JumpTime = CrouchJumpTime;
    }
    void CrouchJumpCancelled()
    {
        JumpHeight = NormalJumpHeight;
        JumpTime = NormalJumpTime; 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(PlayerStateManager.Instance.PlayerIsJumping)
        {
            if (PlayerStateManager.Instance.PlayerIsOnGround)
            {

                if (Coyote)
                {
                    LogSystem.Log(gameObject, "coyote time ENDING pranked");
                    TimerManager.RemoveTimer("PJ_CoyoteTime", CoyoteRelease);
                    Coyote = false;
                    EventManager.TriggerEvent("PJ_JumpStopped");
                }
                else
                {
                    /*
                    if (TimerActive == true)
                    {
                        if (BounceBack == false)
                        {
                            BounceBack = true;
                        } else
                        {
                            BounceBack = false;
                            LogSystem.Log(gameObject, "JUMP touched ground from jumping");

                            TimerManager.RemoveTimer("PJ_JumpStarted", ReleaseJumpTimer);
                            TimerActive = false;
                            EventManager.TriggerEvent("PJ_JumpStopped");
                        }
                    }
                    */
                }
                JumpConstant = JumpInitialConstant;

            }
            else
            {
                if (Coyote)
                {
                    JumpConstant = CollisionDetection.Instance.GravityForceConstant;
                }
                else
                {
                    JumpConstant = JumpAirConstant;
                }
            }

            if (PlayerStateManager.Instance.PlayerDashing == false)
            {
                //Debug.Log(JumpConstant);
                Vector2 JumpVector = (PlayerStateManager.Instance.PlayerGravity) * -1 * JumpHeight * JumpConstant;
                //Debug.Log(JumpVector);
                //RB.AddForce(JumpVector, ForceMode2D.Impulse);
                RB.AddForce(JumpVector);
                //Debug.Log(JumpVector);
                //RB.velocity += JumpVector;
            }
        }
        
    }
}
 