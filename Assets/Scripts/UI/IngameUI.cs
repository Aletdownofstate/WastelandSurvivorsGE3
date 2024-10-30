using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IngameUI : MonoBehaviour
{
    [Header("General UI")]
    public GameObject ingameUI;
    public GameObject pauseMenuUI;
    public Camera MainCamera;
    public Camera mapCamera;
    public CameraController MainCameraController;

    [Header("Map UI")]
    public GameObject miniMapUI;
    public GameObject mainMapUI;

    [Header("Build Menu UI")]
    public GameObject buildMenuUI;

    [Header("Pause Menu UI")]
    public Button exitBtn;
    public Button resumeBtn;
    public Button optionBtn;

    [Header("Option Menu UI")]
    public Toggle fullscreenToggle;
    public Slider volumeSlider;
    public Slider cameraSpeedSlider;
    public Slider fpsSlider;
    public TMP_Dropdown screenResDropdown;
    public TextMeshProUGUI displayVolume;
    public TextMeshProUGUI displayCameraSpeed;
    public TextMeshProUGUI displayFPS;
    public GameObject optionMenuUI;
    public Button returnToPauseMenu;

    [Header("Side Menu UI")]
    public Button optionSideBtn;
    public Button menuSideBtn;
    public Button homeSideBtn;

    private bool isMapOpen;
    private bool isFullscreen;

    void Start()
    {
        volumeSlider.value = OptionMenu.volumeValue;
        cameraSpeedSlider.value = OptionMenu.cameraSpeedValue;
        fpsSlider.value = OptionMenu.fpsValue;

        ingameUI.SetActive(true);
        miniMapUI.SetActive(true);

        pauseMenuUI.SetActive(false);
        optionMenuUI.SetActive(false);
        mainMapUI.SetActive(false);
        isMapOpen = false;
        
        resumeBtn.GetComponent<Button>().onClick.AddListener(delegate { SetGamePause(false); });
        exitBtn.GetComponent<Button>().onClick.AddListener(delegate { Application.Quit(); Debug.Log("closing game"); });
        optionBtn.GetComponent<Button>().onClick.AddListener(delegate { optionMenuUI.SetActive(true);  pauseMenuUI.SetActive(false); });

        optionSideBtn.GetComponent<Button>().onClick.AddListener(delegate { optionMenuUI.SetActive(true); pauseMenuUI.SetActive(false); Time.timeScale = 0; AudioListener.pause = true; });
        menuSideBtn.GetComponent<Button>().onClick.AddListener(delegate { SetGamePause(true); });
        homeSideBtn.GetComponent<Button>().onClick.AddListener(delegate { MainCamera.transform.position = new Vector3(10, 10, -2.99f); mapCamera.transform.position = new Vector3(8.5f, 34.5f, 9.5f); });

        returnToPauseMenu.GetComponent<Button>().onClick.AddListener(delegate {optionMenuUI.SetActive(false); pauseMenuUI.SetActive(true); });

        setFullscreen();
        setRes();
    }

    void Update()
    {

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
            ingameUI.SetActive(false);
            buildMenuUI.SetActive(false);
            mainMapUI.SetActive(true);
            isMapOpen = true;
        } 
        else if (Input.GetKeyDown(KeyCode.M) && isMapOpen == true) 
        {
            miniMapUI.SetActive(true);
            ingameUI.SetActive(true);
            buildMenuUI.SetActive(true);
            mainMapUI.SetActive(false);
            isMapOpen = false;
        }

        AudioListener.volume = volumeSlider.value;
        displayVolume.text = volumeSlider.value.ToString("F2");

        MainCameraController.panSpeed = cameraSpeedSlider.value;
        displayCameraSpeed.text = cameraSpeedSlider.value.ToString("F2");

        Application.targetFrameRate = (int)fpsSlider.value;
        displayFPS.text = fpsSlider.value.ToString("F2");
    }

    void SetGamePause(bool setPause)
    {
        if (setPause == true)
        {
            Time.timeScale = 0;
            pauseMenuUI.SetActive(true);
            optionMenuUI.SetActive(false);
            ingameUI.SetActive(false);
            buildMenuUI.SetActive(false);
            miniMapUI.SetActive(false);
            AudioListener.pause = true;
        } else if (setPause == false)
        {
            Time.timeScale = 1;
            pauseMenuUI.SetActive(false);
            optionMenuUI.SetActive(false);
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

    void setRes()
    {
        if (screenResDropdown.value == 0)
        {
            Screen.SetResolution(2560, 1440, isFullscreen);
        }
        if (screenResDropdown.value == 1)
        {
            Screen.SetResolution(1920, 1080, isFullscreen);
        }
        if (screenResDropdown.value == 2)
        {
            Screen.SetResolution(1280, 720, isFullscreen);
        }
        if (screenResDropdown.value == 3)
        {
            Screen.SetResolution(640, 480, isFullscreen);
        }
    }
}