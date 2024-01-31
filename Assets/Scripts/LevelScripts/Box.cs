using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private float ObjectSpeed;
    private bool Attached;
    private Transform playerTrans;
    public float PlayerToBox;
    public Vector2 GravityVector;
    private Rigidbody2D BoxRB;
    public float ObjectMass;

    // when player touches it, parent to them. 
    // When the player turns away from the center of the object, the box should unparent 

    void OnEnable()
    {
        Attached = false;
        BoxRB = GetComponent<Rigidbody2D>();
        ObjectSpeed = 1f;
        ObjectMass = 10f;
        playerTrans = GameObject.FindWithTag("Player").transform;
        EventManager.StartListening("TS_StopTime", ChangeSpeed);
        EventManager.StartListening("TS_ResumeTime", ChangeSpeed);
    }

    void OnDisable()
    {
        EventManager.StopListening("TS_StopTime", ChangeSpeed);
        EventManager.StopListening("TS_ResumeTime", ChangeSpeed);
    }

    private void ChangeSpeed()
    {
        if (TimeStop.Instance.TimeStopped)
        {
            ObjectSpeed = TimeStop.Instance.ObjectSpeed;
        } else
        {
            ObjectSpeed = 1f;
        }
    }

    private void Unattach()
    {
        Debug.Log("Box leave");
        Attached = false;
        //transform.SetParent(null);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Box collide");
            Attached = true;
            //transform.SetParent(playerTrans);
        }
    }

    private void FixedUpdate()
    {
        if (Attached)
        {
            PlayerToBox = Mathf.Round((transform.position - playerTrans.position).x);
            if (PlayerToBox < 0f & InputManager.Instance.IM_PlayerVector.x >= 0f)
            {
                Unattach();
            }
            else if (PlayerToBox > 0f & InputManager.Instance.IM_PlayerVector.x <= 0f)
            {
                Unattach();
            }
            if (PlayerToBox == 0f)
            {
                Unattach();
            }
            if (TimeStop.Instance.TimeStopped)
            {
                BoxRB.gravityScale = ObjectSpeed;
                BoxRB.mass = ObjectMass / ObjectSpeed;
            } 
        }
        else
        {
            if (!TimeStop.Instance.TimeStopped)
            {
                BoxRB.gravityScale = ObjectSpeed;
                BoxRB.mass = ObjectMass / ObjectSpeed;
            } 
        }
        
    }

}
