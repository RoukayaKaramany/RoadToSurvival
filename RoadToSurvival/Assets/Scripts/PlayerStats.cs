using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int health = 3;
    public int maxHealth = 3;
    public Image healthBar;
    public bool isDead = false;

    private float flickerTime = 0f;
    public float flickerDuration = 0.1f;

    public bool isImmune = false;
    private float immunityTime = 0f;
    public float immunityDuration = 1f;

    public float respawnDelay = 2f;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rigidBody;
    private PlayerController playerController;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
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
                spriteRenderer.enabled = true;
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
            spriteRenderer.enabled = !(spriteRenderer.enabled);
            flickerTime = 0;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isImmune == false && isDead == false)
        {
            health = health - damage;
            if (health < 0)
            {
                health = 0;
            }

            UpdateHealthBar();
            Debug.Log("Player Health: " + health.ToString());

            if (health == 0)
            {
                Die();
            }
            else
            {
                isImmune = true;
                immunityTime = 0f;
            }
        }
    }

    public void RestoreHealth()
    {
        health = maxHealth;
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        healthBar.fillAmount = (float)health / maxHealth;
    }

    void Die()
    {
        Debug.Log("Gameover");
        isDead = true;
        isImmune = false;
        spriteRenderer.enabled = true;

        playerController.enabled = false;
        rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        animator.SetBool("Dead", true);

        Invoke("RespawnOrRestart", respawnDelay);
    }

    void RespawnOrRestart()
    {
        LevelManager levelManager = FindObjectOfType<LevelManager>();

        if (levelManager.currentCheckpoint != null)
        {
            levelManager.RespawnPlayer();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void ComeBackToLife()
    {
        isDead = false;
        RestoreHealth();
        animator.SetBool("Dead", false);
        playerController.enabled = true;
        rigidBody.velocity = new Vector2(0, 0);

        isImmune = true;
        immunityTime = 0f;
    }
}
