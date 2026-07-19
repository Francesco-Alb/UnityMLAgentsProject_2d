using UnityEngine;

public class LeftPad : MonoBehaviour
{
    // Option A: assign manually in the Inspector
    public LunarLander LunarLanderScript;
    public LandingPads LandingPadsScript;

    // Option B: find them by name/tag
    // e.g., leftLeg = GameObject.Find("LeftLeg").GetComponent<Rigidbody2D>(); → then Start().

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Score"))
        {
            LandingPadsScript.leftOnLandingPlatform = true;
            Debug.Log("Left pad on landing area!");

            if (!LandingPadsScript.leftRewardGiven)
            {
                LunarLanderScript.AddReward(+2f);
                LandingPadsScript.leftRewardGiven = true;
            }

        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            LandingPadsScript.leftOnGround = true;
            LunarLanderScript.AddReward(-1e-4f);
            Debug.Log("Left pad on ground!");
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Score"))
        {
            LandingPadsScript.leftOnLandingPlatform = false;
            Debug.Log("Left pad not on landing area anymore!");
        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            LandingPadsScript.leftOnGround = false;
            Debug.Log("Left pad not on the ground anymore!");
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
