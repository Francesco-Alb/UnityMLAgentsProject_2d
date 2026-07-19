using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D), typeof(LineRenderer))]
public class RandomTerrain2D : MonoBehaviour
{
    public int width = 50;          // number of points
    public float scale = 5f;        // controls roughness
    public float height = 3f;       // vertical exaggeration
    public float xOffset = 0f;      // shifts terrain pattern

    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();

        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        Vector3[] linePositions = new Vector3[width];
        Vector2[] colliderPoints = new Vector2[width];

        for (int i = 0; i < width; i++)
        {
            float x = i;
            float y = Mathf.PerlinNoise((i + xOffset) / scale, 0f) * height;
            linePositions[i] = new Vector3(x, y, 0);
            colliderPoints[i] = new Vector2(x, y);
        }

        // Draw visible line
        lineRenderer.positionCount = width;
        lineRenderer.SetPositions(linePositions);

        // Assign collider points
        edgeCollider.points = colliderPoints;
    }
}
