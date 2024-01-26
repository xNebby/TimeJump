using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonClass<PlayerManager>
{
    public Rigidbody2D PlayerRB;
    public GameObject PlayerGO;
    public PlayerMovementManager PMM;

    // Passing Vals
    public int PlayerHealth;
    public int PlayerMaxHealth = 100;

    public float RespawnTime = 5f;


    public override void Awake()
    {
        CheckVars();
        base.Awake();
    }

    void CheckVars()
    {
        PlayerGO = gameObject;
        PlayerRB = GetComponent<Rigidbody2D>();
        PMM = GetComponent<PlayerMovementManager>();
    }

    public bool DamagePlayer(int v_Damage, string v_DamageType, string v_DamageSource)
    {
        // Log the damage source. Should be used by any enemy
        string text = "Player Took " + v_Damage.ToString() + " of the " + v_Damage + " type, From the source " + v_DamageSource;
        LogSystem.Log("PlayerManager", text);
        bool t_Bool;
        t_Bool = DamagePlayer(v_Damage, v_DamageType);
        return t_Bool;
    }

    public bool DamagePlayer(int v_Damage, string v_DamageType)
    {
        // mostly just applies multiplies depending on the damage type provided. This shouldnt really be called by anything that is an enemy.
        if (v_DamageType.ToLower() == "none")
        {
            // no multiplier
        }

        bool t_Bool;
        t_Bool = DamagePlayer(v_Damage);
        return t_Bool;
    }

    public bool DamagePlayer(int v_Damage)
    {
        // Returns if the player is hit- allows for grapples and similar things for the enemy. ASWELL AS HIT SFX
        // Checks if the player is alive and can take damage- if they are, return true- if not return false.
        if (PlayerStateManager.Instance.PlayerIsAlive == true)
        {
            if (PlayerHealth > 0)
            {
                PlayerHealth -= v_Damage;
                return true;
            } else
            {
                return false;
            }
        } else
        {
            return false;
        }
    }

    public void UpdatePMM_IM_Vector(Vector2 IM_PlayerVector)
    {
        PMM.IM_UpdateVelocity(IM_PlayerVector);
    }
    public void UpdatePMM_MSM_Vector(Vector2 MSM_Vector)
    {
        PMM.MSM_UpdateVelocity(MSM_Vector);
    }
    public void UpdatePMM_MSM_Multiplier(float MSM_Multiplier)
    {
        PMM.MSM_UpdateMultiplier(MSM_Multiplier);
    }

    public void KillPlayer()
    {
        LogSystem.Log("PlayerManager",  "Player Died");
        PlayerHealth = 0;
        // Adds to death counter, Should start movement lock- change player sprite to dead player. 
        InputManager.Instance.UpdateVector(Vector2.zero, "Dead", 5);
        EventManager.TriggerEvent("PM_KillPlayer");
        EventManager.TriggerEvent("YouDied"); // Shows the "YOU DIED" text on screen

    }

    void RespawnPlayer()
    {
        if (PlayerStateManager.Instance.PlayerIsAlive == false)
        {
            EventManager.TriggerEvent("PM_CanRespawn");
            LogSystem.Log("PlayerManager", "Player Respawned");
            EventManager.TriggerEvent("PM_RespawnPlayer");
            PlayerRB.gameObject.transform.position = SpawnManager.Instance.RespawnPlayer();
            PlayerHealth = PlayerMaxHealth;
            // Disable Movement lock, change player from death model to alive model

            InputManager.Instance.RemoveVector("Dead");
        }
    }

    void FixedUpdate()
    {
        if (PlayerStateManager.Instance.PlayerIsAlive == true)
        {
            if (PlayerHealth == 0)
            {
                KillPlayer();
            }
            else if (PlayerHealth < 0)
            {
                PlayerHealth = 0;
                KillPlayer();
            }
        } 
        else
        {
            if (PlayerStateManager.Instance.PlayerCanRespawn == true)
            {
                EventManager.TriggerEvent("PM_CannotRespawn");
                TimerManager.AddTimer("PM_Respawn", RespawnTime, RespawnPlayer);
            }
        }
    }
}
