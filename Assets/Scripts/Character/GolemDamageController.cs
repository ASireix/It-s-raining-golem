using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class GolemDamageController : DamageController
{
    [SerializeField] Enemy enemyController;

    public override void TakeDamage(float amount, float force, Transform enemy)
    {
        if(!enemyController.canTakeDamage) { return; }

        enemyController.UpdateHealth(enemyController.hp - amount);


        StartCoroutine(enemyController.BlinkSprite(enemyController.spriteRenderer, Color.red, 1, 0.5f));

        if (enemyController.hp <= 0)
        {
            enemyController.Die();
        }
        else if (enemy)
        {
            Vector2 direction = transform.position - enemy.position;
            direction.Normalize();

            Vector2 test = new Vector2(direction.x, Mathf.Abs(direction.x)).normalized;
            //enemyController.FlipSprite(-direction);
            enemyController.rb.AddForce(test * force, ForceMode2D.Impulse);

            // Tell the enemy script that we are in knockback to stop the chasing movement
            // It will resume the chase when the rigidbody will come to an halt

            enemyController.isKnockback = true;

            StartCoroutine(enemyController.BeginKnockback());
        }

        if (!enemyController.dead)
        {
            StartCoroutine(enemyController.BecomeInvul(enemyController.invul));
        }
    }

    public override void TakePoisonDamage(float amount)
    {
        throw new System.NotImplementedException();
    }
}
