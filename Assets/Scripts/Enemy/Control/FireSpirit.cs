using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FireSpirit : MonoBehaviour
{
    public float bulletSpeed;  　                   //弾の速度
    public float limitSpeed;                        //弾の制限速度


    private Rigidbody2D rb;                         //弾のRigidbody2D
    private Transform bulletTrans;                  //弾のTransform
    private Transform playerTrans;                   //追いかける対象のTransform


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bulletTrans = GetComponent<Transform>();
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        Movement();
        ScaleWithoutInfluence();
    }


    private void Movement()
    {
        Vector2 vector2 = playerTrans.position - bulletTrans.position;  //弾から追いかける対象への方向を計算
        rb.AddForce(vector2.normalized * bulletSpeed);                  //方向の長さを1に正規化、任意の力をAddForceで加える

        float speedXTemp = Mathf.Clamp(rb.velocity.x, -limitSpeed, limitSpeed);　//X方向の速度を制限
        float speedYTemp = Mathf.Clamp(rb.velocity.y, -limitSpeed, limitSpeed);  //Y方向の速度を制限
        rb.velocity = new Vector2(speedXTemp, speedYTemp);　　　　　　　　　　　//実際に制限した値を代入
    }


    private void ScaleWithoutInfluence()
    {
        float xScale = 1;
        if (playerTrans.localScale.x > 0)
        {
            xScale = -1;
        }
        /*
        else if (playerTrans.localScale.x < 0)
        {
            xScale = -1;
        }
        */
        transform.localScale = new Vector3(xScale * transform.localScale.x, 
                                           transform.localScale.y, 
                                           transform.localScale.z);
    }

}
