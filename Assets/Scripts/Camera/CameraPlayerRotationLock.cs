using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayerRotationLock : MonoBehaviour
{
    private GameObject PlayerGameObject;
    public bool LockPlayerRotation = true;
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
        if (LockPlayerRotation)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, Vector2.Angle(PlayerStateManager.Instance.PlayerGravity, Vector2.down));
        }
    }
}
