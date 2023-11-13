using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    private Rigidbody2D PlayerRB;
    public bool ChargingJump;
    public bool ChargedJump;
    public float CrouchHeight;
    public float PlayerScale;
    public float PlayerWidth;
    public float ChargeTime = 2f;

    private bool Grow = false;

    void OnEnable()
    {
        EventManager.StartListening("PC_Crouch", PlayerCrouching);
        EventManager.StartListening("PC_Uncrouch", PlayerUncrouching);
        EventManager.StartListening("PC_CrouchCheck", PlayerCheck);
        EventManager.StartListening("PC_UncrouchCheck", PlayerUnCheck);
        PlayerRB = GetComponent<Rigidbody2D>();
        CrouchHeight = 0.5f;
        PlayerScale = 1f;
        PlayerWidth = 1f;
        Grow = false;
    }
    void OnDisable()
    {
        EventManager.StopListening("PC_Crouch", PlayerCrouching);
        EventManager.StopListening("PC_Uncrouch", PlayerUncrouching);
        EventManager.StopListening("PC_CrouchCheck", PlayerCheck);
        EventManager.StopListening("PC_UncrouchCheck", PlayerUnCheck);
    }

    void PlayerCheck()
    {
        if (PlayerStateManager.Instance.PlayerIsSprint == true)
        {
            EventManager.TriggerEvent("IM_StopSprint");
        }

        EventManager.TriggerEvent("PC_Crouch");
    }

    void PlayerUnCheck()
    {
        EventManager.TriggerEvent("PC_Uncrouch");
    }

    void PlayerCrouching()
    {

        //LogSystem.Log(gameObject, "Crouch Event received");
        gameObject.transform.localScale = new Vector3(PlayerWidth, CrouchHeight * PlayerScale, 1f);
        PlayerRB.MovePosition(PlayerRB.position + (PlayerStateManager.Instance.PlayerGravity * (PlayerScale * CrouchHeight / 2)));

        if (InputManager.Instance.IM_PlayerVector.x == 0)
        {
            LogSystem.Log(gameObject, "Adding Charge");
            // Crouch jump. The player should be crouched for 3 seconds.
            ChargingJump = true;
            TimerManager.AddTimer("PC_CrouchCharge", ChargeTime, CrouchJumpCharged);
        } 
    }
    void PlayerUncrouching()
    {

        LogSystem.Log(gameObject, "UnCrouch Event received");
        PlayerRB.MovePosition(PlayerRB.position - (PlayerStateManager.Instance.PlayerGravity * (PlayerScale * CrouchHeight / 2)));
        Grow = true;
        if (ChargingJump)
        {
            ChargingJump = false;
            TimerManager.RemoveTimer("PC_CrouchCharge", CrouchJumpCharged);
        }
        if (ChargedJump)
        {
            ChargedJump = false;
        }
    }
    void CrouchJumpCharged()
    {
        ChargedJump = true;
        ChargingJump = false;
        LogSystem.Log(gameObject, "Charged Jump.");
        EventManager.TriggerEvent("PC_CrouchJumpCharged");
    }
    void FixedUpdate()
    {
        if (PlayerStateManager.Instance.PlayerIsCrouching == true)
        {
            if (InputManager.Instance.IM_PlayerVector.x == 0 && ChargingJump == false && ChargedJump == false)
            {
                LogSystem.Log(gameObject, "Adding Charge");
                // Crouch jump. The player should be crouched for 3 seconds.
                ChargingJump = true;
                TimerManager.AddTimer("PC_CrouchCharge", 3, CrouchJumpCharged);
            } 
            else if (!(InputManager.Instance.IM_PlayerVector.x == 0))
            {
                if (ChargingJump)
                {
                    ChargingJump = false;
                    LogSystem.Log(gameObject, "Cancelled Charge");
                    Debug.Log(InputManager.Instance.IM_PlayerVector.x);
                    Debug.Log(ChargingJump);
                    Debug.Log(ChargedJump);
                    TimerManager.RemoveTimer("PC_CrouchCharge", CrouchJumpCharged);
                }
                if (ChargedJump)
                {
                    ChargedJump = false;
                }
            }
            
        }
        if (Grow)
        {
            Grow = false;
            gameObject.transform.localScale = new Vector3(PlayerWidth, PlayerScale, 1f);
        }
    }
}
