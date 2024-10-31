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

        StartCoroutine(Fade(cg, 0, 1));

        yield return new WaitForSeconds(5.0f);

        StartCoroutine(Fade(cg, 1, 0));

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

    private IEnumerator Fade(CanvasGroup cg, float startAlpha, float endAlpha)
    {
        float currentTime = 0.0f;
        while (currentTime < 3.0f)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / 3);
            cg.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            yield return null;
        }
        cg.alpha = endAlpha;
    }
}