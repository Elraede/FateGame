using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SaberClass : Servant {

    // Use this for initialization
    void Start()
    {
        damage = 3;
        hitPoint = 50;
        manaPoint = 30;
        speed = 100;
        level = 1;
        armor = 2;
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
