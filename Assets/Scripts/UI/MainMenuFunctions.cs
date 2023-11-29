using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuFunctions : MonoBehaviour
{
    public Button m_Continue, m_NewGame, m_QuitGame;

    void OnEnable()
    {
        m_Continue.onClick.AddListener(delegate { EventManager.TriggerEvent("m_SavesMenuNGFalse"); });
        m_NewGame.onClick.AddListener(delegate { EventManager.TriggerEvent("m_SavesMenuNGTrue"); });
        m_QuitGame.onClick.AddListener(Application.Quit);
    }
}
