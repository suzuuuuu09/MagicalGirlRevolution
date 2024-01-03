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
    [Header("ParticleEffect")]
    public ParticleSystem damageParticle;     // ダメージエフェクト
    [Space(40)]
    public PlayerStatus playerStatus;
    public EnemyDamage enemyDamage;


    private Animator anim = null;
    private Rigidbody2D rb = null;
    private int rateNum = 0;


    private void Start()
    {
        curHP = maxHP;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        rateNum = Random.Range(1, 100);
        if(curHP > maxHP)
        {
            curHP = maxHP;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            anim.SetTrigger("attack");
            if(collision.transform.position.x < transform.position.x)
            {
                playerStatus.isKnockbackFromRight = true;
                EnemyControl.rightTleftF = false;
            }
            if (collision.transform.position.x > transform.position.x)
            {
                playerStatus.isKnockbackFromRight = false;
                EnemyControl.rightTleftF = true;
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
        enemyDamage.SpawnPopup(damage);
        curHP -= damage;
        playerStatus.curMP++;
        anim.SetBool("hurt", true);
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
        if (rateNum <= (int)(hitRate * 100))
        {
            enemyDamage.SpawnPopup(damage);
            curHP -= damage;
            damageParticle.Play();
            if (rateNum <= (int)(recoveryRate * hitRate * 100))
            {
                playerStatus.curMP++;
            }
            anim.SetBool("hurt", true);
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
        playerStatus.curMP++;
    }
}
