using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour
{
    public float playerDamage = 0;

    void OnCollisionEnter2D(Collision2D collision)
    {
        var hit = collision.transform.root.gameObject;
        var health = hit.GetComponent<HealthBar>();
        if (health != null)
        {
           hit.GetComponent<PlatformerCharacter2D>().CmdHitPlayer(playerDamage, hit);
        }
        Destroy(gameObject, 0.02f);
    }
}
 