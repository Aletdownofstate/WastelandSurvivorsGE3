using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
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
    public GameObject mainMenuMenuUI;
    public Button returnToPauseMenu;

    private bool isFullscreen;

    public static int fpsValue = 60;
    public static float volumeValue = 1;
    public static float cameraSpeedValue = 20;

    private void Start()
    {
        returnToPauseMenu.GetComponent<Button>().onClick.AddListener(delegate { optionMenuUI.SetActive(false); mainMenuMenuUI.SetActive(true); });        
        setFullscreen();
        setRes();
    }

    private void Update()
    {
        volumeValue = volumeSlider.value;
        displayVolume.text = volumeSlider.value.ToString("F2");

        cameraSpeedValue = cameraSpeedSlider.value;
        displayCameraSpeed.text = cameraSpeedSlider.value.ToString("F2");

        Application.targetFrameRate = (int)fpsSlider.value;
        fpsValue = (int)fpsSlider.value;
        displayFPS.text = fpsSlider.value.ToString("F2");
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
            Screen.SetResolution(1920, 1080, isFullscreen);
        }
        if (screenResDropdown.value == 1)
        {
            Screen.SetResolution(1280, 720, isFullscreen);
        }
        if (screenResDropdown.value == 2)
        {
            Screen.SetResolution(640, 480, isFullscreen);
        }
    }
}

