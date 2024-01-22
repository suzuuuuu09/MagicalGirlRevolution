using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skelton : MonoBehaviour
{
    [Header("移動")]
    public float speed;                      // 移動速度
    [Header("Jump")]
    public float jumpPower;                  // ジャンプ力
    [Header("接触判定")]
    public ColliderCheck wallCheckR;
    public ColliderCheck groundCheckR;
    public IsGround ground;
    public EnemyStatus enemyStatus;
    

    private Transform player = null;
    private Animator anim = null;
    private Rigidbody2D rb = null;
    private float xSpeed;
    private float abandonCountTime;
    private bool isGround = false;
    private bool isAbandon = false;
    private bool rightTleftF = false;
    private int jumpCount;
    private int jumpRandCount;


    private bool IsPlayerRight()
    {
        if (transform.position.x < player.position.x)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        jumpRandCount = Random.Range(4, 6);
    }


    void FixedUpdate()
    {
        if (!enemyStatus.isKnockback)
        {
            if(!isAbandon)
            {
                Movement();
            }
            else
            {
                AbandonMovement();
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
        xSpeed = speed;
        anim.SetBool("run", true);
        if (wallCheckR.isOn || !groundCheckR.isOn)
        {
            WallCheckAbandon();
            Jump();
        }
        if (IsPlayerRight())
        {
            xSpeed = -speed;
            transform.localScale = new Vector3(2.5f, transform.localScale.y, transform.localScale.z);
        }
        else if (!IsPlayerRight())
        {
            transform.localScale = new Vector3(-2.5f, transform.localScale.y, transform.localScale.z);
        }
        rb.velocity = new Vector2(-xSpeed, rb.velocity.y);
    }


    private void AbandonMovement()
    {
        isGround = ground.IsGrounds();
        xSpeed = speed;
        anim.SetBool("run", true);
        if (wallCheckR.isOn || !groundCheckR.isOn)
        {
            WallCheckRightTleftF();
            Jump();
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
        groundCheckR.isOn = true;
        if (isGround)
        {
            jumpCount++;
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
    }


    private void Knockback()
    {
        groundCheckR.isOn = true;
        isAbandon = false;
        float knockbackForce = enemyStatus.knockbackForce;
        if (IsPlayerRight())
        {
            rb.velocity = new Vector2(-knockbackForce, knockbackForce);
        }
        else if (!IsPlayerRight())
        {
            rb.velocity = new Vector2(knockbackForce, knockbackForce);
        }
    }


    private void WallCheckAbandon()
    {
        if(jumpCount == 0)
        {
            jumpRandCount = Random.Range(4, 6);
            abandonCountTime = Time.time + jumpRandCount;
        }
        if (jumpCount >= jumpRandCount)
        {
            if (Time.time < abandonCountTime)
            {
                isAbandon = true;
            }
            jumpCount = 0;
        }
    }


    private void WallCheckRightTleftF()
    {
        if (jumpCount == 0)
        {
            jumpRandCount = Random.Range(4, 6);
            abandonCountTime = Time.time + jumpRandCount;
        }
        if (jumpCount >= jumpRandCount)
        {
            if (Time.time < abandonCountTime)
            {
                rightTleftF = !rightTleftF;
            }
            jumpCount = 0;
        }
    }
}
