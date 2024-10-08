using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IngameUI : MonoBehaviour
{
    [Header("General UI")]
    public GameObject ingameUI;
    public GameObject pauseMenuUI;

    [Header("Map UI")]
    public GameObject miniMapUI;
    public GameObject mainMapUI;

    [Header("Build Menu UI")]
    public GameObject buildMenuUI;
    public Button closeBuildUI;

    [Header("Main Menu Buttons")]
    public Button exitBtn;
    public Button resumeBtn;
    public Button optionBtn;

    private bool isMapOpen, isBuildOpen;

    // Start is called before the first frame update
    void Start()
    {
        ingameUI.SetActive(true);
        miniMapUI.SetActive(true);

        pauseMenuUI.SetActive(false);
        mainMapUI.SetActive(false);
        isMapOpen = false;

        buildMenuUI.SetActive(false);
        isBuildOpen = false;

    }

    // Update is called once per frame
    void Update()
    {
        resumeBtn.GetComponent<Button>().onClick.AddListener(delegate { SetGamePause(false); });
        //exitBtn.GetComponent<Button>().onClick.AddListener(delegate { SceneManager.LoadScene(0); });
        
        closeBuildUI.GetComponent<Button>().onClick.AddListener (delegate {
            buildMenuUI.SetActive(false);
            isBuildOpen = false;
        });

        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 1)
        {
            SetGamePause(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0)
        {
            SetGamePause(false);
        }

        if (Input.GetKeyDown(KeyCode.M) && isMapOpen == false)
        {
            miniMapUI.SetActive(false);
            mainMapUI.SetActive(true);
            isMapOpen = true;
        } 
        else if (Input.GetKeyDown(KeyCode.M) && isMapOpen == true) 
        {
            miniMapUI.SetActive(true);
            mainMapUI.SetActive(false);
            isMapOpen = false;
        }

        if (Input.GetKeyDown(KeyCode.I) && isBuildOpen == false)
        {
            buildMenuUI.SetActive(true);
            isBuildOpen= true;
        }
        else if (Input.GetKeyDown(KeyCode.I) && isBuildOpen == true)
        {
            buildMenuUI.SetActive(false);
            isBuildOpen = false;
        }
    }

    void SetGamePause(bool setPause)
    {
        if (setPause == true)
        {
            Time.timeScale = 0;
            ingameUI.SetActive(false);
            pauseMenuUI.SetActive(true);
        } else if (setPause == false)
        {
            Time.timeScale = 1;
            pauseMenuUI.SetActive(false);
            ingameUI.SetActive(true);
        }
    }
}
