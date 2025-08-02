using UnityEngine;
public class MultiParticleController : MonoBehaviour
{
    public ParticleSystem[] particleSystems;

    public void PlayAll()
    {
        foreach (var ps in particleSystems)
        {
            ps.Play();
        }
    }
}