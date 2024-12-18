using UnityEngine;

public class CandyCaneMace : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float followSpeed = 10f; // How quickly the mace follows the mouse
    public float maxDistance = 2f; // Max distance mace can stretch from the player

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
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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

}
