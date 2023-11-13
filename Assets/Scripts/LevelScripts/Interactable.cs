using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Interactable : MonoBehaviour
{

    // Script is given to anything that the player interacts with. Different options for requiring input or not. Needs to go in the hitbox. 
    public bool InteractionEnabled;
    public string InteractionEventName;

    public bool RequireInput;
    public bool AwaitingInput;
    public bool PrimaryInput;
    public bool SecondaryInput;

    public bool WiringInput;

    public bool PlayerInHitbox;
    private Rigidbody2D RB;
    private BoxCollider2D BC;

    private int ObjCounter;

    void OnEnable()
    {
        if (gameObject.name != "Hitbox")
        {
            LogSystem.LogError(gameObject, "No Hitbox detected. Please put the Interactable in the hitbox.");
        }
        else
        {
            BC = GetComponent<BoxCollider2D>();
            RB = GetComponent<Rigidbody2D>();
        }
        EventManager.StartListening("ID_PrimaryInput", PrimaryInputGiven);
        EventManager.StartListening("ID_SecondaryInput", SecondaryInputGiven);

        ObjCounter = 0;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }

    void OnDisable()
    {

        EventManager.StopListening("ID_PrimaryInput", PrimaryInputGiven);
        EventManager.StopListening("ID_SecondaryInput", SecondaryInputGiven);
    }

    void PrimaryInputGiven()
    {
        if (AwaitingInput)
        {
            if (PrimaryInput)
            {
                string EventName = "Interaction_" + InteractionEventName + "_Primary";
                EventManager.TriggerEvent(EventName);
            }
        }
    }
    void SecondaryInputGiven()
    {
        if (AwaitingInput)
        {
            if (SecondaryInput)
            {
                string EventName = "Interaction_" + InteractionEventName + "_Secondary";
                EventManager.TriggerEvent(EventName);
            }
        }
    }

    private void InvokeEvent()
    {
        string EventName = "Interaction_" + InteractionEventName + "_Invoked";
        //Debug.Log("Sent event:");
        //Debug.Log(EventName);
        EventManager.TriggerEvent(EventName);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            ObjCounter += 1;
            //LogSystem.Log(gameObject, "Trigger entered");
            // Add a check that it isnt in a cutscene.
            if (InteractionEnabled)
            {
                if (RequireInput || WiringInput)
                {
                    AwaitingInput = true;
                    // Add stuff to display the input buttons.
                    if (PrimaryInput)
                    {
                        gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    }
                    else
                    {
                        gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    }
                    if (SecondaryInput)
                    {
                        gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    }
                    else
                    {
                        gameObject.transform.GetChild(1).gameObject.SetActive(false);
                    }
                }
                else
                {
                    InvokeEvent();
                }
            }
        }
        if (other.gameObject.tag == "PhysicsObj")
        {
            ObjCounter += 1;
            if (RequireInput == false)
            {
                InvokeEvent();
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "PhysicsObj")
        {
            ObjCounter -= 1;
            if (ObjCounter < 0)
            {
                ObjCounter = 0;
            }
            if (ObjCounter == 0)
            {
                //LogSystem.Log(gameObject, "Exited trigger");
                AwaitingInput = false;
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
                gameObject.transform.GetChild(1).gameObject.SetActive(false);
                if (WiringInput == false)
                {
                    string EventName = "Interaction_" + InteractionEventName + "_Revoked";
                    EventManager.TriggerEvent(EventName);
                }
            }
        }
    }
}
