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
    [Header("�`�F�b�N�|�C���g")]
    public Transform[] continuePoint;            // �ĊJ�ʒu
    [Header("�ړ�")]
    public float speed = 10;                     // �ړ����x
    public float jumpPower;                     // �W�����v��
    [Header("����")]
    public GroundCheck ground;                   // �n�ʐڐG����
    public HeadCheck head;                       // ���ڐG����
    [Header("�U��")]
    public Transform attackPointProx;            // �U���͈�
    public float attackRange = 0.5f;             // �U���͈�
    public int attackDamage = 40;                // �_���[�W��
    public float attackRate = 2f;                // �U������
    public LayerMask enemyLayers;                // �G���C���[
    [Header("�K�E�Z")]
    public Transform attackPointUlt;             // �K�E�Z�U���ꏊ
    public Vector2 attackPointSize;              // �K�E�Z�U���͈�
    public float ultRate = 1f;                   // MP�������
    public float ulkCoolDownTime;                // �K�E�Z�N�[���^�C��
    public int attackDamageUlt = 10;             // �K�E�Z�_���[�W��
    public LayerMask enemyLayersUlt;             // �G���C���[
    public float hitRate;                        // �q�b�g�m��
    public float recoveryRate;                   // MP�񕜊m��
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
            // �ߐڍU��
            if (Time.time >= nextAttackTime && Input.GetKeyDown(KeyCode.Z))
            {
                Attack_prox();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        
            // �K�E�Z
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

            // ���E�ړ�
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
                // �������̓��͂��Ȃ��ꍇ�A���x��0��
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            // �W�����v
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

