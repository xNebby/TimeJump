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

    void OnEnable()
    {
        EventManager.StartListening("PC_Crouch", PlayerCrouching);
        EventManager.StartListening("PC_Uncrouch", PlayerUncrouching);
        PlayerRB = GetComponent<Rigidbody2D>();
        CrouchHeight = 0.5f;
        PlayerScale = 1f;
    }
    void OnDisable()
    {
        EventManager.StopListening("PC_Crouch", PlayerCrouching);
        EventManager.StopListening("PC_Uncrouch", PlayerUncrouching);
    }

    void PlayerCrouching()
    {
        //LogSystem.Log(gameObject, "Crouch Event received");
        gameObject.transform.localScale = new Vector3(PlayerScale, CrouchHeight, PlayerScale);
        PlayerRB.MovePosition(PlayerRB.position + (PlayerStateManager.Instance.PlayerGravity * CrouchHeight / 2));

        if (InputManager.Instance.IM_PlayerVector.x == 0)
        {
            LogSystem.Log(gameObject, "Adding Charge");
            // Crouch jump. The player should be crouched for 3 seconds.
            ChargingJump = true;
            TimerManager.AddTimer("PJ_CrouchJump", 3, CrouchJumpCharged);
        } 
    }
    void PlayerUncrouching()
    {
        //LogSystem.Log(gameObject, "UnCrouch Event received");
        PlayerRB.MovePosition(PlayerRB.position - (PlayerStateManager.Instance.PlayerGravity * CrouchHeight / 2));
        gameObject.transform.localScale = new Vector3(PlayerScale, PlayerScale, PlayerScale);
        if (ChargingJump)
        {
            ChargingJump = false;
            TimerManager.RemoveTimer("PJ_CrouchJump", CrouchJumpCharged);
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
            if (InputManager.Instance.IM_PlayerVector.x == 0 && ChargingJump == false)
            {
                LogSystem.Log(gameObject, "Adding Charge");
                // Crouch jump. The player should be crouched for 3 seconds.
                ChargingJump = true;
                TimerManager.AddTimer("PJ_CrouchJump", 3, CrouchJumpCharged);
            } 
            else 
            {
                if (ChargingJump)
                {
                    ChargingJump = false;
                    LogSystem.Log(gameObject, "Cancelled Charge");
                    TimerManager.RemoveTimer("PC_CrouchJump", CrouchJumpCharged);
                }
                if (ChargedJump)
                {
                    ChargingJump = false;
                }
            }
        }
    }
}
