using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup cg;
    [SerializeField] private AudioSource windSound;

    void Start()
    {
        StartCoroutine(IntroText());
    }

    private IEnumerator IntroText()
    {
        windSound.Play();
        StartCoroutine(SoundFade(windSound, 0.0f, 2.0f, 1.0f));

        float currentTime = 0.0f;
        while (currentTime < 3.0f)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / 3);
            cg.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }
        cg.alpha = 1;

        yield return new WaitForSeconds(5.0f);

        currentTime = 0.0f;
        while (currentTime < 3.0f)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / 3);
            cg.alpha = Mathf.Lerp(1, 0, t);
            yield return null;
        }
        cg.alpha = 0;

        StartCoroutine(SoundFade(windSound, 2.0f, 0.0f, 1.0f));

        yield return new WaitForSeconds(3.0f);
        windSound.Stop();

        SceneManager.LoadScene(2);
    }

    private IEnumerator SoundFade(AudioSource audioSource, float startVolume, float endVolume, float pitch)
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
}