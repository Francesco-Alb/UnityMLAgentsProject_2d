using UnityEngine;

public class LandingPads : MonoBehaviour
{
    public bool leftOnGround = false;
    public bool rightOnGround = false;
    public bool leftOnLandingPlatform = false;
    public bool rightOnLandingPlatform = false;
    public LunarLander LunarLanderScript;

    // Set time requirements
    private float landingTimer = 0f;
    public float requiredLandingTime = 0.25f; // originally 0.5f seconds

    // Make sure this is one-time reward (avoids reward farming)
    public bool leftRewardGiven = false;
    public bool rightRewardGiven = false;

    void FixedUpdate()
    {
        CheckLandingStatus();
    }

    public void CheckLandingStatus()
    {
        if (leftOnLandingPlatform && rightOnLandingPlatform)
        {
            landingTimer += Time.fixedDeltaTime; // count up 
        }
        else
        {
            landingTimer -= Time.fixedDeltaTime * 0.05f; // decays slower than it counts up
            landingTimer = Mathf.Max(landingTimer, 0f); // never go below 0
        }


        if (landingTimer >= requiredLandingTime)
        {
            LunarLanderScript.AddReward(+10f);
            Debug.Log("Stable landing achieved! +10");
            ResetReward();
            landingTimer = 0;
            LunarLanderScript.EndEpisode();
        }
    }

    public void ResetReward()
    {
        leftRewardGiven = false;
        rightRewardGiven = false;
    }

}
