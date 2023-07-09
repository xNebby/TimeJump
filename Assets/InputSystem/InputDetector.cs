using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputDetector : MonoBehaviour
{
    public PlayerInputActions m_PlayerInputActions;

    private Vector2 MovementVector;
    private Vector2 LightVector;

    private bool TappedSprint = false;



    void Awake()
    {
        // Create new instance of input sheet.
        m_PlayerInputActions = new PlayerInputActions();
        // Enable inputs from the Player class of the input sheet.
        m_PlayerInputActions.Player.Enable();


        m_PlayerInputActions.Player.Movement.performed += MovementPerformed;
        m_PlayerInputActions.Player.Movement.canceled += MovementCanceled;

        m_PlayerInputActions.Player.LightMove.performed += LightPerformed;
        m_PlayerInputActions.Player.LightMove.canceled += LightCanceled;

        m_PlayerInputActions.Player.InteractPrimary.performed += PrimaryInput;
        m_PlayerInputActions.Player.InteractSecondary.performed += SecondaryInput;

        m_PlayerInputActions.Player.SprintTap.started += SprintStarted;
        m_PlayerInputActions.Player.SprintTap.performed += SprintTapPerformed;
        m_PlayerInputActions.Player.SprintHold.canceled += SprintHoldCanceled;
        m_PlayerInputActions.Player.Jump.started += JumpStarted;
        m_PlayerInputActions.Player.Jump.canceled += JumpCanceled;
    }

    void LightPerformed(InputAction.CallbackContext context)
    {
        LightVector = m_PlayerInputActions.Player.LightMove.ReadValue<Vector2>();
        FireflyFollow.Instance.UpdateVector();
    }
    void LightCanceled(InputAction.CallbackContext context)
    {

    }

    void PrimaryInput(InputAction.CallbackContext context)
    {
        EventManager.TriggerEvent("ID_PrimaryInput");
    }
    void SecondaryInput(InputAction.CallbackContext context)
    {
        EventManager.TriggerEvent("ID_SecondaryInput");
    }
    //
    void MovementPerformed(InputAction.CallbackContext context)
    {
        MovementVector = m_PlayerInputActions.Player.Movement.ReadValue<Vector2>();
        InputManager.Instance.UpdateVector(MovementVector, "Player", 0);

        if (GameSettings.Instance.DownToCrouch == true)
        {
            if (MovementVector.y < 0)
            {
                if (PlayerStateManager.Instance.PlayerIsCrouching == false && PlayerStateManager.Instance.PlayerIsOnGround == true)
                {
                    // Crouch
                    EventManager.TriggerEvent("PC_CrouchCheck");
                }
            }
            else
            {
                if (PlayerStateManager.Instance.PlayerIsCrouching == true)
                {
                    // Uncrouch
                    //LogSystem.Log(gameObject, "ID 1 Is cause of uncrouch.");
                    EventManager.TriggerEvent("PC_UncrouchCheck");
                }
            }
        }
        
    }

    void MovementCanceled(InputAction.CallbackContext context)
    {
        if (PlayerStateManager.Instance.PlayerIsCrouching == true)
        {
            // Uncrouch
            //LogSystem.Log(gameObject, "ID 2 Is cause of uncrouch.");
            EventManager.TriggerEvent("PC_Uncrouch");
        }
        EventManager.TriggerEvent("IM_StopMoving");
        InputManager.Instance.RemoveVector("Player");
    }
    /*
    void CrouchStarted(InputAction.CallbackContext context)
    {
        // Crouch
        EventManager.TriggerEvent("PC_Crouch");
    }

    void CrouchStopped(InputAction.CallbackContext context)
    {
        // Uncrouch
        EventManager.TriggerEvent("PC_Uncrouch");
    }
    */

    void JumpStarted(InputAction.CallbackContext context)
    {
        InputManager.Instance.AddJumpTicket(1, "ID", true);
    }

    void JumpCanceled(InputAction.CallbackContext context)
    {
        LogSystem.Log(gameObject, "Jump cancelled in ID");
        InputManager.Instance.RemoveJumpTicket("ID");
    }

    //
    void SprintStarted(InputAction.CallbackContext context) 
    {
        EventManager.TriggerEvent("IM_StartSprint");
    }

    void SprintTapPerformed(InputAction.CallbackContext context)
    {
        if (GameSettings.Instance.ToggleSprint == false)
        {
            TappedSprint = false;
        } else
        {
            if (TappedSprint == false)
            {
                TappedSprint = true;
            }
            else if (TappedSprint == true)
            {
                TappedSprint = false;
            }
        }
    }

    void SprintHoldCanceled(InputAction.CallbackContext context)
    {
        if (TappedSprint == false)
        {
            EventManager.TriggerEvent("IM_StopSprint");
        }
    }
    //
}
