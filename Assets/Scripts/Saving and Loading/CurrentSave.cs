using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentSave : SingletonClass<CurrentSave>
{
    public string CurrentLoadedLevel;
    public Dictionary<string, LevelStats> Levels;
    public TotalStats m_TotalStats;

    void OnEnable()
    {
        UnloadSave();
    }

    public void CurrentLevel(string LevelName)
    {
        CurrentLoadedLevel = LevelName;
    }

    public void Store()
    {
        if (Levels.ContainsKey(CurrentLoadedLevel))
        {
            if (Levels[CurrentLoadedLevel].m_QuickestTime > Stats.Instance.CurrentPlaytime)
            {
                Levels[CurrentLoadedLevel].m_QuickestTime = Stats.Instance.CurrentPlaytime;
            }
        }
        else
        {
            LevelStats Temp = new LevelStats();
            Temp.m_LevelName = CurrentLoadedLevel;
            Temp.m_Deaths = Stats.Instance.PlayerDeaths;
            Temp.m_TimeTaken = Stats.Instance.CurrentPlaytime;
            Temp.m_QuickestTime = Stats.Instance.CurrentPlaytime;
            Levels.Add(CurrentLoadedLevel, Temp);
        }
        m_TotalStats.m_TotalTime += Stats.Instance.CurrentPlaytime;
        m_TotalStats.m_TotalDeaths += Stats.Instance.PlayerDeaths;

        // Write changes to file.
    }

    public void LevelStatus(bool Status)
    {
        Store();
        if (Levels.ContainsKey(CurrentLoadedLevel))
        {
            Levels[CurrentLoadedLevel].m_LevelCompleted = Status;
        }
    }

    public void LoadSave(int SaveID)
    {
        // Load the save from file
    }

    public void UnloadSave()
    {
        Levels = new Dictionary<string, LevelStats>();
        m_TotalStats = new TotalStats();
    }

    public float RetrievePlaytime()
    {
        return (m_TotalStats.m_TotalTime);
    }
}

public class LevelStats
{
    public string m_LevelName;
    public bool m_LevelCompleted;
    public int m_Deaths;
    public float m_TimeTaken;
    public float m_QuickestTime;
}

public class TotalStats
{
    public int m_TotalDeaths;
    public float m_TotalTime;
}
