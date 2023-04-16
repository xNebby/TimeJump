using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Space(10)]
    [Header("Jump Values")]
    public float JumpSpeed = 1f;
    public float JumpHeight = 1f;
    public float CrouchJumpHeight = 2f;
    public bool CrouchJumping;

    // Start is called before the first frame update
    void OnEnable()
    {
        EventManager.StartListening("IM_StartJump", HoldingJump);
        EventManager.StartListening("IM_StopJump", ReleaseJump);
    }
    void OnDisable()
    {
        EventManager.StopListening("IM_StartJump", HoldingJump);
        EventManager.StopListening("IM_StopJump", ReleaseJump);
    }

    void HoldingJump()
    {
        // if is able to jump, jump
        // if player is on the ground, there can be either a crouch jump upwards, or a normal jump (up or diagonally)
        if (PlayerStateManager.Instance.PlayerIsOnGround)
        {

        }
    }
    void ReleaseJump()
    {

    }

    void CrouchJump()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
