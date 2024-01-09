using DamageNumbersPro.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    [Header("Status")]
    public int maxHP;                         // 最大HP
    public int curHP;                         // 現在HP
    public int LV;                            // レベル
    public int ATK;                           // 攻撃力
    public int DEF;                           // 防御力
    [Header("Knockback")]
    public float knockbackForce;              // ノックバック力量
    public float knockbackTime;               // ノックバック時間
    public bool isKnockback;                  // ノックバックしているか

    [Header("ParticleEffect")]
    public ParticleSystem damageParticle;     // ダメージエフェクト
    [Space(40)]
    public EnemyDamage enemyDamage;


    private Animator anim = null;
    private Rigidbody2D rb = null;


    public static bool Probability(float fPercent)
    {
        float fProbabilityRate = UnityEngine.Random.value * 100.0f;

        if (fPercent == 100.0f && fProbabilityRate == fPercent)
        {
            return true;
        }
        else if (fProbabilityRate < fPercent)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private void Start()
    {
        curHP = maxHP;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        if(curHP > maxHP)
        {
            curHP = maxHP;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if(collision.transform.position.x <= transform.position.x)
            {
                PlayerStatus.isKnockbackFromRight = true;
            }
            else if (collision.transform.position.x > transform.position.x)
            {
                PlayerStatus.isKnockbackFromRight = false;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FallPoint"))
        {
            Destroy(gameObject);
        }
    }


    public void TakeDamage(int damage)
    {
        StartCoroutine(Knockback());
        enemyDamage.SpawnPopup(damage);
        curHP -= damage;
        PlayerStatus.currentMP++;
        anim.SetTrigger("hurt");
        damageParticle.Play();
        if (curHP <= 0)
        {
            Dead();
        }
        else
        {
            AudioManager.instance.Play("Enemy_Damage");
        }
    }


    public void TakeDamageMagic(int damage, float hitRate, float recoveryRate)
    {
        if (Probability(hitRate))
        {
            StartCoroutine(Knockback());
            enemyDamage.SpawnPopup(damage);
            curHP -= damage;
            damageParticle.Play();
            anim.SetTrigger("hurt");
            if (Probability(recoveryRate))
            {
                PlayerStatus.currentMP++;
            }
            if (curHP <= 0)
            {
                Dead();
            }
            else
            {
                AudioManager.instance.Play("Enemy_Damage");
            }
        }
    }

    private void Dead()
    {
        AudioManager.instance.Play("Enemy_Dead");
        anim.SetBool("dead", true);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        curHP = 0;
        PlayerStatus.currentMP++;
    }
    
    
    private IEnumerator Knockback()
    {
        isKnockback = true;

        yield return new WaitForSeconds(knockbackTime);

        isKnockback = false;
    }
}
