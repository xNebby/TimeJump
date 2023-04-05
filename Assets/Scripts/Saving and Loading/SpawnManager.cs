using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : SingletonClass<SpawnManager>
{
    public Dictionary<int, Vector2> Respawns;
    public int CurrentSpawn;
    public int PlayerSpawn;

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
        LogSystem.Log("SpawnManager", "Reset Vars");
        Respawns = new Dictionary<int, Vector2>();
        Respawns.Add(0, Vector2.zero);
        CurrentSpawn = 0;
        PlayerSpawn = 0;
    }

    public int AddSpawn(Vector2 Location)
    {
        CurrentSpawn += 1;
        Respawns.Add(CurrentSpawn, Location);
        return (CurrentSpawn);
    }
    public void SetSpawn(int SpawnID)
    {
        if (Respawns.ContainsKey(SpawnID))
        {
            PlayerSpawn = SpawnID;
        }
    }
    public Vector2 GetSpawn(int SpawnID)
    {
        if (Respawns.ContainsKey(SpawnID))
        {
            return (Respawns[SpawnID]);
        }
        else
        {
            return (Vector2.zero);
        }
    }
    public Vector2 RespawnPlayer()
    {
        return (Respawns[PlayerSpawn]);
    }
}
