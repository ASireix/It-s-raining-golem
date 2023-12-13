using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityState
{
    Startup,
    Active,
    Recovery
}

public abstract class Ability : MonoBehaviour
{

    [SerializeField] protected AbilityData data;
    AbilityState state;

    public bool canUse { get; private set; } = true;

    public List<AbilityLinks> abilityLinks;

    public Dictionary<string, AbilityLinks> dicoAbilityLinks { get; private set; } = new Dictionary<string, AbilityLinks>();

    private void Start()
    {
        foreach (var item in abilityLinks)
        {
            dicoAbilityLinks.TryAdd(item.ability.data.name, item);
        }
    }

    public virtual void TriggerAbility(PlayerController player)
    {
        if (player.state != PlayerState.Recovery && canUse)
        {
            Debug.Log("I can use and the player is not in recovery");
            if (player.abilitySystem.currentAbility == null)
            {
                Cast(player);
                player.abilitySystem.currentAbility = this;
                Debug.Log("There is no ability in play");
            }
            else if (player.abilitySystem.currentAbility.dicoAbilityLinks.TryGetValue(data.name, out AbilityLinks links))
            {
                StartCoroutine(WaitForCorrectState(PlayerState.Recovery, player));

            }
        }
    }

    IEnumerator WaitForCorrectState(PlayerState newState, PlayerController player)
    {
        while(player.state != newState)
        {
            yield return null;
        }

        Cast(player);
        player.abilitySystem.currentAbility = this;
    }

    protected abstract void Cast(PlayerController player);

    public void BeginCooldown()
    {
        canUse = false;
        StartCoroutine(CoolDown());

        IEnumerator CoolDown()
        {
            for (float i = 0.0f; i < data.cooldown; i += Time.deltaTime)
            {
                yield return null;
            }

            canUse = true;
        }
    }
}
