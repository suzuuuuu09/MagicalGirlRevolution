using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl: MonoBehaviour
{
    [Header("ˆÚ“®")]
    public float speed = 10;                     // ˆÚ“®‘¬“x
    public float jump_power;                     // ƒWƒƒƒ“ƒv—Í
    [Header("”»’è")]
    public GroundCheck ground;                   // ’n–ÊÚG”»’è
    public HeadCheck head;                       // “ªÚG”»’è
    [Header("UŒ‚")]
    public Transform attackPointProx;            // UŒ‚”ÍˆÍ
    public float attackRange = 0.5f;             // UŒ‚”ÍˆÍ
    public int attackDamage = 40;                // ƒ_ƒ[ƒW—Ê
    public float attackRate = 2f;                // UŒ‚Š„‡
    public LayerMask enemyLayers;                // “GƒŒƒCƒ„[
    [Header("•KE‹Z")]
    public Transform attackPointUlt;             // •KE‹ZUŒ‚”ÍˆÍ
    public Vector2 attackPointSize;
    public float ultRate = 1f;                   // MPÁ”ïüŠú
    public float ultTime = 0f;
    public int attackDamageUlt = 10;
    public LayerMask enemyLayersUlt;
    public int hitRate;
    public int recoveryRate;
    [Space(40)]
    public PlayerStatus playerStatus;
    public EnemyManager enemyManager;


    private Rigidbody2D rb = null;
    private Animator anim = null;
    private bool head_check = false;
    private bool ground_check = false;
    private bool isJump = false;
    private bool isUlt = false;
    private float nextAttackTime = 0f;


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
        // ‹ßÚUŒ‚
        if (Time.time >= nextAttackTime && !playerStatus.isDead && Input.GetKeyDown(KeyCode.Z))
        {
            Attack_prox();
            nextAttackTime = Time.time + 1f / attackRate;
        }
        
        // •KE‹Z
        if(Input.GetKeyDown(KeyCode.LeftShift) && playerStatus.curMP >= 2)
        {
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

            // ¶‰EˆÚ“®
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
                // ‰¡•ûŒü‚Ì“ü—Í‚ª‚È‚¢ê‡A‘¬“x‚ğ0‚É
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            // ƒWƒƒƒ“ƒv
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
            playerStatus.curMP -= 1;
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
        CinemachineShake.instance.ShakeCamera(2f, .1f);
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPointUlt.position, attackPointSize, 0, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyManager>().TakeDamageMagic(attackDamageUlt, hitRate, recoveryRate);
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
        if(attackPointUlt == null)
        {
            return;
        }
        Gizmos.DrawWireCube(attackPointUlt.position, new Vector3(attackPointSize.x, attackPointSize.y));
    }
}

