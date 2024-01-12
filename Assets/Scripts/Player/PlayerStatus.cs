using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;
using Unity.VisualScripting;
using DamageNumbersPro.Demo;

public class PlayerStatus : MonoBehaviour
{
    [Header("HP")]
    public int maxHP = 6;                            // �ő�HP
    public int curHP = 0;                            // ����HP
    public static int currentHP;                     // ����HP(�i�[�p)
    [Header("MP")]
    public int maxMP = 6;                            // �ő�MP
    public int curMP = 0;                            // ����MP
    public static int currentMP;                     // ����MP(�i�[�p)
    [Header("Level")]
    public int Lv;                                   // ���x��
    public int Exp;                                  // �o���l
    public int LvUpExp;                              // �K�v�o���l
    [Header("KnockBack")]
    public float knockbackTime;                      // �m�b�N�o�b�N����
    public float knockbackForce;                     // �m�b�N�o�b�N��
    public static bool isKnockback;                  // �m�b�N�o�b�N����
    public static bool isKnockbackFromRight;         // �m�b�N�o�b�N��������
    [Header("���G")]
    public float damageTime;                         // ���G����
    public float flashTime;                          // �_�Ŏ���
    [Header("Status")]
    public int ATK;                                  // �U����
    public int DEF;                                  // �h���
    public int RES;                                  // ���ʒ�R
    public int KILL;                                 // �G���E������
    public int COIN;                                 // �R�C��
    public static bool isDead;                       // ���S����
    [Header("�ŏ��")]
    public float poisonNumberMin;
    public float poisonNumberMax;
    public float poisonDamageIntervalTime;
    public float poisonDamage;
    public bool isPoison;                     // �ŏ��
    [Header("ParticleEffect")]
    public ParticleSystem damageParticle;            // �_���[�W�G�t�F�N�g
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
        currentHP = curHP;
        currentMP = curMP;
        // HP,MP����
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

        // ���x������
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
            curHP-= damage;
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


    IEnumerator Knockback()
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
