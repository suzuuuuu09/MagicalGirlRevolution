using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;


public class PlayerStatus : MonoBehaviour
{
    [Header("HP")]
    public int maxHP = 6;                 // �ő�HP
    public int curHP = 0;                 // ����HP
    [Header("MP")]
    public int maxMP = 6;                 // �ő�MP
    public int curMP = 0;                 // ����MP
    [Header("Level")]
    public int Lv;                        // ���x��
    public int Exp;                       // �o���l
    public int LvUpExp;                   // �K�v�o���l
    [Header("KnockBack")]
    public float knockbackTime;           // �m�b�N�o�b�N����
    public float knockbackForce;          // �m�b�N�o�b�N��
    public bool isKnockback;              // �m�b�N�o�b�N����
    public bool isKnockbackFromRight;     // �m�b�N�o�b�N��������
    [Header("���G")]
    public float damageTime;              // ���G����
    public float flashTime;               // �_�Ŏ���
    [Header("Status")]
    public int ATK;                       // �U����
    public int DEF;                       // �h���
    public int RES;                       // ���ʒ�R
    public int KILL;                      // �G���E������
    public int COIN;                      // �R�C��
    public bool isDead = false;           // ���S����
    [Header("ParticleEffect")]
    public ParticleSystem damageParticle; // �_���[�W�G�t�F�N�g
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
