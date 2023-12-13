using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HumanController : MorphController
{
    public Rigidbody2D rb { get; private set; }
    
    public bool canAttack = true;
    public bool canRoll = true;

    public bool counterState { get; private set; }
    public bool countering { get; private set; }
    public bool stunned;

    bool isRolling = false;

    Vector2 rollDir;

    [SerializeField] CharacterStats _stats;
    [SerializeField] MorphStats humanStats;
    [SerializeField] SlashData slashData;
    [SerializeField] RollData rollData;
    [SerializeField] ParryData parryData;

    [SerializeField] Hitbox hitbox;

    readonly int m_HashIdle = Animator.StringToHash("Idle");
    readonly int m_HashRun = Animator.StringToHash("Run");
    readonly int m_HashCombo1 = Animator.StringToHash("Slash 1");
    readonly int m_HashCombo2 = Animator.StringToHash("Slash 2");
    readonly int m_HashCombo3 = Animator.StringToHash("Slash 3");
    readonly int m_HashRoll = Animator.StringToHash("Roll");
    readonly int m_HashParry = Animator.StringToHash("Parry");

    [Header("VFX")]

    [SerializeField] Material parryMat;
    [SerializeField] Material normalMat;

    [Header("SFX")]
    [SerializeField] SoundEffectSO sfx_Step;

    protected override void OnStart()
    {
        hp = maxHP;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();


        inputReader.firstAbilityEvent += OnSlash;
        inputReader.secondAbilityEvent += OnRoll;
        inputReader.thirdAbilityEvent += OnParry;

        hitbox.knockBack = 10f;
    }

    void FixedUpdate()
    {
        CacheAnimatorState();
        CheckMovement();
        CheckAttack();
        CheckParry();
        SetHitbox();

        if (canMove)
        {
            HandleMovement();
            FlipSprite(movement.normalized);
        }else if (isRolling && canRoll)
        {
            HandleRolling();
        }
        else
        {
            //rb.velocity = Vector2.zero;
        }
        
        //ApplyForces();
    }


    public void ToggleCounter(int yn)
    {
        counterState = (yn == 1);
        if (yn == 1)
        {
            StartCoroutine(ShineShield(1f));
        }
    }

    IEnumerator FlashSprite(float speed)
    {
        sprite.material = normalMat;
        Material m = sprite.material;

        for (float i = 0f; i < 1f; i += Time.deltaTime / speed)
        {
            if (i < 0.5f)
            {
                m.SetFloat("_Flash", i * 2);
            }
            else
            {
                m.SetFloat("_Flash", 1 - i);
            }
            yield return null;
        }
        m.SetFloat("_Flash", 0f);
    }

    IEnumerator ShineShield(float speed)
    {
        sprite.material = parryMat;
        Material m = sprite.material;
        
        for (float i = 0.0f; i <= 1.0f; i += Time.deltaTime / speed)
        {
            m.SetFloat("_Parry", i);
            if (i < 0.5f)
            {
                m.SetFloat("_ParryAlpha", i * 2);
            }
            else
            {
                m.SetFloat("_ParryAlpha", 1-i);
            }
            yield return null;
        }
        m.SetFloat("_Parry", 1f);
        m.SetFloat("_ParryAlpha", 0f);
    }

    public void OnParry()
    {
        if (canAttack)
        {
            animator.SetTrigger("Parry");
        }
    }

    public void OnSlash()
    {
        if (m_CurrentStateInfo.shortNameHash == m_HashCombo1 || m_CurrentStateInfo.shortNameHash == m_HashCombo2 && !m_IsAnimatorTransitioning || m_CurrentStateInfo.shortNameHash == m_HashRoll)
        {
            animator.SetTrigger("Combo");
        }
        else if (canAttack )
        {
            animator.SetTrigger("Slash");
        }

        //SoundManager.instance.PlaySFX(slashData.sfx_Slash_First);
    }

    void OnRoll()
    {
        
        if (m_CurrentStateInfo.shortNameHash == m_HashRoll && !m_IsAnimatorTransitioning) { return; }

        StartCoroutine(WaitForFlip(m_HashRoll,movement.normalized));
        float startPos = transform.position.x;

        rollDir = GetSpriteOrientation();

        if (movement.magnitude >= 0.1f)
        {
            rollDir = new Vector2(movement.x, 0f);
        }

        isRolling = true;
        animator.SetTrigger("Roll");
    }

    IEnumerator WaitForFlip(int hash, Vector2 direction, float timeout = 10f)
    {
        float time = 0f;
        while (hash != m_CurrentStateInfo.shortNameHash && time < timeout)
        {
            time += Time.deltaTime;
            yield return null;
        }
        FlipSprite(direction);
    }

    void SetHitbox()
    {
        if (m_CurrentStateInfo.shortNameHash == m_HashCombo1)
        {
            hitbox.knockBack = slashData.firstKnockBack;
            hitbox.damage = slashData.firstSlashDashDamage;
        }else if (m_CurrentStateInfo.shortNameHash == m_HashCombo2)
        {
            hitbox.knockBack = slashData.secondKnockBack;
            hitbox.damage = slashData.secondSlashDashDamage;
        }
        else if (m_CurrentStateInfo.shortNameHash == m_HashCombo3)
        {
            hitbox.knockBack = slashData.thirdKnockBack;
            if (countering)
            {
                hitbox.damage = slashData.thirdSlashDashDamage * 2;
            }
            else
            {
                hitbox.damage = slashData.thirdSlashDashDamage;
            }
            
        }
    }

    public void HandleParry()
    {
        counterState = false;
        countering = true;
        animator.SetTrigger("Combo");
    }

    void HandleRolling()
    {
        if (m_CurrentStateInfo.shortNameHash == m_HashRoll)
        {
            float normalizedTime = m_CurrentStateInfo.normalizedTime;
            animator.SetFloat("RollSpeed", rollData.animationCurve.Evaluate(normalizedTime) * rollData.rollSpeed);

            rb.velocity = rollDir * rollData.animationCurve.Evaluate(normalizedTime) * rollData.rollDistance;
        }
        
        if (m_PreviousCurrentStateInfo.shortNameHash == m_HashRoll && m_CurrentStateInfo.shortNameHash != m_HashRoll)
        {
            isRolling = false;
        }
        //rb.velocity = rollData.rollSpeed;
    }

    void CheckAttack()
    {
        canAttack = m_CurrentStateInfo.shortNameHash == m_HashIdle || m_CurrentStateInfo.shortNameHash == m_HashRun && !stunned;

    }

    void CheckParry()
    {
        if (countering)
            countering = (m_CurrentStateInfo.shortNameHash == m_HashCombo3 || m_CurrentStateInfo.shortNameHash == m_HashParry);
    }

    void CheckMovement()
    {
        canMove = (m_CurrentStateInfo.shortNameHash == m_HashIdle || m_CurrentStateInfo.shortNameHash == m_HashRun && !m_IsAnimatorTransitioning) && !stunned;
    }

    public Vector2 GetSpriteOrientation()
    {
        if (transform.rotation.y == -1)
        {
            return Vector2.left;
        }
        else
        {
            return Vector2.right;
        }
    }

    void FlipSprite(Vector2 dir)
    {
        
        if (dir.x < 0)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            animator.SetBool("Running", true);
        }
        else if (dir.x > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            animator.SetBool("Running", true);
        }
    }

    override protected void HandleMovement()
    {
        movement.y = 0f;
        rb.velocity = movement * _stats.speed * humanStats.speedMultiplier; // normal method
        rb.drag = humanStats.rigidBodyDrag;
        rb.gravityScale = humanStats.gScale;

        animator.SetBool("Running", rb.velocity.magnitude! >= 0.1f);

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, humanStats.speedLimit);
    }

    public void ForwardDash(float dashForce)
    {
        if (dashForce == 0)
        {
            if ((m_CurrentStateInfo.shortNameHash == m_HashCombo2 && !m_IsAnimatorTransitioning) || m_NextStateInfo.shortNameHash == m_HashCombo2)
            {
                dashForce = slashData.secondSlashDashForce;
            }
            else if (m_CurrentStateInfo.shortNameHash == m_HashCombo3 || m_NextStateInfo.shortNameHash == m_HashCombo3)
            {
                dashForce = slashData.thirdSlashDashForce;
            }
        }
        rb.velocity = Vector2.zero;
        rb.AddForce(GetSpriteOrientation() * dashForce * 100f, ForceMode2D.Impulse);
        
    }

    public void UpdateHealth(float amount)
    {
        hp = amount;

        if (hp >= maxHP)
        {
            hp = maxHP;
        }

        if (hp <= 0)
        {
            Die();
        }
        healthChangeEvent.Invoke(hp / maxHP);
    }

    protected override void Die()
    {
        base.Die();
        stunned = true;
        invulnerableState = true;
        animator.SetTrigger("Die");
        Invoke("ReturnToMenu", 3f);
    }

    void ReturnToMenu()
    {
        SceneLoader.LoadSceneByString("MainMenu");
    }

    public void StepSound() // Called in animator
    {
        SoundManager.instance.PlaySFX(sfx_Step);
    }

    public void SwordSound_First() // called in animator
    {
        SoundManager.instance.PlaySFX(slashData.sfx_Slash_First);
    }

    public void SwordSound_Third() // called in animator
    {
        SoundManager.instance.PlaySFX(slashData.sfx_Slash_Third);
    }

    public void SwordSound_Second() // called in animator
    {
        SoundManager.instance.PlaySFX(slashData.sfx_Slash_Second);
    }

    public void DashSound() // called in animator
    {
        SoundManager.instance.PlaySFX(rollData.sfx_Dash);
    }

    protected override void OnDisableOverride()
    {
        inputReader.firstAbilityEvent -= OnSlash;
        inputReader.secondAbilityEvent -= OnRoll;
        inputReader.thirdAbilityEvent -= OnParry;
    }
}
