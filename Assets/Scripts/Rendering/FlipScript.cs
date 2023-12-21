using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipScript : MonoBehaviour
{

    private SpriteRenderer m_SpriteRenderer;
    //private bool CurrentFlip = false;

    void OnEnable()
    {

        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        EventManager.StartListening("FlipScript_Flip", delegate { Flip(true); });
        EventManager.StartListening("FlipScript_UnFlip", delegate { Flip(false); });
    }
    void OnDisable()
    {
        EventManager.StopListening("FlipScript_Flip", delegate { Flip(true); });
        EventManager.StopListening("FlipScript_UnFlip", delegate { Flip(false); });
    }


    void Flip(bool Var)
    {
        m_SpriteRenderer.flipX = Var;
    }

}
