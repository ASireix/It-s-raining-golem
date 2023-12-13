using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityStateBehavior : StateMachineBehaviour
{
    [SerializeField] PlayerState newState;

    protected PlayerController player;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!player)
        {
            player = animator.transform.parent.parent.GetComponent<PlayerController>();
        }

        player.SwitchState(newState);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (newState == PlayerState.Recovery)
        {
            player.SwitchState(PlayerState.Idle);
        }
    }
}
