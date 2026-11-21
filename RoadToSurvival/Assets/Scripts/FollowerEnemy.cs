using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Lab 6, Exercise #22: an enemy that follows the player wherever he goes.
// FollowerEnemy is a subclass of EnemyController.
// The lab's saw uses Vector3.MoveTowards on the position, which would make a zombie float up when Atlas jumps.
// Zombies walk on the ground instead, so we move them along the x-axis with the Rigidbody2D
// (like the walking enemy in Ex #20). That also gives ZombieController a velocity, so the Run animation plays.
// When the zombie reaches Atlas it stops and attacks him (Attack animation + PlayerStats.TakeDamage).
public class FollowerEnemy : EnemyController
{
    public Transform player; //the player's object location (drag Atlas here)
    public float stopDistance = 1.4f; //how close the zombie gets before it stops in front of the player
    public float attackRate = 1.2f; //seconds between two zombie hits
    public float firstHitDelay = 0.3f; //time from starting the attack to the first hit landing (arms up)

    private Rigidbody2D rb;
    private ZombieController zombie;
    private PlayerStats playerStats;
    private float nextHitTime;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        zombie = GetComponent<ZombieController>();
        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStats>();
        }

        //zombies walk through each other instead of pushing each other out of the way
        Collider2D myCollider = GetComponent<Collider2D>();
        foreach (FollowerEnemy other in FindObjectsOfType<FollowerEnemy>())
        {
            Collider2D otherCollider = other.GetComponent<Collider2D>();
            if (other != this && myCollider != null && otherCollider != null)
            {
                Physics2D.IgnoreCollision(myCollider, otherCollider);
            }
        }
    }

    void FixedUpdate()
    {
        if (zombie != null && zombie.isDead) //killed by Atlas: stop and stay where he fell
        {
            zombie.isAttacking = false;
            rb.velocity = Vector2.zero;
            rb.simulated = false; //Atlas can walk past the body
            return;
        }

        if (player == null || (playerStats != null && playerStats.isDead))
        {
            zombie.isAttacking = false;
            rb.velocity = new Vector2(0, rb.velocity.y); //Atlas is dead: stand still
            return;
        }

        float distance = player.position.x - transform.position.x;
        float heightGap = Mathf.Abs(player.position.y - transform.position.y);
        sr.flipX = distance < 0; //the zombie sprites face right, so flip them when Atlas is on the left

        if (Mathf.Abs(distance) > stopDistance)
        {
            //walk towards Atlas
            rb.velocity = new Vector2(Mathf.Sign(distance) * maxSpeed, rb.velocity.y);
            zombie.isAttacking = false;
        }
        else
        {
            //reached Atlas: stand still and attack
            rb.velocity = new Vector2(0, rb.velocity.y);

            if (zombie.isAttacking == false)
            {
                zombie.isAttacking = true; //ZombieController plays the Attack animation
                nextHitTime = Time.time + firstHitDelay;
            }

            //only hurts Atlas if he isn't jumping over the zombie's head
            if (Time.time >= nextHitTime && heightGap < 1.5f)
            {
                if (playerStats != null)
                {
                    playerStats.TakeDamage(damage);
                }
                nextHitTime = Time.time + attackRate;
            }
        }
    }
}
