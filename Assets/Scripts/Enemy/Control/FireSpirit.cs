using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FireSpirit : MonoBehaviour
{
    [Header("移動")]
    public float bulletSpeed;  　                   // 速度
    public float limitSpeed;                        // 制限速度


    private EnemyStatus enemyStatus;
    private Rigidbody2D rb;                         // Rigidbody2D
    private Transform bulletTrans;                  // Transform
    private Transform player;                   //追いかける対象のTransform


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
        Vector2 vector2 = player.position - bulletTrans.position;  //弾から追いかける対象への方向を計算
        rb.AddForce(vector2.normalized * bulletSpeed);                  //方向の長さを1に正規化、任意の力をAddForceで加える

        float speedXTemp = Mathf.Clamp(rb.velocity.x, -limitSpeed, limitSpeed);　//X方向の速度を制限
        float speedYTemp = Mathf.Clamp(rb.velocity.y, -limitSpeed, limitSpeed);  //Y方向の速度を制限
        rb.velocity = new Vector2(speedXTemp, speedYTemp);　　　　　　　　　　　//実際に制限した値を代入
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
