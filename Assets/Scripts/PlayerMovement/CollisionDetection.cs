using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollisionDetection : SingletonClass<CollisionDetection>
{

    // This is for the player Specifically.
    private float FuzzyCheck = 0.5f;
    [Space(10)]
    [Header("Collision Values")]
    private ContactPoint2D[] Contacts = new ContactPoint2D[16];
    private List<ContactPoint2D> ContactsList = new List<ContactPoint2D>(16);
    private Vector2 ContactNormal;
    private Vector2 PlayerNormal;
    [Space(10)]
    [Header("Rotation Values")]
    private float PlayerRotationNormal;
    private float RotateSpeed = 5f;
    private float PlayerWalkAngle = 45f;


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
        PlayerNormal = Vector2.zero;
        PlayerRotationNormal = Vector2.Angle(PlayerStateManager.Instance.PlayerGravity, Vector2.down);
    }

    /// <summary>
    /// START OF COLLISION SECTION
    /// </summary>
    /// <param name="v_Velocity"></param>


    public void CheckCollision(Vector2 v_Velocity)
    {
        ResetVars();
        int count = PlayerManager.Instance.PlayerRB.GetContacts(Contacts);
        ContactsList.Clear();

        for (int i = 0; i < count; i++)
        {
            ContactsList.Add(Contacts[i]);
        }

        bool GroundContact = false;
        bool RampContact = false;
        bool RoofContact = false;
        bool WallContact = false;

        if (ContactsList.Count == 0)
        {
            RotateToNormal();
        }
        PlayerNormal = Vector2Extensions.rotateDeg(PlayerStateManager.Instance.PlayerGravity, PlayerManager.Instance.PlayerRB.rotation);
        //Debug.DrawRay(new Vector3(PlayerManager.Instance.PlayerRB.position.x, PlayerManager.Instance.PlayerRB.position.x, -10), new Vector3(PlayerNormal.x, PlayerNormal.y, -10), Color.red);
        for (int i = 0; i < ContactsList.Count; i++)
        {
            ContactNormal = ContactsList[i].normal;
            if (ContactNormal == PlayerNormal * -1)
            {
                float ContactAngle = Vector2.Angle(PlayerNormal, PlayerStateManager.Instance.PlayerGravity);
                // Touching some kind of floor
                if (ContactAngle == 0)
                {
                    GroundContact = true;
                }
                else if (Mathf.Abs(ContactAngle) <= PlayerWalkAngle)
                {
                    float angl = Mathf.Abs(Vector2.Angle(PlayerStateManager.Instance.PlayerGravity, ContactNormal));
                    //Debug.Log(angl);
                    if (angl < 5f)
                    {
                        GroundContact = true;
                        RotateToNormal();
                    } else
                    {
                        RampContact = true;
                    }
                }
            }

            if (ContactNormal == PlayerNormal)
            {
                RoofContact = true;
            }

            if (Vector2.Perpendicular(ContactNormal) == PlayerNormal)
            {
                WallContact = true;
            } else if (Vector2.Perpendicular(ContactNormal) * -1 == PlayerNormal)
            {
                WallContact = true;
            }

        }

        if (GroundContact & RampContact == false)
        {
            EventManager.TriggerEvent("CD_TouchGround");
        }
        else if (GroundContact == false)
        {
            EventManager.TriggerEvent("CD_LeaveGround");
        }

        if (RampContact)
        {
            EventManager.TriggerEvent("CD_TouchRamp");
        } else
        {
            EventManager.TriggerEvent("CD_LeaveRamp");
        }

        if (WallContact)
        {
            EventManager.TriggerEvent("CD_TouchWall");
        }
        else
        {
            EventManager.TriggerEvent("CD_LeaveWall");
        }

        if (RoofContact)
        {
            EventManager.TriggerEvent("CD_TouchRoof");
        } else
        {
            EventManager.TriggerEvent("CD_LeaveRoof");
        }


    }

    void RotateToNormal()
    {
        if (!(PlayerManager.Instance.PlayerRB.rotation == PlayerRotationNormal))
        {
            /*while (PlayerManager.Instance.PlayerRB.rotation >= 180)
            {
                PlayerManager.Instance.PlayerRB.rotation -= 360f;
            }
            while (PlayerManager.Instance.PlayerRB.rotation <= -180)
            {
                PlayerManager.Instance.PlayerRB.rotation += 360f;
            }*/
            // not touching anything. Rotate to normal. 
            if ((PlayerManager.Instance.PlayerRB.rotation < (FuzzyCheck + PlayerRotationNormal) & PlayerManager.Instance.PlayerRB.rotation > PlayerRotationNormal) || (PlayerManager.Instance.PlayerRB.rotation < (-180f + FuzzyCheck + PlayerRotationNormal) & PlayerManager.Instance.PlayerRB.rotation < PlayerRotationNormal))
            {
                //Debug.Log("Set to 0");
                PlayerManager.Instance.PlayerRB.rotation = PlayerRotationNormal;
            }
            else
            {
                if (PlayerManager.Instance.PlayerRB.rotation <= (180 + PlayerRotationNormal) & PlayerManager.Instance.PlayerRB.rotation > PlayerRotationNormal)
                {
                    //Debug.Log(PlayerManager.Instance.PlayerRB.rotation);
                    //Debug.Log("C");
                    PlayerManager.Instance.PlayerRB.MoveRotation(PlayerManager.Instance.PlayerRB.rotation - (Time.fixedDeltaTime * RotateSpeed));
                }
                else if (PlayerManager.Instance.PlayerRB.rotation < PlayerRotationNormal)
                {
                    //Debug.Log("AC");
                    PlayerManager.Instance.PlayerRB.MoveRotation(PlayerManager.Instance.PlayerRB.rotation + (Time.fixedDeltaTime * RotateSpeed));
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (PlayerManager.Instance.PlayerRB.rotation > PlayerRotationNormal + PlayerWalkAngle)
        {
            PlayerManager.Instance.PlayerRB.rotation = PlayerRotationNormal + PlayerWalkAngle;
        }
        else if (PlayerManager.Instance.PlayerRB.rotation < PlayerRotationNormal - PlayerWalkAngle)
        {
            PlayerManager.Instance.PlayerRB.rotation = PlayerRotationNormal - PlayerWalkAngle;
        }
    }

}
