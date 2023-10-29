using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmScreen : MonoBehaviour
{
    public Button m_Confirm, m_Cancel;
    public GameObject m_ConfirmScreen, m_SaveScreen;
    public int SaveValue;

    // Start is called before the first frame update
    void Start()
    {
        SaveValue = 0; // 0 means invalid save.
        m_Confirm.onClick.AddListener(Confirm);
        m_Cancel.onClick.AddListener(Cancel);
    }

    void Confirm()
    {
        Debug.Log("Overwriting save " + SaveValue.ToString());
        // Load a new save and give it the id, Delete any save if it already has that id. 
    }
    void Cancel()
    {
        m_SaveScreen.SetActive(true);
        m_ConfirmScreen.SetActive(false);
    }

}
