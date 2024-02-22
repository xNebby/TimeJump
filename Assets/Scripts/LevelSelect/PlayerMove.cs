using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public PlayerInputActions m_PlayerInputActions;
    private Vector2 MovementVector;
    private Rigidbody2D RB;
    public float speed;
    public bool Paused;

    void OnEnable()
    {
        RB = GetComponent<Rigidbody2D>();
        EventManager.TriggerEvent("CAM_UpdateFollow");
        m_PlayerInputActions = new PlayerInputActions();
        PlayerActionsListeners();
    }

    void PlayerActionsListeners()
    {

        // Enable inputs from the Player class of the input sheet.
        m_PlayerInputActions.Player.Enable();

        m_PlayerInputActions.Player.Movement.performed += MovementPerformed;
        m_PlayerInputActions.Player.Movement.canceled += MovementCanceled;

        m_PlayerInputActions.Player.InteractPrimary.performed += PrimaryInput;
        m_PlayerInputActions.Player.InteractSecondary.performed += SecondaryInput;
    }

    void PrimaryInput(InputAction.CallbackContext context)
    {
        EventManager.TriggerEvent("ID_PrimaryInput");
    }
    void SecondaryInput(InputAction.CallbackContext context)
    {
        EventManager.TriggerEvent("ID_SecondaryInput");
    }

    void MovementPerformed(InputAction.CallbackContext context)
    {
        MovementVector = m_PlayerInputActions.Player.Movement.ReadValue<Vector2>();
    }
    void MovementCanceled(InputAction.CallbackContext context)
    {
        MovementVector = Vector2.zero;
    }

    void FixedUpdate()
    {
        if (Paused == false)
        {
            RB.velocity = MovementVector * speed;

        }
    }
}
