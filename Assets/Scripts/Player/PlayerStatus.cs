using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;
using Unity.VisualScripting;
using DamageNumbersPro.Demo;

public class PlayerStatus : MonoBehaviour
{
    [Header("HP")]
    public int maxHP = 6;                            // 最大HP
    public int curHP = 0;                            // 現在HP
    public static int currentHP;                     // 現在HP(格納用)
    [Header("MP")]
    public int maxMP = 6;                            // 最大MP
    public int curMP = 0;                            // 現在MP
    public static int currentMP;                     // 現在MP(格納用)
    [Header("Level")]
    public int Lv;                                   // レベル
    public int Exp;                                  // 経験値
    public int LvUpExp;                              // 必要経験値
    [Header("KnockBack")]
    public float knockbackTime;                      // ノックバック時間
    public float knockbackForce;                     // ノックバック力
    public static bool isKnockback;                  // ノックバック判定
    public static bool isKnockbackFromRight;         // ノックバック向き判定
    [Header("無敵")]
    public float damageTime;                         // 無敵時間
    public float flashTime;                          // 点滅時間
    [Header("Status")]
    public int ATK;                                  // 攻撃力
    public int DEF;                                  // 防御力
    public int RES;                                  // 効果抵抗
    public int KILL;                                 // 敵を殺した数
    public int COIN;                                 // コイン
    public static bool isDead;                       // 死亡判定
    [Header("毒状態")]
    public float poisonNumberMin;
    public float poisonNumberMax;
    public float poisonDamageIntervalTime;
    public float poisonDamage;
    public bool isPoison;                     // 毒状態
    [Header("ParticleEffect")]
    public ParticleSystem damageParticle;            // ダメージエフェクト
    [Space(30)]
    public HP hp;
    public PlayerDamage playerDamage;


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
        currentHP = curHP;
        currentMP = curMP;
        curHP = Mathf.Clamp(curHP, 0, maxHP);
        curMP = Mathf.Clamp(curMP, 0, maxMP);

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
            EnemyStatus enemyStatus = collision.collider.GetComponent<EnemyStatus>();
            float randDamageRate = Random.Range(0.85f, 1.15f);
            int damage = (int)((float)enemyStatus.ATK * randDamageRate - (DEF / 2));
            Damage(damage);
        }
    }


    public void Damage(int damage)
    {
        if (!isDead)
        {
            playerDamage.SpawnPopup(damage);
            gameObject.layer = LayerMask.NameToLayer("PlayerDamage");
            StartCoroutine(Knockback());
            StartCoroutine(Invincible());
            damageParticle.Play();
            anim.SetTrigger("hurt");
            curHP -= damage;
            ProCamera2DShake.Instance.Shake("PlayerDamage");
            if (curHP <= 0)
            {
                Dead();
            }
            else
            {
                AudioManager.instance.Play("Player_Damage");
            }
        }
    }


    private void Dead()
    {
        AudioManager.instance.StopAll();
        AudioManager.instance.Play("Player_Dead");
        anim.SetTrigger("dead");
        isDead = true;
    }


    IEnumerator Knockback()
    {
        isKnockback = true;
        
        yield return new WaitForSeconds(knockbackTime);
        
        isKnockback = false;
    }


    IEnumerator Invincible()
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
        if (!isDead)
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }


    IEnumerator Poison()
    {
        if(isPoison)
        {
            for (int i = 0;i < 5; i++)
            {
                yield return new WaitForSeconds(poisonDamageIntervalTime);
            }
        }
    }
}
