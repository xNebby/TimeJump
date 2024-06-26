using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : SingletonClass<LevelSelect>
{
    public string LoadingInto;

    public int SaveValue = 0;

    public bool WaitingForDeload = false;
    public bool WaitingForHide = false;
    public bool LoadBackground = false;


    // Start is called before the first frame update
    void OnEnable()
    {
        SaveValue = SaveLoader.Instance.CurrentLoadedSave;
        //Debug.Log("LevelSelect Loaded");
        EventManager.TriggerEvent("LS_EndLoad");
        EventManager.TriggerEvent("LevelSelectLoaded");
        EventManager.StartListening("SaveLoader_Loaded", delegate { QuitLoad(false); });
        EventManager.StartListening("LevelBackground_Loaded", delegate { QuitLoad(true); });
        EventManager.StartListening("LS_Hidden", HideConfirm);
    }

    void OnDisable()
    {
        EventManager.StopListening("SaveLoader_Loaded", delegate { QuitLoad(false); });
        EventManager.StopListening("LevelBackground_Loaded", delegate { QuitLoad(true); });
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
        CurrentSave.Instance.CurrentLevel(LevelID);
        // Save the current Save's information.
        EventManager.TriggerEvent("LS_Load");
    }
    public void DeloadLevelSelect(string LevelID, string SpawnID, int RoomID)
    {
        DeloadLevelSelect(LevelID);

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

    void QuitLoad(bool value)
    {
        if (WaitingForDeload)
        {
            WaitingForDeload = false;
            SceneManager.UnloadSceneAsync("LevelSelect");
            if (value == false)
            {
                EventManager.TriggerEvent("LS_EndLoad");
            }
        }
    }

    public void LoadLevel(string LevelID)
    {
        // To load a level: Load the level given, the background, and unload the LoadManager.
        DeloadLevelSelect(LevelID);
    }
    public void LoadLevel(string LevelID, string SpawnID, int RoomID)
    {

    }

}
