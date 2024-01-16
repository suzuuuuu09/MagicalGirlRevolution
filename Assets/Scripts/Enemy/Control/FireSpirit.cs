using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FireSpirit : MonoBehaviour
{
    [Header("�ړ�")]
    public float bulletSpeed;  �@                   // ���x
    public float limitSpeed;                        // �������x


    private EnemyStatus enemyStatus;
    private Rigidbody2D rb;                         // Rigidbody2D
    private Transform bulletTrans;                  // Transform
    private Transform player;                   //�ǂ�������Ώۂ�Transform


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bulletTrans = GetComponent<Transform>();
        enemyStatus = GetComponent<EnemyStatus>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    private void FixedUpdate()
    {
        ScaleWithoutInfluence();
        if (!enemyStatus.isDead)
        {
            if (!enemyStatus.isKnockback)
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
        Vector2 vector2 = player.position - bulletTrans.position;  //�e����ǂ�������Ώۂւ̕������v�Z
        rb.AddForce(vector2.normalized * bulletSpeed);                  //�����̒�����1�ɐ��K���A�C�ӂ̗͂�AddForce�ŉ�����

        float speedXTemp = Mathf.Clamp(rb.velocity.x, -limitSpeed, limitSpeed);�@//X�����̑��x�𐧌�
        float speedYTemp = Mathf.Clamp(rb.velocity.y, -limitSpeed, limitSpeed);  //Y�����̑��x�𐧌�
        rb.velocity = new Vector2(speedXTemp, speedYTemp);�@�@�@�@�@�@�@�@�@�@�@//���ۂɐ��������l����
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


    private void ScaleWithoutInfluence()
    {
        float xScale = -1;
        if (player.position.x > transform.position.x)
        {
            xScale = 1;
        }
        transform.localScale = new Vector3(xScale * 2.5f, transform.localScale.y, transform.localScale.z);
    }

}
