using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : SingletonClass<PlayerInfo>
{
    public GameObject PlayerGameObject;
    public Rigidbody2D PlayerRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        UpdateVars();
    }
    public override void Awake()
    {
        UpdateVars();
        base.Awake();
    }
    void UpdateVars()
    {
        PlayerGameObject = gameObject;
        PlayerRigidbody = PlayerGameObject.GetComponent<Rigidbody2D>();
    }
}
