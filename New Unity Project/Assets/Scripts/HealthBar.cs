using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Networking;

public class HealthBar : NetworkBehaviour
{

    public Image structHealthBar;
    public Image healthBar;
    public Text ratioText;

    public Canvas badCanvas;
    public GameObject ennemyLifeBar;
    
    public SpriteRenderer healthBarForEnnemy;

    private Animator m_Anim;            // Reference to the player's animator component.

    private float MaxHitPoint = 100;

    [SyncVar(hook = "OnChangeHealth")]
    private float HitPoint;

    public bool death = false;
    private Cooldown cooldown;
    Material _gaugeMaterial;
    Material _gaugeMaterialForEnnemy;

    private void InitGauge()
    {
        _gaugeMaterial = new Material(healthBar.material);
        healthBar.material = _gaugeMaterial;
        //healthBar.material = _gaugeMaterial = new Material(healthBar.material);

        _gaugeMaterial.SetFloat("_Gauge", UnityEngine.Random.Range(0f, 1f));

        _gaugeMaterialForEnnemy = new Material(healthBarForEnnemy.material);
        healthBarForEnnemy.material = _gaugeMaterialForEnnemy;
        //healthBar.material = _gaugeMaterial = new Material(healthBar.material);

        _gaugeMaterialForEnnemy.SetFloat("_Gauge", UnityEngine.Random.Range(0f, 1f));
        MaxHitPoint = transform.GetComponent<PlatformerCharacter2D>().servantStat.hitPoint;
        HitPoint = MaxHitPoint;
    }

    private void RefreshGauge(float health,  float animDuration = 0.25f)
    {
        float newValue = health/MaxHitPoint;
        if (animDuration > 0)
        {
            _gaugeMaterial.DOFloat(newValue, "_Gauge", animDuration);
            _gaugeMaterialForEnnemy.DOFloat(newValue, "_Gauge", animDuration);
        }
        else
        {
            DOTween.Kill(_gaugeMaterial);
            _gaugeMaterial.SetFloat("_Gauge", newValue);
            DOTween.Kill(_gaugeMaterialForEnnemy);
            _gaugeMaterialForEnnemy.SetFloat("_Gauge", newValue);
        }
    }
    // Use this for initialization
    void Start () {
        InitGauge();
        UpdateHealthBar(HitPoint);
        cooldown = new Cooldown();

        m_Anim = GetComponent<Animator>();
    }
    
    private void UpdateHealthBar(float health)
    {
        
        float ratio = health / MaxHitPoint;
        ratioText.text = (ratio * 100).ToString("0") + '%';
        RefreshGauge( health);
    }

    [ClientRpc]
    public void RpcTakeDamage(float damage)
    {
        float dammage = damage - transform.GetComponent<Servant>().armor;
        if (dammage <= 0)
        {
            dammage = 1;
        }
        HitPoint -= dammage;
        if (HitPoint <= 0)
        {
            HitPoint = 0;
            death = true;
            cooldown.coolDownTimer = cooldown.coolDown;
            //print(cooldown.coolDownTimer);
            m_Anim.SetBool("Death", death);


        }
        //if (isLocalPlayer)
        //    healthBar.value = health;
        //else
        //    healthBarTP.value = health;

    }

    private void HealDamage(float heal)
    {
        HitPoint += heal;
        if (HitPoint>MaxHitPoint)
        {
            HitPoint = MaxHitPoint;
        }
    }

    // Update is called once per frame
    void Update () {
        if (!isLocalPlayer)
        {
            badCanvas.enabled = false;
            ennemyLifeBar.SetActive(true);
            return;
        }
        else
        {
            ennemyLifeBar.SetActive(false);
        }
        m_Anim.SetBool("Death", death);
        cooldown.cooldownLaunch();
        //print(cooldown.coolDownTimer);
        if (death && cooldown.coolDownTimer==0)
        {
            CmdRespawn();
        }
    }

    void OnChangeHealth(float health)
    {
        UpdateHealthBar(health);
    }

    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            // move back to zero location
            transform.position = Vector3.zero;
        }
        HitPoint = MaxHitPoint;
        death = false;
    }

    [Command]
    void CmdRespawn()
    {
        death = false;
        RpcRespawn();
        HitPoint = MaxHitPoint;
    }
}
