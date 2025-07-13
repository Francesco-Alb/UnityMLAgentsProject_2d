using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public GameObject tilePrefab;
    public int gridWidth = 10;
    public int gridLength = 10;
    public float spacing = 0.1f;

    [Header("Environment Prefabs")]
    public GameObject agentPrefab;
    public GameObject goalPrefab;
    public GameObject obstaclePrefab;
    public int numObstacles = 4;

    private Dictionary<Vector2Int, GameObject> gridTiles = new Dictionary<Vector2Int, GameObject>();
    public List<Vector2Int> occupiedPositions = new List<Vector2Int>();

    private GameObject agentInstance;
    private GameObject goalInstance;
    private List<GameObject> obstacleInstances = new List<GameObject>();

    void Start()
    {
        CreateGrid();
        SpawnEnvironment();
    }

    // Generate grid tiles and store their positions
    void CreateGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridLength; y++)
            {
                Vector3 spawnPos = new Vector3(x + x * spacing, y + y * spacing, 0);
                GameObject tile = Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);
                gridTiles[new Vector2Int(x, y)] = tile;
            }
        }
    }

    // Spawn the agent, goal, and obstacles randomly
    void SpawnEnvironment()
    {
        occupiedPositions.Clear();

        // Spawn agent
        Vector2Int agentPos = GetFreePosition();
        Vector3 agentWorldPos = GridToWorld(agentPos);
        agentInstance = Instantiate(agentPrefab, agentWorldPos, Quaternion.identity);
        occupiedPositions.Add(agentPos);

        // Spawn goal
        Vector2Int goalPos = GetFreePosition();
        Vector3 goalWorldPos = GridToWorld(goalPos);
        goalInstance = Instantiate(goalPrefab, goalWorldPos, Quaternion.identity);
        occupiedPositions.Add(goalPos);

        // Spawn obstacles
        for (int i = 0; i < numObstacles; i++)
        {
            Vector2Int obstaclePos = GetFreePosition();
            Vector3 obstacleWorldPos = GridToWorld(obstaclePos);
            GameObject obstacle = Instantiate(obstaclePrefab, obstacleWorldPos, Quaternion.identity);
            obstacleInstances.Add(obstacle);
            occupiedPositions.Add(obstaclePos);
        }

        // Initialize agent (tell it where it started and where the goal is)
        GridAgent gridAgent = agentInstance.GetComponent<GridAgent>();
        gridAgent.Initialize(agentPos, goalPos, this);
    }

    // Convert grid coordinates to world position
    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x + gridPos.x * spacing, gridPos.y + gridPos.y * spacing, 0);
    }

    // Find a random free tile that is not occupied
    Vector2Int GetFreePosition()
    {
        while (true)
        {
            int x = Random.Range(0, gridWidth);
            int y = Random.Range(0, gridLength);
            Vector2Int pos = new Vector2Int(x, y);

            if (!occupiedPositions.Contains(pos))
                return pos;
        }
    }

    // Reset the environment for ML-Agents
    public void ResetEnvironment()
    {
        Destroy(agentInstance);
        Destroy(goalInstance);
        foreach (var obs in obstacleInstances)
        {
            Destroy(obs);
        }
        obstacleInstances.Clear();

        SpawnEnvironment();
    }
}