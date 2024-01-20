using DamageNumbersPro.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyStatus : MonoBehaviour
{
    [Header("Status")]
    public int maxHP;                         // �ő�HP
    public int curHP;                         // ����HP
    public int ATK;                           // �U����
    public static int atk;                    // �U����(�i�[�p)
    public int DEF;                           // �h���
    public bool isDead;                       // ���S����
    [Header("Knockback")]
    public float knockbackForce;              // �m�b�N�o�b�N�͗�
    public float knockbackTime;               // �m�b�N�o�b�N����
    public bool isKnockback;                  // �m�b�N�o�b�N���Ă��邩

    [Header("ParticleEffect")]
    public ParticleSystem damageParticle;     // �_���[�W�G�t�F�N�g
    [Space(40)]
    public EnemyDamage enemyDamage;
    public EnemyHPBar enemyHPBar;


    private Animator anim = null;
    private Rigidbody2D rb = null;
    private SpriteRenderer sr = null;


    private void Start()
    {
        curHP = maxHP;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        curHP = Mathf.Clamp(curHP, 0, maxHP);
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


    private void OnBecameInvisible()
    {
        if(isDead)
        {
            enemyHPBar.OnDestroy();
            Destroy(gameObject);
        }
    }


    public void TakeDamage(int ATK)
    {
        StartCoroutine(Knockback());
        float randDamageRate = Random.Range(0.85f, 1.15f);
        int damage = (int)((float)ATK * randDamageRate - (DEF / 2));
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
        if (MathCheck.Probability(hitRate))
        {
            StartCoroutine(Knockback());
            enemyDamage.SpawnPopup(damage);
            curHP -= damage;
            damageParticle.Play();
            anim.SetTrigger("hurt");
            if (MathCheck.Probability(recoveryRate))
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
        isDead = true;
    }
    
    
    private IEnumerator Knockback()
    {
        isKnockback = true;

        yield return new WaitForSeconds(knockbackTime);

        isKnockback = false;
    }
}
