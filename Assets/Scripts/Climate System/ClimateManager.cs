using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimateManager : MonoBehaviour
{
    public static ClimateManager Instance { get; private set; }

    [SerializeField] private ParticleSystem rainPs;
    [SerializeField] private AudioSource rainSound, thunderSound, thunderSoundAlt;
    [SerializeField] private Light sunLight;

    public int temp;
    private int minTemp;
    private int maxTemp;
    private int tempOffset;
    private int prevTemp;

    private bool isRaining = false;
    private bool canThunder = true;
    private bool isAudioPlaying = false;
    private bool isRainFading = false;
    private bool isLightFading = false;


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
        minTemp = 5;
        maxTemp = 25;
        temp = 15;
        prevTemp = temp;

        isRaining = false;

        var main = rainPs.main;
        main.startLifetime = 0;

        StartCoroutine(Temperature());
    }

    private void Update()
    {
        var main = rainPs.main;
        temp = Mathf.Clamp(temp, minTemp, maxTemp);

        if (temp < 10 && !isRaining)
        {
            main.startLifetime = 1;
            isRaining = true;
            Debug.Log("It has started raining");
        }
        else if (temp > 10 && isRaining)
        {
            main.startLifetime = 0;
            isRaining = false;
            Debug.Log("It has stopped raining");
        }

        if (isRaining && !isAudioPlaying && !isRainFading && !isLightFading)
        {
            StartCoroutine(RainStartFade());
            StartCoroutine(LightStartFade());
        }

        if (!isRaining && isAudioPlaying && !isRainFading && !isLightFading)
        {
            StartCoroutine(RainEndFade());
            StartCoroutine(LightEndFade());
        }

        if (isRaining && canThunder)
        {
            PlayThunder();
        }
    }

    private IEnumerator Temperature()
    {
        if (temp == minTemp)
        {
            tempOffset = Random.Range(0, 3);
        }
        if (temp == maxTemp)
        {
            tempOffset = Random.Range(-3, 0);
        }
        else
        {
            tempOffset = Random.Range(-3, 3);
        }

        temp += tempOffset;

        if (temp <= 10 && prevTemp > 10)
        {
            MoraleManager.Instance.DecreaseMorale();
        }
        else if (temp >= 11 && temp < 20 && prevTemp < 11)
        {
            MoraleManager.Instance.IncreaseMorale();
        }
        else if (temp <= 11 & temp < 20 && prevTemp >= 20)
        {
            MoraleManager.Instance.DecreaseMorale();
        }
        else if (temp >= 21 && prevTemp < 21)
        {
            MoraleManager.Instance.IncreaseMorale();
        }

        prevTemp = temp;

        yield return new WaitForSeconds(10f);
        StartCoroutine(Temperature());
    }

    private IEnumerator RainStartFade()
    {
        isRainFading = true;
        rainSound.Play();
        float currentTime = 0.0f;
        while (currentTime < 3.0f)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / 3);
            rainSound.volume = Mathf.Lerp(0.0f, 0.6f, t);
            yield return null;
        }
        rainSound.volume = 0.6f;
        isRainFading = false;
        isAudioPlaying = true;
    }

    private IEnumerator RainEndFade()
    {
        isRainFading = true;
        float currentTime = 0.0f;
        while (currentTime < 3.0f)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / 3);
            rainSound.volume = Mathf.Lerp(0.6f, 0.0f, t);
            yield return null;
        }
        rainSound.Stop();
        rainSound.volume = 0.0f;
        isAudioPlaying = false;
        isRainFading = false;
    }

    private IEnumerator LightStartFade()
    {
        isLightFading = true;
        float currentTime = 0.0f;
        while (currentTime < 3.0f)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / 3);
            sunLight.intensity = Mathf.Lerp(1.0f, 0.3f, t);
            yield return null;
        }
        sunLight.intensity = 0.3f;
        isLightFading = false;
    }

    private IEnumerator LightEndFade()
    {
        isLightFading = true;
        float currentTime = 0.0f;
        while (currentTime < 3.0f)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / 3);
            sunLight.intensity = Mathf.Lerp(0.3f, 1.0f, t);
            yield return null;
        }
        sunLight.intensity = 1.0f;
        isLightFading = false;
    }

    private void PlayThunder()
    {
        AudioSource randomThunderSound = null;

        int thunderChance = Random.Range(0, 99);

        if (thunderChance >= 75)
        {
            canThunder = false;
            int i = Random.Range(0, 2);

            switch (i)
            {
                case (0):
                    randomThunderSound = thunderSound;
                    break;
                case (1):
                    randomThunderSound = thunderSoundAlt;
                    break;
            }
            randomThunderSound.Play();
            StartCoroutine(ThunderDelay());
        }
        else
        {
            canThunder = false;
            StartCoroutine(ThunderDelay());
        }
    }

    private IEnumerator ThunderDelay()
    {
        yield return new WaitForSeconds(15.0f);
        canThunder = true;
    }
}
