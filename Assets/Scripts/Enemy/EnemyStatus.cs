using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    [Header("Status")]
    public int maxHP;                         // ç≈ëÂHP
    public int curHP;                         // åªç›HP
    public int LV;                            // ÉåÉxÉã
    public int ATK;                           // çUåÇóÕ
    public int DEF;                           // ñhå‰óÕ
    [Header("KnockBack")]
    public float knockbackForce;
    [Space(40)]
    public PlayerStatus playerStatus;


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
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            anim.SetTrigger("attack");
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
        curHP -= damage;
        playerStatus.curMP++;
        anim.SetBool("hurt", true);
        if (curHP <= 0)
        {
            Dead();
        }
    }


    public void TakeDamageMagic(int damage, float hitRate, float recoveryRate)
    {
        if (rateNum <= (int)(hitRate * 100))
        {
            curHP -= damage;
            if (rateNum <= (int)(recoveryRate * hitRate * 100))
            {
                playerStatus.curMP++;
            }
            anim.SetBool("hurt", true);
            if (curHP <= 0)
            {
                Dead();
            }
        }
    }

    private void Dead()
    {
        AudioManager.instance.Play("Destroy");
        anim.SetBool("dead", true);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        curHP = 0;
        playerStatus.curMP++;
    }
}
