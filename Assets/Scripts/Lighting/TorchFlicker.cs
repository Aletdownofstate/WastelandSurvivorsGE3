using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchFlicker : MonoBehaviour
{
    public Light pointLight;  // Reference to the point light
    public float minIntensity = 0.8f; // Minimum intensity for flicker
    public float maxIntensity = 1.2f; // Maximum intensity for flicker
    public float minRange = 5f; // Minimum range for flicker
    public float maxRange = 7f; // Maximum range for flicker
    public float flickerSpeed = 0.1f; // Speed of flickering

    private float randomTimeOffset;  // To desynchronize multiple torches

    void Start()
    {
        if (pointLight == null)
        {
            pointLight = GetComponent<Light>();
        }

        randomTimeOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed + randomTimeOffset, 0.0f);

        pointLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
        pointLight.range = Mathf.Lerp(minRange, maxRange, noise);
    }
}
