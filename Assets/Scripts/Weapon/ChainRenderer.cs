using UnityEngine;

public class ChainRenderer : MonoBehaviour
{
    public Transform player; // The player's transform
    private LineRenderer lineRenderer;

    void Start()
    {
        // Get the LineRenderer component attached to this GameObject (Flail)
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // The chain has two endpoints
    }

    void Update()
    {
        // Set the positions of the line: from the flail to the player
        lineRenderer.SetPosition(0, transform.position); // Start at the flail (this GameObject)
        lineRenderer.SetPosition(1, player.position);    // End at the player
    }
}
