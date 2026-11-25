using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpHeight;
    public KeyCode moveLeftKey;
    public KeyCode moveRightKey;
    public KeyCode jumpKey;
    public KeyCode attackKey;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    private bool isGrounded;

    public Collider2D batHitbox;
    public float batSwingTime;
    public float timeBetweenAttacks;
    private float nextAttackTime;

    private Animator animator;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    void Update()
    {
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        if (Input.GetKey(moveLeftKey))
        {
            rigidBody.velocity = new Vector2(-moveSpeed, rigidBody.velocity.y);
            spriteRenderer.flipX = true;
        }
        else if (Input.GetKey(moveRightKey))
        {
            rigidBody.velocity = new Vector2(moveSpeed, rigidBody.velocity.y);
            spriteRenderer.flipX = false;
        }
        else
        {
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        }

        if (Input.GetKeyDown(attackKey) && Time.time >= nextAttackTime)
        {
            Attack();
        }

        animator.SetFloat("Speed", Mathf.Abs(rigidBody.velocity.x));
        animator.SetFloat("Height", rigidBody.velocity.y);
        animator.SetBool("Grounded", isGrounded);
    }

    void Jump()
    {
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpHeight);
    }

    void Attack()
    {
        nextAttackTime = Time.time + timeBetweenAttacks;
        animator.SetTrigger("Attack");

        Vector3 hitboxPosition = batHitbox.transform.localPosition;
        if (spriteRenderer.flipX == true)
        {
            hitboxPosition.x = -Mathf.Abs(hitboxPosition.x);
        }
        else
        {
            hitboxPosition.x = Mathf.Abs(hitboxPosition.x);
        }
        batHitbox.transform.localPosition = hitboxPosition;

        batHitbox.enabled = true;
        Invoke("EndAttack", batSwingTime);
    }

    void EndAttack()
    {
        batHitbox.enabled = false;
    }
}
