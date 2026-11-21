using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Lab 6, Exercise #18: Player Stats (health + temporary immunity), attached to Atlas.
// Game rule: 3 hits and Atlas dies, then the level restarts.
// (The lab's lives + LevelManager.RespawnPlayer() part comes later with checkpoints, Lab 5.)
public class PlayerStats : MonoBehaviour
{
    public int health = 3;
    public bool isDead = false;

    private float flickerTime = 0f;
    public float flickerDuration = 0.1f;

    private SpriteRenderer sr;
    private Animator anim;
    private Rigidbody2D rb;

    public bool isImmune = false;
    private float immunityTime = 0f;
    public float immunityDuration = 1f; //how long Atlas can't be hit again after taking a hit

    public float restartDelay = 2f; //seconds to show the death animation before the level restarts

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isImmune == true)
        {
            SpriteFlicker();
            immunityTime = immunityTime + Time.deltaTime;
            if (immunityTime >= immunityDuration)
            {
                isImmune = false;
                sr.enabled = true;
            }
        }
    }

    void SpriteFlicker()
    {
        if (flickerTime < flickerDuration)
        {
            flickerTime = flickerTime + Time.deltaTime;
        }
        else if (flickerTime >= flickerDuration)
        {
            sr.enabled = !(sr.enabled);
            flickerTime = 0;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isImmune == false && isDead == false)
        {
            health = health - damage;
            if (health < 0)
                health = 0;

            Debug.Log("Player Health:" + health.ToString());

            if (health == 0)
            {
                Die();
                return;
            }

            isImmune = true;
            immunityTime = 0f;
        }
    }

    void Die()
    {
        Debug.Log("Gameover");
        isDead = true;
        isImmune = false;
        sr.enabled = true;

        GetComponent<PlayerController>().enabled = false; //no more moving or attacking
        rb.velocity = new Vector2(0, rb.velocity.y);
        anim.SetBool("Dead", true); //plays Atlas_Death

        Invoke("RestartLevel", restartDelay);
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
