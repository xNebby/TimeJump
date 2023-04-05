using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    [SerializeField]
    private GameObject PlayerPrefab;
    private Vector2 Spawnplace;

    // Start is called before the first frame update
    void Start()
    {
        LoadPlayer();
    }


    void LoadPlayer()
    {
        if (SceneManager.GetActiveScene().name == "Background")
        {
            Spawnplace = SpawnManager.Instance.RespawnPlayer();
            //Debug.Log(Spawnplace);
            Instantiate(PlayerPrefab, Spawnplace, Quaternion.identity);
        }
    }
}
