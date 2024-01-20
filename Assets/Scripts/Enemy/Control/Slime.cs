using Com.LuisPedroFonseca.ProCamera2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [Header("移動")]
    public float moveSpeed;                      // 移動速度
    public float jumpPower;                      // ジャンプ力
    public float moveTimeMax;                    // 移動時間Max
    public float moveTimeMin;                    // 移動時間Min
    public float turnRate;                       // Playerの方を向く確率
    [Header("画面外でも動かす")]
    public bool nonVisibleAct;                   // 画面外でも動かす
    [Space(40)]
    public ColliderCheck groundCheck;


    private EnemyStatus enemyStatus;
    private Transform player = null;
    private SpriteRenderer sr = null;
    private Animator anim = null;
    private Rigidbody2D rb = null;
    private bool isScreen = false;
    private float timeCount = 0;
    private float xVector;
    private float xScale;
    private float moveTimeRand;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        enemyStatus = GetComponent<EnemyStatus>();
        if (transform.position.x >= player.position.x)
        {
            xVector = -1;
            xScale = -1;
        }
        if (transform.position.x < player.position.x)
        {
            xVector = 1;
            xScale = 1;
        }
        moveTimeRand = Random.Range(moveTimeMin, moveTimeMax);
    }

    void Update()
    {
        if (sr.isVisible)
        {
            isScreen = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!enemyStatus.isDead)
        {
            if (!enemyStatus.isKnockback && (isScreen || nonVisibleAct))
            {
                Movement();
            }
            else if (enemyStatus.isKnockback)
            {
                Knockback();
            }
        }
    }
    
    
    private void Movement()
    {
        if (timeCount > moveTimeRand)
        {
            moveTimeRand = Random.Range(moveTimeMin, moveTimeMax);
            anim.SetBool("jump", true);
            groundCheck.isOn = false;
            if (transform.position.x >= player.position.x && MathCheck.Probability(turnRate))
            {
                xVector = -1;
                xScale = -1;
            }
            if (transform.position.x < player.position.x && MathCheck.Probability(turnRate))
            {
                xVector = 1;
                xScale = 1;
            }
            timeCount = 0;
            rb.velocity = new Vector2(xVector * moveSpeed, jumpPower);
            transform.localScale = new Vector3(xScale * transform.localScale.x, 
                transform.localScale.y, transform.localScale.z);
        }
        if(groundCheck.isOn)
        {
            anim.SetBool("jump", false);
            timeCount += Time.deltaTime;
        }
    }


    private void Knockback()
    {
        float knockbackForce = enemyStatus.knockbackForce;
        if (transform.position.x > player.position.x)
        {
            rb.velocity = new Vector2(knockbackForce, knockbackForce);
            transform.localScale = new Vector3(-2.5f, transform.localScale.y, transform.localScale.z);
        }
        if (transform.position.x < player.position.x)
        {
            rb.velocity = new Vector2(-knockbackForce, knockbackForce);
            transform.localScale = new Vector3(2.5f, transform.localScale.y, transform.localScale.z);
        }
    }
}
