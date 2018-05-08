using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Cooldown : NetworkBehaviour {

    public float coolDown = 3;
    public float coolDownTimer;

    public void cooldownLaunch()
    {
        if (coolDownTimer>0)
        {
            coolDownTimer -= Time.deltaTime;
        }

        if (coolDownTimer < 0)
        {
            coolDownTimer = 0;
        }


    }
}
