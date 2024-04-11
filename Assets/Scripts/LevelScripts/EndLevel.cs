using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    private Interactable m_Interactable;
    public string EventName;
    public bool Completed = false;

    void OnEnable()
    {
        EventName = "FinishLevel";
        m_Interactable.InteractionEventName = EventName;
        EventName = "Interaction_" + EventName;
        EventManager.StartListening(EventName + "_Invoked", FinishedLevel);
    }
    void OnDisable()
    {
        EventManager.StopListening(EventName + "_Invoked", FinishedLevel);
    }

    void FinishedLevel()
    {
        Completed = true;
        ExitLevel();
    }

    void ExitLevel()
    {
        if (CurrentSave.Instance.LevelStatus())
        {
            CurrentSave.Instance.LevelStatus(true);
        } else
        {
            CurrentSave.Instance.LevelStatus(Completed);
        }
    }
}
