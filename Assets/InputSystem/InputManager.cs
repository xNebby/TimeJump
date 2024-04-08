using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingletonClass<InputManager>
{
    // Requests are sent to this given priority, the higher the priority the way it overrules. The vectors must be removed also, as they do not have timers.
    // This exists to be an interpreter for the players inputs, handles other input methods preventing player inputs also
    public Dictionary<string, Vector3> Keylist = new Dictionary<string, Vector3>();
    public Vector2 ID_MovementVector;
    public int CurrentPriority;
    public Vector2 IM_PlayerVector;
    public Dictionary<string, JumpPriorityTicket> JumpList = new Dictionary<string, JumpPriorityTicket>();
    public int CurrentJumpPriority;
    public bool IM_JumpBool;

    public override void Awake()
    {
        Keylist = new Dictionary<string, Vector3>();
        ID_MovementVector = Vector2.zero;
        CurrentPriority = 0;
        CurrentJumpPriority = 0;
        IM_JumpBool = false;
        JumpList = new Dictionary<string, JumpPriorityTicket>();
        /*JumpPriorityTicket DefaultVar;
        DefaultVar.constructor(-1, "Default", false);
        JumpList.Add("Default", DefaultVar);*/
        base.Awake();
    }

    public void ChangedJump()
    {
        string TempText = "The jumpbool is " + IM_JumpBool.ToString(); 
        //LogSystem.Log(gameObject, TempText);
        if (IM_JumpBool == true)
        {
            EventManager.TriggerEvent("IM_StartJump");
        } else
        {
            EventManager.TriggerEvent("IM_StopJump");
        }
    }

    public void AddJumpTicket(int v_Priority, string v_Source, bool v_Jump)
    {
        JumpPriorityTicket TempTicket = new JumpPriorityTicket();
        TempTicket.constructor(v_Priority, v_Source, v_Jump);
        if (JumpList.ContainsKey(v_Source))
        {
            JumpList[v_Source] = TempTicket;
        } else
        {
            JumpList.Add(v_Source, TempTicket);
        }
        if (v_Priority >= CurrentJumpPriority)
        {
            CurrentJumpPriority = v_Priority;
            IM_JumpBool = v_Jump;
            ChangedJump();
        }
    }
    public bool RemoveJumpTicket(string v_Source)
    {
        if (JumpList.ContainsKey(v_Source))
        {
            //LogSystem.Log(gameObject, v_Source + " is found in the jumplist dictionary");
            JumpList.Remove(v_Source);
            JumpPriorityCheck();
            ChangedJump();
            return true;
        } else
        {
            //LogSystem.Log(gameObject, v_Source + " is not found in the jumplist dictionary");
            return false;
        }
    }
    /*
    public void AddJumpTicket(int v_Priority, bool v_Jump)
    {
        JumpPriorityTicket TempTicket = new JumpPriorityTicket();
        TempTicket.constructor(v_Priority, v_Source, v_Jump);
    }*/

    public void JumpPriorityCheck()
    {
        IM_JumpBool = false;
        CurrentJumpPriority = 0;
        Dictionary<string, JumpPriorityTicket> JumpListClone = new Dictionary<string, JumpPriorityTicket>(JumpList);
        foreach (string key in JumpListClone.Keys)
        {
            if (JumpList[key].Priority > CurrentJumpPriority)
            {
                IM_JumpBool = JumpList[key].Jump;
                CurrentJumpPriority = JumpList[key].Priority;
            }
        }
    }

    // The .z of the vector3 contains the priority, the .x and .y represent the vector.
    public void UpdateVector(Vector2 v_Velocity, string v_Name)
    {
        int TempPriority;
        if (v_Name == "Player")
        {
            TempPriority = 0;
        } else
        { // Unknown string passed, will not be applied. 
            TempPriority = -1;
        }
        UpdateVector(v_Velocity, v_Name, TempPriority);
    }

    public void UpdateVector(Vector2 v_Velocity, string v_Name, int v_Priority)
    {
        //Debug.Log("h");
        Vector3 tempVector = new Vector3(v_Velocity.x, v_Velocity.y, v_Priority); 
        if (v_Priority >= 0)
        {
            if (CurrentPriority < v_Priority)
            {
                CurrentPriority = v_Priority;
                if (Keylist.ContainsKey(v_Name))
                {
                    Keylist[v_Name] = tempVector;
                } else
                {
                    Keylist.Add(v_Name, tempVector);
                }
                ID_MovementVector = v_Velocity;
                ChangedVector();
            }
            else if (CurrentPriority == v_Priority)
            {
                if (Keylist.ContainsKey(v_Name))
                {
                    Keylist[v_Name] = tempVector;
                    CheckVector();
                }
                else
                {
                    Keylist.Add(v_Name, tempVector);
                    ID_MovementVector += v_Velocity;
                    ChangedVector();
                }
            }
            else if (CurrentPriority > v_Priority)
            {
                if (Keylist.ContainsKey(v_Name))
                {
                    Keylist[v_Name] = tempVector;
                } else
                {
                    Keylist.Add(v_Name, tempVector);
                }
            }
        }
    }
    public void RemoveVector(string v_Name)
    {
        if (Keylist.ContainsKey(v_Name))
        {
            if (Keylist[v_Name].z == CurrentPriority)
            {// The priority of the removed value is equal to the current instruction being used.
                Keylist.Remove(v_Name);
                CheckVector();
            } 
            else
            {// Dont need to check the current value, as the value being used is higher priority anyways. 
                Keylist.Remove(v_Name);
            }
        }
    }
    private void CheckVector()
    {
        CurrentPriority = 0;
        ID_MovementVector = Vector2.zero;
        Dictionary<string, Vector3> KeyListClone = new Dictionary<string, Vector3>(Keylist);
        foreach(string key in KeyListClone.Keys)
        {
            if (KeyListClone[key].z == CurrentPriority)
            {
                ID_MovementVector += new Vector2(KeyListClone[key].x, KeyListClone[key].y);
            } 
            else if (KeyListClone[key].z > CurrentPriority)
            {
                CurrentPriority = System.Convert.ToInt32(KeyListClone[key].z);
                ID_MovementVector = new Vector2(KeyListClone[key].x, KeyListClone[key].y);
            }
        }
        ChangedVector();
    }

    public void ChangedVector()
    {
        //Debug.Log("uhhhhhhhh");
        PlayerManager.Instance.UpdatePMM_IM_Vector(ID_MovementVector);

        IM_PlayerVector = ID_MovementVector;
        
        /*
        if (IM_PlayerVector == Vector2.zero)
        {
            EventManager.TriggerEvent("PAH_Idle");
        }*/

        if (IM_PlayerVector.x < 0)
        {
            EventManager.TriggerEvent("FlipScript_Flip");
        } else if (IM_PlayerVector.x > 0)
        {
            EventManager.TriggerEvent("FlipScript_UnFlip");
        }
    }
}

public class JumpPriorityTicket
{
    public int Priority;
    public string Source;
    public bool Jump;
    public void constructor(int v_Priority, string v_Source, bool v_Jump)
    {
        Source = v_Source;
        constructor(v_Priority, v_Jump);
    }
    public void constructor(int v_Priority, bool v_Jump)
    {
        Priority = v_Priority;
        Jump = v_Jump;
    }
}
