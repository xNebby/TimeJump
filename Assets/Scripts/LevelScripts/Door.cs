using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    static int MaxDoorIndex = 0;
    private int CurrentDoorIndex;
    private bool PlayerInDoor;
    private Interactable m_Interactable;
    private Animator m_Animator;
    public string EventName;

    public bool RequireKey;
    public string KeyName;

    private BoxCollider2D m_BoxCollide;

    void OnEnable()
    {
        m_Animator = gameObject.transform.GetChild(2).GetComponent<Animator>();
        m_BoxCollide = gameObject.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>();
        m_Interactable = gameObject.transform.GetChild(1).GetComponent<Interactable>();
        if (m_Interactable.InteractionEventName == null || m_Interactable.InteractionEventName == "")
        {
            MaxDoorIndex += 1;
            CurrentDoorIndex = MaxDoorIndex;
            EventName = "Door" + CurrentDoorIndex.ToString();
            m_Interactable.InteractionEventName = EventName;
        }
        else
        {
            EventName = m_Interactable.InteractionEventName;
        }
        EventName = "Interaction_" + EventName;
        EventManager.StartListening(EventName + "_Invoked", Walk);
        EventManager.StartListening(EventName + "_Primary", Primary);
        EventManager.StartListening(EventName + "_Secondary", Secondary);
        EventManager.StartListening(EventName + "_Revoked", Close);

        EventManager.StartListening("Anim_DoorClose", DoorClose);
        EventManager.StartListening("Anim_DoorOpen", DoorOpen);
    }
    void OnDisable()
    {
        EventManager.StopListening(EventName + "_Invoked", Walk);
        EventManager.StopListening(EventName + "_Primary", Primary);
        EventManager.StopListening(EventName + "_Secondary", Secondary);
        EventManager.StopListening(EventName + "_Revoked", Close);

        EventManager.StopListening("Anim_DoorClose", DoorClose);
        EventManager.StopListening("Anim_DoorOpen", DoorOpen);
    }

    void Walk()
    {
        PlayerInDoor = true;
        if (RequireKey == false)
        {
            Open();
        }
        else
        {
            KeyCheck();
        }
    }

    void DoorOpen()
    {
        m_Animator.SetBool("IsOpen", true);
        
    }
    void DoorClose()
    {
        m_Animator.SetBool("IsOpen", false);
    }

    void Open()
    {
        m_BoxCollide.enabled = false;
        if (m_Animator.GetBool("IsOpen") == false & PlayerInDoor)
        {
            m_Animator.SetTrigger("Open");
        }
    }
    void Close()
    {
        Debug.Log("Close Door");
        m_BoxCollide.enabled = true;
        if (m_Animator.GetBool("IsOpen") == true & PlayerInDoor)
        {
            m_Animator.SetTrigger("Close");
        }
        PlayerInDoor = false;
    }

    bool KeyCheck()
    {
        // Check if the player has the key, if they dont then dont open. 
        bool hasKey = true;
        if (hasKey)
        {
            Open();
            return true;
        } else
        {
            return false;
        }
    }

    void Primary()
    {
        LogSystem.Log(gameObject, "Primary key received.");
        if (RequireKey == false)
        {
            Open();
        }
        else
        {
            KeyCheck();
        }
    }
    void Secondary()
    {
        LogSystem.Log(gameObject, "Secondary key received.");
        if (RequireKey == false)
        {
            Open();
        }
        else
        {
            KeyCheck();
        }
    }

}
