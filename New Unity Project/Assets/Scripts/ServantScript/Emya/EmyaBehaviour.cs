using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EmyaBehaviour : PlatformerCharacter2D
{

    public override void Move(float move, bool jump, bool attack, bool parade)
    {

        base.Move(move, jump, attack, parade);
        if ((move > 0 && !netFacingRight) || (move < 0 && netFacingRight))
        {
            netFacingRight = !netFacingRight;
            CmdFlipSprite(netFacingRight);
        }
    }

    [SyncVar(hook = "FacingCallback")]
    public bool netFacingRight = true;
    [Command]
    public virtual void CmdFlipSprite(bool facing)
    {
        netFacingRight = facing;
        if (netFacingRight)
        {
            Vector3 SpriteScale = transform.localScale;
            SpriteScale.x = -5;
            transform.localScale = SpriteScale;
        }
        else
        {
            Vector3 SpriteScale = transform.localScale;
            SpriteScale.x = 5;
            transform.localScale = SpriteScale;
        }
    }

    protected virtual void FacingCallback(bool facing)
    {
        netFacingRight = facing;
        if (netFacingRight)
        {
            Vector3 SpriteScale = transform.localScale;
            SpriteScale.x = -5;
            transform.localScale = SpriteScale;
        }
        else
        {
            Vector3 SpriteScale = transform.localScale;
            SpriteScale.x = 5;
            transform.localScale = SpriteScale;
        }
    }

    public void Shot()
    {

        Vector3 worldMousePos = camera.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = (Vector2)((worldMousePos - transform.position));
        direction.Normalize();
        if (isLocalPlayer)
        {
            CmdFire(direction, netFacingRight);
        }
    }
    [Command]
    public void CmdFire(Vector2 direction, bool netfacingRight)
    {

        // Creates the bullet locally
        GameObject bullet = (GameObject)Instantiate(
                                arrowPrefab,
                                arrowSpawn.position + (Vector3)(direction * 0.5f),
                                new Quaternion(direction.x * 0.5f, direction.y * 0.5f, 0, 0));

        bullet.GetComponent<Bullet>().playerDamage = servantStat.damageCalcul(servantStat.damage, servantStat.level);
        float facing;
        float fights;
        if (netFacingRight)
        {
            bullet.GetComponent<Collider2D>().offset = new Vector2(-2.22f, 0);
            facing = 1;
        }
        else
        {
            facing = -1;

            bullet.GetComponent<Collider2D>().offset = new Vector2(2.22f, 0);
        }
        // Add velocity to the bullet
        //bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(facing*300, bullet.GetComponent<Rigidbody2D>().velocity.y);
        // Adds velocity to the bullet
        if (direction.x < 0)
        {
            fights = -1;
        }
        else
        {
            fights = 1;
        }
        bullet.GetComponent<Rigidbody2D>().velocity = fights * facing * direction * 300;

        //Spawn the bullet on all client
        NetworkServer.Spawn(bullet);
        // Destroy the bullet after 2 seconds
        Destroy(bullet, 6.0f);
    }
    [ClientRpc]
    public void RpcBulletSpawn(GameObject bullet)
    {
        NetworkServer.Spawn(bullet);

    }
}
