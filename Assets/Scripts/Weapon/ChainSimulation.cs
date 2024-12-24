using UnityEngine;

public class ChainSimulation : MonoBehaviour
{
    public Transform player;  // Start point (e.g., player position)
    public Transform weaponEnd;  // End point (e.g., weapon position)
    public int segmentCount = 20;  // Number of rope segments
    public float waveFrequency = 2f;  // Wave frequency for curvature
    public float waveAmplitude = 0.5f;  // Wave amplitude for curvature

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segmentCount;
        lineRenderer.material.mainTexture.wrapMode = TextureWrapMode.Repeat;  
    }

    void Update()
    {
        Vector3 start = player.position;
        Vector3 end = weaponEnd.position;

        float totalDistance = Vector3.Distance(start, end);  

        for (int i = 0; i < segmentCount; i++)
        {
            float t = i / (float)(segmentCount - 1);
            Vector3 point = Vector3.Lerp(start, end, t);

            float waveOffset = Mathf.Sin(t * Mathf.PI * waveFrequency) * waveAmplitude;
            point.y += waveOffset;

            lineRenderer.SetPosition(i, point);
        }

        lineRenderer.material.mainTextureScale = new Vector2(totalDistance, 1);
    }
}
