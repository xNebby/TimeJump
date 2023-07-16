using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Vibration_Test : MonoBehaviour
{

    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;
    private float VibrationLevel = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("joystick button 0"))
        {
            GamePad.SetVibration(playerIndex, VibrationLevel, VibrationLevel);
        }
        if (Input.GetKeyDown("joystick button 1"))
        {
            GamePad.SetVibration(playerIndex, 0f, 0f);
        }
    }
}
