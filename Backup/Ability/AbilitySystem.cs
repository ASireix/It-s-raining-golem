using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilitySystem : MonoBehaviour
{
    Ability[] abilities;
    PlayerController playerController;
    public Ability currentAbility;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void SetAbilities(Ability[] ab)
    {
        abilities = ab;
    }

    public virtual void CastFirstAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            abilities[0].TriggerAbility(playerController);
        }
    }

    public virtual void CastSecondAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            abilities[1].TriggerAbility(playerController);
        }
        
    }

    public virtual void CastThirdAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            abilities[2].TriggerAbility(playerController);
        }
        
    }
}
