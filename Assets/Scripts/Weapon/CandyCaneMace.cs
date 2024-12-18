using UnityEngine;

public class CandyCaneMace : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float followSpeed = 10f; // How quickly the mace follows the mouse
    public float maxDistance = 2f; // Max distance mace can stretch from the player
    public GameObject projectilePrefab; // The projectile prefab to be shot
    public float projectileSpread = 15f; // The angle spread between projectiles
    public LayerMask projectileLayer; // LayerMask to control collisions for projectiles
    public GameObject muzzleFlashPrefab; // Particle effect prefab for firing animation

    private Rigidbody2D rb; // Rigidbody for physics
    private Vector2 previousPosition; // Tracks previous position to calculate velocity

    // Upgradable properties
    private int projectileCount = 1; // Number of projectiles to shoot
    private float projectileSpeed = 10f; // Speed of the projectiles
    private float projectileSize = 1f; // Size multiplier for the projectiles

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        previousPosition = rb.position;
    }

    void FixedUpdate()
    {
        if (player != null && player.gameObject != null)
        {
            FollowMouseWithPhysics();
        }
    }

    void FollowMouseWithPhysics()
    {
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos = new Vector2(mousePos3D.x, mousePos3D.y);
        
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

        rb.drag = 2f;
    }

    public float GetDamage()
    {
        float velocity = (rb.position - previousPosition).magnitude / Time.fixedDeltaTime;
        previousPosition = rb.position;

        return Mathf.Clamp(velocity * 5f, 0f, 100f);
    }

    public void IncreaseMaceSpeed(float amount)
    {
        followSpeed += amount;
        Debug.Log($"Mace speed increased to {followSpeed}!");
    }

    public void UpgradeProjectileCount(int amount)
    {
        projectileCount += amount;
        Debug.Log($"Projectile count increased to {projectileCount}!");
    }

    public void UpgradeProjectileSpeed(float amount)
    {
        projectileSpeed += amount;
        Debug.Log($"Projectile speed increased to {projectileSpeed}!");
    }

    public void UpgradeProjectileSize(float multiplier)
    {
        projectileSize *= multiplier;
        Debug.Log($"Projectile size increased to {projectileSize}x!");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootProjectiles();
        }
    }

   void ShootProjectiles()
{
    if (projectileCount <= 0 || player == null) return;

    // Get the mouse position in world coordinates
    Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    if (float.IsNaN(mousePos3D.x) || float.IsNaN(mousePos3D.y))
    {
        Debug.LogError("Invalid mouse position. Unable to shoot projectiles.");
        return;
    }

    Vector2 mousePos = new Vector2(mousePos3D.x, mousePos3D.y);
    Vector2 playerPos = player.position;

    if (float.IsNaN(playerPos.x) || float.IsNaN(playerPos.y))
    {
        Debug.LogError("Invalid player position. Unable to shoot projectiles.");
        return;
    }

    // Base direction (mouse - player)
    Vector2 baseDirection = (mousePos - playerPos).normalized;
    if (baseDirection == Vector2.zero)
    {
        Debug.LogError("Base direction is zero. Unable to shoot projectiles.");
        return;
    }

    // Calculate angle spread
    float angleStep = projectileSpread / Mathf.Max(projectileCount - 1, 1);
    float startAngle = -projectileSpread / 2;

    // Shoot projectiles
    for (int i = 0; i < projectileCount; i++)
    {
        float angle = startAngle + i * angleStep;
        Vector2 direction = Quaternion.Euler(0, 0, angle) * baseDirection;

        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, rb.position, Quaternion.identity);
        Rigidbody2D rbProjectile = projectile.GetComponent<Rigidbody2D>();

        if (rbProjectile != null)
        {
            rbProjectile.velocity = direction * projectileSpeed;

            // Adjust size of projectile
            projectile.transform.localScale *= projectileSize;
        }

        // Instantiate the particle effect
        if (muzzleFlashPrefab != null)
        {
            GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, rb.position, Quaternion.identity);
            ParticleSystem particleSystem = muzzleFlash.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Play();
            }

            // Destroy the particle system after its duration
            Destroy(muzzleFlash, 1f);
        }
    }
}

}
