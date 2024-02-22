using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{

    private Animator m_Animator;



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
        // Add checks to do the other ones
        m_Animator.SetTrigger("StartIdle");
    }
    public void RunAnim()
    {
        m_Animator.SetTrigger("StartRun");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
