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

    public int GameState = 0; // 0 = Playing game, 1 = Paused game, 2 = Cutscene, 3 = UI
    public int PrevGameState = 0;



    void OnEnable()
    {
        EventManager.StartListening("PauseMenu_Resume", Paused);
        EventManager.StartListening("OpenUI", OpenedUI);
        // Create new instance of input sheet.
        m_PlayerInputActions = new PlayerInputActions();
        PlayerActionsListeners();
        UIActionsListeners();

        // Enable inputs from the Player class of the input sheet.
        m_PlayerInputActions.Player.Enable();
    }

    void OnDisable()
    {
        EventManager.StopListening("PauseMenu_Resume", Paused);
        EventManager.StopListening("OpenUI", OpenedUI);

    }

    void PlayerActionsListeners()
    {
        m_PlayerInputActions.Player.Movement.performed += MovementPerformed;
        m_PlayerInputActions.Player.Movement.canceled += MovementCanceled;

        m_PlayerInputActions.Player.InteractPrimary.performed += PrimaryInput;
        m_PlayerInputActions.Player.InteractSecondary.performed += SecondaryInput;

        m_PlayerInputActions.Player.Dash.performed += DashPerformed;

        m_PlayerInputActions.Player.Pause.performed += Paused;

        m_PlayerInputActions.Player.SprintTap.started += SprintStarted;
        m_PlayerInputActions.Player.SprintTap.performed += SprintTapPerformed;
        m_PlayerInputActions.Player.SprintHold.canceled += SprintHoldCanceled;

        m_PlayerInputActions.Player.Jump.started += JumpStarted;
        m_PlayerInputActions.Player.Jump.canceled += JumpCanceled;

        m_PlayerInputActions.Player.PrimarySpecialAbility.started += PrimarySpecial;
        m_PlayerInputActions.Player.SecondarySpecialAbility.started += SecondarySpecial;
        m_PlayerInputActions.Player.TertiarySpecialAbility.started += TertiarySpecial;
    }

    void UIActionsListeners()
    {
        m_PlayerInputActions.UI.Back.started += UIBack;
        m_PlayerInputActions.UI.Close.started += UIClose;

    }

    void CutsceneActionsListeners()
    {
        m_PlayerInputActions.Cutscenes.Pause.started += Paused;
        m_PlayerInputActions.Cutscenes.Skip.started += CutsceneSkip;
    }

    //
    void CutsceneSkip(InputAction.CallbackContext context)
    {

    }

    //
    void UIBack(InputAction.CallbackContext context)
    {

    }
    void UIClose(InputAction.CallbackContext context)
    {
        if (GameState == 1)
        {
            Paused(context);
        } else
        {
            EventManager.TriggerEvent("CloseUI");
        }
    }
    void OpenedUI()
    {
        PrevGameState = GameState;
        GameState = 3;
    }


    //

    //
    void PrimarySpecial(InputAction.CallbackContext context)
    {

    }
    void SecondarySpecial(InputAction.CallbackContext context)
    {

    }
    void TertiarySpecial(InputAction.CallbackContext context)
    {

    }

    // 
    void Paused(InputAction.CallbackContext context)
    {
        Paused();
    }
    void Paused()
    {
        // Disable player actions, enable ui actions, enable ui, prevent physics updates etc. 
        if (!(GameState == 1))
        {
            EventManager.TriggerEvent("ID_Paused"); // Shows ui 
            if (GameState == 0)
            {
                m_PlayerInputActions.Player.Disable();
            }
            else if (GameState == 2)
            {
                m_PlayerInputActions.Cutscenes.Disable();
            }
            m_PlayerInputActions.UI.Enable();
            Time.timeScale = 0;
            PrevGameState = GameState;
            GameState = 1;
        }
        else
        {
            Time.timeScale = 1;
            GameState = PrevGameState; 
            EventManager.TriggerEvent("ID_UnPaused");
            if (GameState == 0)
            {
                m_PlayerInputActions.Player.Enable();
                m_PlayerInputActions.UI.Disable();
            }
            else if (GameState == 2)
            {
                m_PlayerInputActions.Cutscenes.Enable();
                m_PlayerInputActions.UI.Disable();
            }
        }
    }

    //
    void DashPerformed(InputAction.CallbackContext context)
    {
        EventManager.TriggerEvent("ID_Dash");
    }

    //
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
