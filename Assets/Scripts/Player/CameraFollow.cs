using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float smoothSpeed = 5f; // Smoothness of the camera movement
    public Vector3 offset; // Offset to position the camera slightly above or away from the player

    void FixedUpdate()
    {
        if (player != null)
        {
            // Target position for the camera
            Vector3 targetPosition = player.position + offset;

            // Smoothly interpolate between the current camera position and the target position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.fixedDeltaTime);

            // Apply the new position to the camera
            transform.position = smoothedPosition;
        }
    }
}
