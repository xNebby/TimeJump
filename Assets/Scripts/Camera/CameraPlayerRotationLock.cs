using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayerRotationLock : MonoBehaviour
{
    public bool LockPlayerRotation = true;
    void Update()
    {
        if (LockPlayerRotation)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, Vector2.Angle(PlayerStateManager.Instance.PlayerGravity, Vector2.down));
        }
    }
}
