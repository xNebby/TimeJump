using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : SingletonClass<CollisionDetection>
{

    // This is for the player Specifically.
    public ContactPoint2D[] Contacts = new ContactPoint2D[16];
    public List<ContactPoint2D> ContactsList = new List<ContactPoint2D>(16);
    public Vector2 ContactNormal;    
    public Rigidbody2D PlayerRB;


    void OnEnable()
    {
        ResetVars();
    }
    override public void Awake()
    {
        ResetVars();
        base.Awake();
    }
    void ResetVars()
    {
        ContactNormal = Vector2.zero;
        Contacts = new ContactPoint2D[16];
        ContactsList = new List<ContactPoint2D>(16);
        PlayerRB = PlayerInfo.Instance.PlayerRigidbody;
    }

    public void CheckCollision(Vector2 v_Velocity)
    {
        ResetVars();
        int count = PlayerRB.GetContacts(Contacts);
        ContactsList.Clear();

        for (int i = 0; i < count; i++)
        {
            ContactsList.Add(Contacts[i]);
        }

        if (ContactsList.Count == 0)
        {
            // not touching anything. Rotate to normal. 
            if (PlayerRB.Rotation == Vector2.zero)
            {

            } else
            {
                PlayerRB.MoveRotation(0f);
            }
        }

        for (int i = 0; i < ContactsList.Count; i++)
        {
            ContactNormal = ContactsList[i].normal;


        }
    }

}
