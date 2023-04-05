using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSpawnpoint : MonoBehaviour
{
    public int SpawnID = -1;
    public string SpawnName; // Cosmetic used to tell where players are (UPDATE PER INDIVIDUAL SPAWN)
    public bool WorldSpawn = false; // Use one per loaded scene!
    void Awake()
    {
        //LogSystem.Log("SpawnPoint", "Assign Spawn");
        if (SpawnID == -1)
        {
            SpawnID = SpawnManager.Instance.AddSpawn(gameObject.transform.position);
        }
        if (WorldSpawn == true)
        {
            //string text = SpawnID.ToString() + " Is set as the world spawn";
            //LogSystem.Log("SpawnPoint", text);
            SpawnManager.Instance.SetSpawn(SpawnID);
        }
        if (SpawnName == "")
        {
            SpawnName = gameObject.name;
        }
    }
}
