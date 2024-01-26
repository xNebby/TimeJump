using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : SingletonClass<SpawnManager>
{
    public Dictionary<string, SpawnEntry> Respawns;
    public int CurrentSpawn;
    public string PlayerSpawn;

    /*public void Start()
    {
        ResetVars();
    }*/
    public override void Awake()
    {
        ResetVars();
        base.Awake();
    }

    void ResetVars()
    {
        //LogSystem.Log("SpawnManager", "Reset Vars");
        Respawns = new Dictionary<string, SpawnEntry>();
        SpawnEntry TempEntry = new SpawnEntry();
        TempEntry.m_SpawnVector = Vector2.zero;
        TempEntry.m_SpawnID = 0;
        Respawns.Add("0", TempEntry);
        CurrentSpawn = 0;
        PlayerSpawn = "0";
    }

    public int AddSpawn(Vector2 Location, string Name, int RoomID)
    {
        if (Respawns.ContainsKey(Name) == false)
        {
            CurrentSpawn += 1;
            SpawnEntry TempEntry = new SpawnEntry();
            TempEntry.m_SpawnID = CurrentSpawn;
            TempEntry.m_SpawnVector = Location;
            TempEntry.m_RoomID = RoomID;
            Respawns.Add(Name, TempEntry);
            return (CurrentSpawn);
        } else
        {
            Debug.Log("Spawnpoint already exists: " + Name);
            return (-1);
        }
    }
    public void SetSpawn(string SpawnName)
    {
        if (Respawns.ContainsKey(SpawnName))
        {
            PlayerSpawn = SpawnName;
        }
    }
    public Vector2 GetSpawn(string SpawnName)
    {
        if (Respawns.ContainsKey(SpawnName))
        {
            
            return (Respawns[SpawnName].m_SpawnVector);
        }
        else
        {
            return (Vector2.zero);
        }
    }
    public Vector2 RespawnPlayer()
    {
        RoomLoader.Instance.LoadRoom(Respawns[PlayerSpawn].m_RoomID);
        return (Respawns[PlayerSpawn].m_SpawnVector);
    }
}

public class SpawnEntry
{
    public int m_SpawnID;
    public Vector2 m_SpawnVector;
    public int m_RoomID;
}