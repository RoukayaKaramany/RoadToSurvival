using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Atlas's bat. Sits on the AttackHitbox child object together with a triggered BoxCollider2D
// (Is Trigger checked, like the enemy and bullet colliders in Lab 6 and Lab 9).
// PlayerController switches the collider on for a moment when Space is pressed.
public class BatHitbox : MonoBehaviour
{
    public int damage = 1; //how much health one swing takes from an enemy

    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage); //Level 1 zombie has 2 health, so it takes 2 hits
            return;
        }

        ZombieController zombie = other.GetComponent<ZombieController>();
        if (zombie != null)
        {
            zombie.isDead = true; //zombies without an enemy script still die in one hit
        }
    }
}
