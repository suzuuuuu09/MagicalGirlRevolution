using Com.LuisPedroFonseca.ProCamera2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [Header("ˆÚ“®")]
    public float moveSpeed;                      // ˆÚ“®‘¬“x
    public float jumpPower;
    public float moveTime;
    [Header("‰æ–ÊŠO‚Å‚à“®‚©‚·")]
    public bool nonVisibleAct;               // ‰æ–ÊŠO‚Å‚à“®‚©‚·
    [Space(40)]
    public EnemyStatus enemyStatus;
    public Transform player;
    public ColliderCheck groundCheck;


    public static bool rightTleftF = false;

    private float xSpeed;
    private SpriteRenderer sr = null;
    private Animator anim = null;
    private Rigidbody2D rb = null;
    private bool isScreen = false;
    private float timeCount = 0;


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
        }
        else if (enemyStatus.isKnockback)
        {
            Knockback();
        }
    }


    private void Movement()
    {
        timeCount += Time.deltaTime;
        if (timeCount > moveTime)
        {
            groundCheck.isOn = false;
            if (transform.position.x > player.position.x)
            {
                rb.velocity = new Vector2(-moveSpeed, jumpPower);
            }
            if (transform.position.x < player.position.x)
            {
                rb.velocity = new Vector2(moveSpeed, jumpPower);
            }
            timeCount = 0;
        }
        if(groundCheck.isOn)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
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
