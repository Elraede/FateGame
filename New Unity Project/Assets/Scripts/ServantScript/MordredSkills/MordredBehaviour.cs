using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MordredBehaviour : PlatformerCharacter2D
{

    bool MordredSkill1 = false;
    bool skillOneUsed = false;
    bool skillTwoUsed = false;
    bool skillThreeUsed = false;
    public bool ispositif = false;
    public bool isnegatif = false;
    public Vector3 finalVelocity = new Vector3();

    public GameObject Skill2;
    public GameObject Skill3;
    public GameObject Skill1Thunder2;

    [SyncVar(hook = "FacingCallback")]
    public bool netFacingRight = true;
    // Update is called once per frame
    protected override void FixedUpdate () {
        base.FixedUpdate();
        skillOneUsed = servantStat.skills[0].isUsed;
        skillTwoUsed = servantStat.skills[1].isUsed;
        skillThreeUsed = servantStat.skills[2].isUsed;
    }

    public override void Move(float move, bool jump, bool attack, bool parade)
    {
        if ((m_Anim.GetCurrentAnimatorStateInfo(0).IsTag("Skill") || skillThreeUsed))
        {
            m_Anim.SetBool("Skill3", skillThreeUsed);
            //move = 0;
            //jump = false;
            attack = false;
            parade = false;
        }
        if ((m_Anim.GetCurrentAnimatorStateInfo(0).IsTag("Skill")||skillTwoUsed))
        {
            m_Anim.SetBool("Skill2", skillTwoUsed);
            move = 0;
            jump = false;
            attack = false;
            parade = false;
        }
        if (!servantStat.skills[1].isActive)
        {
            servantStat.armor = baseArmor;
        }
        if ((m_Anim.GetCurrentAnimatorStateInfo(0).IsTag("Skill") || servantStat.skills[0].isActive))
        {
            m_Anim.SetBool("Skill1", skillOneUsed);
            if ((m_Anim.GetCurrentAnimatorStateInfo(0).IsTag("Skill")))
            {
                move = 0;
                jump = false;

            }
            attack = false;
            parade = false;
        }
        base.Move(move, jump, attack, parade);
        if ((move > 0 && !netFacingRight) || (move < 0 && netFacingRight))
        {
            netFacingRight = !netFacingRight;
            CmdFlipSprite(netFacingRight);
        }
        m_Anim.SetBool("Skill1End", MordredSkill1);
        //Skill1Thunder1.SetActive(false);
        if (MordredSkill1)
        {
            servantStat.skills[0].isUsed = false;
            Vector3 initialPosition1 = m_Rigidbody2D.position;
            if ((GetComponentInChildren<JumpSkill>().mousePosition.x > m_Rigidbody2D.transform.position.x || ispositif) && !isnegatif)
            {
                netFacingRight = true;
                isnegatif = false;
                ispositif = true;
                m_Rigidbody2D.velocity = new Vector2(finalVelocity.x, m_Rigidbody2D.velocity.y);
                m_Rigidbody2D.transform.position = Vector3.Lerp(transform.position, initialPosition1, Time.deltaTime * 5);
                for (int i = 0; i < Skill1Thunder2.transform.childCount; i++)
                {

                    Skill1Thunder2.transform.GetChild(i).transform.localScale = new Vector3(5, 5, 5);
                }
                if (m_Rigidbody2D.transform.position.x >= GetComponentInChildren<JumpSkill>().mousePosition.x)
                {
                    ispositif = false;
                    MordredSkill1 = false;
                    GetComponentInChildren<JumpSkill>().isActive = false;
                    m_Rigidbody2D.gravityScale = 20;
                    CmdLaunchAttack();
                    //Thunder2();
                }


            }
            else
            {
                ispositif = false;
                isnegatif = true;
                m_Rigidbody2D.velocity = new Vector2(finalVelocity.x, m_Rigidbody2D.velocity.y);
                m_Rigidbody2D.transform.position = Vector3.Lerp(transform.position, initialPosition1, Time.deltaTime * 5);
                netFacingRight = false;
                if (m_Rigidbody2D.transform.position.x <= GetComponentInChildren<JumpSkill>().mousePosition.x)
                {
                    MordredSkill1 = false;
                    isnegatif = false;
                    GetComponentInChildren<JumpSkill>().isActive = false;
                    m_Rigidbody2D.gravityScale = 20;
                    CmdLaunchAttack();
                    //Thunder2();
                }

            }

        }
        bool hasPary = false;
        if (parade)
        {
            servantStat.armor = servantStat.armor + 10;
            hasPary = true;
        }
        if(!parade && hasPary)
        {
            servantStat.armor = baseArmor;
        }

        if (!servantStat.skills[1].effectIsActive)
        {
            servantStat.armor = baseArmor;
            CmdSkill2Effect();
        }

    }

    [Command]
    public void CmdSkill2Effect()
    {
        RpcSkill2Effect();
    }
    [ClientRpc]
    public void RpcSkill2Effect()
    {
        Skill2.SetActive(false);

    }

    public void skill2EffectApperance()
    {
            Skill2.SetActive(true);
    }

    public void jumpSkillMovement()
    {
        m_Rigidbody2D.gravityScale = 100;
        Vector3 initialPosition = m_Rigidbody2D.position;
        Vector3 p = GetComponentInChildren<JumpSkill>().mousePosition;
        float yDistance = p.y - transform.position.y;
        float R = p.x - initialPosition.x;
        float gravity = 150 * Physics.gravity.magnitude;
        if (R<50)
        {
            gravity = 200 * Physics.gravity.magnitude;
        }
        if (yDistance <= 0)
        {
            gravity = 100 * Physics.gravity.magnitude;

        }
            // Selected angle in radians
            //float angle = Mathf.Asin(gravity*R/Mathf.Pow(speed, 2)) * Mathf.Deg2Rad;
            //Debug.Log(angle);
            // Positions of this object and the target on the same plane
            var rigid = transform.GetComponent<Rigidbody2D>();

        // Positions of this object and the target on the same plane
        Vector3 planarTarget = new Vector3(p.x, 0, p.z);
        Vector3 planarPostion = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 planarTargety = new Vector3(0, p.y, p.z);
        Vector3 planarPostiony = new Vector3(0, transform.position.y, transform.position.z);

        // Planar distance between objects
        float distance = Vector3.Distance(planarTarget, planarPostion);
        float angle = Mathf.Atan2(Vector3.Distance(planarTargety, planarPostiony), distance);
        if (angle*Mathf.Rad2Deg<45)
        {
            angle = 45 * Mathf.Deg2Rad;
        }
        // Distance along the y axis between objects
        float yOffset = transform.position.y - p.y;

        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / distance * Mathf.Tan(angle) + yOffset);

        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        // Rotate our velocity to match the direction between the two objects
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion) * (p.x > transform.position.x ? 1 : -1);
        finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;
        // Fire!
        if (isLocalPlayer)
        {
            if (finalVelocity.y > 450)
            {
                finalVelocity.y = 450;
            }
            if (distance < 50 && yDistance> 80)
            {
                rigid.AddForce(new Vector2(finalVelocity.x, 250), ForceMode2D.Impulse);
            }
            else if (distance < 50 && yDistance > 20)
            {
                rigid.AddForce(new Vector2(finalVelocity.x, 175), ForceMode2D.Impulse);
            }
            else
            {
                rigid.AddForce(new Vector2(finalVelocity.x, finalVelocity.y), ForceMode2D.Impulse);
            }
            //Debug.Log(finalVelocity);
            //Thunnder1();
            MordredSkill1 = true;
        }
    }

    public void resistanceSkill()
    {
        if (isLocalPlayer)
        {
            servantStat.skills[1].isUsed = false;
            servantStat.armor = servantStat.armor + 2 * servantStat.level;
        }
    }
    public void slashSkill()
    {
        if (isLocalPlayer)
        {
            servantStat.skills[2].isUsed = false;
        }
    }

    [Command]
    public void CmdFlipSprite(bool facing)
    {
        netFacingRight = facing;
        if (netFacingRight)
        {
            for (int i = 0; i < Skill1Thunder2.transform.childCount; i++)
            {
                Skill1Thunder2.transform.GetChild(i).transform.localScale = new Vector3(5, 5, 5);
            }
            for (int i = 0; i < Skill3.transform.childCount; i++)
            {
                Skill3.transform.GetChild(i).transform.localScale = new Vector3(1, 1, 1);
            }
            Vector3 SpriteScale = transform.localScale;
            SpriteScale.x = -5;
            transform.localScale = SpriteScale;
        }
        else
        {
            for (int i = 0; i < Skill1Thunder2.transform.childCount; i++)
            {
                Skill1Thunder2.transform.GetChild(i).transform.localScale = new Vector3(-5, 5, 5);
            }

            for (int i = 0; i < Skill3.transform.childCount; i++)
            {
                Skill3.transform.GetChild(i).transform.localScale = new Vector3(-1, 1, 1);
            }
            Vector3 SpriteScale = transform.localScale;
            SpriteScale.x = 5;
            transform.localScale = SpriteScale;
        }
    }

    protected void FacingCallback(bool facing)
    {
        netFacingRight = facing;
        if (netFacingRight)
        {
            for (int i = 0; i < Skill1Thunder2.transform.childCount; i++)
            {
                Skill1Thunder2.transform.GetChild(i).transform.localScale = new Vector3(5, 5, 5);
            }
            Vector3 SpriteScale = transform.localScale;
            SpriteScale.x = -5;
            transform.localScale = SpriteScale;
        }
        else
        {
            for (int i = 0; i < Skill1Thunder2.transform.childCount; i++)
            {
                Skill1Thunder2.transform.GetChild(i).transform.localScale = new Vector3(-5, 5, 5);
            }
            Vector3 SpriteScale = transform.localScale;
            SpriteScale.x = 5;
            transform.localScale = SpriteScale;
        }
    }
}
