using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDamageController : DamageController
{
    [SerializeField] HumanController humanController;

    bool invulnerableState;
    public override void TakeDamage(float amount, float force, Transform enemy)
    {
        if (humanController.counterState)
        {
            StartCoroutine(TriggerInvulnerability(2f));
            humanController.HandleParry();
        }
        else if (!invulnerableState && humanController.hp>0)
        {
            ApplyKnockBack(force, enemy);
            StartCoroutine(TriggerInvulnerability());
            if (humanController.hp - amount <= 0)
            {
                humanController.UpdateHealth(0);
            }
            else
            {
                humanController.UpdateHealth(humanController.hp - amount);
            }
        }
    }

    IEnumerator TriggerInvulnerability(float duration = -1f)
    {
        invulnerableState = true;
        if (duration < 0)
        {
            yield return new WaitForSeconds(1f);
        }
        else
        {
            yield return new WaitForSeconds(duration);
        }
        invulnerableState = false;
    }

    void ApplyKnockBack(float impactForce, Transform from)
    {

        humanController.stunned = true;

        StartCoroutine(Stun(0.2f));

        Vector2 direction = transform.position - from.position;

        direction.Normalize();

        Vector2 test = new Vector2(direction.x, Mathf.Abs(direction.x)).normalized;

        //Debug.Log("direction = " + direction);
        //AddForce(test * impactForce);
        //rb.AddForce(test * impactForce * 1000, ForceMode2D.);
        humanController.rb.velocity = Vector2.zero;
        humanController.rb.AddRelativeForce(test * impactForce * 1000, ForceMode2D.Impulse);
    }

    IEnumerator Stun(float duration)
    {
        humanController.stunned = true;
        humanController.canRoll = false;
        for (float i = 0f; i < 1f; i += Time.deltaTime / duration)
        {
            yield return new WaitForEndOfFrame();
        }

        while (humanController.rb.velocity.magnitude > 0.1f)
        {
            yield return new WaitForEndOfFrame();
        }
        humanController.stunned = false;
        humanController.canRoll = true;
    }

    public override void TakePoisonDamage(float amount)
    {
        if (humanController.hp > 0)
        {
            humanController.UpdateHealth(humanController.hp - amount);
        }
    }
}
