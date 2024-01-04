using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skelton : MonoBehaviour
{
    [Header("移動")]
    public float speed;                      // 移動速度
    [Header("画面外でも動かす")]
    public bool nonVisibleAct;               // 画面外でも動かす
    [Header("接触判定")]
    public WallCheckR wallCheckR;
    public WallCheckL wallCheckL;
    public EnemyStatus enemyStatus;
    public Transform player;
    

    public static bool rightTleftF = false;

    private float xSpeed;
    private SpriteRenderer sr = null;
    private Animator anim = null;
    private Rigidbody2D rb = null;
    private bool isScreen = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
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
        if (!enemyStatus.isKnockback)
        {
            if (isScreen || nonVisibleAct)
            {
                Movement();
            }
            else
            {
                anim.SetBool("run", false);
                rb.Sleep();
            }
        }
        else if (enemyStatus.isKnockback)
        {
            Knockback();
        }
    }


    private void Movement()
    {
        xSpeed = speed;
        anim.SetBool("run", true);
        if (wallCheckR.isOn || wallCheckL.isOn)
        {
            rightTleftF = !rightTleftF;
        }
        if (rightTleftF)
        {
            xSpeed = -speed;
            transform.localScale = new Vector3(2.5f, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-2.5f, transform.localScale.y, transform.localScale.z);
        }
        rb.velocity = new Vector2(-xSpeed, rb.velocity.y);
    }


    private void Knockback()
    {
        float knockbackForce = enemyStatus.knockbackForce;
        if (transform.position.x > player.position.x)
        {
            rb.velocity = new Vector2(knockbackForce, knockbackForce);
        }
        if (transform.position.x < player.position.x)
        {
            rb.velocity = new Vector2(-knockbackForce, knockbackForce);
        }
    }
}
