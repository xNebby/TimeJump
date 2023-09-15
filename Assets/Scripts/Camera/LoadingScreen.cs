using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LoadingScreen : MonoBehaviour
{
    public bool Loading;
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
            StartShake();
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
                    PositionVector = Vector3.zero;
                } else
                {
                    VisibleState = true;
                    PositionVector = TransformVector;
                }
            }
        } else
        {
            gameObject.transform.localPosition = PositionVector;
        }
    }

    void Load()
    {
        Loading = true;
        VisibleState = false;
        PositionVector = Vector3.zero;
        Counter = 0;
    }
    void EndLoad()
    {
        Counter = 0;
        VisibleState = true;
        PositionVector = TransformVector;
        Loading = true;
    }
    void StartShake()
    {
        Counter = 0;
        Loading = true;
    }
}
