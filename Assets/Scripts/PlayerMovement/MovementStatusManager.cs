using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStatusManager : SingletonClass<MovementStatusManager>
{

    public Dictionary<string, EffectTicket> EffectTickets = new Dictionary<string, EffectTicket>();
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
        MSM_StatusVector = Vector2.zero;
        MultiplierSum = 1f;

    }

    public void RemoveEffect(string Name)
    {
        EffectTickets.Remove(Name);
        SumLists();
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
    }
    public void AddMultiplierEffect(string Name, float Multiplier)
    {
        if (EffectTickets.ContainsKey(Name))
        {
            EffectTickets[Name].m_Multiplier = Multiplier;
            MultiplierSum *= Multiplier;
            updateVars();
        }
    }
    public void AddVelocityEffect(string Name, Vector2 Velocity)
    {
        if (EffectTickets.ContainsKey(Name))
        {
            EffectTickets[Name].m_Vector = Velocity;
            MSM_StatusVector += Velocity;
            updateVars();
        }
    }
    public void AddTimer(string Name, float Time)
    {
        if (EffectTickets.ContainsKey(Name))
        {
            EffectTickets[Name].m_Timer = Time;
        }

    }
    void SumLists()
    {
        MultiplierSum = 1f;
        MSM_StatusVector = Vector2.zero;
        foreach (string Key in EffectTickets.Keys)
        {
            MultiplierSum *= EffectTickets[Key].m_Multiplier;
            MSM_StatusVector += EffectTickets[Key].m_Vector;
        }
        updateVars();
    }
    void updateVars()
    {
        PlayerManager.Instance.UpdatePMM_MSM_Vector(MSM_StatusVector);
        PlayerManager.Instance.UpdatePMM_MSM_Multiplier(MultiplierSum);
    }

    public void FixedUpdate()
    {
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