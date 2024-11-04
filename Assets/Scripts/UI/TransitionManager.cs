using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{    
    public static TransitionManager Instance { get; private set; }

    [SerializeField] private GameObject transition;
    public CanvasGroup cg;

    public bool canFadeIn;
    public bool canFadeOut;

    public bool isTransitionComplete = false;

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
        transition.SetActive(true);
        cg.alpha = 1;        
    }

    private void Start()
    {
        cg.alpha = 1;        
    }

    private void Update()
    {
        FadeInFromBlack();
        FadeOutToBlack();
    }

    private void FadeOutToBlack()
    {
        if (canFadeOut)
        {
            transition.SetActive(true);
            cg.alpha += Time.deltaTime / 4;
            if (cg.alpha == 1)
            {
                canFadeOut = false;                
            }
        }
    }

    private void FadeInFromBlack()
    {
        if (canFadeIn)
        {
            transition.SetActive(true);
            cg.alpha -= Time.deltaTime / 4;
            if (cg.alpha == 0)
            {
                canFadeIn = false;
                transition.SetActive(false);                
            }
        }
    }
}
