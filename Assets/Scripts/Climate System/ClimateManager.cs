using System.Collections;
using UnityEngine;

public class ClimateManager : MonoBehaviour
{
    public static ClimateManager Instance { get; private set; }

    [SerializeField] private ParticleSystem rainPs;
    [SerializeField] private AudioSource rainSound, thunderSound, thunderAltSound, birdsSound, wolvesSound, strongWindSound;
    [SerializeField] private Light sunLight;

    public int temp;
    private int minTemp;
    private int maxTemp;
    private int tempOffset;
    private int prevTemp;

    private bool isRaining, isFading, isLightFading, isRainAudioPlaying, isWindAudioPlaying, areBirdsSinging = false;    
    private bool canThunder, canWolves = true;

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

        // Rain

        if (temp < 10 && !isRaining)
        {
            main.startLifetime = 3;
            isRaining = true;
            Debug.Log("It has started raining");
        }
        else if (temp > 10 && isRaining)
        {
            main.startLifetime = 0;
            isRaining = false;
            Debug.Log("It has stopped raining");
        }

        if (isRaining && !isRainAudioPlaying && !isFading && !isLightFading)
        {
            isRainAudioPlaying = true;
            StartCoroutine(AudioStart(rainSound, 0.0f, 0.6f, 1.0f));
            StartCoroutine(LightStartFade());
            Debug.Log($"{rainSound} is playing");
        }
        else if (!isRaining && isRainAudioPlaying && !isFading && !isLightFading)
        {
            isRainAudioPlaying = false;
            StartCoroutine(AudioStop(rainSound, 0.6f, 0.0f, 1.0f));
            StartCoroutine(LightEndFade());
            Debug.Log($"{rainSound} has stopped playing");
        }

        // Thunder

        if (isRaining && !isFading && canThunder)
        {
            PlayThunder();
        }

        // Strong Wind

        if (isRaining && !isFading && !isWindAudioPlaying)
        {
            isWindAudioPlaying = true;
            StartCoroutine(AudioStart(strongWindSound, 0.0f, 1.0f, 1.0f));
            Debug.Log($"{strongWindSound} is playing");
        }
        else if (!isRaining && !isFading && isWindAudioPlaying)
        {
            isWindAudioPlaying = false;
            StartCoroutine(AudioStop(strongWindSound, 1.0f, 0.0f, 1.0f));
            Debug.Log($"{strongWindSound} has stopped playing");
        }

        // Birds

        if (!isRaining && temp >= 14 && !isFading && !areBirdsSinging)
        {
            areBirdsSinging = true;
            StartCoroutine(AudioStart(birdsSound, 0.0f, 1.35f, 1.0f));
        }
        else if (temp < 14 && areBirdsSinging)
        {
            areBirdsSinging = false;
            StartCoroutine(AudioStop(birdsSound, 1.35f, 0.0f, 1.0f));
        }

        // Wolves

        if (isRaining && !isFading && canWolves)
        {
            PlayWolves();
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

    private IEnumerator AudioStart(AudioSource audioSource, float startVolume, float endVolume, float pitch)
    {
        isFading = true;
        audioSource.Play();
        StartCoroutine(SoundFade(audioSource, startVolume, endVolume, pitch));
        yield return new WaitForSeconds(3.0f);
        isFading = false;
    }    

    private IEnumerator AudioStop(AudioSource audioSource, float startVolume, float endVolume, float pitch)
    {
        isFading = true;

        StartCoroutine(SoundFade(rainSound, startVolume, endVolume, pitch));
        yield return new WaitForSeconds(3.0f);
        audioSource.Stop();        
        isFading = false;
    }

    private IEnumerator SoundFade(AudioSource audioSource, float startVolume,  float endVolume, float pitch)
    {
        audioSource.pitch = pitch;        
        float currentTime = 0.0f;
        while (currentTime < 3.0f)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / 3);
            audioSource.volume = Mathf.Lerp(startVolume, endVolume, t);
            yield return null;
        }
        audioSource.volume = endVolume;
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
                    randomThunderSound = thunderAltSound;
                    break;
            }
            randomThunderSound.Play();
            StartCoroutine(ThunderDelay());
            Debug.Log($"{randomThunderSound} is playing");
        }
        else
        {
            canThunder = false;
            StartCoroutine(ThunderDelay());
        }
    }

    private void PlayWolves()
    {
        int wolvesChance = Random.Range(0, 99);

        if (wolvesChance >= 75)
        {
            canWolves = false;
            float pitch = Random.Range(0.8f, 1.2f);
            wolvesSound.pitch = pitch;
            wolvesSound.volume = 0.4f;
            wolvesSound.Play();
            StartCoroutine(ThunderDelay());
            Debug.Log($"{wolvesSound} is playing");
        }
        else
        {
            canWolves = false;
            StartCoroutine(WolvesDelay());
        }

    }

    private IEnumerator ThunderDelay()
    {
        yield return new WaitForSeconds(15.0f);
        canThunder = true;
    }

    private IEnumerator WolvesDelay()
    {
        yield return new WaitForSeconds(45.0f);
        canWolves = true;
    }
}