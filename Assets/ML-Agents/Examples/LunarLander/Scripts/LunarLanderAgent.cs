using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
// using System.Numerics;
// using System.Threading.Tasks.Dataflow;

public class LunarLander : Agent
{
    Rigidbody2D rb;

    public LandingPlatform LandingPlatformScript;

    public Transform landingPad;
    public Boosters boostersLogic;

    public float maxFuel = 100f;
    private float lastDistance;
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

public override void OnEpisodeBegin()
    {
    LandingPlatformScript.SpawnLandingPad();

    SpawnAgent();

    lastDistance = Vector2.Distance(transform.position, landingPad.position);


    // // Reset fuel
    // fuel = maxFuel;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Add ship position relative to landing pad Convert landingPad.position to 2D
        sensor.AddObservation(rb.position - (Vector2)landingPad.position);

        // Add velocity, angular velocity, and rotation (better to normalize [-1, 1], but for now I'll leave it as is)
        sensor.AddObservation(rb.velocity);
        sensor.AddObservation(rb.angularVelocity);

        // Rotation could be simply given by: sensor.AddObservation(rb.rotation)
        // but ML-Agents often prefers normalized values or sine/cosine encoding for angles
        // Otherwise, an angle of 359° vs 0° looks huge numerically even though it’s almost upright.
        float angleRad = rb.rotation * Mathf.Deg2Rad;
        sensor.AddObservation(Mathf.Cos(angleRad));
        sensor.AddObservation(Mathf.Sin(angleRad));

        // // Optionally add remaining fuel
        // sensor.AddObservation(fuel);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Map actions to boosters
        int verticalBoost = actions.DiscreteActions[0]; // 0 = no boost, 1 = vertical boost
        int leftBoost = actions.DiscreteActions[1];     // 0 = no boost, 1 = left boost
        int rightBoost = actions.DiscreteActions[2];    // 0 = no boost, 1 = right boost

        // Apply AddForce based on actions
        boostersLogic.FireBoosters(
        verticalBoost == 1 ? 10f : 0f,
        leftBoost == 1 ? 5f : 0f,
        rightBoost == 1 ? 5f : 0f
        );

        // Time penalty to incentivize (objective: quick completion)
        // AddReward(-1e-5f);

        // Fuel penalty when boosters are used (objective: fuel efficiency)
        if (verticalBoost == 1 || leftBoost == 1 || rightBoost == 1)
        {
            // AddReward(-1e-10f); 
        }

        // Add reward/penalty for lunar lander's uprightness
        float uprightness = Vector2.Dot(transform.up, Vector2.up);
        AddReward(uprightness * 0.05f);

        // Add asymmetric reward/penalty for distance from landing pad: positive if moving closer, else (stronger) negative
        float currentDistance = Vector2.Distance(transform.localPosition, landingPad.localPosition);
        float deltaDistance = lastDistance - currentDistance;

        if (deltaDistance > 0)
        {
            AddReward(deltaDistance * 0.01f);
        }
        else
        {
            AddReward(deltaDistance * 0.015f); // deltaDistance is negative → stronger penalty
        }

        lastDistance = currentDistance;

        // Add rewards for landing softly, reaching target, and small negative reward for time/fuel
        // ???
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.W)) discreteActionsOut[0] = 1;
        if (Input.GetKey(KeyCode.D)) discreteActionsOut[1] = 1;
        if (Input.GetKey(KeyCode.A)) discreteActionsOut[2] = 1;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // Add penalty if the BODY of the ship hits something
        if (other.gameObject.CompareTag("Score") || other.gameObject.CompareTag("Ground"))
        {
            AddReward(-3f);
            Debug.Log("Crashed!");
            EndEpisode();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            AddReward(-5f);
            EndEpisode();
            Debug.Log("Collided with death barrier!");
        }
    }

public void SpawnAgent()
{
    // 1. Calculate the new state
    float xAgentSpawn = Random.Range(-6.5f, 6.5f);
    float yAgentSpawn = Random.Range(12f, 15f);
    float smallTilt = Random.Range(-10f, 10f);

    // 2. Move the actual Transform (This is the most important part)
    transform.position = new Vector3(xAgentSpawn, yAgentSpawn, 0f);
    transform.rotation = Quaternion.Euler(0, 0, smallTilt);

    // 3. Reset Physics velocities immediately
    rb.velocity = Vector2.zero;
    rb.angularVelocity = 0f;

    // 4. Force the Rigidbody to sync with the new Transform position/rotation
    rb.position = transform.position;
    rb.rotation = smallTilt;

    // 5. Apply your small starting impulses
    Vector2 randomPush = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0.5f, 2f));
    rb.AddForce(randomPush, ForceMode2D.Impulse);
    rb.AddTorque(Random.Range(-0.1f, 0.1f), ForceMode2D.Impulse);
}

}