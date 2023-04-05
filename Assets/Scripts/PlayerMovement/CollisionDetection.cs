using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollisionDetection : SingletonClass<CollisionDetection>
{

    // This is for the player Specifically.
    public float FuzzyCheck = 0.5f;
    [Space(10)]
    [Header("Collision Values")]
    public ContactPoint2D[] Contacts = new ContactPoint2D[16];
    public List<ContactPoint2D> ContactsList = new List<ContactPoint2D>(16);
    public Vector2 ContactNormal;
    public Vector2 PlayerNormal;
    [Space(10)]
    [Header("Rotation Values")]
    public float PlayerRotationNormal;
    public float RotateSpeed = 5f;
    public float PlayerWalkAngle = 45f;
    [Space(10)]
    [Header("Gravity Values")]
    public bool EnableGravity = true;
    [Space(5)]
    public float WorldGravityScale = 10f;
    public float PlayerGravityScale = 10f;
    public float CurrentGravityMult = 0f;
    public float TerminalMult = 40f;


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
        int count = PlayerInfo.Instance.PlayerRigidbody.GetContacts(Contacts);
        ContactsList.Clear();

        for (int i = 0; i < count; i++)
        {
            ContactsList.Add(Contacts[i]);
        }

        if (ContactsList.Count == 0)
        {
            RotateToNormal();
            EventManager.TriggerEvent("CD_LeaveGround");
            EventManager.TriggerEvent("CD_LeaveRamp");
        }
        PlayerNormal = Vector2Extensions.rotateDeg(PlayerStateManager.Instance.PlayerGravity, PlayerInfo.Instance.PlayerRigidbody.rotation);
        //Debug.DrawRay(new Vector3(PlayerInfo.Instance.PlayerRigidbody.position.x, PlayerInfo.Instance.PlayerRigidbody.position.x, -10), new Vector3(PlayerNormal.x, PlayerNormal.y, -10), Color.red);
        for (int i = 0; i < ContactsList.Count; i++)
        {
            ContactNormal = ContactsList[i].normal;
            if (ContactNormal == PlayerNormal * -1)
            {
                float ContactAngle = Vector2.Angle(PlayerNormal, PlayerStateManager.Instance.PlayerGravity);
                // Touching some kind of floor
                if (ContactAngle == 0)
                {
                    EventManager.TriggerEvent("CD_TouchGround");
                }
                else if (Mathf.Abs(ContactAngle) <= PlayerWalkAngle)
                {
                    float angl = Mathf.Abs(Vector2.Angle(PlayerStateManager.Instance.PlayerGravity, ContactNormal));
                    Debug.Log(angl);
                    if (angl < 5f)
                    {
                        EventManager.TriggerEvent("CD_TouchGround");
                        RotateToNormal();
                    } else
                    {
                        EventManager.TriggerEvent("CD_LeaveGround");
                        EventManager.TriggerEvent("CD_TouchRamp");
                    }
                }
                else
                {
                    EventManager.TriggerEvent("CD_LeaveGround");
                    EventManager.TriggerEvent("CD_LeaveRamp");
                }
            }

        }
    }

    void RotateToNormal()
    {
        if (!(PlayerInfo.Instance.PlayerRigidbody.rotation == PlayerRotationNormal))
        {
            /*while (PlayerInfo.Instance.PlayerRigidbody.rotation >= 180)
            {
                PlayerInfo.Instance.PlayerRigidbody.rotation -= 360f;
            }
            while (PlayerInfo.Instance.PlayerRigidbody.rotation <= -180)
            {
                PlayerInfo.Instance.PlayerRigidbody.rotation += 360f;
            }*/
            // not touching anything. Rotate to normal. 
            if ((PlayerInfo.Instance.PlayerRigidbody.rotation < (FuzzyCheck + PlayerRotationNormal) & PlayerInfo.Instance.PlayerRigidbody.rotation > PlayerRotationNormal) || (PlayerInfo.Instance.PlayerRigidbody.rotation < (-180f + FuzzyCheck + PlayerRotationNormal) & PlayerInfo.Instance.PlayerRigidbody.rotation < PlayerRotationNormal))
            {
                //Debug.Log("Set to 0");
                PlayerInfo.Instance.PlayerRigidbody.rotation = PlayerRotationNormal;
            }
            else
            {
                if (PlayerInfo.Instance.PlayerRigidbody.rotation <= (180 + PlayerRotationNormal) & PlayerInfo.Instance.PlayerRigidbody.rotation > PlayerRotationNormal)
                {
                    //Debug.Log(PlayerInfo.Instance.PlayerRigidbody.rotation);
                    //Debug.Log("C");
                    PlayerInfo.Instance.PlayerRigidbody.MoveRotation(PlayerInfo.Instance.PlayerRigidbody.rotation - (Time.fixedDeltaTime * RotateSpeed));
                }
                else if (PlayerInfo.Instance.PlayerRigidbody.rotation < PlayerRotationNormal)
                {
                    //Debug.Log("AC");
                    PlayerInfo.Instance.PlayerRigidbody.MoveRotation(PlayerInfo.Instance.PlayerRigidbody.rotation + (Time.fixedDeltaTime * RotateSpeed));
                }
            }
        }
    }

    /// <summary>
    /// END OF ROTATION SECTION
    /// START OF GRAVITY SECTION
    /// </summary>

    void IncreaseGravity()
    {
        if (EnableGravity)
        {
            if (PlayerStateManager.Instance.PlayerIsOnGround == false & PlayerStateManager.Instance.PlayerIsOnRamp == false & PlayerStateManager.Instance.PlayerIsWallSlide == false)
            {
                if (CurrentGravityMult < TerminalMult)
                {
                    CurrentGravityMult += PlayerGravityScale * Time.fixedDeltaTime;
                }
                else
                {
                    CurrentGravityMult = TerminalMult;
                }
                ApplyGravity();
            }
            else
            {
                CurrentGravityMult = 0;
            }
        }
    }

    public void ApplyGravity()
    {
        // Check downwards in the relative gravity- if can move towards gravity, do so. 
        // If you can move down, but the player has a collision- then it means they are either touching a wall or a ramp.
        // In either case, the normals must be compared to find the rotation in comparison to the players current rotation. 
        // If the rotation is within the walking angle boundary, change the players rotation to match and move downwards.
        // If the rotation is not within the walking angle boundary, then change the player's state to wall grabbing. 

        //PlayerInfo.Instance.PlayerRigidbody.MovePosition()
        PlayerInfo.Instance.PlayerRigidbody.velocity += (PlayerStateManager.Instance.PlayerGravity * CurrentGravityMult);
    }

    public void ResetGravity()
    {
        CurrentGravityMult = 0f;
    }
    /// <summary>
    ///  END OF GRAVITY SECTION
    /// </summary>

    void FixedUpdate()
    {
        if (PlayerInfo.Instance.PlayerRigidbody.rotation > PlayerRotationNormal + PlayerWalkAngle)
        {
            PlayerInfo.Instance.PlayerRigidbody.rotation = PlayerRotationNormal + PlayerWalkAngle;
        }
        else if (PlayerInfo.Instance.PlayerRigidbody.rotation < PlayerRotationNormal - PlayerWalkAngle)
        {
            PlayerInfo.Instance.PlayerRigidbody.rotation = PlayerRotationNormal - PlayerWalkAngle;
        }
        IncreaseGravity();
    }

}
