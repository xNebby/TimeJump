using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public int PlayerDeaths;
    public int Playtime;
    public int CurrentPlaytime;

    void LoadStats()
    {
        CurrentPlaytime = 0;
        // Load the stats from file, if file does not exist load the default stats (Following stats:)
        PlayerDeaths = 0;
        Playtime = 0;
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

    void PlayerDies()
    {
        PlayerDeaths++;
    }
    void FixedUpdate()
    {
        Playtime++;
        CurrentPlaytime++;
    }
}
