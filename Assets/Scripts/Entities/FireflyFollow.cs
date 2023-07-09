using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireflyFollow : SingletonClass<FireflyFollow>
{
    private Rigidbody2D FireflyRB;

    //private float FireflySize = 0.25f;

    public Vector2 MouseVector;
    public Vector2 VectorToMouse;
    public Vector2 MousePlayerVector;
    public Vector3 MousePoint;
    public Vector2 MousePosition;
    public Vector2 MouseCoords;
    private float MaxDistance = 3;
    private float MaxDistanceSq;

    private Camera cam;


    void OnEnable()
    {
        MaxDistanceSq = MaxDistance * MaxDistance;
        FireflyRB = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        /*
        EventManager.StartListening("PC_Crouch", FireflyCrouch);
        EventManager.StartListening("PC_Uncrouch", FireflyUncrouch);*/

    }
    void OnDisable()
    {
/*
        EventManager.StopListening("PC_Crouch", FireflyCrouch);
        EventManager.StopListening("PC_Uncrouch", FireflyUncrouch);*/
    }
/*
    public void FireflyCrouch()
    {
        UpdateVector();
        gameObject.transform.localScale = new Vector3(FireflySize, FireflySize * 2, FireflySize);
        UpdateVector();
        
    }
    public void FireflyUncrouch()
    {
        UpdateVector();
        gameObject.transform.localScale = new Vector3(FireflySize, FireflySize, FireflySize);
        UpdateVector();

    }

    public void PlayerMove(Vector2 v_Move)
    {
        //FireflyRB.velocity = v_Move;
    }*/

    // Move Towards mouse, if mouse is out of range then move to shortest distance between the mouse. 
    public void UpdateVector()
    {
        //Debug.Log("follow");
        MouseCoords = Mouse.current.position.ReadValue();
        MousePoint = cam.ScreenToWorldPoint(new Vector3(MouseCoords.x, MouseCoords.y, cam.nearClipPlane));
        MousePosition = new Vector2(MousePoint.x, MousePoint.y);
        VectorToMouse = MousePosition - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        Vector2 ParentPos = new Vector2(PlayerManager.Instance.PlayerGO.transform.position.x, PlayerManager.Instance.PlayerGO.transform.position.y);
        MousePlayerVector = MousePosition - ParentPos;
        if ((MousePlayerVector).sqrMagnitude < MaxDistanceSq)
        {
            FireflyRB.MovePosition(MousePoint);
        }
        else
        {
            FireflyRB.MovePosition((MousePlayerVector / MousePlayerVector.magnitude * MaxDistance) + ParentPos);
        }

    }
/*
    public void BasicMove()
    {
        MouseCoords = Mouse.current.position.ReadValue();
        MousePoint = cam.ScreenToWorldPoint(new Vector3(MouseCoords.x, MouseCoords.y, 0));
        FireflyRB.velocity = (MousePoint - FireflyRB.transform.position);
    }
*/
    void Update()
    {
        UpdateVector();
        //BasicMove();
    }


}
