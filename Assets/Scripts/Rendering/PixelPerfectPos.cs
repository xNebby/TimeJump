using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelPerfectPos : MonoBehaviour
{
    public int PixelResolution = 32;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Mathf.Round(transform.position.x * PixelResolution) / PixelResolution,
            Mathf.Round(transform.position.y * PixelResolution) / PixelResolution,
            Mathf.Round(transform.position.z * PixelResolution) / PixelResolution);
    }
}
