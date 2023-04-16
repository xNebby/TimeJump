using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : SingletonClass<GameSettings>
{
    // V_ = Variable which may be modified by other scripts
    // F_ = Function which may be accessed by other scripts
    // _ = another script which might be accessed
    // T_ = Temporary variable used in functions

    public bool ToggleSprint = true;
    public bool HoldSprint = false;
    public int FPSCount = 75;
    public bool VSyncEnabled = false;
    public bool HoldJump = false;
    public bool TapUpToJump = false;
    public bool MovementStopEndSprint = false;
    public bool DownToCrouch = true;

    public override void Awake()
    {
        ReloadSettings();
        base.Awake();
    }

    public void ReloadSettings()
    {
        FPSReload();
        SprintSettingsReload();

    }

    public void FPSReload()
    {
        Application.targetFrameRate = FPSCount;
        if (VSyncEnabled)
        {
            QualitySettings.vSyncCount = 1;
        }
        else if (VSyncEnabled == false)
        {
            QualitySettings.vSyncCount = 0;
        }
    }

    public void SprintSettingsReload()
    {

    }

}
