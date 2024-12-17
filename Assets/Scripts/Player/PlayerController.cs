using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Player movement speed
    private Rigidbody2D rb;

    private Vector2 movement; // Stores player input

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D
    }

    void Update()
    {
        // Get input from keyboard
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized; // Normalize to prevent faster diagonal movement
    }

    void FixedUpdate()
    {
        // Move the player using Rigidbody2D
        rb.velocity = movement * moveSpeed;
    }
}
