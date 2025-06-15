using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public CarAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] checkpointObjects = GameObject.FindGameObjectsWithTag("Score");
        Transform[] transforms = new Transform[checkpointObjects.Length];
        for (int i = 0; i < checkpointObjects.Length; i++)
        {
            transforms[i] = checkpointObjects[i].transform;
        }
        agent.checkpoints = transforms;
        agent.spawnPoints = transforms;
    }
}
