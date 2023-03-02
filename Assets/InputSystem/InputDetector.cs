using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputDetector : MonoBehaviour
{
    public PlayerInputActions m_PlayerInputActions;

    private Vector2 MovementVector;

    private bool TappedSprint = false;


    void Awake()
    {
        // Create new instance of input sheet.
        m_PlayerInputActions = new PlayerInputActions();
        // Enable inputs from the Player class of the input sheet.
        m_PlayerInputActions.Player.Enable();


        m_PlayerInputActions.Player.Movement.performed += MovementPerformed;
        m_PlayerInputActions.Player.Movement.canceled += MovementCanceled;

        m_PlayerInputActions.Player.SprintTap.started += SprintStarted;
        m_PlayerInputActions.Player.SprintTap.performed += SprintTapPerformed;
        m_PlayerInputActions.Player.SprintHold.canceled += SprintHoldCanceled;
        m_PlayerInputActions.Player.Jump.started += JumpStarted;
        m_PlayerInputActions.Player.Jump.canceled += JumpCanceled;
    }

    //
    void MovementPerformed(InputAction.CallbackContext context)
    {
        MovementVector = m_PlayerInputActions.Player.Movement.ReadValue<Vector2>();
        
        InputManager.Instance.UpdateVector(MovementVector, "Player", 0);
        
    }

    void MovementCanceled(InputAction.CallbackContext context)
    {
        EventManager.TriggerEvent("IM_StopMoving");
        InputManager.Instance.RemoveVector("Player");
    }
    //

    void JumpStarted(InputAction.CallbackContext context)
    {
        
    }

    void JumpCanceled(InputAction.CallbackContext context)
    {

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
