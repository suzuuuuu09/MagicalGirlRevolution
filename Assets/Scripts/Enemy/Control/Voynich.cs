using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voynich : MonoBehaviour
{
    [Header("èÛë‘")]
    public VoynichState voynichState = VoynichState.Idle;
    public VoynichTactic voynichTactic = VoynichTactic.Inattention;
    [Header("à⁄ìÆ")]
    public float walkSpeed;
    [Header("Attack")]
    public Transform attackPoint;
    public float attackDistance;
    public float attackRange;
    public LayerMask playerLayer;


    private Transform player = null;
    private Animator anim = null;
    private Rigidbody2D rb = null;
    private int attackCount = 0;
    private float attackTime = 0;
    private float attackInternal = 0;


    public enum VoynichState
    {
        Idle,
        Walk,
        Attack,
        Cast,
        Wait
    }

    public enum VoynichTactic
    {
        Inattention = 1,
        Vigilance = 2,
        Ready = 3
    }


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        UpdateState();

        switch (voynichState)
        {
            case VoynichState.Idle:
                Idle();
                break;
            case VoynichState.Walk:
                Walk();
                break;
            case VoynichState.Attack:
                Attack();
                break;
            case VoynichState.Cast:
                Cast();
                break;
            case VoynichState.Wait:
                Wait();
                break;
                
        }

        if (transform.position.x >= player.position.x)
        {
            transform.localScale = new Vector3(7f, transform.localScale.y, transform.localScale.z);
        }
        if (transform.position.x < player.position.x)
        {
            transform.localScale = new Vector3(-7f, transform.localScale.y, transform.localScale.z);
        }
    }


    private void UpdateState()
    {
        float dist = Vector2.Distance(player.position, transform.position);
        attackInternal = Random.Range(1f, 4f);
        if (dist <= attackDistance)
        {
            if(attackTime <= attackInternal)
            {
                if(attackCount < 2)
                {
                    voynichState = VoynichState.Attack;
                }
                else if (attackCount >= 2)
                {
                    attackTime += Time.deltaTime;
                }
            }
            else
            {
                attackTime = 0;
                attackCount = 0;
            }
        }
        else
        {
            voynichState= VoynichState.Walk;
        }
    }


    #region VoynichState

    private void Idle()
    {

    }


    private void Walk()
    {
        anim.SetBool("walk", true);
        if (transform.position.x >= player.position.x)
        {
            rb.velocity = new Vector2(-walkSpeed, rb.velocity.y);
        }
        if (transform.position.x < player.position.x)
        {
            rb.velocity = new Vector2(walkSpeed, rb.velocity.y);
        }
    }


    private void Attack()
    {
        anim.SetTrigger("attack");
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }


    private void Cast()
    {

    }


    private void Wait()
    {

    }


    #endregion VoynichState


    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return;
        }
        Gizmos.color = new Color(1f, 0, 0, 1f);
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }


    private void AnimationAttack()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(
            attackPoint.position, attackRange, playerLayer);
        foreach (Collider2D players in hitPlayer)
        {
            players.GetComponent<PlayerStatus>().Damage(3);
            //PlayerStatus.isPoison = true;
        }
        if (player.position.x <= transform.position.x)
        {
            PlayerStatus.isKnockbackFromRight = true;
        }
        else if (player.position.x > transform.position.x)
        {
            PlayerStatus.isKnockbackFromRight = false;
        }
    }


    private void AnimationAttackCount()
    {
        attackCount++;
    }
}
