using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : SingletonClass<Stats>
{
    public int PlayerDeaths;
    public float Playtime;
    public float CurrentPlaytime;
    public List<string> CurrentSpawnpoints;
    public string MostRecentSpawnpoint;

    void LoadStats()
    {
        
    }
    void SaveStats()
    {
        // Save all the variables- Except for current playtime, which can be discarded.
    }
    void OnEnable()
    {
        LoadStats();
        EventManager.StartListening("PM_KillPlayer", PlayerDies);
    }
    void OnDisable()
    {
        LoadStats();
        EventManager.StopListening("PM_KillPlayer", PlayerDies);
    }
    void UpdateSpawnpoint(string v_spawnName)
    {
        if (!CurrentSpawnpoints.Contains(v_spawnName))
        {
            CurrentSpawnpoints.Add(v_spawnName);
        }
        MostRecentSpawnpoint = v_spawnName;
        CurrentSave.Instance.StoreLevel();
    }

    void PlayerDies()
    {
        PlayerDeaths++;
    }
    void FixedUpdate()
    {
        Playtime++;
        CurrentPlaytime += Time.fixedDeltaTime;
    }
}
