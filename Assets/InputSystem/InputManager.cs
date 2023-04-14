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

    public override void Awake()
    {
        Keylist = new Dictionary<string, Vector3>();
        ID_MovementVector = Vector2.zero;
        CurrentPriority = 0;
        base.Awake();
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
        PlayerMovementManager.Instance.IM_UpdateVelocity(ID_MovementVector);
    }
}
