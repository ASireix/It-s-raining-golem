using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Header("Global")]
    public CharacterStats stats;
    public PlayerState state;
    [System.NonSerialized] public UnityEvent<PlayerState> stateChangeEvent = new UnityEvent<PlayerState>();

    public float health { get; private set; }
    public float mana { get; private set; }

    public bool followUpState;
    public AbilitySystem abilitySystem { get; private set; }
    public bool dead { get; private set; }

    [Header("Human")]
    public MorphStats humanStats;

    [Header("Bat")]
    public MorphStats batStats;

    [Header("Wolf")]
    public MorphStats wolfStats;
    // Start is called before the first frame update
    void Start()
    {
        abilitySystem = GetComponent<AbilitySystem>();
        
    }

    // Update is called once per frame
    void Update()
    {
        RegenMana();
    }


    public void SwitchState(PlayerState newState)
    {
        state = newState;
        if (newState == PlayerState.Idle)
        {
            GetComponent<CharacterMovements>().UnlockMovement();
        }

        if (newState == PlayerState.Recovery)
        {
            abilitySystem.currentAbility.BeginCooldown();
            abilitySystem.currentAbility = null;
            
        }
        stateChangeEvent.Invoke(newState);
    }

    void RegenMana()
    {
        mana += stats.manaRegenSpeed * Time.deltaTime;

        if (mana > stats.maxMana) mana = stats.maxMana;
    }

    public bool CheckState()
    {
        return state == PlayerState.Idle;
    }

    public bool CheckEnergy(float amount)
    {
        return amount <= mana;
    }

    public void UseMana(float amount)
    {
        mana -= amount;
    }
}
