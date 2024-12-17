using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f; // Enemy's maximum health
    private float currentHealth;   // Enemy's current health

    [Header("Knockback Settings")]
    public float knockbackForce = 10f; // Knockback force when hit by the flail
    private Rigidbody2D rb;            // Enemy's Rigidbody2D

    [Header("Shooting Settings")]
    public GameObject projectilePrefab; // Projectile prefab to shoot
    public Transform shootPoint;        // Where the projectile spawns
    public float fireRate = 2f;         // Shots per second
    public float projectileSpeed = 5f;  // Speed of the projectiles

    private Transform player;           // Player's transform for targeting
    private float nextFireTime = 0f;    // Timer to control shooting rate

    [Header("Visual Feedback")]
    public Color damageFlashColor = Color.red; // Color when taking damage
    private SpriteRenderer spriteRenderer;     // For visual feedback

    void Start()
    {
        currentHealth = maxHealth; // Initialize health
        rb = GetComponent<Rigidbody2D>(); // Get Rigidbody2D component
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get SpriteRenderer
        player = GameObject.FindGameObjectWithTag("Player")?.transform; // Find the player by tag
    }

    void Update()
    {
        // Handle shooting at regular intervals
        if (player != null && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    // Handle collisions with the flail
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Mace"))
        {
            // Get the velocity of the mace
            Rigidbody2D maceRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (maceRb != null)
            {
                float velocity = maceRb.velocity.magnitude;

                // Calculate and apply damage
                float damage = CalculateDamage(velocity);
                TakeDamage(damage);

                // Apply knockback to the enemy
                Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
                rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }

    // Calculate damage based on the mace's velocity (momentum)
    private float CalculateDamage(float velocity)
    {
        float damage = velocity * 10f; // Scale damage with velocity
        return Mathf.Clamp(damage, 5f, 200f); // Clamp damage between 5 and 200
    }

    // Apply damage to the enemy
    private void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Enemy took {damage} damage! Current health: {currentHealth}");

        StartCoroutine(DamageFlash());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Handle enemy death
    private void Die()
    {
        Debug.Log("Enemy has been defeated!");
        Destroy(gameObject); // Destroy the enemy GameObject
    }

    // Visual feedback: Flash the enemy red when taking damage
    private System.Collections.IEnumerator DamageFlash()
    {
        spriteRenderer.color = damageFlashColor; // Set to damage color
        yield return new WaitForSeconds(0.1f);   // Wait for 0.1 seconds
        spriteRenderer.color = Color.white;      // Reset to normal color
    }

    // Shoot a projectile toward the player
    private void Shoot()
    {
        if (projectilePrefab != null && shootPoint != null && player != null)
        {
            // Calculate direction to the player
            Vector2 direction = (player.position - shootPoint.position).normalized;

            // Instantiate and launch the projectile
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

            if (projectileRb != null)
            {
                projectileRb.velocity = direction * projectileSpeed;
            }
        }
    }
}
