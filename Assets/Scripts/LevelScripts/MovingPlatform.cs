using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Dictionary<int, Vector2> Locations;
    public int LocationIndex;
    public Vector2 CurrentVelocity;
    public float Speed;
    private Rigidbody2D RB;

    void OnEnable()
    {
        Locations = new Dictionary<int, Vector2>();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        if (hit.transform.tag == "Player")
        {

        } 
    }

    void FixedUpdate()
    {
        if ((Locations[LocationIndex] - new Vector2(transform.position.x, transform.position.y)).magnitude < 5)
        {

        } else
        {

        }
    }

}
