using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource windSound;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        StartCoroutine(SoundFadeIn(windSound, 1.25f, 1.0f));
    }

    private IEnumerator SoundFadeIn(AudioSource audioSource, float volume, float pitch)
    {
        audioSource.pitch = pitch;
        audioSource.Play();
        float currentTime = 0.0f;
        while (currentTime < 3.0f)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / 3);
            audioSource.volume = Mathf.Lerp(0.0f, volume, t);
            yield return null;
        }
        audioSource.volume = volume;        
    }
}