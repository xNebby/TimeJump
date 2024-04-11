using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public string CurrentPlayerCheckpoint;
    private bool WaitingForHide = false;
    private bool WaitingForDeload = false;
    private string file = "Save.txt";

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
        UnloadSave(SaveID);

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
        Load(SaveID);
        WaitingForHide = true;
        if (CurrentPlayerCheckpoint != null)
        {
            Debug.Log(CurrentPlayerCheckpoint);
            SpawnManager.Instance.SetSpawn(CurrentPlayerCheckpoint);
        }
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

    public void Load(int SaveID)
    {

        string json = ReadFromFIle(file + SaveID.ToString());
        JsonUtility.FromJsonOverwrite(json, );
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(data);
        WriteToFile(file, json);
    }

    private void WriteToFile(string fileName, string json)
    {
        string path = GetFilePath(fileName);
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(json);
        }
    }

    private string ReadFromFIle(string fileName)
    {
        string path = GetFilePath(fileName);
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }
        else
        {
            Debug.LogWarning("File not found");
        }

        return "Success";
    }

    private string GetFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
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
        CurrentSave.Instance.CurrentLevel(LevelName);
    }

}
