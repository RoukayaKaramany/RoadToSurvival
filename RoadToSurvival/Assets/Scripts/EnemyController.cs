using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float maxSpeed = 2;
    public int damage = 1;
    public int health = 2;
    public SpriteRenderer spriteRenderer;

    public void Flip()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    public void TakeDamage(int damageTaken)
    {
        ZombieController zombie = GetComponent<ZombieController>();

        if (zombie.isDead == true)
        {
            return;
        }

        health = health - damageTaken;
        Debug.Log(gameObject.name + " health: " + health.ToString());

        if (health <= 0)
        {
            health = 0;
            zombie.isDead = true;
        }
        else
        {
            spriteRenderer.color = Color.red;
            Invoke("StopRedFlash", 0.15f);
        }
    }

    void StopRedFlash()
    {
        spriteRenderer.color = Color.white;
    }
}
