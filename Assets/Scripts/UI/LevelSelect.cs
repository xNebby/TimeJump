using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        EventManager.TriggerEvent("LevelSelectLoaded");
    }



}
