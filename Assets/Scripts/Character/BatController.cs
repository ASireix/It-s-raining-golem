using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MorphController
{
    Rigidbody2D rb;

    [SerializeField] MorphStats batStats;

    readonly int m_HashIdle = Animator.StringToHash("Idle");
    readonly int m_HashRun = Animator.StringToHash("Run");



    protected override void OnStart()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        CacheAnimatorState();
    }

    protected override void HandleMovement()
    {
        
    }
}
