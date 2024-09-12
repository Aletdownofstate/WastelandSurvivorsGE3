using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameUI : MonoBehaviour
{
    public GameObject ingameUI, pauseMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenuUI.SetActive(false);
        ingameUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                ingameUI.SetActive(false);
                pauseMenuUI.SetActive(true);
                
            }
            else if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                pauseMenuUI.SetActive(false);
                ingameUI.SetActive(true);
            }
        }
    }
}
