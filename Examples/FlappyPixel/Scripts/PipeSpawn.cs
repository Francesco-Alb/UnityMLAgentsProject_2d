using UnityEngine;

public class PipeSpawn : MonoBehaviour
{
    // Reference to your PipePair prefab
    public GameObject pipePairPrefab;
    public float spawnInterval = 1.5f;  // How often to spawn new pipe pairs (in seconds)
    public float verticalRange = 2f;    // How far up or down the pipe pair's center can appear

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        // Spawn a pipe pair every spawnInterval seconds
        if (timer >= spawnInterval)
        {
            SpawnPipePair();
            timer = 0f;
        }
    }

    // Spawns a single pipe pair with randomized vertical position
    void SpawnPipePair()
    {
        // Move pipes on the y axis
        float yOffset = Random.Range(-verticalRange, verticalRange);
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + yOffset, 0f);

        // Instantiate the prefab
        Instantiate(pipePairPrefab, spawnPosition, Quaternion.identity);
    }

    public void ResetPipes()
    {
        // Destroy all pipes in the scene
        GameObject[] pipes = GameObject.FindGameObjectsWithTag("Pipe");
        foreach (GameObject pipe in pipes)
        {
            Destroy(pipe);
        }

        // Reset the spawn timer
        timer = 0f;

    }

}
