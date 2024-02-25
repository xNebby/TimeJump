using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSpawnpoint : MonoBehaviour
{
    public int SpawnID = -1;
    public string SpawnName;
    public int RoomID;
    public bool WorldSpawn = false; // Use one per loaded scene!
    public bool Interactable = false;
    public bool Visible = true;

    private SpriteRenderer spriteRenderer;
    public Sprite PoweredRespawn;
    private Interactable m_Interactable;
    private string EventName;
    private Animator m_Animator;
    private GameObject Light;
    void OnEnable() 
    {
        if (SpawnName == "")
        {
            SpawnName = gameObject.name;
        }
        EventManager.StartListening("RespawnAnimation_Finished", ChangeSprite);
        m_Animator = GetComponent<Animator>();
        m_Interactable = gameObject.transform.GetChild(0).GetComponent<Interactable>();
        Light = gameObject.transform.GetChild(1).gameObject;
        EventName = "Respawn" + SpawnID.ToString();
        m_Interactable.InteractionEventName = EventName;
        EventName = "Interaction_" + EventName;
        EventManager.StartListening(EventName + "_Invoked", PowerRespawn);
        //EventManager.StartListening(EventName + "_Primary", Primary);
        //EventManager.StartListening(EventName + "_Secondary", Secondary);
        //EventManager.StartListening(EventName + "_Revoked", Close);



        if (Visible == false)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;
        }
        //LogSystem.Log("SpawnPoint", "Assign Spawn");
        if (SpawnID == -1)
        {
            SpawnID = SpawnManager.Instance.AddSpawn(gameObject.transform.position, SpawnName, RoomID);
        }
        if (WorldSpawn == true)
        {
            //string text = SpawnID.ToString() + " Is set as the world spawn";
            //LogSystem.Log("SpawnPoint", text);
            if (SpawnManager.Instance.PlayerSpawn == "0")
            {
                SpawnManager.Instance.SetSpawn(SpawnName);
            }
        }
        
    }

    void OnDisable()
    {
        EventManager.StopListening("RespawnAnimation_Finished", ChangeSprite);

        EventManager.StopListening(EventName + "_Invoked", PowerRespawn);
        //EventManager.StopListening(EventName + "_Primary", Primary);
        //EventManager.StopListening(EventName + "_Secondary", Secondary);
        //EventManager.StopListening(EventName + "_Revoked", Close);
        SpawnManager.Instance.SetSpawn(SpawnName);
    }

    void PowerRespawn()
    {
        //Debug.Log("event Received");
        if (Interactable)
        {
            if (m_Interactable.PlayerInHitbox)
            {
                // Add an interaction to the interactions script.
                m_Animator.SetTrigger("Activate");
                SpawnManager.Instance.SetSpawn(SpawnName);
            }
        }
    }

    void ChangeSprite()
    {
        // If the player is currently touching the hitbox, change this sprite to active mode.
        Debug.Log("Animation Finished");
        //spriteRenderer.sprite = PoweredRespawn;
        Light.SetActive(true);
    }
}
