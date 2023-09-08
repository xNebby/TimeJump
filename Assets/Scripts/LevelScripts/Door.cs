using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    static int MaxDoorIndex = 0;
    private int CurrentDoorIndex;
    private Interactable m_Interactable;
    private string EventName;

    public bool RequireKey;
    public string KeyName;

    void OnEnable()
    {

        m_Interactable = gameObject.transform.GetChild(1).GetComponent<Interactable>();
        if (m_Interactable.InteractionEventName == null)
        {
            MaxDoorIndex += 1;
            CurrentDoorIndex = MaxDoorIndex;
            EventName = "Door" + CurrentDoorIndex.ToString();
            m_Interactable.InteractionEventName = EventName;
        } else
        {
            EventName = m_Interactable.InteractionEventName;
        }
        EventName = "Interaction_" + EventName;
        EventManager.StartListening(EventName + "_Invoked", Walk);
        EventManager.StartListening(EventName + "_Primary", Primary);
        EventManager.StartListening(EventName + "_Secondary", Secondary);
        EventManager.StartListening(EventName + "_Revoked", Close);
    }
    void OnDisable()
    {
        EventManager.StopListening(EventName + "_Invoked", Walk);
        EventManager.StopListening(EventName + "_Primary", Primary);
        EventManager.StopListening(EventName + "_Secondary", Secondary);
        EventManager.StopListening(EventName + "_Revoked", Close);
    }

    void Walk()
    {
        if (RequireKey == false)
        {
            Open();
        }
        else
        {
            KeyCheck();
        }
    }

    void Open()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
    void Close()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
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
