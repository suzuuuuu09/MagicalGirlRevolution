using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Com.LuisPedroFonseca.ProCamera2D;

public class PlayerControl: MonoBehaviour
{
    [Header("`FbN|Cg")]
    public Transform[] continuePoint;            // ÄJÊu
    [Header("Ú®")]
    public float speed = 10;                     // Ú®¬x
    public float jumpPower;                     // WvÍ
    [Header("»è")]
    public GroundCheck ground;                   // nÊÚG»è
    public HeadCheck head;                       // ªÚG»è
    [Header("U")]
    public Transform attackPointProx;            // UÍÍ
    public float attackRange = 0.5f;             // UÍÍ
    public int attackDamage = 40;                // _[WÊ
    public float attackRate = 2f;                // U
    public LayerMask enemyLayers;                // GC[
    [Header("KEZ")]
    public Transform attackPointUlt;             // KEZUê
    public Vector2 attackPointSize;              // KEZUÍÍ
    public float ultRate = 1f;                   // MPÁïüú
    public float ulkCoolDownTime;                // KEZN[^C
    public int attackDamageUlt = 10;             // KEZ_[WÊ
    public LayerMask enemyLayersUlt;             // GC[
    public float hitRate;                        // qbgm¦
    public float recoveryRate;                   // MPñm¦
    [Space(40)]
    public EnemyStatus[] enemyStatus;


    private Rigidbody2D rb = null;
    private Animator anim = null;
    private PhysicsMaterial2D pm = null;
    private PlayerStatus playerStatus;
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
        pm = GetComponent<CapsuleCollider2D>().sharedMaterial;
        playerStatus = GetComponent<PlayerStatus>();
        transform.position = continuePoint[0].position;
    }


    private void Update()
    {
        if (!PlayerStatus.isDead)
        {
            // ßÚU
            if (Time.time >= nextAttackTime && Input.GetKeyDown(KeyCode.Z))
            {
                Attack_prox();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        
            // KEZ
            if (PlayerStatus.currentMP >= 2 && Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= nextUltTime)
            {
                nextUltTime = Time.time + ulkCoolDownTime;
                isUlt = true;
                PlayerStatus.currentMP -= 2;
                AudioManager.instance.Play("Ult");
                anim.SetTrigger("atck_ult_start");
                StartCoroutine(Attack_ult());
            }
            if (Input.GetKeyUp(KeyCode.LeftShift) || isJump || PlayerStatus.currentMP <= 0)
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
        if (!PlayerStatus.isDead && !PlayerStatus.isKnockback)
        {
            float horizontalKey = Input.GetAxisRaw("Horizontal");
            float verticalKey = Input.GetAxisRaw("Vertical");
            anim.SetBool("run", false);

            // ¶EÚ®
            if (horizontalKey > 0)
            {
                anim.SetBool("run", true);
                rb.velocity = new Vector2(speed, rb.velocity.y);
                pm.friction = 0f;
                if (!isUlt)
                {
                    transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
                }
            }
            else if (horizontalKey < 0)
            {
                anim.SetBool("run", true);
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                pm.friction = 0f;
                if (!isUlt) 
                {
                    transform.localScale = new Vector3(-2.5f, 2.5f, 2.5f);
                }
            }
            else
            {
                // ¡ûüÌüÍªÈ¢êA¬xð0É
                pm.friction = 8f;
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            // Wv
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
        else if(!PlayerStatus.isDead && PlayerStatus.isKnockback)
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
            enemy.GetComponent<EnemyStatus>().TakeDamage(playerStatus.ATK);
        }
    }

    
    private IEnumerator Attack_ult()
    {
        yield return new WaitForSeconds(ultRate);
        for (; ; )
        {
            if (PlayerStatus.currentMP <= 0 || !isUlt)
            {
                break;
            }

            yield return new WaitForSeconds(ultRate / 2);
            AttackUlt();
            if (PlayerStatus.currentMP <= 0 || !isUlt)
            {
                break;
            }

            yield return new WaitForSeconds(ultRate / 2);
            PlayerStatus.currentMP--;
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
        bool isKnockbackFromRight = PlayerStatus.isKnockbackFromRight;
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

