using UnityEngine;

// Lab 3, Exercise #08: links the zombie's movement to its animations.
// Put this script on the zombie, together with an Animator and a Rigidbody2D.
public class ZombieController : MonoBehaviour
{
    private Animator anim; //instance of Animator object to control the character's animation in code

    public bool isDead;       // tick in the Inspector (or set from code) to play the death animation
    public bool isAttacking;  // level 2 zombie only: plays the attack animation while true

    private bool hasAttack;   // the fat zombie's Animator has no Attack parameter

    void Start()
    {
        anim = GetComponent<Animator>(); //The zombie now has an Animator component attached to it, and the animation will play accordingly when the character moves
        hasAttack = System.Array.Exists(anim.parameters, p => p.name == "Attack");
    }

    void Update()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        }

        anim.SetBool("Dead", isDead);

        if (hasAttack)
        {
            anim.SetBool("Attack", isAttacking);
        }
    }
}
