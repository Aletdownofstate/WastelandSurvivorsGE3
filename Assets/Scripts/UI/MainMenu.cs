using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button playBtn;
    // Start is called before the first frame update
    void Start()
    {
        playBtn.GetComponent<Button>().onClick.AddListener(delegate { SceneManager.LoadScene(1); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
