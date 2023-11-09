using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public string LoadingInto;

    public int SaveValue = 0;

    public bool WaitingForDeload = false;
    public bool WaitingForHide = false;
    public bool LoadBackground = false;

    public List<GameObject> Levels = new List<GameObject>();

    // Start is called before the first frame update
    void OnEnable()
    {
        SaveValue = SaveLoader.Instance.CurrentLoadedSave;
        //Debug.Log("LevelSelect Loaded");
        EventManager.TriggerEvent("LS_EndLoad");
        EventManager.TriggerEvent("LevelSelectLoaded");
        EventManager.StartListening("SaveLoader_Loaded", QuitLoad);
        EventManager.StartListening("LevelBackground_Loaded", QuitLoad);
        EventManager.StartListening("LS_Hidden", HideConfirm);

        foreach (GameObject Entry in Levels)
        {
            Button Temp = Entry.GetComponent<Button>();
            Temp.onClick.AddListener(delegate { LoadLevel(Entry.name); });
        }
    }

    void OnDisable()
    {
        EventManager.StopListening("SaveLoader_Loaded", QuitLoad);
        EventManager.StopListening("LS_Hidden", HideConfirm);
    }

    public void DeloadLevelSelect()
    {
        WaitingForDeload = true;
        WaitingForHide = true;
        LoadingInto = "Menu";
        LoadBackground = false;
        // Save the current Save's information.
        EventManager.TriggerEvent("LS_Load");
    }
    public void DeloadLevelSelect(string LevelID)
    {
        WaitingForDeload = true;
        WaitingForHide = true;
        LoadBackground = true;
        LoadingInto = LevelID;
        // Save the current Save's information.
        EventManager.TriggerEvent("LS_Load");

    }

    void HideConfirm()
    {
        if (WaitingForHide)
        {
            WaitingForHide = false;
            SceneManager.LoadScene(LoadingInto, LoadSceneMode.Additive);
            if (LoadBackground)
            {
                SceneManager.LoadScene("Background", LoadSceneMode.Additive);
            }
        }
    }

    void QuitLoad()
    {
        if (WaitingForDeload)
        {
            WaitingForDeload = false;
            SceneManager.UnloadSceneAsync("LevelSelect");
            EventManager.TriggerEvent("LS_EndLoad");
        }
    }

    void LoadLevel(string LevelID)
    {
        // To load a level: Load the level given, the background, and unload the LoadManager.
        DeloadLevelSelect(LevelID);
    }
}
