﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionnalArrowSkill : Skill {
    void Start()
    {
        manaCost = 10;
        cooldown = new Cooldown();
        cooldown.coolDown = 6;
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
    }
}
