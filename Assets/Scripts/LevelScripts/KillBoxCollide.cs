using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBoxCollide : MonoBehaviour
{
    void Start()
    {
        //LogSystem.Log("KillBox", "start");
    }

    private void OnTriggerEnter2D(Collider2D Entity)
    {
        //LogSystem.Log("KillBox", "Triggered");
        if (Entity.tag == "Player")
        {
            LogSystem.Log("KillBox", "Sent Kill Player command");
            PlayerManager.Instance.KillPlayer();
        }
    }
}
