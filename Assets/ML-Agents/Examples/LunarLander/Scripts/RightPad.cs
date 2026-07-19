using UnityEngine;

public class RightPad : MonoBehaviour
{
    // Option A: assign manually in the Inspector
    public LunarLander LunarLanderScript;
    public LandingPads LandingPadsScript;

    // Option B: find them by name/tag
    // e.g., rightLeg = GameObject.Find("RightLeg").GetComponent<Rigidbody2D>(); → then Start().

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Score"))
        {
            LandingPadsScript.rightOnLandingPlatform = true;
            Debug.Log("Right pad on landing area!");

            if (!LandingPadsScript.rightRewardGiven)
            {
                LunarLanderScript.AddReward(+2f);
                LandingPadsScript.rightRewardGiven = true;
            }
        }

        else if (other.gameObject.CompareTag("Ground"))
        {
            LandingPadsScript.rightOnGround = true;
            LunarLanderScript.AddReward(-1e-4f);
            Debug.Log("Right pad on ground!");
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Score"))
        {
            LandingPadsScript.rightOnLandingPlatform = false;
            Debug.Log("Right pad not on landing area anymore!");
        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            LandingPadsScript.rightOnGround = false;
            Debug.Log("Right pad not on the ground anymore!");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            LandingPadsScript.ResetReward();
            LunarLanderScript.AddReward(-5f);
            LunarLanderScript.EndEpisode();
            Debug.Log("Collided with death barrier!");
        }
    }
}
