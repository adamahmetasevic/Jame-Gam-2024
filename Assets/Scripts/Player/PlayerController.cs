using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public float moveSpeed = 5f;           // Player movement speed

    [Header("Mace Settings")]
    public GameObject mace;                // Reference to the mace object
    public float maceFollowSpeed = 5f;     // Speed at which the mace follows the mouse
    public float maceBaseDamage = 5f;      // Base damage of the mace
    public float maceDamageMultiplier = 1f; // Multiplier for mace damage
    public bool canShootProjectiles = false; // Whether the mace can shoot projectiles

    private Rigidbody2D rb;
    private Vector2 movement;
    private PlayerHealth playerHealth;     // Reference to the PlayerHealth component

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<PlayerHealth>();

        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth script not found on the Player object!");
        }
    }

    void Update()
    {
        HandleMovement();
        UpdateMacePosition();
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        movement = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        rb.velocity = movement * moveSpeed;
    }

    void UpdateMacePosition()
    {
        if (mace != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            // Smoothly move the mace towards the mouse position
            mace.transform.position = Vector3.Lerp(
                mace.transform.position,
                mousePosition,
                maceFollowSpeed * Time.deltaTime
            );
        }
    }

    public float CalculateMaceDamage(float velocityMagnitude)
    {
        return maceBaseDamage * maceDamageMultiplier * velocityMagnitude;
    }

    // Interface with PlayerHealth
    public void TakeDamage(int damage)
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    public void IncreaseHealth(int amount)
    {
        if (playerHealth != null)
        {
            playerHealth.IncreaseMaxHealth(amount);
        }
    }

    // Upgrade Effects
    public void IncreaseMaceSpeed(float amount)
    {
        maceFollowSpeed += amount;
        Debug.Log($"Mace speed increased to {maceFollowSpeed}!");
    }

    public void IncreaseMaceDamage(float multiplier)
    {
        maceDamageMultiplier += multiplier;
        Debug.Log($"Mace damage multiplier increased to {maceDamageMultiplier}!");
    }

    public void EnableMaceProjectileAttack()
    {
        canShootProjectiles = true;
        Debug.Log("Mace projectile attack unlocked!");
    }
}
