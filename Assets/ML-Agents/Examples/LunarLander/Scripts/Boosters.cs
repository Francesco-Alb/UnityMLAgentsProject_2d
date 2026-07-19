using UnityEngine;

public class Boosters : MonoBehaviour
{
    public Rigidbody2D shipRb;

    public Transform vBooster;
    public Transform leftBooster;
    public Transform rightBooster;

    public ParticleSystem mainBoosterParticles;
    public ParticleSystem leftBoosterParticles;
    public ParticleSystem rightBoosterParticles;

    public void FireBoosters(float verticalForce, float leftForce, float rightForce)
    {
        if (verticalForce > 0)
        {
            shipRb.AddForceAtPosition(vBooster.up * verticalForce, vBooster.position);
            // mainBoosterParticles.Play();
        }
        if (leftForce > 0)
        {
            shipRb.AddForceAtPosition(leftBooster.up * leftForce, leftBooster.position);
            // leftBoosterParticles.Play();
        }
        if (rightForce > 0)
        {
            shipRb.AddForceAtPosition(rightBooster.up * rightForce, rightBooster.position);
            // rightBoosterParticles.Play();
        }

        //  Trigger particle effects
        HandleParticles(mainBoosterParticles, verticalForce > 0);
        HandleParticles(leftBoosterParticles, leftForce > 0); 
        HandleParticles(rightBoosterParticles, rightForce > 0);
        }

    private void HandleParticles(ParticleSystem ps, bool shouldPlay)
    {
        if (ps == null) return;

        if (shouldPlay)
        {
            if (!ps.isPlaying)
            {
                ps.Play();
            }
                
        }
        else
        {
            if (ps.isPlaying)
            {
                ps.Stop();
            }
        }
    }

}
