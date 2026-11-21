using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Lab 6, Exercise #19: super class for all enemies.
// Every enemy type (FollowerEnemy, walking enemy, ...) inherits these variables and functions.
public class EnemyController : MonoBehaviour
{
    public float maxSpeed = 2; //how fast the enemy moves
    public int damage = 1; //how much health the enemy takes from the player
    public int health = 2; //how many hits from Atlas it takes to kill this enemy (Level 1 zombie = 2)
    public SpriteRenderer sr;

    //If true turn to false, if false turn to true
    public void Flip()
    {
        sr.flipX = !sr.flipX;
    }

    //called by Atlas's PlayerController when the bat hits this enemy
    public void TakeDamage(int amount)
    {
        ZombieController zombie = GetComponent<ZombieController>();
        if (zombie != null && zombie.isDead)
        {
            return; //already dead
        }

        health = health - amount;
        Debug.Log(gameObject.name + " health: " + health.ToString());

        if (health <= 0)
        {
            health = 0;
            if (zombie != null)
            {
                zombie.isDead = true; //ZombieController plays the Death animation
            }
        }
        else
        {
            StartCoroutine(HitFlash());
        }
    }

    //flash red for a moment so the player can see the hit landed
    IEnumerator HitFlash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        sr.color = Color.white;
    }
}
