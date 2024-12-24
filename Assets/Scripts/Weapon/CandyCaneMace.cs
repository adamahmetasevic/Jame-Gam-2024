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
    private float projectileSize = 2f; // Size multiplier for the projectiles
    private float maceSize = 1f; // Mace size multiplier (visual size)
    private float maceDamageMultiplier = 1f; // Mace damage multiplier
    private float projectileDamage = 10f; // or whatever base value


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        previousPosition = rb.position;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Projectile"), LayerMask.NameToLayer("Mace"));
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

        return Mathf.Clamp(velocity * 5f * maceDamageMultiplier, 0f, 100f); // Adjust damage with the multiplier
    }


public void UpgradeProjectileDamage(float amount) 
{ 
    projectileDamage += amount;
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
        projectileSize += multiplier;
        Debug.Log($"Projectile size increased to {projectileSize}x!");
    }

 public void UpgradeMaceSize(float multiplier)
{
    maceSize += multiplier; // Increase the size of the mace
    transform.localScale = new Vector3(maceSize, maceSize, maceSize); // Apply size change

    maxDistance *= (1 + multiplier);  // Increase the max distance proportionally to the mace size

    Debug.Log($"Mace size increased to {maceSize}x, max distance adjusted to {maxDistance}!");
}

    public void UpgradeMaceDamageMultiplier(float multiplier)
    {
        maceDamageMultiplier *= multiplier; // Increase damage multiplier
        Debug.Log($"Mace damage multiplier increased to {maceDamageMultiplier}x!");
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
        GameObject projectileObj = Instantiate(projectilePrefab, rb.position, Quaternion.identity);
        
        // Get the MaceProjectile component and set its damage
        MaceProjectile maceProjectile = projectileObj.GetComponent<MaceProjectile>();
        if (maceProjectile != null)
        {
            maceProjectile.SetDamage(projectileDamage);
        }

        Rigidbody2D rbProjectile = projectileObj.GetComponent<Rigidbody2D>();

        if (rbProjectile != null)
        {
            rbProjectile.velocity = direction * projectileSpeed;

            // Adjust size of projectile
            projectileObj.transform.localScale *= projectileSize;
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






    //getters

        public float GetMaceSpeed()
{
    return followSpeed; 
}
public float GetProjectileDamage() { return projectileDamage; }

public float GetMaceSize()
{
    Vector3 currentScale = transform.localScale;
    Debug.Log($"Current Mace Scale: {currentScale}");
    return currentScale.x; // Assuming uniform scaling
}
public int GetProjectileCount()
{
    return projectileCount; 
}

public float GetProjectileSpeed()
{
    return projectileSpeed; 
}

public float GetMaceDamageMultiplier()
{
    return maceDamageMultiplier; 
}
public float GetProjectileSize()
{
    return projectileSize; 
}


}
