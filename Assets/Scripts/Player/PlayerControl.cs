using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

//PlayerControlManager
public class PlayerControl: MonoBehaviour
{
    [Header("�ړ����l")]
    public float speed = 10;                     // �ړ����x
    public float jump_power;                     // �W�����v��
    [Header("����")]
    public GroundCheck ground;                   // �n�ʐڐG����
    public HeadCheck head;                       // ���ڐG����
    [Header("�U��")]
    public Transform attackPointProx;            // �U���ꏊ
    public float attackRange = 0.5f;             // �U���͈�
    public int attackDamage = 40;                // �_���[�W��
    public float attackRate = 2f;                // �U������
    public float nextAttackTime = 0f;            // ���ɍU������܂ł̎���
    public LayerMask enemyLayers;                // �G���C���[
    [Header("�K�E�Z")]
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
        // �ߐڍU��
        if (Time.time >= nextAttackTime && !playerStatus.isDead && Input.GetKeyDown(KeyCode.Z))
        {
            Attack_prox();
            nextAttackTime = Time.time + 1f / attackRate;
        }
        
        // �K�E�Z
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

            // ���E�ړ�
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
                // �������̓��͂��Ȃ��ꍇ�A���x��0��
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            // �W�����v
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

