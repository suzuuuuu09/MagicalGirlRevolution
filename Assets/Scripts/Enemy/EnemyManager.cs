using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float speed;            // 移動速度
    public int maxHP = 100;        // 最大HP
    public int curHP;              // 現在HP
    [Header("画面外でも動かす")]
    public bool nonVisibleAct;     // 画面外でも動かす
    [Header("接触判定")]
    public WallCheckR wallCheckR;  // 壁接触判定右
    public WallCheckL wallCheckL;  // 壁接触判定左
    [Space(40)]
    public PlayerStatus playerStatus;


    private SpriteRenderer sr = null;
    private Animator anim = null;
    private Rigidbody2D rb = null;
    private bool rightTleftF = false;
    private bool isScreen = false;
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
        if(num <= hit)
        {
            curHP -= damage;
            if(num <= recovery)
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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        curHP = maxHP;
    }

    void Update()
    {
        if (sr.isVisible)
        {
            isScreen = true;
        }
        num = Random.Range(1, 100);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isScreen || nonVisibleAct)
        {
            anim.SetBool("run", true);
            int xVector = 1;
            if (wallCheckR.isOn || wallCheckL.isOn)
            {
                rightTleftF = !rightTleftF;
            }
            if (rightTleftF)
            {
                xVector = -1;
                transform.localScale = new Vector3(2.5f, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(-2.5f, transform.localScale.y, transform.localScale.z);
            }
            transform.position += new Vector3(-xVector * speed, 0, 0);
            rb.velocity = new Vector2(-xVector, rb.velocity.y);
        }
        else
        {
            anim.SetBool("run", false);
            rb.Sleep();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            anim.SetTrigger("attack");
        }
    }
}
