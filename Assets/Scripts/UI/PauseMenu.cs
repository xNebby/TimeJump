using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Button m_Resume, m_Settings, m_QuitMap, m_QuitGame;

    void OnEnable()
    {
        m_Resume.onClick.AddListener(HideMenu);
        //m_Settings.onClick.AddListener();
        //m_QuitMap.onClick.AddListener();
        //m_QuitGame.onClick.AddListener();
        EventManager.StartListening("ID_Paused", ShowMenu);
        EventManager.StartListening("ID_UnPaused", HideMenu2);
    }
    void OnDisable()
    {
        EventManager.StopListening("ID_UnPaused", HideMenu2);
        EventManager.StopListening("ID_Paused", ShowMenu);
    }

    void ShowMenu()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
    void HideMenu()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        EventManager.TriggerEvent("PauseMenu_Resume");
    }
    void HideMenu2()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
