using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchDamageController : DamageController
{
    [SerializeField] Witch enemyController;

    public override void TakeDamage(float amount, float force, Transform enemy)
    {
        if (!enemyController.canTakeDamage) { return; }

        enemyController.UpdateHealth(enemyController.hp - amount);

        StartCoroutine(enemyController.BlinkSprite(enemyController.spriteRenderer, Color.red, 1, 0.5f));

        if (enemyController.hp <= 0)
        {
            enemyController.Die();
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
