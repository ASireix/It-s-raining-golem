using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBehavior : StateMachineBehaviour
{
    protected Rigidbody2D rb;
    protected HumanController human;

    [SerializeField] float dashForce;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (!rb)
        {
            rb = animator.GetComponent<Rigidbody2D>();
        }

        if (!human)
        {
            human = rb.GetComponent<HumanController>();
        }


        rb.AddForce(human.GetSpriteOrientation() * dashForce * 100f, ForceMode2D.Impulse);
    }
}
