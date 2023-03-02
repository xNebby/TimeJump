using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayerRotationLock : MonoBehaviour
{
    private GameObject PlayerGameObject;

    // Start is called before the first frame update
    void Start()
    {
        PlayerGameObject = PlayerInfo.Instance.PlayerGameObject;
    }
    void Awake()
    {
        PlayerGameObject = PlayerInfo.Instance.PlayerGameObject;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = PlayerGameObject.transform.rotation;
    }
}
