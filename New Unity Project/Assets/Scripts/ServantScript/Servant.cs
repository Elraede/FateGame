using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Servant : NetworkBehaviour {
    public float hitPoint;
    public int level;
    public float manaPoint;
    public float damage;
    public int armor;
    public int speed;
    public Skill[] skills;

    virtual public float damageCalcul(float damage, int level) { return 0; }
}
