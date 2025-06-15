using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CarAgent : Agent
{
    Rigidbody2D rb;

    public float moveSpeed = 10f;
    public float turnSpeed = 200f;

    public Transform[] checkpoints; // Assign these via inspector (tagged as "Score")
    public Transform[] spawnPoints; // Possible spawn locations (checkpoints reused as spawns)

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnEpisodeBegin()
    {
        // Reset car physics
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Randomly select spawn point
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawn = spawnPoints[spawnIndex];

        transform.position = spawn.position;

        // 50/50 chance to face original or flipped X direction
        bool flip = Random.value > 0.5f;
        transform.rotation = flip ? Quaternion.Euler(0, 0, 180) : spawn.rotation;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Velocity in local space
        Vector2 localVelocity = transform.InverseTransformDirection(rb.velocity);
        sensor.AddObservation(localVelocity);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int throttleAction = actions.DiscreteActions[0]; // 0 = neutral, 1 = forward, 2 = backward
        int steerAction = actions.DiscreteActions[1];    // 0 = none, 1 = left, 2 = right
        // Debug.Log($"Throttle: {throttleAction}, Steer: {steerAction}");


        float throttle = 0f;
        float steer = 0f;

        if (throttleAction == 1) throttle = 1f;
        else if (throttleAction == 2) throttle = -0.5f;

        if (steerAction == 1) steer = -1f;
        else if (steerAction == 2) steer = 1f;

        // Move forward/backward
        rb.AddForce(transform.right * throttle * moveSpeed);

        // Steer
        if (rb.velocity.magnitude > 0.15f) // only steer if moving
        {
           rb.MoveRotation(rb.rotation - steer * turnSpeed * Time.fixedDeltaTime);
        }

        // Time penalty
        AddReward(-0.001f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;

        // Throttle: 0 = neutral, 1 = forward, 2 = backward
        if (Input.GetKey(KeyCode.W)) discreteActionsOut[0] = 1;
        else if (Input.GetKey(KeyCode.S)) discreteActionsOut[0] = 2;
        else discreteActionsOut[0] = 0;

        // Steering: 0 = none, 1 = left, 2 = right
        if (Input.GetKey(KeyCode.A)) discreteActionsOut[1] = 1;
        else if (Input.GetKey(KeyCode.D)) discreteActionsOut[1] = 2;
        else discreteActionsOut[1] = 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Score"))
        {
            AddReward(5f); // Reward for hitting a checkpoint
            Debug.Log("Score!");
        }
        else if (other.CompareTag("Border"))
        {
            AddReward(-1f); // Penalty for hitting track border
            Debug.Log("Collision!");
            EndEpisode(); // Optional: end episode on border collision
        }
    }
}