using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class GridAgent : Agent
{
    private Vector2Int initialPos; // Temporary solution: at start, initial pos was seen as obstacle because of the obstacle logic below
    private Vector2Int gridPos;
    private Vector2Int goalPos;
    public GridManager gridManager;

    public void Initialize(Vector2Int startPos, Vector2Int goalPosition, GridManager manager)
    {
        initialPos = startPos;
        gridPos = startPos;
        goalPos = goalPosition;
        gridManager = manager;
    }

    public override void OnEpisodeBegin()
    {
        
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Observations (Add to support visual learning or leave out)
        // sensor.AddObservation(gridPos.x);
        // sensor.AddObservation(gridPos.y);
        // sensor.AddObservation(goalPos.x);
        // sensor.AddObservation(goalPos.y);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int move = actions.DiscreteActions[0];

        Vector2Int newPos = gridPos;

        switch (move)
        {
            case 0: newPos += Vector2Int.up; break;
            case 1: newPos += Vector2Int.down; break;
            case 2: newPos += Vector2Int.left; break;
            case 3: newPos += Vector2Int.right; break;
            case 4: break; // stay still
        }

        // Move agent
        gridPos = newPos;
        transform.position = gridManager.GridToWorld(gridPos);

        // Check boundaries
        if (newPos.x < 0 || newPos.x >= gridManager.gridWidth ||
            newPos.y < 0 || newPos.y >= gridManager.gridLength)
        {
            Debug.Log("Out of bounds!");
            AddReward(-1.0f);
            EndEpisode();
            gridManager.ResetEnvironment();
        }

        // Check if on goal
        if (gridPos == goalPos)
        {
            Debug.Log("Goal!");
            AddReward(+5.0f);
            EndEpisode();
            gridManager.ResetEnvironment();
        }

        // Check if on obstacle
        bool hitObstacle = false;
        foreach (var pos in gridManager.occupiedPositions)
        {
            if (pos == gridPos && pos != goalPos && pos != initialPos)
            {
                hitObstacle = true;
                break;
            }
        }

        if (hitObstacle)
        {
            Debug.Log("Obstacle!");
            AddReward(-1.0f);
            EndEpisode();
            gridManager.ResetEnvironment();
        }

        // Step penalty
        AddReward(-0.05f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;

        if (Input.GetKeyDown(KeyCode.W)) discreteActions[0] = 0;
        else if (Input.GetKeyDown(KeyCode.S)) discreteActions[0] = 1;
        else if (Input.GetKeyDown(KeyCode.A)) discreteActions[0] = 2;
        else if (Input.GetKeyDown(KeyCode.D)) discreteActions[0] = 3;
        else discreteActions[0] = 4;
    }
}