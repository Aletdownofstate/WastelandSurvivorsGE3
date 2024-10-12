using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private CameraController cameraController;

    public enum GameState { Intro, ChapterOne, ChapterTwo, ChapterThree }
    public GameState currentGameState;

    private bool startDelay = false;
    private bool isDelayComplete = true;
    private bool isEventVisible = false;

    private bool hasDelayTriggered = false;

    private bool chapterOneGoalOne;
    private bool chapterOneGoalTwo;

    private bool chapterTwoGoalOne;

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
        cameraController = Camera.main.GetComponent<CameraController>();
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
            
            if (!startDelay && isDelayComplete)
            {
                startDelay = true;
                StartCoroutine(Delay(3.0f));
            }

            if (isDelayComplete && !isEventVisible)
            {
                EventManager.Instance.InitialiseEventText($"Chapter One", $"\"We need to gather supplies and build shelter if we're going to survive out here!\"", $"Gather 500 of each resource and build two tents");
                EventManager.Instance.ShowEventUI();
                isEventVisible = true;
                isDelayComplete = false;
                Debug.Log("Entering Chapter One");
                currentGameState = GameState.ChapterOne;
            }
        }

        if (currentGameState == GameState.ChapterOne)
        {
            cameraController.canPlayerContol = true;

            int food = ResourceManager.Instance.GetResourceAmount("Food");
            int water = ResourceManager.Instance.GetResourceAmount("Water");
            int wood = ResourceManager.Instance.GetResourceAmount("Wood");
            int metal = ResourceManager.Instance.GetResourceAmount("Metal");

            if (!chapterOneGoalOne)
            {
                if (wood >= 500 && metal >= 500 && water >= 500 && food >= 500)
                {
                    chapterOneGoalOne = true;
                    Debug.Log($"{currentGameState}: Goal One complete");
                }
            }

            GameObject[] tents = GameObject.FindGameObjectsWithTag("Tent");

            if (!chapterOneGoalTwo)
            {
                if (tents.Length >= 2 && !chapterOneGoalTwo)
                {
                    chapterOneGoalTwo = true;
                    Debug.Log($"{currentGameState}: Goal Two complete");
                }
            }

            if (chapterOneGoalOne && chapterOneGoalTwo)
            {
                if (startDelay && !hasDelayTriggered)
                {
                    if (isDelayComplete)
                    {
                        isDelayComplete = false;
                    }
                    if (isEventVisible)
                    {
                        isEventVisible = false;
                    }
                    startDelay = false;
                    hasDelayTriggered = true;
                }

                if (!startDelay && !isDelayComplete)
                {
                    Debug.Log("Starting the delay coroutine.");
                    startDelay = true;
                    StartCoroutine(Delay(3.0f));                    
                }

                if (isDelayComplete && !isEventVisible)
                {
                    EventManager.Instance.InitialiseEventText($"Chapter Two", $"\"All these supplies are going to need to be stored somewhere.\"", $"Build a warehouse");
                    EventManager.Instance.ShowEventUI();
                    isEventVisible = true;
                    Debug.Log("Entering Chapter Two");
                    currentGameState = GameState.ChapterTwo;

                    startDelay = false;
                }
            }
        }

        if (currentGameState == GameState.ChapterTwo)
        {
            if (!chapterTwoGoalOne)
            {

            }

            //if (chapter goals completed)
            //{

            //    if (startDelay && !hasDelayTriggered)
            //    {
            //        if (isDelayComplete)
            //        {
            //            isDelayComplete = false;
            //        }
            //        if (isEventVisible)
            //        {
            //            isEventVisible = false;
            //        }
            //        startDelay = false;
            //        hasDelayTriggered = true;
            //    }

            //    if (!startDelay && !isDelayComplete)
            //    {
            //        Debug.Log("Starting the delay coroutine.");
            //        startDelay = true;
            //        StartCoroutine(Delay(3.0f));
            //    }

            //    if (isDelayComplete && !isEventVisible)
            //    {
            //        EventManager.Instance.InitialiseEventText($"Chapter Three", $"\"Do whatever\"", $"Some goal");
            //        EventManager.Instance.ShowEventUI();
            //        isEventVisible = true;
            //        Debug.Log("Entering Chapter Three");
            //        currentGameState = GameState.ChapterThree;

            //        startDelay = false;
            //    }
            //}
        }
    }

    private IEnumerator Delay(float delay)
    {
        Debug.Log($"Waiting for delay: {delay} seconds.");
        isDelayComplete = false;
        yield return new WaitForSeconds(delay);
        isDelayComplete = true;
        Debug.Log("Delay finished, isDelayComplete set to true.");
    }
}
