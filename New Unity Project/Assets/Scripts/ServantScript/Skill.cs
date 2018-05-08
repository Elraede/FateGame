using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Skill : MonoBehaviour {
    public Cooldown cooldown;
    public int manaCost;
    public bool isCooldown = false;
    public Image imageCooldown;
    public bool effectIsActive = false;
    public bool isUsed;
    public bool isActive;
}
