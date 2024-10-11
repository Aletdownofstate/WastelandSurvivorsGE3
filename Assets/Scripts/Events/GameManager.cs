using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { Intro, ChapterOne, ChapterTwo, ChapterThree }
    public GameState currentGameState;

    private bool startDelay = false;
    private bool isDelayComplete;
    private bool isEventVisible = false;

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
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        currentGameState = GameState.Intro;
        Debug.Log(currentGameState);
    }

    void Update()
    {
        if (currentGameState == GameState.Intro)
        {
            TransitionManager.Instance.canFadeIn = true;
            
            if (!startDelay)
            {                
                StartCoroutine(Delay(4.0f));
            }

            if (isDelayComplete && !isEventVisible)
            {
                EventManager.Instance.InitialiseEventText($"Chapter One", $"\"We need to gather supplies and build shelter if we're going to survive out here!\"", $"Gather 500 of each resource and build two tents");
                EventManager.Instance.ShowEventUI();
                isEventVisible = true;
                currentGameState = GameState.ChapterOne;
            }

        }

        if (currentGameState == GameState.ChapterOne)
        {
            CameraController cameraController = Camera.main.GetComponent<CameraController>();
            cameraController.canPlayerContol = true;

            isEventVisible = false;

            bool goalOne= false;
            bool goalTwo = false;            

            if (ResourceManager.Instance.GetResourceAmount("Wood") >= 500
                && ResourceManager.Instance.GetResourceAmount("Water") >= 500 
                && ResourceManager.Instance.GetResourceAmount("Metal") >= 500 
                && ResourceManager.Instance.GetResourceAmount("Food") >= 500)
            {
                goalOne = true;                
            }

            GameObject[] tents = GameObject.FindGameObjectsWithTag("Tent");            
            if (tents.Length >= 2)
            {
                goalTwo = true;
            }


            if (goalOne && goalTwo)
            {
                if (!startDelay)
                {
                    StartCoroutine(Delay(3.0f));
                }

                if (isDelayComplete && !isEventVisible)
                {
                    cameraController.canPlayerContol = false;
                    EventManager.Instance.InitialiseEventText($"Chapter Two", $"\"Some description\"", $"Some goal");
                    EventManager.Instance.ShowEventUI();
                    isEventVisible = true;
                    currentGameState = GameState.ChapterTwo;
                }
            }
        }

        if (currentGameState == GameState.ChapterTwo)
        {
            // Do stuff here.
        }

        if (currentGameState == GameState.ChapterThree)
        {
            // Do stuff here.
        }
    }

    private IEnumerator Delay(float delay)
    {
        startDelay = true;
        isDelayComplete = false;
        yield return new WaitForSeconds(delay);
        isDelayComplete = true;
    }
}
