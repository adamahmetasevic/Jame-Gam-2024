using UnityEngine;

public class CandyCaneMace : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float followSpeed = 10f; // How quickly the mace follows the mouse
    public float maxDistance = 2f; // Max distance mace can stretch from the player
    public bool canShootProjectiles = false; // Whether the mace can shoot projectiles
    public GameObject projectilePrefab; // The projectile prefab to be shot
    public float projectileSpread = 15f; // The angle spread between projectiles
    public LayerMask projectileLayer; // LayerMask to control collisions for projectiles

    private Rigidbody2D rb; // Rigidbody for physics
    private Vector2 previousPosition; // Tracks previous position to calculate velocity

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        previousPosition = rb.position;
    }

    void FixedUpdate()
    {
        FollowMouseWithPhysics();
    }

    void FollowMouseWithPhysics()
    {
        // Get mouse position in world coordinates
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos = new Vector2(mousePos3D.x, mousePos3D.y);  // Convert to Vector2 (ignore z-axis)
        
        Vector2 direction = (mousePos - (Vector2)player.position);

        // Clamp the mace position to a max distance from the player
        if (direction.magnitude > maxDistance)
        {
            direction = direction.normalized * maxDistance;
        }

        Vector2 targetPosition = (Vector2)player.position + direction;

        // Move the mace with physics
        Vector2 force = (targetPosition - rb.position) * followSpeed;
        rb.AddForce(force);

        // Optional: Add drag to stabilize
        rb.drag = 2f;
    }

    // Calculate damage based on momentum
    public float GetDamage()
    {
        float velocity = (rb.position - previousPosition).magnitude / Time.fixedDeltaTime;
        previousPosition = rb.position;

        // Damage scales with velocity
        return Mathf.Clamp(velocity * 5f, 0f, 100f); // Scale damage
    }

    public void IncreaseMaceSpeed(float amount)
    {
        followSpeed += amount;
        Debug.Log($"Mace speed increased to {followSpeed}!");
    }

    public void EnableMaceProjectileAttack()
    {
        canShootProjectiles = true;
        Debug.Log("Mace projectile attack unlocked!");
    }

    void Update()
    {
        // Shoot projectiles when the mace projectile attack is enabled
        if (canShootProjectiles && Input.GetKeyDown(KeyCode.Space)) // Replace Space with desired input
        {
            ShootProjectiles();
        }
    }

    void ShootProjectiles()
    {
        // Shoot three projectiles with slight spread from the mace position
        for (int i = -1; i <= 1; i++) // Loop for three projectiles
        {
            // Get direction from the mace to the mouse position
Vector2 direction = (new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y) - (Vector2)player.position);
            direction = Quaternion.Euler(0, 0, i * projectileSpread) * direction; // Apply spread

            // Instantiate the projectile at the mace's position
            GameObject projectile = Instantiate(projectilePrefab, rb.position, Quaternion.identity);
            Rigidbody2D rbProjectile = projectile.GetComponent<Rigidbody2D>();

            if (rbProjectile != null)
            {
                rbProjectile.velocity = direction * 10f; // Set the velocity for the projectile

                // Set the projectile's layer to the correct layer
                projectile.layer = LayerMask.NameToLayer("Projectile");

                // Ignore collision between projectile and mace
            }
        }
    }
}
