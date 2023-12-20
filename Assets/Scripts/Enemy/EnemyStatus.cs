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
    private int num = 0;


    public void TakeDamage(int damage)
    {
        curHP -= damage;
        playerStatus.curMP++;
        anim.SetBool("hurt", true);
        if (curHP <= 0)
        {
            AudioManager.instance.Play("Destroy");
            anim.SetBool("dead", true);
            GetComponent<Collider2D>().enabled = false;
            this.enabled = false;
            curHP = 0;
            playerStatus.curMP++;
        }
    }


    public void TakeDamageMagic(int damage, int hit, int recovery)
    {
        if (num <= hit)
        {
            curHP -= damage;
            if (num <= recovery)
            {
                playerStatus.curMP++;
            }
            anim.SetBool("hurt", true);
            if (curHP <= 0)
            {
                AudioManager.instance.Play("Destroy");
                anim.SetBool("dead", true);
                GetComponent<Collider2D>().enabled = false;
                this.enabled = false;
                curHP = 0;
                playerStatus.curMP++;
            }
        }
    }


    private void Start()
    {
        curHP = maxHP;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        num = Random.Range(1, 100);
    }
}
