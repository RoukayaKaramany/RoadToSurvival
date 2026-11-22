using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerEnemy : EnemyController
{
    public Transform player;
    public float stopDistance = 1.4f;
    public float chaseRange = 12f;
    public float timeBetweenHits = 1.2f;
    public float firstHitDelay = 0.3f;

    private Rigidbody2D rigidBody;
    private ZombieController zombie;
    private PlayerStats playerStats;
    private float nextHitTime;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        zombie = GetComponent<ZombieController>();
        playerStats = player.GetComponent<PlayerStats>();
    }

    void FixedUpdate()
    {
        if (zombie.isDead == true)
        {
            zombie.isAttacking = false;
            rigidBody.velocity = new Vector2(0, 0);
            rigidBody.simulated = false;
            return;
        }

        if (playerStats.isDead == true)
        {
            zombie.isAttacking = false;
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
            return;
        }

        float distanceToPlayer = player.position.x - transform.position.x;
        float heightDifference = Mathf.Abs(player.position.y - transform.position.y);

        if (distanceToPlayer < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        if (Mathf.Abs(distanceToPlayer) > chaseRange)
        {
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
            zombie.isAttacking = false;
        }
        else if (Mathf.Abs(distanceToPlayer) > stopDistance)
        {
            if (distanceToPlayer > 0)
            {
                rigidBody.velocity = new Vector2(maxSpeed, rigidBody.velocity.y);
            }
            else
            {
                rigidBody.velocity = new Vector2(-maxSpeed, rigidBody.velocity.y);
            }
            zombie.isAttacking = false;
        }
        else
        {
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);

            if (zombie.isAttacking == false)
            {
                zombie.isAttacking = true;
                nextHitTime = Time.time + firstHitDelay;
            }

            if (Time.time >= nextHitTime && heightDifference < 1.5f)
            {
                playerStats.TakeDamage(damage);
                nextHitTime = Time.time + timeBetweenHits;
            }
        }
    }
}
