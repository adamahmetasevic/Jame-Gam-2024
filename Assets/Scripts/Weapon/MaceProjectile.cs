using UnityEngine;

public class MaceProjectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f; // How long the projectile will exist before being destroyed
    [SerializeField] private float damage = 20f; // Damage dealt by the projectile
    
    private void Start()
    {
        // Destroy the projectile after a set lifetime
        Destroy(gameObject, lifetime);

        // Make sure the projectile is on the "Projectile" layer
        //gameObject.layer = LayerMask.NameToLayer("Projectile");

        // Ignore collisions between the projectile and the enemy layer
        //Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Projectile"), LayerMask.NameToLayer("Enemy"));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Get the Enemy component and apply damage
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Deal damage to the enemy
            }

            // Destroy the projectile after hitting the enemy
            Destroy(gameObject);
        }

        // Destroy projectile if it hits something else (optional, but keeps consistency)
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
