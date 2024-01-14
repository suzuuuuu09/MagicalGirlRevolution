using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FireSpirit : MonoBehaviour
{
    public float bulletSpeed;  �@                   //�e�̑��x
    public float limitSpeed;                        //�e�̐������x


    private Rigidbody2D rb;                         //�e��Rigidbody2D
    private Transform bulletTrans;                  //�e��Transform
    private Transform playerTrans;                   //�ǂ�������Ώۂ�Transform


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
        Vector2 vector2 = playerTrans.position - bulletTrans.position;  //�e����ǂ�������Ώۂւ̕������v�Z
        rb.AddForce(vector2.normalized * bulletSpeed);                  //�����̒�����1�ɐ��K���A�C�ӂ̗͂�AddForce�ŉ�����

        float speedXTemp = Mathf.Clamp(rb.velocity.x, -limitSpeed, limitSpeed);�@//X�����̑��x�𐧌�
        float speedYTemp = Mathf.Clamp(rb.velocity.y, -limitSpeed, limitSpeed);  //Y�����̑��x�𐧌�
        rb.velocity = new Vector2(speedXTemp, speedYTemp);�@�@�@�@�@�@�@�@�@�@�@//���ۂɐ��������l����
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
