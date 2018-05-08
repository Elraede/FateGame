using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;

[RequireComponent(typeof (PlatformerCharacter2D))]
public class Platformer2DUserControl : NetworkBehaviour
{
    private PlatformerCharacter2D m_Character;
    private bool m_Jump;
    private bool m_Attack;
    private bool m_parade;

    private void Awake()
    {
        m_Character = GetComponent<PlatformerCharacter2D>();
    }


    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (!m_Jump)
        {
                
            // Read the jump input in Update so button presses aren't missed.
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }
        if (!m_Attack)
        {
            m_Attack = CrossPlatformInputManager.GetButtonDown("Fire1");
        }
            

    }


    private void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        // Read the inputs.
        m_parade = CrossPlatformInputManager.GetButton("Fire2");
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        // Pass all parameters to the character control script.
        m_Character.Move(h, m_Jump, m_Attack, m_parade);
        m_Jump = false;
        m_Attack = false;
        m_parade = false;
    }
}

