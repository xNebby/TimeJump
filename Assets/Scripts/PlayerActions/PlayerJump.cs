using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Space(10)]
    [Header("Jump Values")]
    //public float JumpSpeed = 1f;
    public float JumpHeight = 1.5f;
    public float NormalJumpHeight = 1f;
    public float CrouchJumpHeight = 1.5f;
    public float JumpAirConstant = 600f;
    public float JumpInitialConstant = 800f;
    public float JumpConstant = 0f;
    public float JumpTime = 0.4f;
    public float CrouchJumpTime = 0.5f;
    public float NormalJumpTime = 0.2f;
    public float CoyoteTime = 0.15f;
    public float PeakMult = 1.1f;
    public Rigidbody2D RB;
    public PlayerMovementManager m_PMM;
    public bool TimerActive = false;
    public bool Coyote = false;
    public bool BounceBack = false;
    public bool Jumping = false;
    public bool CanJumpDied = true;

    // Start is called before the first frame update
    void OnEnable()
    {
        RB = GetComponent<Rigidbody2D>();
        m_PMM = GetComponent<PlayerMovementManager>();
        EventManager.StartListening("IM_StartJump", HoldingJump);
        EventManager.StartListening("IM_StopJump", ReleaseJumpButton);

        EventManager.StartListening("PC_CrouchJumpCharged", CrouchJumpCharged);
        EventManager.StartListening("PC_CrouchJumpCancelled", CrouchJumpCancelled);

        EventManager.StartListening("PM_KillPlayer", DiedForceStop);
        EventManager.StartListening("PM_RespawnPlayer", RemoveDiedForceStop);
    }
    void OnDisable()
    {
        EventManager.StopListening("IM_StartJump", HoldingJump);
        EventManager.StopListening("IM_StopJump", ReleaseJumpButton);

        EventManager.StopListening("PC_CrouchJumpCharged", CrouchJumpCharged);
        EventManager.StopListening("PC_CrouchJumpCancelled", CrouchJumpCancelled);

        EventManager.StopListening("PM_KillPlayer", DiedForceStop);
        EventManager.StopListening("PM_RespawnPlayer", RemoveDiedForceStop);
    }

    void HoldingJump()
    {
        if (CanJumpDied)
        {
            // if is able to jump, jump
            // if player is on the ground, there can be either a crouch jump upwards, or a normal jump (up or diagonally)
            LogSystem.Log(gameObject, "Jump command received by PJ module.");
            if (PlayerStateManager.Instance.PlayerIsOnGround)
            {
                TimerManager.AddTimer("PJ_JumpStarted", JumpTime, ReleaseJumpTimer);
                TimerActive = true;
                Jumping = true;
                EventManager.TriggerEvent("PJ_JumpStarted");
            }
            else
            {
                LogSystem.Log(gameObject, "Jump cannot start- not on ground.");
            }
        }
    }

    void DiedForceStop()
    {
        ReleaseJumpButton();
        CanJumpDied = false;
    }
    void RemoveDiedForceStop()
    {
        CanJumpDied = true;
    }

    void ReleaseJumpButton()
    {
        if (Coyote == false & Jumping == true)
        {
            LogSystem.Log(gameObject, "Jump stopped via button.");//
            ReleaseJump();
        }
    }
    void ReleaseJumpTimer()
    {
        TimerActive = false;
        if (Coyote == false & Jumping == true)
        {
            LogSystem.Log(gameObject, "Jump stopped via time.");
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
                EventManager.TriggerEvent("PJ_CoyoteTimeStart");
                TimerManager.AddTimer("PJ_CoyoteTime", CoyoteTime, CoyoteRelease);
                MovementStatusManager.Instance.AddTimedMovementEffect("Coyote", PeakMult, CoyoteTime);
                Coyote = true;
                //LogSystem.Log(gameObject, "Coyote timer added.");//
            }
            Jumping = false;
            m_PMM.AddJumpVector(Vector2.zero);
        }
    }

    void CoyoteRelease()
    {
        EventManager.TriggerEvent("PJ_CoyoteTimeEnd");
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
                    EventManager.TriggerEvent("PJ_CoyoteTimeEnd");
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
                m_PMM.AddJumpVector(JumpVector);
                //Debug.Log(JumpVector);
                //RB.AddForce(JumpVector, ForceMode2D.Impulse);
                //RB.AddForce(JumpVector);
                //Debug.Log(JumpVector);
                //RB.velocity += JumpVector;
                
            }
            if (Jumping == false)
            {
                m_PMM.AddJumpVector(Vector2.zero);
            }
        }
        
    }
}
 