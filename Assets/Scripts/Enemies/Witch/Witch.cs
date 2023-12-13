using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MileStones
{
    [Range(0f, 100f)]
    public float hpPercent;
    public PotionThrow potionThrow;
    public bool targetPlayer;//Add the player as a target
    public Transform[] targetPositions;
    public float minCooldown;
    public float maxCooldown;

    public float callCooldown;
    public float stunDuration;
    public float stunMaxFill; // stun get filled after every hit taken and attack launched

    public Vector2[] GetTargetPositions()
    {
        Vector2[] result = new Vector2[targetPositions.Length + (targetPlayer ? 1 : 0)];

        for (int i = 0; i < targetPositions.Length; i++)
        {
            result[i] = targetPositions[i].position;
        }

        if (targetPlayer)
        {
            result[result.Length - 1] = GameManager.instance.player.transform.position;
        }

        return result;
    }
}
/* Pitch
 100% : - Throw 3 in succession, poison or health
	- Low duration
	- Target Player
	- Health low chances

70% : - Throw 2 poison
	- Throw 1 Health
	- Target whole floor

50% : - Throw 2 poison
	- Throw 1 Health
	- Target whole floor
	- Potions get revealed at the end

20% : - Golems comes in and do the 50% throw 
 */

public class Witch : Enemy
{
    WitchStates states;
    [SerializeField] MileStones[] milestones; // goes from level 0 to 3 and the
    int currentMileStoneIndex;

    [SerializeField] Transform potionThrowPosition; // From where are throwed the potions

    float cooldown;

    [SerializeField] float introLength;
    [SerializeField] float minimalCallDistance;

    [System.NonSerialized]
    public UnityEvent<float> onBossHealthChangeEvent = new UnityEvent<float>();

    float stunMeter; // at stunMaxFill, gets stunned for stunDuration
    [SerializeField] float stunPerHitTaken;
    [SerializeField] float stunPerHitGiven;
    float stunCurrentTimer;
    float golemTimer;

    [SerializeField] GameObject golemThrow;
    [SerializeField] Transform golemThrowPosition;
    [SerializeField] int lastPhaseGolemAmount;
    [SerializeField] GameObject lastPhaseGolemToSpawn;

    List<Enemy> golems;
    bool golemSpawned;
    protected override void OnStart()
    {
        states = WitchStates.Intro;
        ReorderMilestones();
        StartCoroutine(StartIntro());

        Vector2 dir = transform.position - _player.position;
        FlipSprite(dir);
    }

    private void Update()
    {
        if (dead) return;
        switch (states)
        {
            case WitchStates.Intro:
                break;
            case WitchStates.Idle:
                cooldown -= Time.deltaTime;
                if (cooldown < 0)
                {
                    ResetCooldown();
                    states = WitchStates.Potions;
                }
                CheckProximity();
                break;
            case WitchStates.Potions:
                anim.SetTrigger("Throw");
                states = WitchStates.Idle;

                CheckProximity();
                break;
            case WitchStates.Golem:
                golemTimer -= Time.deltaTime;
                if (golemTimer < 0)
                {
                    states = WitchStates.Idle;
                }
                break;
            case WitchStates.Stunned:
                stunCurrentTimer -= Time.deltaTime;
                if (stunCurrentTimer < 0)
                {
                    anim.SetBool("Stunned", false);
                    stunCurrentTimer = 2f;
                    states = WitchStates.Wakeup;
                }
                break;
            case WitchStates.Wakeup:
                stunCurrentTimer -= Time.deltaTime;
                if (stunCurrentTimer < 0)
                {
                    ResetCooldown();
                    if (currentMileStoneIndex == milestones.Length - 1 && !golemSpawned)
                    {
                        golems = GameManager.instance.SpawnEnemyAtSide(lastPhaseGolemToSpawn, lastPhaseGolemAmount,"left");
                        golems.AddRange(GameManager.instance.SpawnEnemyAtSide(lastPhaseGolemToSpawn, lastPhaseGolemAmount, "right"));
                        golemSpawned = true;
                    }
                    states = WitchStates.Idle;
                }
                break;
            default:
                break;
        }
    }

    IEnumerator StartIntro()
    {
        for (float i = 0f; i < 1f; i += Time.deltaTime / introLength)
        {
            yield return new WaitForEndOfFrame();
        }
        onBossIntroFinished?.Invoke(transform);
        TriggerAggro();
    }

    public void TriggerAggro()
    {
        states = WitchStates.Idle;
        ResetCooldown();
    }

    void ResetCooldown()
    {
        cooldown = UnityEngine.Random.Range(milestones[currentMileStoneIndex].minCooldown, milestones[currentMileStoneIndex].maxCooldown);
        anim.ResetTrigger("Call");
        anim.ResetTrigger("Throw");
    }

    int CompareMilestones(MileStones x, MileStones y)
    {
        return x.hpPercent > y.hpPercent ? -1 : 1;
    }

    void ReorderMilestones()
    {
        Array.Sort(milestones, CompareMilestones);
    }

    //Called in animation event of witch throw
    public void ThrowPotions()
    {
        MileStones milestone = milestones[currentMileStoneIndex];

        milestone.potionThrow.Throw(potionThrowPosition.position, milestone.GetTargetPositions());

        AddStun(stunPerHitGiven);
    }

    public void SendGolems()
    {
        Instantiate(golemThrow, golemThrowPosition.position, Quaternion.identity);
    }

    void AddStun(float amount)
    {
        if (states != WitchStates.Stunned)
        {
            stunMeter += amount;

            if (stunMeter > milestones[currentMileStoneIndex].stunMaxFill)
            {
                anim.SetBool("Stunned", true);
                stunCurrentTimer = milestones[currentMileStoneIndex].stunDuration;
                stunMeter = 0;
                states = WitchStates.Stunned;
            }
        }
    }

    public override void UpdateHealth(float amount)
    {
        
        if (amount < hp)
        {
            AddStun(stunPerHitTaken);
        }
        
        base.UpdateHealth(amount);

        onBossHealthChangeEvent?.Invoke(amount/stats.maxHP);

        UpdateMilestone();
    }

    void UpdateMilestone()
    {
        if (currentMileStoneIndex + 1 >= milestones.Length) return;

        if (milestones[currentMileStoneIndex + 1].hpPercent/100f > hp / stats.maxHP)
        {
            currentMileStoneIndex += 1;
        }

        
    }

    void CheckProximity()
    {
        float distance = Vector2.Distance(_player.position,transform.position);
        if (distance < minimalCallDistance)
        {
            anim.SetTrigger("Call");
            golemTimer = milestones[currentMileStoneIndex].callCooldown;
            states = WitchStates.Golem;
        }
    }

    public override void Die()
    {
        try
        {
            foreach (var item in golems)
            {
                if (item)
                {
                    item.Die();
                }
            }
        }
        catch (Exception)
        {
        }
        

        Hitstop.TriggerHitstop(2f);
        anim.SetTrigger("Die");
        dead = true;
        canTakeDamage = false;
        StartCoroutine(FlashSprite(0.05f, 2.2f));
        onEnemyDeath?.Invoke(this);
        
        Invoke("EndFight", 3f);
        //gameObject.SetActive(false);
    }

    public void PlaySoundInAnimator(SoundEffectSO sound)
    {
        SoundManager.instance.PlaySFX(sound);
    }

    void EndFight()
    {
        SceneLoader.LoadSceneByString("MainMenu");
    }
}
