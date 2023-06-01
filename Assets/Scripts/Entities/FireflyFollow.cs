using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireflyFollow : MonoBehaviour
{
    private GameObject PlayerGO;
    private Rigidbody2D FireflyRB;
    Vector3 MousePosInWorldSpace()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private float ManhattenDistanceCam;
    private float ManhattenDistancePlayer;
    public float MaxSqDist;
    public float MaxDist = 10;
    private Vector2 MouseVector;
    private Vector2 MaxMousePos;
    private Vector2 FireflyVector;
    public float MovementSpeed = 5;

    void OnEnable()
    {
        PlayerGO = transform.parent.gameObject;
        MaxSqDist = (MaxDist * MaxDist);
        FireflyRB = GetComponent<Rigidbody2D>();
    }
    void OnDisable()
    {

    }

    void Update()
    {
        Vector3 currentMousePos = MousePosInWorldSpace();
        ManhattenDistanceCam = Mathf.Abs(PlayerGO.transform.position.x - currentMousePos.x) + Mathf.Abs(PlayerGO.transform.position.y - currentMousePos.y);
        ManhattenDistancePlayer = Mathf.Abs(PlayerGO.transform.position.x - gameObject.transform.position.x) + Mathf.Abs(PlayerGO.transform.position.y - gameObject.transform.position.y);

        if ((ManhattenDistanceCam < MaxSqDist))
        {
            // Move towards mouse as it is within range of the player.
            MoveToPos(currentMousePos);
        } 
        else
        {
            // The mouse is out of range of the player, calculate the closest position to the mouse. 
            // Vector to the mouse from the player, find the distance then get the position of where the vector is at the magnitude of the distance.
            MouseVector = new Vector3((currentMousePos.x - PlayerGO.transform.position.x), (currentMousePos.y - PlayerGO.transform.position.y));
            MouseVector = (MouseVector / MouseVector.magnitude) * MaxDist;

            MaxMousePos = new Vector2(PlayerGO.transform.position.x, PlayerGO.transform.position.y) + MouseVector;
            MoveToPos(MaxMousePos);

        }

    }

    void MoveToPos(Vector2 Position)
    {
        FireflyVector = new Vector2((Position.x - gameObject.transform.position.x), (Position.y - gameObject.transform.position.y));
        FireflyVector = ((FireflyVector * MovementSpeed) / FireflyVector.magnitude);
        string text = "moving by (" + FireflyVector.x + ", " + FireflyVector.y + ")";
        LogSystem.Log(gameObject, text);

        FireflyRB.velocity = (gameObject.transform.position + ((new Vector3(FireflyVector.x, FireflyVector.y, 0) * PlayerStateManager.Instance.TimeScale)));
    }



}
