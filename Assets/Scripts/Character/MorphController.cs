using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MorphController : MonoBehaviour
{
    [SerializeField] protected InputReader inputReader;
    [SerializeField] protected float maxHP;
    public float hp { get; protected set; }

    protected Animator animator;
    protected Vector2 movement;

    public bool canMove = true;

    protected SpriteRenderer sprite;

    protected AnimatorStateInfo m_CurrentStateInfo;    // Information about the base layer of the animator cached.
    protected AnimatorStateInfo m_NextStateInfo;
    protected bool m_IsAnimatorTransitioning;
    protected AnimatorStateInfo m_PreviousCurrentStateInfo;    // Information about the base layer of the animator from last frame.
    protected AnimatorStateInfo m_PreviousNextStateInfo;
    protected bool m_PreviousIsAnimatorTransitioning;

    public bool invulnerableState { get; protected set; }

    [System.NonSerialized]
    public UnityEvent<float> healthChangeEvent = new UnityEvent<float>();

    protected virtual void OnStart()
    {

    }

    private void Start()
    {
        inputReader.moveEvent += OnMove;
        GameManager.instance.SetPlayer(this);
        OnStart();
    }


    protected void CacheAnimatorState()
    {
        m_PreviousCurrentStateInfo = m_CurrentStateInfo;
        m_PreviousNextStateInfo = m_NextStateInfo;
        m_PreviousIsAnimatorTransitioning = m_IsAnimatorTransitioning;

        m_CurrentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        m_NextStateInfo = animator.GetNextAnimatorStateInfo(0);
        m_IsAnimatorTransitioning = animator.IsInTransition(0);
    }

    public virtual void TakeDamage(float amount, float force, Transform enemy) { }

    public virtual void Heal(float amount) { }

    protected virtual void Die() { }

    public void OnMove(Vector2 v)
    {
        movement = v;
    }


    protected virtual void HandleMovement()
    {

    }

    protected virtual void OnDisableOverride()
    {

    }

    private void OnDisable()
    {
        inputReader.moveEvent += OnMove;
        OnDisableOverride();
    }
}
