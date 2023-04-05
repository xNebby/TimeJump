using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHook : MonoBehaviour
{
    public Rigidbody2D PlayerRB;
    void Start()
    {
        UpdatePlayerInfo();
    }
    void OnEnable()
    {
        UpdatePlayerInfo();
    }
    void UpdatePlayerInfo()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        PlayerInfo.Instance.UpdateValues(gameObject, PlayerRB);
    }
}
