using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Atlas's controller for Level 2.
// Lab 2, Exercise #06: movement, sprite flipping, groundCheck + single jump.
// Lab 3, Exercise #08/#09: Speed / Height / Grounded animator parameters (Idle, Run, Jumping Motion blend tree).
// Controls: Right Arrow = move right, Left Arrow = move left, Up Arrow = jump, Space = attack.
public class PlayerController : MonoBehaviour
{
    public float moveSpeed; //how fast the character moves
    public float jumpHeight; //how high the character jumps
    public KeyCode Spacebar; //attack button. In the Inspector choose Space from the dropdown list
    public KeyCode L; //left movement button (Left Arrow)
    public KeyCode R; //right movement button (Right Arrow)
    public KeyCode Up; //jump button (Up Arrow)

    public Transform groundCheck; //an invisible point at Atlas's feet. We use it to see if he is touching the ground
    public float groundCheckRadius; //how big the circle around his feet is
    public LayerMask whatIsGround; //this variable stores what is considered a ground to the character
    private bool grounded; //check if the character is standing on solid ground

    public Collider2D attackHitbox; //triggered BoxCollider2D in front of Atlas (the bat). Off until he swings
    public float attackActiveTime; //how long the bat's collider stays on during a swing
    public float attackCooldown; //seconds between two swings
    private float nextAttackTime;

    private Animator anim; //instance of Animator object to control the character's animation in code
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    void Start()
    {
        anim = GetComponent<Animator>(); //Atlas now has an Animator component attached to it
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        //this statement calculates when exactly the character is considered by Unity's engine to be standing on the ground
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(Up) && grounded) //when user presses the up arrow ONCE, and only if Atlas is on the ground
        {
            Jump();
        }

        if (Input.GetKey(L)) //when user presses the left arrow button
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            //player character moves horizontally to the left along the x-axis without disrupting jump
            if (sr != null)
            {
                sr.flipX = true; //Atlas's sprites face right, so flip them to face left
            }
        }
        else if (Input.GetKey(R)) //when user presses the right arrow button
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            //player character moves horizontally to the right along the x-axis without disrupting jump
            if (sr != null)
            {
                sr.flipX = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y); //no arrow held: stop, so the Idle animation plays
        }

        if (Input.GetKeyDown(Spacebar) && Time.time >= nextAttackTime) //when user presses the space button ONCE
        {
            Attack();
        }

        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x)); //Idle <-> Run
        anim.SetFloat("Height", rb.velocity.y); //+ve y plays Jump, -ve y plays Fall (Jumping Motion blend tree)
        anim.SetBool("Grounded", grounded);
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        //player character jumps vertically along the y-axis without disrupting horizontal walk
    }

    void Attack()
    {
        nextAttackTime = Time.time + attackCooldown;
        anim.SetTrigger("Attack"); //Any State -> Attack, then back to Idle when the swing finishes

        if (attackHitbox == null)
        {
            return;
        }

        //keep the hitbox on the side Atlas is facing
        Vector3 p = attackHitbox.transform.localPosition;
        p.x = sr.flipX ? -Mathf.Abs(p.x) : Mathf.Abs(p.x);
        attackHitbox.transform.localPosition = p;

        //switch the bat's trigger collider on; BatHitbox.OnTriggerEnter2D() damages any enemy it touches
        attackHitbox.enabled = true;
        Invoke("EndAttack", attackActiveTime);
    }

    void EndAttack()
    {
        attackHitbox.enabled = false;
    }
}
