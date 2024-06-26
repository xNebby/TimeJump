using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveScreen : MonoBehaviour
{
    public Button m_SaveOne, m_SaveTwo, m_SaveThree, m_SaveFour, m_SaveFive, m_Close;

    public GameObject m_MainMenu, m_SavesMenu, m_ConfirmMenu;

    public ConfirmScreen m_ConfirmScreenScript;

    public bool NG = false;
    // Start is called before the first frame update
    void Start()
    {
        m_SaveOne.onClick.AddListener(delegate { LoadSave(1); });
        m_SaveTwo.onClick.AddListener(delegate { LoadSave(2); });
        m_SaveThree.onClick.AddListener(delegate { LoadSave(3); });
        m_SaveFour.onClick.AddListener(delegate { LoadSave(4); });
        m_SaveFive.onClick.AddListener(delegate { LoadSave(5); });
        m_Close.onClick.AddListener(Close);

        m_ConfirmScreenScript = m_ConfirmMenu.GetComponent<ConfirmScreen>();

    }
    void OnEnable()
    {
        EventManager.StartListening("m_SavesMenuNGTrue", delegate { NewGame(true); });
        EventManager.StartListening("m_SavesMenuNGFalse", delegate { NewGame(false); });
    }
    void OnDisable()
    {
        EventManager.StopListening("m_SavesMenuNGTrue", delegate { NewGame(true); });
        EventManager.StopListening("m_SavesMenuNGFalse", delegate { NewGame(false); });
    }

    void NewGame(bool value)
    {
        NG = value;
    }

    void LoadSave(int SaveNumber)
    {
        Debug.Log(SaveNumber);
        if (NG == true)
        {
            // Send to confirmation screen.
            m_ConfirmMenu.SetActive(true);
            Debug.Log(m_ConfirmScreenScript.SyncValue(SaveNumber));
            m_SavesMenu.SetActive(false);
            Debug.Log("Synced save number!");
        } else
        {
            // Check if the save exists.
            // Load the save.
            SaveLoader.Instance.Continue(SaveNumber);
        }
    }
    void Close()
    {
        m_MainMenu.SetActive(true);
        m_SavesMenu.SetActive(false);
    }
}
