using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;        // Speed at which the enemy moves
    public float minDistanceToPlayer = 5f; // Minimum distance to maintain from player
    public float maxDistanceToPlayer = 10f; // Distance at which enemy starts following
    private bool isFacingRight = true;  // Track facing direction

    [Header("Knockback Settings")]
    public float knockbackForce = 10f;
    private Rigidbody2D rb;

    [Header("Shooting Settings")]
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float fireRate = 2f;
    public float projectileSpeed = 5f;

    private Transform player;
    private float nextFireTime = 0f;

    [Header("Visual Feedback")]
    public Color damageFlashColor = Color.red;
    private SpriteRenderer spriteRenderer;
    private GameManager gameManager;


    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (player != null)
        {
            // Handle movement
            MoveTowardPlayer();

            // Handle shooting at regular intervals
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
    }

    private void MoveTowardPlayer()
    {
        // Calculate distance to player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        // Calculate direction to player
        Vector2 direction = (player.position - transform.position).normalized;

        // Only move if we're too far from the player
        if (distanceToPlayer > minDistanceToPlayer)
        {
            // If we're within the max distance, move toward player
            if (distanceToPlayer < maxDistanceToPlayer)
            {
                rb.velocity = direction * moveSpeed;
            }
            else
            {
                // Stop moving if we're too far
                rb.velocity = Vector2.zero;
            }
        }
        else
        {
            // Too close to player, move away slightly
            rb.velocity = -direction * moveSpeed;
        }

        // Handle sprite flipping
        if (direction.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (direction.x < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Mace"))
        {
            Rigidbody2D maceRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (maceRb != null)
            {
                float velocity = maceRb.velocity.magnitude;
                float damage = CalculateDamage(velocity);
                TakeDamage(damage);

                Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
                rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }

    private float CalculateDamage(float velocity)
    {
        float damage = velocity * 10f;
        return Mathf.Clamp(damage, 5f, 200f);
    }

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

    private void Die()
    {
            Debug.Log("Enemy has been defeated!");
    
    // Grant XP when enemy dies (enough to trigger a level up)
    if (gameManager != null)
    {
        gameManager.AddXP(100); // This will grant 100 XP, which should trigger a level up
    }
    
    Destroy(gameObject);
    }

    private System.Collections.IEnumerator DamageFlash()
    {
        spriteRenderer.color = damageFlashColor;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void Shoot()
    {
        if (projectilePrefab != null && shootPoint != null && player != null)
        {
            Vector2 direction = (player.position - shootPoint.position).normalized;

            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

            if (projectileRb != null)
            {
                projectileRb.velocity = direction * projectileSpeed;
            }
        }
    }
}