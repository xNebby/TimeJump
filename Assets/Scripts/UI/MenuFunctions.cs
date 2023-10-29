using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuFunctions : MonoBehaviour
{
    public Button m_Continue, m_NewGame, m_Settings, m_Credits, m_QuitGame;

    public GameObject m_MainMenu, m_SavesMenu;

    

    void Start()
    {
        //Calls the TaskOnClick/TaskWithParameters/ButtonClicked method when you click the Button
        m_Continue.onClick.AddListener(Continue);
        //m_NewGame.onClick.AddListener(delegate { TaskWithParameters("Hello"); });
        //m_YourThirdButton.onClick.AddListener(() => ButtonClicked(42));
        m_NewGame.onClick.AddListener(NewGame);
        m_Settings.onClick.AddListener(Settings);
        m_Credits.onClick.AddListener(Credits);
        m_QuitGame.onClick.AddListener(QuitGame);
    }

    void Continue()
    {
        Debug.Log("Continue");
        m_SavesMenu.SetActive(true);
        //m_SavesMenu.SaveScreen.NewGame = false;
        EventManager.TriggerEvent("m_SavesMenuNGFalse");
        m_MainMenu.SetActive(false);

    }

    void NewGame()
    {
        Debug.Log("New game");
        m_SavesMenu.SetActive(true);
        //m_SavesMenu.SaveScreen.NewGame = true;
        EventManager.TriggerEvent("m_SavesMenuNGTrue");
        m_MainMenu.SetActive(false);

    }

    void Settings()
    {
        Debug.Log("Settings");

    }

    void Credits()
    {
        Debug.Log("Credits");

    }

    void QuitGame()
    {
        Debug.Log("Quit game!");
        Application.Quit();
    }
}
