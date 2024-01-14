using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skelton : MonoBehaviour
{
    [Header("移動")]
    public float speed;                      // 移動速度
    [Header("Jump")]
    public float jumpPower;
    [Header("画面外でも動かす")]
    public bool nonVisibleAct;               // 画面外でも動かす
    [Header("接触判定")]
    public ColliderCheck wallCheckR;
    public ColliderCheck wallCheckL;
    public IsGround groundCheckR;
    public IsGround groundCheckL;
    public IsGround ground;
    public EnemyStatus enemyStatus;
    

    public static bool rightTleftF = false;

    private float xSpeed;
    private Transform player = null;
    private SpriteRenderer sr = null;
    private Animator anim = null;
    private Rigidbody2D rb = null;
    private bool isScreen = false;
    private bool isGround = false;
    private bool isGroundR = false;
    private bool isGroundL = false;



    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
        isGround = ground.IsGrounds();
        isGroundL = groundCheckL.IsGrounds();
        isGroundR = groundCheckR.IsGrounds();
        xSpeed = speed;
        anim.SetBool("run", true);
        if ((wallCheckR.isOn || wallCheckL.isOn) || (!isGroundL || !isGroundR))
        {
            Jump();
            //rightTleftF = !rightTleftF;
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


    private void Jump()
    {
        if (isGround)
        {
            isGroundL = true;
            isGroundR = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
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
