using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;

public class PlatformerCharacter2D : NetworkBehaviour
{
    [SerializeField] public float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField] public float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
    [SerializeField] protected bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
    [SerializeField] protected LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    protected bool m_Grounded;            // Whether or not the player is grounded.
    protected Transform m_CeilingCheck;   // A position marking where to check for ceilings
    const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
    protected Animator m_Anim;            // Reference to the player's animator component.
    protected Rigidbody2D m_Rigidbody2D;
    public GameObject arrowPrefab;
    public Transform arrowSpawn;
    public bool facingRight;  // For determining which way the player is currently facing.

    public bool doublejump;
    public bool m_jump;
    public bool m_move;

    public bool jump;
    public bool m_Attack;
    public bool m_Parade;

    public GameObject Slash1;
    public Collider2D Hitbox;
    public HealthBar healthbar;
    public Camera camera;

    public Servant servantStat;
    public int baseArmor = 0;

    private void Awake()
    {
        // Setting up references.
        m_GroundCheck = transform.Find("GroundCheck");
        m_CeilingCheck = transform.Find("CeilingCheck");
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Grounded = true;
        doublejump = false;
        m_Attack = false;
        m_Parade = false;
        facingRight = true;
        baseArmor = servantStat.armor;
    }


    protected virtual void FixedUpdate()
    {
        m_Grounded = false;
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                doublejump = false;
            }
        }

        jump = false;
        m_Anim.SetBool("Ground", m_Grounded);

        //Set the vertical animation
        m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
        m_Anim.SetBool("doubleJump", doublejump);
        m_Anim.SetBool("Attack", m_Attack);


    }
    public void EndSlash()
    {
        Slash1.SetActive(false);
    }

    public virtual void Move(float move, bool jump, bool attack, bool parade)
    {
        if (parade)
        {
            m_Parade = true;
            m_Anim.SetBool("Parade", m_Parade);
            move = move / 2;
            if (isLocalPlayer)
            {
                servantStat.armor = 100;
            }
        }
        else
        {

            m_Parade = parade;
            m_Anim.SetBool("Parade", m_Parade);
        }
        //parade = true;
        m_Parade = parade;
        //If crouching, check to see if the character can stand up
        //if (!crouch && m_Anim.GetBool("Crouch"))
        //{
        //    If the character has a ceiling preventing them from standing up, keep them crouching
        //    if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
        //    {
        //        crouch = true;
        //    }
        //}

        //Set whether or not the character is paring in the animator

        //only control the player if grounded or airControl is turned on
        if ((m_Grounded || m_AirControl))
        {
        //if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        //{
        // The Speed animator parameter is set to the absolute value of the horizontal input.
            if (move == 0)
            {
                m_move = false;
            }
            else
            {
                m_move = true;

            }
            m_Anim.SetBool("Move", m_move);

            // Move the character
            Vector3 initialPosition = m_Rigidbody2D.position;
            m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);
            m_Rigidbody2D.transform.position = Vector3.Lerp(transform.position, initialPosition, Time.deltaTime * 5);

            //}
            //else
            //{
            //m_Rigidbody2D.velocity = Vector2.zero;
            //}
            //// Reduce the speed if crouching by the crouchSpeed multiplier
            //move = (crouch ? move * m_CrouchSpeed : move);
        }
        // If the player should jump...
        if ((m_Grounded||!doublejump) && jump )
        {
            // Add a vertical force to the player.
            m_Anim.SetBool("Ground", false);
            if (m_Grounded)
            {
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));

            }
            else
            {
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
            if (!doublejump && !m_Grounded)
            {
                doublejump = true;
                //GetComponent<NetworkAnimator>().animator.Play("DoubleJumping");
            }
            m_Grounded = false;
            //m_doublejump = true;
        }

        if (attack && !m_Anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            //Slash1.SetActive(true);
            //LaunchAttack(Hitbox);
            m_Attack = true;
            //m_Rigidbody2D.velocity = Vector2.zero;
        }
        
        m_Attack = attack;

        

    }
    [Command]
    public void CmdLaunchAttack()
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(Hitbox.bounds.center, Hitbox.bounds.extents, 0, LayerMask.GetMask("Hitbox"));
        foreach(Collider2D c in cols)
        {
            if (c.transform.root == transform)
                continue;
            float damage = 0;
            switch (c.name)
            {
                case "ColliderHit":
                    damage = servantStat.damageCalcul(servantStat.damage,servantStat.level);
                    break;
            }
            if (isLocalPlayer)
            {
                CmdHitPlayer(damage, c.transform.root.gameObject);
            }
            //c.transform.root.GetComponent<HealthBar>().TakeDamage(damage);
            //RpcAttack();
            Debug.Log(c.transform.root);
        }
            
    }

    [Command]
    public void CmdHitPlayer(float dmg, GameObject player)
    {
       player.GetComponent<HealthBar>().RpcTakeDamage(dmg);
    }

    //[SyncVar(hook = "FacingCallback")]
    //public bool netFacingRight = true;
    //[Command]
    //public virtual void CmdFlipSprite(bool facing)
    //{
    //    netFacingRight = facing;
    //    if (netFacingRight)
    //    {
    //        Vector3 SpriteScale = transform.localScale;
    //        SpriteScale.x = -5;
    //        transform.localScale = SpriteScale;
    //    }
    //    else
    //    {
    //        Vector3 SpriteScale = transform.localScale;
    //        SpriteScale.x = 5;
    //        transform.localScale = SpriteScale;
    //    }
    //}

    //protected virtual void FacingCallback(bool facing)
    //{
    //    netFacingRight = facing;
    //    if (netFacingRight)
    //    {
    //        Vector3 SpriteScale = transform.localScale;
    //        SpriteScale.x = -5;
    //        transform.localScale = SpriteScale;
    //    }
    //    else
    //    {
    //        Vector3 SpriteScale = transform.localScale;
    //        SpriteScale.x = 5;
    //        transform.localScale = SpriteScale;
    //    }
    //}


    public override void OnStartLocalPlayer()
    {

        

        GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(1, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(2, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(3, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(4, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(5, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(6, true);
    }

    public override void PreStartClient()
    {
        GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(1, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(2, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(3, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(4, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(5, true);
        GetComponent<NetworkAnimator>().SetParameterAutoSend(6, true);
    }
}
