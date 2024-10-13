using UnityEngine;

public class SnowController : MonoBehaviour
{
    private ParticleSystem snowParticleSystem;
    private ParticleSystem.EmissionModule emissionModule;

    [SerializeField] private float minSnowRate = 10f;
    [SerializeField] private float maxSnowRate = 50f;

    void Start()
    {
        snowParticleSystem = GetComponent<ParticleSystem>();
        emissionModule = snowParticleSystem.emission;

    }

    void Update()
    {
 
        if (snowParticleSystem.isPlaying)
        {
            float snowRate = Mathf.Lerp(minSnowRate, maxSnowRate, Mathf.PingPong(Time.time, 1f));
            emissionModule.rateOverTime = snowRate;
        }
    }
}