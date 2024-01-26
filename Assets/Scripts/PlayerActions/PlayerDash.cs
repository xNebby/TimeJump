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
            LogSystem.Log(gameObject, "Dashing!");
            // Get directional input then go that way in 8 cardinal. 
            // add Enable invuln if thats at a certain point- decide what portion of dash that is soon!!!!!
            PlayerVector = InputManager.Instance.IM_PlayerVector;
            DashVector = (new Vector2(Mathf.Round(PlayerVector.x), Mathf.Round(PlayerVector.y))).normalized;

            TimerManager.AddTimer("PD_DashStarted", DashTime, DashEnded);

            IsDashing = true;
            EventManager.TriggerEvent("PD_DashStarted");
        }
    }

    void DashEnded()
    {
        //LogSystem.Log(gameObject, "NOT Dashing!");
        // add Disable invuln
        IsDashing = false;
        EventManager.TriggerEvent("PD_DashStopped");
        // If on floor allow for dash to occur again.
    }

    void FixedUpdate()
    {
        if (IsDashing)
        {
            RB.AddForce(DashVector * DashLength);
        }
    }
}
