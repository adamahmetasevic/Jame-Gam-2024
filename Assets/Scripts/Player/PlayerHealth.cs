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

        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + healthBarOffset);
    }

    void Update()
    {
        healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + healthBarOffset);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth; 

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        healthBar.value = currentHealth;
        Debug.Log($"Player healed by {amount}. Current health: {currentHealth}");
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;

            currentHealth += amount; 
        

        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        Debug.Log($"Max health increased by {amount}. Current max health: {maxHealth}");
    }

    void Die()
    {
        Debug.Log("Player has died!");

        DefeatManager defeatManager = FindObjectOfType<DefeatManager>();
        if (defeatManager != null)
        {
            defeatManager.TriggerDefeat();
        }

        Destroy(gameObject); 
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Heal"))
        {
            HealObject healObject = collision.GetComponent<HealObject>();
            if (healObject != null)
            {
                Heal(healObject.healAmount);
                Destroy(collision.gameObject); 
            }
        }
    }
}
