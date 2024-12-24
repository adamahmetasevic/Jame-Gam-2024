using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;        // Speed at which the enemy moves
    public float minDistanceToPlayer = 5f; // Minimum distance to maintain from player
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

    [Header("Victory Settings")]
    public bool isSantaBoss = false; // Flag to determine if this is the Santa Boss




    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
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
        // Calculate direction to player
        Vector2 direction = (player.position - transform.position).normalized;

        // Always move toward the player unless too close
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer > minDistanceToPlayer)
        {
            rb.velocity = direction * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
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

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        //Debug.Log($"Enemy took {damage} damage! Current health: {currentHealth}");

        StartCoroutine(DamageFlash());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

private void Die()
{
    StopAllCoroutines(); 

    // Grant XP when enemy dies
    GameManager.Instance.AddXP(75);

    // If this is the Santa Boss, show the victory UI
    if (isSantaBoss)
    {
        Debug.Log("Santa Boss defeated! Showing victory UI...");
        VictoryUIManager victoryUIManager = FindObjectOfType<VictoryUIManager>();
        if (victoryUIManager != null)
        {
            victoryUIManager.ShowVictoryUI(); // Show the victory UI and pause the game
        }
    }

    Destroy(gameObject);
}

 private System.Collections.IEnumerator DamageFlash()
{
    spriteRenderer.color = damageFlashColor;
    yield return new WaitForSeconds(0.1f);
    if (this != null)
    {
        spriteRenderer.color = Color.white;
    }
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
