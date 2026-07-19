using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingPlatform : MonoBehaviour
{

    public void SpawnLandingPad()
    {
        // Generate random spawn positions for the Landing Platform
        float xTargetSpawn = Random.Range(-6.0f, +12.0f);
        float yTargetSpawn = Random.Range(-0.25f, +0.5f);

        // Spawn Landing Platform
        transform.localPosition = new Vector2(xTargetSpawn, yTargetSpawn);
    }

}
