using UnityEngine;

public class PipeMover : MonoBehaviour
{
    public float speed = 3f;
    public float destroyX = -20f;

    void Update()
    {
        // Move the pipe pair left
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Destroy if off-screen
        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }

}
