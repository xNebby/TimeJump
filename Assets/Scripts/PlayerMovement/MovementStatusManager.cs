using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStatusManager : SingletonClass<MovementStatusManager>
{

    public Dictionary<string, EffectTicket> EffectTickets = new Dictionary<string, EffectTicket>();

    /*
    public Dictionary<string, Vector3> EffectIDs = new Dictionary<string, Vector3>();
    public Dictionary<string, Vector3> EffectIDsClone = new Dictionary<string, Vector3>();
    // X - Vector List location Y - Multiplier list location Z - Timer list location
    public List<Vector2> VectorList = new List<Vector2>();
    public List<float> MultiplierList = new List<float>();
    public List<int> TimerList = new List<int>();
    public List<Vector3> FreeLocations = new List<Vector3>();
    */
    public bool TimerListChanged = true;
    public float MultiplierSum = 1f;
    public Vector2 MSM_StatusVector = Vector2.zero;

    public override void Awake()
    {
        ClearLists();
        base.Awake();
    }
    void OnEnable()
    {
        ClearLists();
    }
    void ClearLists()
    {
        EffectTickets = new Dictionary<string, EffectTicket>();
        /*
        EffectIDs = new Dictionary<string, Vector3>();
        EffectIDsClone = new Dictionary<string, Vector3>();
        VectorList = new List<Vector2>();
        TimerList = new List<int>();
        MultiplierList = new List<float>();
        FreeLocations = new List<Vector3>();
        */
        MSM_StatusVector = Vector2.zero;
        MultiplierSum = 1f;
        TimerListChanged = true;

    }

    public void RemoveEffect(string Name)
    {
        TimerListChanged = true;
        MultiplierSum /= EffectTickets[Name].m_Multiplier;
        MSM_StatusVector -= EffectTickets[Name].m_Vector;
        EffectTickets.Remove(Name);
        /*
        FreeLocations.Add(EffectIDs[Name]);
        VectorList[Mathf.RoundToInt(EffectIDs[Name].x)] = Vector2.zero;
        MultiplierList[Mathf.RoundToInt(EffectIDs[Name].y)] = 1f;
        TimerList[Mathf.RoundToInt(EffectIDs[Name].z)] = 0;
        EffectIDs.Remove(Name);
        SumLists();*/
    }

    public void AddTimedMovementEffect(string Name, Vector2 Velocity, float Multiplier, float Timer)
    {
        AddNamedEffect(Name);
        AddVelocityEffect(Name, Velocity);
        AddMultiplierEffect(Name, Multiplier);
        AddTimer(Name, Timer);
    }
    public void AddTimedMovementEffect(string Name, Vector2 Velocity, float Timer)
    {
        AddNamedEffect(Name);
        AddVelocityEffect(Name, Velocity);
        AddTimer(Name, Timer);
    }
    public void AddTimedMovementEffect(string Name, float Multiplier, float timer)
    {
        AddNamedEffect(Name);
        AddMultiplierEffect(Name, Multiplier);
        AddTimer(Name, timer);
    }
    public void AddMovementEffect(string Name, Vector2 Velocity, float Multiplier)
    {
        AddNamedEffect(Name);
        AddVelocityEffect(Name, Velocity);
        AddMultiplierEffect(Name, Multiplier);
    }
    public void AddMovementEffect(string Name, float Multiplier)
    {
        AddNamedEffect(Name);
        AddMultiplierEffect(Name, Multiplier);
    }

    public void AddNamedEffect(string Name)
    {
        if (EffectTickets.ContainsKey(Name))
        {
            RemoveEffect(Name);
        }
        EffectTicket tempTicket = new EffectTicket();
        EffectTickets.Add(Name, tempTicket);
        /*
        if (FreeLocations.Count > 0)
        {
            EffectIDs.Add(Name, FreeLocations[0]);
            FreeLocations.RemoveAt(0);
        }
        else
        {
            EffectIDs.Add(Name, new Vector3(VectorList.Count, MultiplierList.Count, TimerList.Count));
            MultiplierList.Add(1f);
            VectorList.Add(Vector2.zero);
            TimerList.Add(-1);
        }
        */
    }
    public void AddMultiplierEffect(string Name, float Multiplier)
    {
        if (EffectTickets.ContainsKey(Name))
        {
            EffectTickets[Name].m_Multiplier = Multiplier;
            //MultiplierList[Mathf.RoundToInt(EffectIDs[Name].y)] = Multiplier;
            MultiplierSum *= Multiplier;
            updateVars();
        }
    }
    public void AddVelocityEffect(string Name, Vector2 Velocity)
    {
        if (EffectTickets.ContainsKey(Name))
        {
            EffectTickets[Name].m_Vector = Velocity;
            //VectorList[Mathf.RoundToInt(EffectIDs[Name].x)] = Velocity;
            MSM_StatusVector += Velocity;
            updateVars();
        }
    }
    public void AddTimer(string Name, float Time)
    {
        if (EffectTickets.ContainsKey(Name))
        {
            EffectTickets[Name].m_Timer = Time;
            //TimerList[Mathf.RoundToInt(EffectIDs[Name].z)] = Time;
            TimerListChanged = true;
        }

    }
    /*
    void SumLists()
    {
        MultiplierSum = 1f;
        MSM_StatusVector = Vector2.zero;
        for (int index = 0; index < MultiplierList.Count; index++)
        {
            MultiplierSum *= MultiplierList[index];
        }
        for (int index = 0; index < VectorList.Count; index++)
        {
            MSM_StatusVector += VectorList[index];
        }
        updateVars();
    }
    */
    void updateVars()
    {
        PlayerManager.Instance.UpdatePMM_MSM_Vector(MSM_StatusVector);
        PlayerManager.Instance.UpdatePMM_MSM_Multiplier(MultiplierSum);
    }

    public void FixedUpdate()
    {
        /*
        if (EffectIDs.Count > 0)
        {
            if (TimerListChanged == true)
            {
                EffectIDsClone = new Dictionary<string, Vector3>(EffectIDs);
                TimerListChanged = false;
            }
            foreach (string KeyVar in EffectIDsClone.Keys)
            {
                if (TimerList[Mathf.RoundToInt(EffectIDs[KeyVar].z)] > -1)
                {
                    TimerList[Mathf.RoundToInt(EffectIDs[KeyVar].z)] = TimerList[Mathf.RoundToInt(EffectIDs[KeyVar].z)] - 1;
                    if (TimerList[Mathf.RoundToInt(EffectIDs[KeyVar].z)] == 0)
                    {
                        RemoveEffect(KeyVar);
                    }
                }
            }
        } */
        if (EffectTickets.Count > 0)
        {
            Dictionary<string, EffectTicket> EffectTicketsClone = new Dictionary<string, EffectTicket>(EffectTickets);
            foreach (string Key in EffectTicketsClone.Keys)
            {
                if (EffectTicketsClone[Key].m_Timer > -1)
                {
                    EffectTicketsClone[Key].m_Timer -= Time.fixedDeltaTime; 
                    if (EffectTicketsClone[Key].m_Timer <= 0)
                    {
                        RemoveEffect(Key);
                    }
                } 
            }
        }
    }
}


public class EffectTicket
{
    public Vector2 m_Vector = Vector2.zero;
    public float m_Multiplier = 1f;
    public float m_Timer = -1;
}