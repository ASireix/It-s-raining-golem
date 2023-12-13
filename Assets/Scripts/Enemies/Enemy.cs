using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected EnemyStats stats;
    public float hp { get; protected set; }
    protected Transform _player;
    [SerializeField] float progressFill; //how fast the progress bar fill when the unit die
    [HideInInspector]
    public BossProgress bossProgress;
    

    public SpriteRenderer spriteRenderer {  get; protected set; }
    Color startColor;
    public Rigidbody2D rb {  get; protected set; }

    public float invul;
    public bool canTakeDamage = true;
    public bool dead { get; protected set; } = false;
    public bool isKnockback = false;
    

    protected EnemyState state;

    protected Animator anim;

    protected AnimatorStateInfo m_CurrentStateInfo;    // Information about the base layer of the animator cached.
    protected AnimatorStateInfo m_NextStateInfo;
    protected bool m_IsAnimatorTransitioning;
    protected AnimatorStateInfo m_PreviousCurrentStateInfo;    // Information about the base layer of the animator from last frame.
    protected AnimatorStateInfo m_PreviousNextStateInfo;
    protected bool m_PreviousIsAnimatorTransitioning;

    [System.NonSerialized]
    public static UnityEvent<Enemy> onEnemyDeath = new UnityEvent<Enemy>();

    [System.NonSerialized]
    public UnityEvent<Transform> onBossIntroFinished = new UnityEvent<Transform>();

    protected void CacheAnimatorState()
    {
        m_PreviousCurrentStateInfo = m_CurrentStateInfo;
        m_PreviousNextStateInfo = m_NextStateInfo;
        m_PreviousIsAnimatorTransitioning = m_IsAnimatorTransitioning;

        m_CurrentStateInfo = anim.GetCurrentAnimatorStateInfo(0);
        m_NextStateInfo = anim.GetNextAnimatorStateInfo(0);
        m_IsAnimatorTransitioning = anim.IsInTransition(0);
    }


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        state = EnemyState.Chasing;
        _player = GameManager.instance.player.transform;
        anim = GetComponent<Animator>();
        startColor = spriteRenderer.color;

        hp = stats.maxHP;
        OnStart();
    }

    protected virtual void OnStart() { }

    private void Update()
    {
        CacheAnimatorState();
        if (dead) { return; }
        switch (state)
        {
            case EnemyState.Patroling:
                HandlePatroling();
                break;
            case EnemyState.Attacking:
                HandleAttacking();
                break;
            case EnemyState.Chasing:
                if (!isKnockback)
                {
                    HandleChasing();
                }
                break;
            default:
                break;
        }
    }

    protected virtual void HandlePatroling()
    {

    }

    protected virtual void HandleAttacking()
    {

    }

    protected virtual void HandleChasing()
    {

    }

    protected virtual void HandleKnockBack()
    {
        
    }

    public void FlipSprite(Vector2 dir)
    {
        if (dir.x < 0)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (dir.x > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }


    public virtual void UpdateHealth(float amount)
    {
        hp = amount;
    }

    public IEnumerator BecomeInvul(float duration)
    {
        canTakeDamage = false;
        for (float i = 0.0f; i < 1.0f; i += Time.deltaTime / duration)
        {

            yield return null;
        }
        canTakeDamage = true;
    }

    public IEnumerator BlinkSprite(SpriteRenderer spriteRenderer, Color blinkColor, int amountOfBlink, float duration)
    {
        for (int j = 0; j < amountOfBlink; j++)
        {
            for (float i = 0.0f; i < 1.0f; i += Time.deltaTime / (duration / amountOfBlink))
            {
                if (i > 0.5f)
                {
                    spriteRenderer.color = Color.Lerp(blinkColor, startColor, 1 - i * 2);
                }
                else
                {
                    spriteRenderer.color = Color.Lerp(startColor, blinkColor, i * 2);
                }
                yield return null;
            }
            yield return null;
        }
        spriteRenderer.color = startColor;
    }

    public IEnumerator BeginKnockback(float timeOut = 10f)
    {
        isKnockback = true;
        float t = 0f;
        while(rb.velocity.magnitude > 0.1f && t < timeOut)
        {
            t += Time.deltaTime;
            yield return null;
        }
        isKnockback = false;
    }

    public virtual void Die()
    {
        Hitstop.TriggerHitstop(0.5f);
        anim.SetTrigger("Die");
        dead = true;
        canTakeDamage = false;
        StartCoroutine(FlashSprite(0.05f,2.2f));
        bossProgress?.AddProgress(progressFill);
        onEnemyDeath?.Invoke(this);
        //gameObject.SetActive(false);
    }

    public IEnumerator FlashSprite(float duration, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        Material mat = spriteRenderer.material;
        for (float i = 0f; i < 1f; i += Time.deltaTime / duration / 2)
        {
            mat.SetFloat("_Flash", Mathf.Lerp(0f, 0.1f, i));
            yield return null;
        }

        mat.SetFloat("_Flash", 0.1f);
        Color col = spriteRenderer.color;

        for (float i = 0f; i < 1f; i += Time.deltaTime / duration / 2)
        {
            mat.SetFloat("_Flash", Mathf.Lerp(0.1f, 0f, i));
            col.a = Mathf.Lerp(1f, 0f, i);
            spriteRenderer.color = col;
            yield return null;
        }

        mat.SetFloat("_Flash", 0f);
        col.a = 0f;
        spriteRenderer.color = col;
        gameObject.SetActive(false);
    }
}
