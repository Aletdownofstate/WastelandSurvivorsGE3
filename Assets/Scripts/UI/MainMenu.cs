using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button playBtn;
    public Button optionBtn;
    public Button howToBtn;

    [SerializeField] private GameObject transition;
    [SerializeField] private CanvasGroup cg;

    public GameObject mainMenuMenuUI;
    public GameObject optionMenuUI;

    private bool canFade;
    private bool isDelayComplete = false;
    
    private void Start()
    {
        mainMenuMenuUI.SetActive(true);
        optionMenuUI.SetActive(false);        
        optionBtn.GetComponent<Button>().onClick.AddListener(delegate { mainMenuMenuUI.SetActive(false); optionMenuUI.SetActive(true); });
    }

    private void Update()
    {
        Fade();

        if (isDelayComplete)
        {
            Debug.Log("Loading next scene");
            SceneManager.LoadScene(1);
        }
    }

    public void StartGame()
    {
        canFade = true;

        StartCoroutine(Delay());        
    }

    private void Fade()
    {
        if (canFade)
        {            
            transition.SetActive(true);
            cg.alpha += Time.deltaTime / 2;
            if (cg.alpha == 0)
            {
                canFade = false;                
            }
        }
    }

    private IEnumerator Delay()
    {
        Debug.Log($"Waiting for delay: 2.0 seconds.");
        isDelayComplete = false;
        yield return new WaitForSeconds(2.0f);
        isDelayComplete = true;
        Debug.Log("Delay finished, isDelayComplete set to true.");        
    }
}
