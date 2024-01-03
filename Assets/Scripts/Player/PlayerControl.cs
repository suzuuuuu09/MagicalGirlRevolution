using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Com.LuisPedroFonseca.ProCamera2D;

public class PlayerControl: MonoBehaviour
{
    [Header("チェックポイント")]
    public Transform[] continuePoint;            // 再開位置
    [Header("移動")]
    public float speed = 10;                     // 移動速度
    public float jumpPower;                     // ジャンプ力
    [Header("判定")]
    public GroundCheck ground;                   // 地面接触判定
    public HeadCheck head;                       // 頭接触判定
    [Header("攻撃")]
    public Transform attackPointProx;            // 攻撃範囲
    public float attackRange = 0.5f;             // 攻撃範囲
    public int attackDamage = 40;                // ダメージ量
    public float attackRate = 2f;                // 攻撃割合
    public LayerMask enemyLayers;                // 敵レイヤー
    [Header("必殺技")]
    public Transform attackPointUlt;             // 必殺技攻撃場所
    public Vector2 attackPointSize;              // 必殺技攻撃範囲
    public float ultRate = 1f;                   // MP消費周期
    public float ulkCoolDownTime;                // 必殺技クールタイム
    public int attackDamageUlt = 10;             // 必殺技ダメージ量
    public LayerMask enemyLayersUlt;             // 敵レイヤー
    public float hitRate;                        // ヒット確率
    public float recoveryRate;                   // MP回復確率
    [Space(40)]
    public PlayerStatus playerStatus;
    public EnemyStatus enemyStatus;


    private Rigidbody2D rb = null;
    private Animator anim = null;
    private bool headCheck = false;
    private bool groundCheck = false;
    private bool isJump = false;
    private float nextAttackTime = 0f;
    private float nextUltTime = 0f;
    private bool isUlt = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        transform.position = continuePoint[0].position;
    }


    private void Update()
    {
        if (!playerStatus.isDead)
        {
            // 近接攻撃
            if (Time.time >= nextAttackTime && Input.GetKeyDown(KeyCode.Z))
            {
                Attack_prox();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        
            // 必殺技
            if (playerStatus.curMP >= 2 && Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= nextUltTime)
            {
                nextUltTime = Time.time + ulkCoolDownTime;
                isUlt = true;
                playerStatus.curMP -= 2;
                AudioManager.instance.Play("Ult");
                anim.SetTrigger("atck_ult_start");
                StartCoroutine(Attack_ult());
            }
            if (Input.GetKeyUp(KeyCode.LeftShift) || isJump || playerStatus.curMP <= 0)
            {
                isUlt = false;
                StopCoroutine(Attack_ult());
                anim.SetTrigger("atck_ult_end");
            }
        }
    }


    void FixedUpdate()
    {
        groundCheck = ground.IsGround();
        headCheck = head.IsGround();
        if (!playerStatus.isDead && !playerStatus.isKnockback)
        {
            float horizontalKey = Input.GetAxisRaw("Horizontal");
            float verticalKey = Input.GetAxisRaw("Vertical");
            anim.SetBool("run", false);

            // 左右移動
            if (horizontalKey > 0)
            {
                anim.SetBool("run", true);
                rb.velocity = new Vector2(speed, rb.velocity.y);
                if (!isUlt)
                {
                    transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
                }
            }
            else if (horizontalKey < 0)
            {
                anim.SetBool("run", true);
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                if(!isUlt) 
                {
                    transform.localScale = new Vector3(-2.5f, 2.5f, 2.5f);
                }
            }
            else
            {
                // 横方向の入力がない場合、速度を0に
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            // ジャンプ
            if (groundCheck && !headCheck && verticalKey > 0)
            {
                Jump();
            }
            else
            {
                isJump = false;
            }
            anim.SetBool("jump", isJump);
            anim.SetBool("ground", groundCheck);
        }
        else if(!playerStatus.isDead && playerStatus.isKnockback)
        {
            Knockback();
        }
    }


    private void Attack_prox()
    {
        if (attackPointProx == null)
        {
            Debug.LogError("Attack point is not assigned!");
            return;
        }
        ProCamera2DShake.Instance.Shake("AttackProx");
        anim.SetTrigger("atck_prox");
        AudioManager.instance.Play("Attack_prox");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPointProx.position, attackRange, enemyLayers);
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyStatus>().TakeDamage(attackDamage);
        }
    }

    
    IEnumerator Attack_ult()
    {
        yield return new WaitForSeconds(ultRate);
        for (; ; )
        {
            if (playerStatus.curMP <= 0 || !isUlt)
            {
                break;
            }

            yield return new WaitForSeconds(ultRate / 2);
            AttackUlt();
            if (playerStatus.curMP <= 0 || !isUlt)
            {
                break;
            }

            yield return new WaitForSeconds(ultRate / 2);
            playerStatus.curMP--;
            AttackUlt();
        }
    }


    void AttackUlt()
    {
        if (attackPointUlt == null)
        {
            Debug.LogError("Attack point is not assigned!");
            return;
        }
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(
            attackPointUlt.position, attackPointSize, 0, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            ProCamera2DShake.Instance.Shake("AttackUlt");
            enemy.GetComponent<EnemyStatus>().TakeDamageMagic(
                attackDamageUlt, hitRate, recoveryRate);
        }
    }


    private void Jump()
    {
        AudioManager.instance.Play("Jump");
        rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        groundCheck = false;
        isJump = true;
    }


    private void Knockback()
    {
        groundCheck = false;
        isJump = true;
        float knockbackForce = playerStatus.knockbackForce;
        bool isKnockbackFromRight = playerStatus.isKnockbackFromRight;
        if (isKnockbackFromRight)
        {
            rb.velocity = new Vector2(-knockbackForce, knockbackForce);
        }
        else if (!isKnockbackFromRight)
        {
            rb.velocity = new Vector2(knockbackForce, knockbackForce);
        }
        anim.SetBool("jump", isJump);
        anim.SetBool("ground", groundCheck);
    }


    private void OnDrawGizmosSelected()
    {
        if(attackPointProx == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPointProx.position, attackRange);
        if(attackPointUlt == null)
        {
            return;
        }
        Gizmos.DrawWireCube(attackPointUlt.position, 
            new Vector3(attackPointSize.x, attackPointSize.y));
    }


    private void StartToMiddle()
    {
        anim.SetTrigger("atck_ult_middle");
    }


    private void ResetUltEnd()
    {
        anim.ResetTrigger("atck_ult_end");
    }


}

