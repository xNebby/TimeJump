using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{

    private Animator m_Animator;

    // Current State: 0- Idle, 1- Walk, 2- Run

    private void OnEnable()
    {
        EventManager.StartListening("PAH_Idle", IdleAnim);
        EventManager.StartListening("PAH_Run", RunAnim);
        m_Animator = GetComponent<Animator>();
    }
    private void OnDisable()
    {
        EventManager.StopListening("PAH_Idle", IdleAnim);
        EventManager.StopListening("PAH_Run", RunAnim);

    }

    public void IdleAnim()
    {
        if (m_Animator.GetInteger("State") > 0 )
        {
            Debug.Log("Received Event Idle");
            // Add checks to do the other ones
            m_Animator.SetTrigger("StartIdle");
            m_Animator.SetInteger("State", 0);
        }
    }
    public void RunAnim()
    {
        Debug.Log("Received Event Run");
        m_Animator.SetTrigger("StartRun");
        m_Animator.SetInteger("State", 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
