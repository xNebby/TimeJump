using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private Rigidbody2D RB;


    public Vector2 PlayerVector;
    public Vector2 DashVector;

    private float DashTime = 0.2f;
    //private bool DashInvuln = false;
    private float DashLength = 700f;
    public float DashScale = 3f;

    public bool IsDashing = false;


    void OnEnable()
    {
        RB = GetComponent<Rigidbody2D>();

        EventManager.StartListening("ID_Dash", Dash);
    }
    void OnDisable()
    {
        EventManager.StopListening("ID_Dash", Dash);
    }

    void Dash()
    {
        if (PlayerStateManager.Instance.PlayerCanDash)
        {
            FindFirstObjectByType<GhostTrail>().ShowGhost();
            LogSystem.Log(gameObject, "Dashing!");
            // Get directional input then go that way in 8 cardinal. 
            // add Enable invuln if thats at a certain point- decide what portion of dash that is soon!!!!!
            PlayerVector = InputManager.Instance.IM_PlayerVector;
            DashVector = ((new Vector2(Mathf.Round(PlayerVector.x), Mathf.Round(PlayerVector.y))).normalized) * DashScale;

            TimerManager.AddTimer("PD_DashStarted", DashTime, DashEnded);
            MovementStatusManager.Instance.AddJumpMovementEffect("Dash", DashVector, 0f, 0f);
            PlayerManager.Instance.UpdatePMM_PD_DashVector(DashVector);


            IsDashing = true;
            EventManager.TriggerEvent("PD_DashStarted");
            EventManager.TriggerEvent("IM_StopJump");
        }
    }

    void DashEnded()
    {
        //LogSystem.Log(gameObject, "NOT Dashing!");
        // add Disable invuln
        IsDashing = false;
        MovementStatusManager.Instance.RemoveEffect("Dash");
        EventManager.TriggerEvent("PD_DashStopped");
        // If on floor allow for dash to occur again.
    }

    /*void FixedUpdate()
    {
        if (IsDashing)
        {
            RB.AddForce(DashVector * DashLength);
        }
    }*/
}
