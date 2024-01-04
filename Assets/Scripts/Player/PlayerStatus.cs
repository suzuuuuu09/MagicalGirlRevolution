using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;


public class PlayerStatus : MonoBehaviour
{
    [Header("HP")]
    public int maxHP = 6;                 // 最大HP
    public int curHP = 0;                 // 現在HP
    [Header("MP")]
    public int maxMP = 6;                 // 最大MP
    public int curMP = 0;                 // 現在MP
    [Header("Level")]
    public int Lv;                        // レベル
    public int Exp;                       // 経験値
    public int LvUpExp;                   // 必要経験値
    [Header("KnockBack")]
    public float knockbackTime;           // ノックバック時間
    public float knockbackForce;          // ノックバック力
    public bool isKnockback;              // ノックバック判定
    public bool isKnockbackFromRight;     // ノックバック向き判定
    [Header("無敵")]
    public float damageTime;              // 無敵時間
    public float flashTime;               // 点滅時間
    [Header("Status")]
    public int ATK;                       // 攻撃力
    public int DEF;                       // 防御力
    public int RES;                       // 効果抵抗
    public int KILL;                      // 敵を殺した数
    public int COIN;                      // コイン
    public bool isDead = false;           // 死亡判定
    [Header("ParticleEffect")]
    public ParticleSystem damageParticle; // ダメージエフェクト
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
        // HP,MP処理
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

        // レベル処理
        if (Exp >= LvUpExp)
        {
            Exp -= LvUpExp;
            float _LvUpExp = (float)LvUpExp * 1.2f;
            LvUpExp = (int)_LvUpExp;
            Lv++;
            AudioManager.instance.Play("LvUP");
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            gameObject.layer = LayerMask.NameToLayer("PlayerDamage");
            Damage();
            StartCoroutine(Knockback());
            StartCoroutine(Invincible());
        }
    }


    private void Damage()
    {
        if (!isDead)
        {
            damageParticle.Play();
            anim.SetTrigger("hurt");
            curHP--;
            ProCamera2DShake.Instance.Shake("PlayerDamage");
            if (curHP <= 0)
            {
                anim.SetTrigger("dead");
                isDead = true;
            }
            else
            {
                AudioManager.instance.Play("Player_Damage");
            }
        }
    }


    private IEnumerator Knockback()
    {
        isKnockback = true;
        
        yield return new WaitForSeconds(knockbackTime);
        
        isKnockback = false;
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
