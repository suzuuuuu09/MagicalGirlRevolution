using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("HP")]
    public int maxHP = 6;                 // Å‘åHP
    public int curHP = 0;                 // Œ»İHP
    [Header("MP")]
    public int maxMP = 6;                 // Å‘åMP
    public int curMP = 0;                 // Œ»İMP
    [Header("Level")]
    public int Lv;                        // ƒŒƒxƒ‹
    public int Exp;                       // ŒoŒ±’l
    public int LvUpExp;                   // •K—vŒoŒ±’l
    [Header("–³“G")]
    public float damageTime;              // –³“GŠÔ
    public float flashTime;               // “_–ÅŠÔ
    [Header("Status")]
    public int ATK;                     // UŒ‚—Í
    public int DEF;                     // –hŒä—Í
    public int RES;                     // Œø‰Ê’ïR
    public int KILL;                    // “G‚ğE‚µ‚½”
    public int COIN;                    // ƒRƒCƒ“
    public bool isDead = false;           // €–S”»’è
    [Header("ParticleEffect")]
    public ParticleSystem damageParticle;
    [Space(30)]
    public HP hp;


    private Animator anim = null;
    private SpriteRenderer spriteRenderer;


    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        curHP = maxHP;
        curMP = 0;
    }


    void Update()
    {
        // HP,MPˆ—
        if (curHP > maxHP)
        {
            curHP = maxHP;
        }
        if (curMP > maxMP)
        {
            curMP = maxMP;
        }
        if (curHP < 0)
        {
            curHP = 0;
        }
        if (curMP < 0)
        {
            curMP = 0;
        }

        // ƒŒƒxƒ‹ˆ—
        if (Exp >= LvUpExp)
        {
            Exp -= LvUpExp;
            float _LvUpExp = (float)LvUpExp * 1.2f;
            LvUpExp = (int)_LvUpExp;
            Lv++;
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            gameObject.layer = LayerMask.NameToLayer("PlayerDamage");
            hp.freq = 8;
            Damage();
            StartCoroutine(Invincible());
            hp.freq = 0.8f;
        }
    }


    void Damage()
    {
        if (!isDead)
        {
            damageParticle.Play();
            anim.SetTrigger("hurt");
            curHP--;
            CinemachineShake.instance.ShakeCamera(5f, 0.1f);
            if (curHP <= 0)
            {
                anim.SetTrigger("dead");
                isDead = true;
            }
            else
            {
                AudioManager.instance.Play("PlayerDamage");
            }
        }
    }

    IEnumerator Invincible()
    {
        if (!isDead)
        {
            Color color = spriteRenderer.color;
            for (int i = 0; i < damageTime; i++)
            {
                yield return new WaitForSeconds(flashTime);
                spriteRenderer.color = new Color(color.r, color.g, color.b, 0f);

                yield return new WaitForSeconds(flashTime);
                spriteRenderer.color = new Color(color.r, color.g, color.b, 1.0f);
            }
            spriteRenderer.color = color;
            gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }

}
