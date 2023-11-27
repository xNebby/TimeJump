using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Dictionary<int, Vector2> Locations;
    private int CurrentIDs;
    private int MaxID;
    public List<Vector2> LocationTemp;
    public int LocationIndex;
    public Vector2 CurrentVelocity;
    public float Speed;
    private Rigidbody2D RB;

    void OnEnable()
    {
        Locations = new Dictionary<int, Vector2>();
        LocationTemp = new List<Vector2>();
        CurrentIDs = 0;
        LocationIndex = 0;
        foreach (Vector2 Entry in LocationTemp)
        {
            Locations.Add(CurrentIDs, Entry);
            CurrentIDs += 1;
        } 
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
            LocationIndex += 1;
        } else
        {
            CurrentVelocity = Locations[LocationIndex] - new Vector2(transform.position.x, transform.position.y);
        }
    }

}
