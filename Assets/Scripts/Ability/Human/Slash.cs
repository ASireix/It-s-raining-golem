using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : Ability
{
    [SerializeField] string triggerAnim;
    [SerializeField] string firstTriggerAnim;
    [SerializeField] string secondTriggerAnim;

    public int followUpState = 0;

    protected override void Cast(PlayerController player)
    {

        Animator anim = player.GetComponent<CharacterMorph>().GetCurrentForm().
            _sprite.GetComponent<Animator>();

        if (player.abilitySystem.currentAbility == this && followUpState != 2)
        {
            followUpState++;
        }
        else
        {
            followUpState = 0;
        }

        switch (followUpState)
        {
            case 0:
                anim.SetTrigger(triggerAnim);
                break;
            case 1:
                anim.SetTrigger(firstTriggerAnim);
                break;
            case 2:
                anim.SetTrigger(secondTriggerAnim);
                break;
            default:
                break;
        }
    }
}
