using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [Header("無敵")]
    public float damageTime;              // 無敵時間
    public float flashTime;               // 点滅時間
    [Header("Status")]
    public float ATK;
    public bool isDead = false;           // 死亡判定
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
            anim.SetTrigger("hurt");
            curHP--;
            CinemachineShake.instance.ShakeCamera(5f, .1f);
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
