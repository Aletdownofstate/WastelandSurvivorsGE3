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


    [Header("Pause Menu UI")]
    public Button exitBtn;
    public Button resumeBtn;
    public Button optionBtn;
    public Toggle fullscreenToggle;

    [Header("Side Menu UI")]
    public Button optionSideBtn;
    public Button menuSideBtn;
    public Button homeSideBtn;

    private bool isMapOpen;
    private bool isFullscreen;

    // Start is called before the first frame update
    void Start()
    {
        ingameUI.SetActive(true);
        miniMapUI.SetActive(true);

        pauseMenuUI.SetActive(false);
        mainMapUI.SetActive(false);
        isMapOpen = false;
        
        resumeBtn.GetComponent<Button>().onClick.AddListener(delegate { SetGamePause(false); });
        exitBtn.GetComponent<Button>().onClick.AddListener(delegate { Application.Quit(); Debug.Log("closing game"); });
        optionBtn.GetComponent<Button>().onClick.AddListener(delegate { /*setactive option ui*/ });

        optionSideBtn.GetComponent<Button>().onClick.AddListener(delegate {/*setactive option ui*/ });
        menuSideBtn.GetComponent<Button>().onClick.AddListener(delegate { SetGamePause(true); });
        homeSideBtn.GetComponent<Button>().onClick.AddListener(delegate { Debug.Log("home button pressed"); });



        setFullscreen();
    }

    // Update is called once per frame
    void Update()
    {
        //resumeBtn.GetComponent<Button>().onClick.AddListener(delegate { SetGamePause(false); });

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
    }

    void SetGamePause(bool setPause)
    {
        if (setPause == true)
        {
            Time.timeScale = 0;
            pauseMenuUI.SetActive(true);

            ingameUI.SetActive(false);
            buildMenuUI.SetActive(false);
            miniMapUI.SetActive(false);
            AudioListener.pause = true;
        } else if (setPause == false)
        {
            Time.timeScale = 1;
            pauseMenuUI.SetActive(false);

            ingameUI.SetActive(true);
            buildMenuUI.SetActive(true);
            miniMapUI.SetActive(true);
            AudioListener.pause = false;
        }
    }

    void setFullscreen()
    {
        if (fullscreenToggle.isOn)
        {
            isFullscreen = true;
        }
        if (!fullscreenToggle.isOn)
        {
            isFullscreen = false;
        }
        Screen.fullScreen = isFullscreen;
    }
}
