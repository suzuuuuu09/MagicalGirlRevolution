using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    [Header("ステータス")]
    public int maxHP = 100;        // 最大HP
    public int curHP;              // 現在HP
    [Space(40)]
    public PlayerStatus playerStatus;


    private Animator anim = null;
    private int rateNum = 0;


    private void Start()
    {
        curHP = maxHP;
        anim = GetComponent<Animator>();
    }


    private void Update()
    {
        rateNum = Random.Range(1, 100);
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
