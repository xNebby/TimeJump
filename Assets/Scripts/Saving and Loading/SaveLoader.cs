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

    void OnEnable()
    {
        EventManager.StartListening("LevelSelectLoaded", FinaliseLoad);
    }

    void OnDisable()
    {
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
        SceneManager.UnloadSceneAsync("Menu");
        EventManager.TriggerEvent("LS_EndLoad");
    }

    public void LoadSave(int SaveID)
    {
        CurrentLoadedSave = SaveID;
        // Load the level select, deload main menu
        EventManager.TriggerEvent("LS_Load"); 
        SceneManager.LoadScene("LevelSelect", LoadSceneMode.Additive);


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
        CurrentLoadedSave = 0;
        // Load the main menu, deload Level select

    }
}
