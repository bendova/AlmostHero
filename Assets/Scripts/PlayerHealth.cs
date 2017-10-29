using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{	
	public float health = 100f;					// The player's health.
	public float repeatDamagePeriod = 2f;		// How frequently the player can be damaged.
	public AudioClip[] ouchClips;				// Array of clips to play when the player is damaged.
	public float hurtForce = 10f;				// The force with which the player is pushed when hurt.
	public float damageAmount = 10f;			// The amount of damage to take when enemies touch the player

	private float lastHitTime;					// The time at which the player was last hit.
	private Vector3 healthScale;				// The local scale of the health bar initially (with full health).
	private PlayerControl playerControl;		// Reference to the PlayerControl script.
	private Animator anim;						// Reference to the Animator on the player


	void Awake ()
	{
		// Setting up references.
		playerControl = GetComponent<PlayerControl>();
		anim = GetComponent<Animator>();
	}

	void OnCollisionEnter2D (Collision2D col)
	{
        CheckEnemyCollision(col);
	}

    void OnCollisionStay2D(Collision2D col)
    {
        CheckEnemyCollision(col);
    }

    void CheckEnemyCollision(Collision2D col)
    {
        // If the colliding gameobject is an Enemy...
        if (col.gameObject.tag == "Enemy")
        {
            // ... and if the time exceeds the time of the last hit plus the time between hits...
            if (Time.time > lastHitTime + repeatDamagePeriod)
            {
                // ... and if the player still has health...
                if (health > 0f)
                {
                    // ... take damage and reset the lastHitTime.
                    TakeDamageFromEnemy(col.transform);
                    lastHitTime = Time.time;
                }
            }
        }
    }

	void TakeDamageFromEnemy(Transform enemy)
	{
		// Make sure the player can't jump.
		playerControl.jump = false;

		// Create a vector that's from the enemy to the player with an upwards boost.
		Vector3 hurtVector = transform.position - enemy.position + Vector3.up * 5f;

		// Add a force to the player in the direction of the vector and multiply by the hurtForce.
		GetComponent<Rigidbody2D>().AddForce(hurtVector * hurtForce);

        TakeDamage(damageAmount);
	}

    void TakeDamage(float damage)
    {
        // Reduce the player's health by 10.
        health -= damage;

        Debug.Log("TakeDamage() health: " + health);

        // Play a random clip of the player getting hurt.
        int i = Random.Range(0, ouchClips.Length);
        AudioSource.PlayClipAtPoint(ouchClips[i], transform.position);

        if (health <= 0f)
        {
            TriggerDeath();
        }
    }

    void TriggerDeath()
    {
        // Find all of the colliders on the gameobject and set them all to be triggers.
        Collider2D[] cols = GetComponents<Collider2D>();
        foreach (Collider2D c in cols)
        {
            c.isTrigger = true;
        }

        // Move all sprite parts of the player to the front
        SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer s in spr)
        {
            s.sortingLayerName = "UI";
        }

        // ... disable user Player Control script
        GetComponent<PlayerControl>().enabled = false;

        // ... Trigger the 'Die' animation state
        anim.SetTrigger("Die");

        GameManager.s_instance.OnPlayerDied();
    }

    public void DieNow()
    {
        TakeDamage(health);
    }
}
