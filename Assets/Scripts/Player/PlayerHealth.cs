using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10; // Maximum health
    private int currentHealth; // Current health

    public Slider healthBar;   // Reference to the health bar UI
    public Vector3 healthBarOffset = new Vector3(0, -1.5f, 0); // Offset to position health bar

    void Start()
    {
        currentHealth = maxHealth;

        // Initialize the health bar
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        // Position the health bar right under the player
        healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + healthBarOffset);
    }

    void Update()
    {
        // Update the health bar position to follow the player
        healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + healthBarOffset);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth; // Update health bar UI

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player has died!");
        Destroy(gameObject); // Destroy player object
        // Optional: Trigger a game over screen or reset the level
    }
}
