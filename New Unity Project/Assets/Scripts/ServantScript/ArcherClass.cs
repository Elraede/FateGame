using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherClass : Servant {

	// Use this for initialization
    void Start()
    {
        damage = 1;
        hitPoint = 40;
        manaPoint = 40;
        speed = 110;
        level = 1;
        armor = 1;
        transform.GetComponent<PlatformerCharacter2D>().m_MaxSpeed = speed;


    }

    public override float damageCalcul(float damage, int level)
    {
        System.Random aleatoire = new System.Random();
        float dammage = 0;
        dammage = damage + level * (aleatoire.Next(4) + 1);

        return dammage;
    }
}
