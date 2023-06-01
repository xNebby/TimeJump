using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireflyFollow : SingletonClass<FireflyFollow>
{
    private Rigidbody2D FireflyRB;

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
    }

    public void PlayerMove(Vector2 v_Move)
    {
        FireflyRB.velocity = v_Move;
    }

    // Move Towards mouse, if mouse is out of range then move to shortest distance between the mouse. 
    public void UpdateVector(Vector2 v_MouseVector)
    {
        MouseCoords = Mouse.current.position.ReadValue();
        MousePoint = cam.ScreenToWorldPoint(new Vector3(MouseCoords.x, MouseCoords.y, cam.nearClipPlane));
        MousePosition = new Vector2(MousePoint.x, MousePoint.y);
        VectorToMouse = MousePosition - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        MousePlayerVector = MousePosition - new Vector2(gameObject.transform.parent.parent.position.x, gameObject.transform.parent.parent.position.y);
        if ((MousePlayerVector).sqrMagnitude < MaxDistanceSq)
        {
            FireflyRB.MovePosition(MousePoint);
        }
    
    }


}
