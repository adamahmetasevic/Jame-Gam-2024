using UnityEngine;

public class MaceProjectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;
    private float damage; // Remove baseDamage and currentDamage, just use a single damage value

    private void Start()
    {
        Destroy(gameObject, lifetime);
        gameObject.layer = LayerMask.NameToLayer("Playerbullet");
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Playerbullet"), LayerMask.NameToLayer("Mace"));
    }

    // Method to set the damage when the projectile is spawned
    public void SetDamage(float damageValue)
    {
        damage = damageValue;
    }

    // Method to get current damage (if needed)
    public float GetDamage()
    {
        return damage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}