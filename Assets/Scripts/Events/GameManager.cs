using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private FischlWorks_FogWar.csFogWar fogWar;

    [SerializeField] GameObject maleWorkerA, maleWorkerB, femaleWorker;
    private CameraController cameraController;

    public enum GameState { Intro, ChapterOne, ChapterTwo, ChapterThree, ChapterFour, ChapterFive, End }
    public GameState currentGameState;

    private bool startDelay = false;
    private bool isDelayComplete = true;
    private bool isEventVisible = false;
    private bool hasDelayTriggered = false;

    private bool isChapterStarted = false;

    private bool chapterOneGoalOne, chapterOneGoalTwo;    
    private int tentAmount = 0;

    private bool chapterTwoGoalOne, chapterThreeFlags;    
    private int warehouseAmount = 0;

    private bool chapterFourGoal, chapterFourFlags;

    private bool chapterFiveGoal, chapterFiveFlags;

    [HideInInspector] public bool chapterThreeGoalOne;

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

        cameraController = Camera.main.GetComponent<CameraController>();
        fogWar = GameObject.Find("FogWar").GetComponent<FischlWorks_FogWar.csFogWar>();

        GameObject homePoint = GameObject.Find("Home Point");
        fogWar.AddFogRevealer(new FischlWorks_FogWar.csFogWar.FogRevealer(homePoint.transform, 15, false));
    }

    void Start()
    {
        currentGameState = GameState.Intro;
        Debug.Log(currentGameState);

        InitialiseWorkers();
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
                EventManager.Instance.InitialiseEventText($"Chapter One", 
                    $"\"We need to gather supplies and build shelter if we're going to survive out here!\"", 
                    $"Gather 500 of each resource and build two tents");
                EventManager.Instance.ShowEventUI();
                isEventVisible = true;
                isDelayComplete = false;
                Debug.Log("Entering Chapter One");
                currentGameState = GameState.ChapterOne;
            }
        }

        // Gather 500 of each material & build two tents

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
                foreach (var tent in tents)
                {
                    TentObject tentObject = tent.GetComponent<TentObject>();

                    if (tentObject.isPlaced && !tentObject.isChecked)
                    {
                        tentAmount++;
                        tentObject.isChecked = true;
                    }
                }

                if (tentAmount >= 2)
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
                    EventManager.Instance.InitialiseEventText($"Chapter Two", 
                        $"\"All these supplies are going to need to be stored somewhere.\"", 
                        $"Build a warehouse");
                    EventManager.Instance.ShowEventUI();
                    isEventVisible = true;
                    Debug.Log("Entering Chapter Two");
                    currentGameState = GameState.ChapterTwo;

                    startDelay = false;
                }
            }
        }

        // Build a warehouse

        if (currentGameState == GameState.ChapterTwo)
        {            
            if (!isChapterStarted)
            {
                isDelayComplete = false;
                isEventVisible = false;
                hasDelayTriggered = false;
                isChapterStarted = true;
            }

            GameObject[] warehouses = GameObject.FindGameObjectsWithTag("Warehouse");

            if (!chapterTwoGoalOne)
            {
                foreach (var warehouse in warehouses)
                {
                    WarehouseObject warehouseObject = warehouse.GetComponent<WarehouseObject>();

                    if (warehouseObject.isPlaced && !warehouseObject.isChecked)
                    {
                        warehouseAmount++;
                        warehouseObject.isChecked = true;
                    }
                }

                if (warehouseAmount >= 1)
                {
                    chapterTwoGoalOne = true;
                    Debug.Log($"{currentGameState}: Goal One complete");
                }
            }

            if (chapterTwoGoalOne)
            {
                if (startDelay && !hasDelayTriggered)
                {
                    Debug.Log("Resetting event flags");

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
                    EventManager.Instance.InitialiseEventText($"Chapter Three", $"\"We've heard reports of a communications tower south of here, we should take go take a look at it.\"", $"Locate the communication tower");
                    EventManager.Instance.ShowEventUI();
                    isEventVisible = true;
                    Debug.Log("Entering Chapter Three");                    
                    currentGameState = GameState.ChapterThree;
                }
            }
        }

        // Find the comms tower

        if (currentGameState == GameState.ChapterThree)
        {
            if (!chapterThreeFlags)
            {
                Debug.Log("Resetting the chapter flags");

                isDelayComplete = false;
                isEventVisible = false;
                hasDelayTriggered = false;
                startDelay = true;
                chapterThreeFlags = true;
            }

            if (chapterThreeGoalOne)
            {
                if (startDelay && !hasDelayTriggered)
                {
                    Debug.Log("Resetting event flags");

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
                    EventManager.Instance.InitialiseEventText($"Chapter Four", $"\"We can use the tower to expand your settlement now. Recruit more survivors!\"", $"Increase population to 10");
                    EventManager.Instance.ShowEventUI();
                    isEventVisible = true;
                    Debug.Log("Entering Chapter Four");
                    currentGameState = GameState.ChapterFour;
                }
            }
        }

        // Reach a population of at least 10

        if (currentGameState == GameState.ChapterFour)
        {
            if (!chapterFourFlags)
            {
                Debug.Log("Resetting the chapter flags");
                isDelayComplete = false;
                isEventVisible = false;
                hasDelayTriggered = false;
                startDelay = true;
                chapterFourFlags = true;
            }

            GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");

            if (!chapterFourGoal)
            {
                if (units.Length >= 10)
                {
                    chapterFourGoal = true;
                }
            }

            if (chapterFourGoal)
            {
                if (startDelay && !hasDelayTriggered)
                {
                    Debug.Log("Resetting event flags");

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
                    EventManager.Instance.InitialiseEventText($"Chapter Five", $"\"Do whatever\"", $"Some goal");
                    EventManager.Instance.ShowEventUI();
                    isEventVisible = true;
                    Debug.Log("Entering Chapter Five");
                    currentGameState = GameState.ChapterFive;
                }
            }
        }

        // Gather resources before the winter

        if (currentGameState == GameState.ChapterFive)
        {
            if (!chapterFiveFlags)
            {
                Debug.Log("Resetting the chapter flags");
                isDelayComplete = false;
                isEventVisible = false;
                hasDelayTriggered = false;
                startDelay = true;
                chapterFiveFlags = true;
            }

            int food = ResourceManager.Instance.GetResourceAmount("Food");
            int water = ResourceManager.Instance.GetResourceAmount("Water");
            int wood = ResourceManager.Instance.GetResourceAmount("Wood");
            int metal = ResourceManager.Instance.GetResourceAmount("Metal");

            if (!chapterFiveGoal)
            {
                if (wood >= 20000 && metal >= 20000 && water >= 20000 && food >= 20000)
                {
                    chapterOneGoalOne = true;
                    Debug.Log($"{currentGameState}: Goal One complete");
                }
            }

            if (chapterFiveGoal)
            {
                if (startDelay && !hasDelayTriggered)
                {
                    Debug.Log("Resetting event flags");

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
                    EventManager.Instance.InitialiseEventText($"Chapter Four", $"\"Do whatever\"", $"Some goal");
                    EventManager.Instance.ShowEventUI();
                    isEventVisible = true;
                    Debug.Log("Entering Ending");
                    currentGameState = GameState.End;
                }
            }
        }

        // End

        if (currentGameState == GameState.End)
        {

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

    private void InitialiseWorkers()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        GameObject worker = null;

        foreach (var spawnPoint in spawnPoints)
        {
            int i = Random.Range(0, 3);

            switch (i)
            {
                case (0):
                    worker = maleWorkerA;
                    break;
                case (1):
                    worker = maleWorkerB;
                    break;
                case (2):
                    worker = femaleWorker;
                    break;
            }
            Instantiate(worker, spawnPoint.transform.position, Quaternion.identity);
        }
    }
}