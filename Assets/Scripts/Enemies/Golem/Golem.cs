using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Enemy
{
    int attack = 0;
    public float knockForce = 15f;
    readonly int m_HashAttack = Animator.StringToHash("Attack");
    [SerializeField] float dashForce = 0f;
    [SerializeField] float dashDelay = 0.8f;
    Vector2 playerDirection;

    [Header("SFX")]
    [SerializeField] SoundEffectSO sfx_Die;
    [SerializeField] SoundEffectSO sfx_Trigger;
    [SerializeField] SoundEffectSO sfx_Attack;
    [SerializeField] SoundEffectSO sfx_Windup;

    protected override void HandleChasing()
    {
        if (m_HashAttack == m_CurrentStateInfo.shortNameHash) { return; }

        anim.SetBool("isWalking", true);
        playerDirection = transform.position - _player.position;
        playerDirection.Normalize();

        Vector2 newPos = new Vector2(playerDirection.x * stats.speed, 0f);
        Vector2 position = new Vector2(transform.position.x, transform.position.y);

        rb.MovePosition(rb.position - newPos * Time.deltaTime);

        FlipSprite(-playerDirection);

        if (Vector2.Distance(_player.position, rb.position) < stats.attackRange)
        {
            state = EnemyState.Attacking;
            anim.SetBool("isWalking", false);
        }
    }

    protected override void HandleAttacking()
    {
        base.HandleAttacking();

        if (attack == 0 && m_HashAttack != m_CurrentStateInfo.shortNameHash)
        {
            attack = 1;
            anim.SetTrigger("Attack");
            StartCoroutine(StartDash(dashDelay));
            StartCoroutine(BeginCooldown(stats.attackCooldown));
        }

        if (Vector2.Distance(_player.position, rb.position) > stats.attackRange)
        {
            state = EnemyState.Chasing;
            anim.SetBool("isWalking", true);
            attack = 0;
        }
    }

    IEnumerator StartDash(float delay)
    {
        Vector2 dir = new Vector2(0f,0f);
        if (playerDirection.x > 0)
        {
            dir = new Vector2(-1f, 0f);
        }
        else if (playerDirection.x < 0)
        {
            dir = new Vector2(1f, 0f);
        }

        yield return new WaitForSeconds(dashDelay);

        GetComponent<Rigidbody2D>().AddForce(dir * dashForce * 10f);
    }

    IEnumerator BeginCooldown(float duration)
    {
        for (float i = 0f; i < 1f; i += Time.deltaTime / duration)
        {
            yield return null;
        }

        attack = 0;
    }

    public void TriggerSound() // called in animators
    {
        SoundManager.instance.PlaySFX(sfx_Trigger);
    }

    public void SoundWindup() // called in animators
    {
        SoundManager.instance.PlaySFX(sfx_Windup);
    }

    public void SoundAttack() // called in animators
    {
        SoundManager.instance.PlaySFX(sfx_Attack);
    }

    public override void Die()
    {
        base.Die();
        SoundManager.instance.PlaySFX(sfx_Die);
    }
}
