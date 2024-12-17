using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f; // How long the projectile exists before self-destroying
    
    private void Start()
    {
        // Destroy the projectile after lifetime seconds
        Destroy(gameObject, lifetime);
        
        // Make sure the projectile is on the "Projectile" layer
        gameObject.layer = LayerMask.NameToLayer("Projectile");
        
        // Ignore collisions between projectile layer and enemy layer specifically
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Projectile"), LayerMask.NameToLayer("Enemy"));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Deal damage to the player
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1); // Each projectile does 1 damage
            }

            // Destroy the projectile after hitting the player
            Destroy(gameObject);
        }

        // Destroy projectile if it hits something else
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}