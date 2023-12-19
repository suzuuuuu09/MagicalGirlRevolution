using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

//PlayerControlManager
public class PlayerControl: MonoBehaviour
{
    [Header("移動数値")]
    public float speed = 10;                     // 移動速度
    public float jump_power;                     // ジャンプ力
    [Header("判定")]
    public GroundCheck ground;                   // 地面接触判定
    public HeadCheck head;                       // 頭接触判定
    [Header("攻撃")]
    public Transform attackPointProx;            // 攻撃場所
    public float attackRange = 0.5f;             // 攻撃範囲
    public int attackDamage = 40;                // ダメージ量
    public float attackRate = 2f;                // 攻撃割合
    public float nextAttackTime = 0f;            // 次に攻撃するまでの時間
    public LayerMask enemyLayers;                // 敵レイヤー
    [Header("必殺技")]
    public float ultTime = 0f;
    [Space(40)]
    public PlayerStatus playerStatus;


    private Rigidbody2D rb = null;
    private Animator anim = null;
    private bool head_check = false;
    private bool ground_check = false;
    private bool isJump = false;


    public void StartToMiddle()
    {
        anim.SetTrigger("atck_ult_middle");
    }


    public void UltEnd()
    {
        anim.ResetTrigger("atck_ult_end");
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }


    private void Update()
    {
        // 近接攻撃
        if (Time.time >= nextAttackTime && !playerStatus.isDead && Input.GetKeyDown(KeyCode.Z))
        {
            Attack_prox();
            nextAttackTime = Time.time + 1f / attackRate;
        }
        
        // 必殺技
        if(Input.GetKeyDown(KeyCode.LeftShift) && playerStatus.curMP >= 2)
        {
            playerStatus.curMP -= 2;
            AudioManager.instance.Play("Ult");
            anim.SetTrigger("atck_ult_start");
            StartCoroutine(Attack_ult());
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || isJump)
        {
            anim.SetTrigger("atck_ult_end");
        }
        /*
        if (Input.GetKeyDown(KeyCode.LeftControl) && ground_check)
        {
            anim.SetBool("roll", true);
        }
        */
    }


    void FixedUpdate()
    {
        if (!playerStatus.isDead)
        {
            ground_check = ground.IsGround();
            head_check = head.IsGround();
            float horizontalKey = Input.GetAxisRaw("Horizontal");
            float verticalKey = Input.GetAxisRaw("Vertical");
            anim.SetBool("run", false);

            // 左右移動
            if (horizontalKey > 0)
            {
                anim.SetBool("run", true);
                transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
            else if (horizontalKey < 0)
            {
                anim.SetBool("run", true);
                transform.localScale = new Vector3(-2.5f, 2.5f, 2.5f);
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            }
            else
            {
                // 横方向の入力がない場合、速度を0に
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            // ジャンプ
            if (ground_check && !head_check && verticalKey > 0)
            {
                Jump();
            }
            else
            {
                isJump = false;
            }

            anim.SetBool("jump", isJump);
            anim.SetBool("ground", ground_check);
        }
    }


    IEnumerator Attack_ult()
    {
        print("a");
        yield return new WaitForSeconds(1.0f);

    }


    private void Attack_prox()
    {
        if (attackPointProx == null)
        {
            Debug.LogError("Attack point is not assigned!");
            return;
        }
        CinemachineShake.instance.ShakeCamera(2f, .1f);
        anim.SetTrigger("atck_prox");
        AudioManager.instance.Play("Attack_prox");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPointProx.position, attackRange, enemyLayers);
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyManager>().TakeDamage(attackDamage);
        }
    }


    private void Jump()
    {
        AudioManager.instance.Play("Jump");
        rb.velocity = new Vector2(rb.velocity.x, jump_power);
        ground_check = false;
        isJump = true;
    }


    private void OnDrawGizmosSelected()
    {
        if(attackPointProx == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPointProx.position, attackRange);
    }
}

