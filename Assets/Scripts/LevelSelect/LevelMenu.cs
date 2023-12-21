using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMenu : MonoBehaviour
{
    public string EventName;
    public bool Visible;
    public GameObject m_LevelMenu, m_Indicator, m_PrimaryIndicator, m_SecondaryIndicator;

    void OnEnable() 
    {
        var ParentObj = gameObject.transform.parent;
        m_PrimaryIndicator = ParentObj.GetChild(0).GetChild(0).gameObject;
        m_SecondaryIndicator = ParentObj.GetChild(0).GetChild(1).gameObject;
        EventName = ParentObj.gameObject.name;
        var m_Interactable = ParentObj.GetChild(0).gameObject.GetComponent<Interactable>();
        m_Interactable.ChangeEventName(EventName);
        m_LevelMenu = gameObject.transform.GetChild(0).gameObject;
        m_Indicator = gameObject.transform.GetChild(1).gameObject;
        Visible = false;
        EventManager.StartListening("Interaction_" + EventName + "_Primary", Primary);
        EventManager.StartListening("Interaction_" + EventName + "_Secondary", Secondary);
        EventManager.StartListening("Interaction_" + EventName + "_Invoked", Invoked);
        EventManager.StartListening("Interaction_" + EventName + "_Revoked", Revoked);
        EventManager.StartListening("UI_CloseMenu", CloseMenu);
    }
    void OnDisable()
    {
        EventManager.StopListening("Interaction_" + EventName + "_Primary", Primary);
        EventManager.StopListening("Interaction_" + EventName + "_Secondary", Secondary);
        EventManager.StopListening("Interaction_" + EventName + "_Invoked", Invoked);
        EventManager.StopListening("Interaction_" + EventName + "_Revoked", Revoked);
        EventManager.StopListening("UI_CloseMenu", CloseMenu);
    }

    void CloseMenu()
    {
        // Closes the menu if visible
        if (Visible)
        {
            m_LevelMenu.SetActive(false);
        }
        m_Indicator.SetActive(true);
    }
    void Primary()
    {
        // Brings up the menu. Closes the temp thing.
        Debug.Log("Primary");
        m_Indicator.SetActive(false);
        m_LevelMenu.SetActive(true);
        m_PrimaryIndicator.SetActive(false);
        m_SecondaryIndicator.SetActive(false);
    }
    void Secondary()
    {
        
    }
    void Invoked()
    {
        Debug.Log("invoked!");
        // Sets the temporary thing to visible
        m_Indicator.SetActive(true);
    }
    void Revoked()
    {
        // Sets both the temporary thing and the level popup to invisible.
        m_Indicator.SetActive(false);
        m_LevelMenu.SetActive(false);
    }
}
