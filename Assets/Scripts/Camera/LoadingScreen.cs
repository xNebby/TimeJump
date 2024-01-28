using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LoadingScreen : MonoBehaviour
{
    public bool Loading;
    public bool LoadWaiting;
    public bool VisibleState = false;
    public float Counter;
    public float TimePeriod = 2f;
    public float Magnitude;
    public Vector3 TransformVector = new Vector3(0, 0, 0);
    public Vector3 PositionVector = new Vector3(0, 0, 0);

    public void OnEnable()
    {
        
        EventManager.StartListening("LS_Load", Load);
        EventManager.StartListening("LS_EndLoad", EndLoad);
        /*if (VisibleState)
        {
            EndLoad();
        }*/
    }
    public void OnDisable()
    {
        EventManager.StopListening("LS_Load", Load);
        EventManager.StopListening("LS_EndLoad", EndLoad);

    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (Loading || VisibleState)
            {
                EndLoad();
            } else
            {
                Load();
            }
        }

        if (Loading == true)
        {
            Counter += Time.deltaTime;
            Magnitude = Mathf.Sin((Counter * Mathf.PI / (TimePeriod * 2)) + (Mathf.PI * 3 / 2)) + 1;
            gameObject.transform.localPosition = (TransformVector * Magnitude) + PositionVector;
            if (Counter >= TimePeriod)
            {
                Loading = false;
                if (VisibleState)
                {
                    VisibleState = false;
                    EventManager.TriggerEvent("LS_Visible");
                    PositionVector = Vector3.zero;
                } else
                {
                    VisibleState = true;
                    EventManager.TriggerEvent("LS_Hidden");
                    PositionVector = TransformVector;
                }

                if (LoadWaiting == true)
                {
                    LoadWaiting = false;
                    Loading = true;
                    VisibleState = true;
                    PositionVector = TransformVector;
                    Counter = 0;
                }
            }
        } else
        {
            gameObject.transform.localPosition = PositionVector;
        }
    }

    void Load()
    {
        if (Loading == true)
        {
            LoadWaiting = true;
        }
        else
        {
            Loading = true;
            VisibleState = false;
            PositionVector = Vector3.zero;
            Counter = 0;
        }
    }
    void EndLoad()
    {
        if (Loading == true)
        {

        } else
        {
            Loading = true;
            VisibleState = true;
            PositionVector = TransformVector;
            Counter = 0;
            LoadWaiting = false;
        }
    }
    void StartShake()
    {
        Counter = 0;
        Loading = true;
    }
}
