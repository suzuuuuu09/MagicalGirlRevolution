using Com.LuisPedroFonseca.ProCamera2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [Header("�ړ�")]
    public float moveSpeed;                      // �ړ����x
    public float jumpPower;                      // �W�����v��
    public float moveTime;                       // �ړ�����
    [Header("��ʊO�ł�������")]
    public bool nonVisibleAct;                   // ��ʊO�ł�������
    [Space(40)]
    public EnemyStatus enemyStatus;
    public ColliderCheck groundCheck;


    private Transform player = null;
    private SpriteRenderer sr = null;
    private Animator anim = null;
    private Rigidbody2D rb = null;
    private bool isScreen = false;
    private float timeCount = 0;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        if (transform.position.x >= player.position.x)
        {
            transform.localScale = new Vector3(-2.5f, transform.localScale.y, transform.localScale.z);
        }
        if (transform.position.x < player.position.x)
        {
            transform.localScale = new Vector3(2.5f, transform.localScale.y, transform.localScale.z);
        }
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
        if (timeCount > moveTime)
        {
            anim.SetBool("jump", true);
            groundCheck.isOn = false;
            if (transform.position.x >= player.position.x)
            {
                rb.velocity = new Vector2(-moveSpeed, jumpPower);
                transform.localScale = new Vector3(-2.5f, transform.localScale.y, transform.localScale.z);
            }
            if (transform.position.x < player.position.x)
            {
                rb.velocity = new Vector2(moveSpeed, jumpPower);
                transform.localScale = new Vector3(2.5f, transform.localScale.y, transform.localScale.z);
            }
            timeCount = 0;
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
        }
        if (transform.position.x < player.position.x)
        {
            rb.velocity = new Vector2(-knockbackForce, knockbackForce);
        }
    }
}
