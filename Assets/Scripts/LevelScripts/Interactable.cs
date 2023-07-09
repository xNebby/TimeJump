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

    public bool PlayerInHitbox;
    private Rigidbody2D RB;
    private BoxCollider2D BC;

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
            string EventName = "Interaction_" + InteractionEventName + "_Primary";
            EventManager.TriggerEvent(EventName);
        }
    }
    void SecondaryInputGiven()
    {
        if (AwaitingInput)
        {
            string EventName = "Interaction_" + InteractionEventName + "_Secondary";
            EventManager.TriggerEvent(EventName);
        }
    }

    private void InvokeEvent()
    {
        string EventName = "Interaction_" + InteractionEventName + "_Invoked";
        EventManager.TriggerEvent(EventName);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {

            //LogSystem.Log(gameObject, "Trigger entered");
            // Add a check that it isnt in a cutscene.
            if (InteractionEnabled)
            {
                if (RequireInput)
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
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //LogSystem.Log(gameObject, "Exited trigger");
            AwaitingInput = false;
            string EventName = "Interaction_" + InteractionEventName + "_Revoked";
            EventManager.TriggerEvent(EventName);
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
