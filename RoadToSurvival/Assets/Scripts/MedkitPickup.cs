using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedkitPickup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            if (playerStats.health < playerStats.maxHealth)
            {
                playerStats.RestoreHealth();
                Debug.Log("Medkit used! Health: " + playerStats.health.ToString());
                Destroy(gameObject);
            }
        }
    }
}
