using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [Header("移動")]
    public float speed;            // 移動速度
    [Header("画面外でも動かす")]
    public bool nonVisibleAct;     // 画面外でも動かす
    [Header("接触判定")]
    public WallCheckR wallCheckR;  // 壁接触判定右
    public WallCheckL wallCheckL;  // 壁接触判定左
    

    public static bool rightTleftF = false;


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
        if (isScreen || nonVisibleAct)
        {
            anim.SetBool("run", true);
            int xVector = 1;
            if (wallCheckR.isOn || wallCheckL.isOn)
            {
                rightTleftF = !rightTleftF;
            }
            if (rightTleftF)
            {
                xVector = -1;
                transform.localScale = new Vector3(2.5f, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(-2.5f, transform.localScale.y, transform.localScale.z);
            }
            transform.position += new Vector3(-xVector * speed, 0, 0);
            rb.velocity = new Vector2(-xVector, rb.velocity.y);
        }
        else
        {
            anim.SetBool("run", false);
            rb.Sleep();
        }
    }
}
