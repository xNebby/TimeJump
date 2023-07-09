using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    static int MaxPlatformIndex = 0;
    private int CurrentPlatformIndex;
    private Interactable m_Interactable;
    private string EventName;

    void OnEnable()
    {
        MaxPlatformIndex += 1;
        CurrentPlatformIndex = MaxPlatformIndex;
        EventName = "Platform" + CurrentPlatformIndex.ToString();
        m_Interactable = gameObject.transform.GetChild(1).GetComponent<Interactable>();
        m_Interactable.InteractionEventName = EventName;
        EventName = "Interaction_" + EventName;
        EventManager.StartListening(EventName + "_Invoked", Open);
        EventManager.StartListening(EventName + "_Revoked", Close);
    }
    void OnDisable()
    {
        EventManager.StopListening(EventName + "_Invoked", Open);
        EventManager.StopListening(EventName + "_Revoked", Close);
    }

    void Open()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
    void Close()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }
}
