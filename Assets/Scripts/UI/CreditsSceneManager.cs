using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditsSceneManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup cgCredits;
    [SerializeField] private CanvasGroup cgSpecialThanks;
    [SerializeField] private CanvasGroup cgThanksForPlaying;
    [SerializeField] private CanvasGroup cgButton;
    [SerializeField] private Button continueButton;

    private void Awake()
    {
        cgThanksForPlaying.alpha = 0;
        cgCredits.alpha = 0;
        cgSpecialThanks.alpha = 0;
        continueButton.enabled = false;
    }

    private void Start()
    { 
        StartCoroutine(Credits());
    }

    private IEnumerator Credits()
    {
        StartCoroutine(Fade(cgThanksForPlaying, 0, 1));

        yield return new WaitForSeconds(1.5f);

        StartCoroutine(Fade(cgCredits, 0, 1));

        yield return new WaitForSeconds(5.0f);

        StartCoroutine(Fade(cgCredits, 1, 0));

        yield return new WaitForSeconds(4.0f);

        StartCoroutine(Fade(cgSpecialThanks, 0, 1));
        continueButton.enabled = true;
        StartCoroutine(Fade(cgButton, 0, 1));
        
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

    public void ContinueButton()
    {
        SceneManager.LoadScene(0);
    }
}