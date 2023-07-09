using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    [SerializeField]
    private GameObject PlayerPrefab;
    [SerializeField]
    private GameObject CameraPrefab;
    [SerializeField]
    private GameObject LightPrefab;
    private Vector2 Spawnplace;

    // Start is called before the first frame update
    void Start()
    {
        LoadList();
    }

    void LoadList()
    {
        if (SceneManager.GetActiveScene().name == "Background")
        {
            Spawnplace = SpawnManager.Instance.RespawnPlayer();
            //Debug.Log(Spawnplace);
            LoadPlayer();
            LoadCam();
            LoadLight();
        }
    }

    void LoadPlayer()
    {
        Instantiate(PlayerPrefab, Spawnplace, Quaternion.identity);
        
    }
    void LoadCam()
    {
        Instantiate(CameraPrefab, Spawnplace, Quaternion.identity);
    }
    void LoadLight()
    {
        Instantiate(LightPrefab, Spawnplace, Quaternion.identity);
    }
}
