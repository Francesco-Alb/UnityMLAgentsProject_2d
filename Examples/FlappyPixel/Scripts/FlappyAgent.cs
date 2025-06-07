using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class FlappyAgent : Agent
{
    public PipeSpawn pipeSpawn; // drag the PipeSpawner object here

    private Rigidbody2D rb;
    public float flapStrength = 8f;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnEpisodeBegin()
    {
        // Reset pixel and pipes
        transform.position = new Vector3(-2f, 0f, 0f);
        rb.velocity = Vector2.zero;

        // Reset all pipes
        pipeSpawn.ResetPipes();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // observe vertical position (perhaps unnecessary with sensors)
        // sensor.AddObservation(transform.position.y);

        // observe vertical position and velocity
        sensor.AddObservation(rb.velocity.y);

        // Ray sensors will add additional observations automatically
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        Debug.Log($"Action Received: {actions.DiscreteActions[0]}");
        // Discrete action: 0 = do nothing, 1 = flap
        if (actions.DiscreteActions[0] == 1)
        {
            rb.velocity = Vector2.up * flapStrength;
            Debug.Log("Flap!");
        }

        // Small positive reward for staying alive
        AddReward(0.01f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // Commands for human tester
        actionsOut.DiscreteActions.Array[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Fail if bird touches the ground or the pipes
        if (other.CompareTag("Border") || other.CompareTag("Pipe"))
        {
            AddReward(-1f);
            EndEpisode();
            Debug.Log("Collision!");
        }

        // Positive reward for passing through the pipes
        if (other.CompareTag("Score"))
        {
            AddReward(2f);
            Debug.Log("Score!");
        }
    }
}
