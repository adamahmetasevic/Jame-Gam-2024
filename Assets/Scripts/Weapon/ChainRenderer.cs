using UnityEngine;

public class ChainRendererFlail : MonoBehaviour
{
    public Transform player; // Player reference
    public LineRenderer lineRenderer;

    void Start()
    {
        // Ensure Texture Mode is set to Tile in the Material
        lineRenderer.material.mainTexture.wrapMode = TextureWrapMode.Repeat;
    }

    void Update()
    {
        // Update the line's positions
        lineRenderer.SetPosition(0, player.position);
        lineRenderer.SetPosition(1, transform.position);

        // Dynamically adjust texture tiling
        float distance = Vector3.Distance(player.position, transform.position);
        lineRenderer.material.mainTextureScale = new Vector2(distance, 1);
    }
}
