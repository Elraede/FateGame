using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistanceSkill : Skill {
    
    public Cooldown cooldownEffect;
    float cool;
    // Use this for initialization
    void Start()
    {
        cooldownEffect = new Cooldown();
        cooldownEffect.coolDown = 10;
        manaCost = 10;
        cooldown = new Cooldown();
        cooldown.coolDown = 30;
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2) && isCooldown == false)
        {
            isActive = true;
            isCooldown = true;
            effectIsActive = true;
            imageCooldown.fillAmount = 1;
            isUsed = true;
            cool = cooldownEffect.coolDown;
        }
        if (isCooldown)
        {
            imageCooldown.fillAmount -= 1 / cooldown.coolDown * Time.deltaTime;

            if (imageCooldown.fillAmount <= 0)
            {
                imageCooldown.fillAmount = 0;
                isCooldown = false;
                isUsed = false;
                isActive = false;
            }
        }

        if (effectIsActive)
        {
            cool -= Time.deltaTime;
            if (cool <= 0)
            {
                effectIsActive = false;

            }
        }
    }
}
