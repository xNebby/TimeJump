using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : SingletonClass<PlayerInfo>
{
    public GameObject PlayerGameObject;
    public Rigidbody2D PlayerRigidbody;
    public void UpdateValues(GameObject v_GO, Rigidbody2D v_RB)
    {
        PlayerGameObject = v_GO;
        PlayerRigidbody = v_RB;
    }
}
