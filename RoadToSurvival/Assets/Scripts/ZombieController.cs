using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public bool isDead;
    public bool isAttacking;

    private Animator animator;
    private Rigidbody2D rigidBody;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        animator.SetFloat("Speed", Mathf.Abs(rigidBody.velocity.x));
        animator.SetBool("Dead", isDead);
        animator.SetBool("Attack", isAttacking);
    }
}
