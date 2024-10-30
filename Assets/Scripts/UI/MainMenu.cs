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

    public GameObject mainMenuMenuUI;
    public GameObject optionMenuUI;
    
    private void Start()
    {
        mainMenuMenuUI.SetActive(true);
        optionMenuUI.SetActive(false);
        playBtn.GetComponent<Button>().onClick.AddListener(delegate { SceneManager.LoadScene(1); });
        optionBtn.GetComponent<Button>().onClick.AddListener(delegate { mainMenuMenuUI.SetActive(false); optionMenuUI.SetActive(true); });
    }
}
