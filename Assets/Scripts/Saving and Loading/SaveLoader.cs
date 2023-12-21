using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoader : SingletonClass<SaveLoader>
{

    // Functions to add:
    // Create save, Load save, Delete save.
    // new game: Deletes save, then creates a new one, then loads it. 
    // Continue: Loads game 
    // Load game: Load into the level select. Enable the scene Async, passing along the save 

    public int CurrentLoadedSave = 0;
    public string CurrentLoadedLevel;
    private bool WaitingForHide = false;
    private bool WaitingForDeload = false;

    void OnEnable()
    {
        EventManager.TriggerEvent("SaveLoader_Loaded");
        EventManager.StartListening("LevelSelectLoaded", FinaliseLoad);
        EventManager.StartListening("LS_Hidden", HideConfirm);
    }

    void OnDisable()
    {
        EventManager.StopListening("LS_Hidden", HideConfirm);
        EventManager.StopListening("LevelSelectLoaded", FinaliseLoad);
    }

    public void NewGame(int SaveID)
    {
        DeleteSave(SaveID);
        CreateSave(SaveID);
        LoadSave(SaveID);
    }

    public void Continue(int SaveID)
    {
        LoadSave(SaveID);
    }

    void FinaliseLoad()
    {
        if (WaitingForDeload)
        {
            WaitingForDeload = false;
            //Debug.Log("Unload Menu");
            SceneManager.UnloadSceneAsync("Menu");
            EventManager.TriggerEvent("LS_EndLoad");
        }
    }

    public void LoadSave(int SaveID)
    {
        WaitingForDeload = true;
        CurrentLoadedSave = SaveID;
        // Load the level select, deload main menu
        WaitingForHide = true;
        EventManager.TriggerEvent("LS_Load");

    }

    void HideConfirm()
    {
        //Debug.Log("HideConfirm Called");
        if (WaitingForHide == true)
        {
            WaitingForHide = false;
            SceneManager.LoadScene("LevelSelect", LoadSceneMode.Additive);
        }
    }

    public void CreateSave(int SaveID)
    {
        // Copy a template to the save ID provided


    }

    public void DeleteSave(int SaveID)
    {



    }

    public void UnloadSave(int SaveID)
    {
        CurrentSave.Instance.UnloadSave();
        CurrentLoadedSave = 0;
        // Load the main menu, deload Level select

    }

    public void CurrentLevel(string LevelName)
    {
        CurrentLoadedLevel = LevelName;
    }

}
