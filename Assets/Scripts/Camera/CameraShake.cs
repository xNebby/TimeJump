using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{

    private CinemachineImpulseSource _ImpulseSource;

    // Start is called before the first frame update
    void Start()
    {
        _ImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Shake();
        }
    }

    public void Shake()
    {
        _ImpulseSource.GenerateImpulse();
    }
}
