using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speen : MonoBehaviour
{
    private RectTransform Trans;
    public float RotSpeed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        Trans = gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Trans.RotateAround(Vector3.zero, Vector3.forward, RotSpeed);
    }
}
