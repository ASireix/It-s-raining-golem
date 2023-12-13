using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovements : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 movement;
    CharacterMorph characterMorph;

    PlayerController playerController;
    Animator currentAnimator;

    public bool canMove = true;

    protected AnimatorStateInfo m_CurrentStateInfo;    // Information about the base layer of the animator cached.
    protected AnimatorStateInfo m_NextStateInfo;
    protected bool m_IsAnimatorTransitioning;
    protected AnimatorStateInfo m_PreviousCurrentStateInfo;    // Information about the base layer of the animator from last frame.
    protected AnimatorStateInfo m_PreviousNextStateInfo;
    protected bool m_PreviousIsAnimatorTransitioning;

    readonly int m_HashIdle = Animator.StringToHash("Idle");
    readonly int m_HashRun = Animator.StringToHash("Run");

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        characterMorph = GetComponent<CharacterMorph>();
        currentAnimator = characterMorph.GetCurrentForm()._sprite.GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        characterMorph.OnMorphChange.AddListener(UpdateForm);
    }

    public Vector2 GetCharacterOrientation()
    {
        if (characterMorph.currentForm._sprite.flipX)
        {
            return new Vector2(-1f,0f);
        }
        else
        {
            return new Vector2(1f,0f);
        }
        
    }

    public Vector2 GetInputOrientation()
    {
        return movement.normalized;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CacheAnimatorState();

        CheckMovement();

        if (!canMove)
        {
            
        }
        else
        {
            switch (characterMorph.currentForm._formName)
            {
                case FormNames.Human:
                    HandleHumanMovement();
                    break;
                case FormNames.Wolf:
                    HandleWolfMovement();
                    break;
                case FormNames.Bat:
                    HandleBatMovement();
                    break;
                default:
                    break;
            }
            Vector2 dir = movement.normalized;
            FlipSprite(dir);
        }
    }

    void CacheAnimatorState()
    {
        m_PreviousCurrentStateInfo = m_CurrentStateInfo;
        m_PreviousNextStateInfo = m_NextStateInfo;
        m_PreviousIsAnimatorTransitioning = m_IsAnimatorTransitioning;

        m_CurrentStateInfo = currentAnimator.GetCurrentAnimatorStateInfo(0);
        m_NextStateInfo = currentAnimator.GetNextAnimatorStateInfo(0);
        m_IsAnimatorTransitioning = currentAnimator.IsInTransition(0);

    }

    void CheckMovement()
    {
        if (m_CurrentStateInfo.shortNameHash == m_HashIdle || m_CurrentStateInfo.shortNameHash == m_HashRun && !m_IsAnimatorTransitioning)
        {
            canMove = true;
        }
        else
        {
            canMove = false;
        }
    }

    void FlipSprite(Vector2 dir)
    {
        for (int i = 0; i < characterMorph.characterForms.Length; i++)
        {
            if (dir.x < 0)
            {
                characterMorph.characterForms[i]._sprite.flipX = true;
            }
            else if (dir.x > 0)
            {
                characterMorph.characterForms[i]._sprite.flipX = false;
            }
        }
    }

    void UpdateForm()
    {
        currentAnimator = characterMorph.GetCurrentForm()._sprite.GetComponent<Animator>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    void HandleHumanMovement()
    {
        movement.y = 0f;
        rb.velocity = movement * playerController.stats.speed * playerController.humanStats.speedMultiplier; // normal method
        rb.drag = playerController.humanStats.rigidBodyDrag;
        rb.gravityScale = playerController.humanStats.gScale;

        currentAnimator.SetBool("Running", rb.velocity.magnitude! >= 0.1f);

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, playerController.humanStats.speedLimit);
    }

    void HandleWolfMovement()
    {
        movement.y = 0f;
        rb.velocity = movement * playerController.stats.speed * playerController.wolfStats.speedMultiplier; // normal method
        rb.drag = playerController.wolfStats.rigidBodyDrag;
        rb.gravityScale = playerController.wolfStats.gScale;

        currentAnimator.SetBool("Running", rb.velocity.magnitude !>= 0.1f);

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, playerController.wolfStats.speedLimit);
    }

    void HandleBatMovement()
    {
        //rb.velocity = movement * stats.speed; // normal method
        rb.drag = playerController.batStats.rigidBodyDrag;
        rb.gravityScale = playerController.batStats.gScale;

        Vector2 dir = movement.normalized;
        rb.AddForce(dir * playerController.stats.speed * playerController.batStats.speedMultiplier);

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, playerController.batStats.speedLimit);

    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }

    
}
