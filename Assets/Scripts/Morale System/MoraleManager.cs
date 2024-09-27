using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoraleManager : MonoBehaviour
{
    public static MoraleManager Instance { get; private set; }

    public enum MoraleState { Low, Neutral, High };
    public MoraleState currentState;

    private int minMorale = 0;
    private int maxMorale = 10;
    private int morale;

    private bool isCheckingMorale = false;
    private bool isFirstCheck = true;

    private int previousFood;
    private int previousWater;
    private int previousPopulation;

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

        morale = 5;
    }

    private void Start()
    {
        previousFood = ResourceManager.Instance.GetResourceAmount("Food");
        previousWater = ResourceManager.Instance.GetResourceAmount("Water");
        previousPopulation = PopulationManager.Instance.population;
    }

    void Update()
    {
        morale = Mathf.Clamp(morale, minMorale, maxMorale);

        if (morale >= 8)
        {
            currentState = MoraleState.High;
        }
        else if (morale <= 3)
        {
            currentState = MoraleState.Low;
        }
        else
        {
            currentState = MoraleState.Neutral;
        }

        if (!isCheckingMorale)
        {
            StartCoroutine(MoraleCheck());
        }
    }

    public void IncreaseMorale()
    {
        morale++;
    }

    public void DecreaseMorale()
    {
        morale--;
    }

    private IEnumerator MoraleCheck()
    {
        isCheckingMorale = true;

        int currentFood = ResourceManager.Instance.GetResourceAmount("Food");
        int currentWater = ResourceManager.Instance.GetResourceAmount("Water");
        int currentPopulation = PopulationManager.Instance.population;

        bool decreaseMoraleForFood = false;
        bool decreaseMoraleForWater = false;
        bool increaseMoraleForFood = false;
        bool increaseMoraleForWater = false;

        if (isFirstCheck || currentFood != previousFood || currentPopulation != previousPopulation)
        {
            float foodPerPerson = (float)currentFood / currentPopulation;

            Debug.Log("Food per Person: " + foodPerPerson);

            if (foodPerPerson < 50.0f)
            {
                Debug.Log("Food per person is less than 50. Decreasing morale.");
                decreaseMoraleForFood = true;
            }
            else
            {
                Debug.Log("Food per person is 50 or more. Increasing morale.");
                IncreaseMorale();
            }

            previousFood = currentFood;
        }

        if (isFirstCheck || currentWater != previousWater || currentPopulation != previousPopulation)
        {
            float waterPerPerson = (float)currentWater / currentPopulation;

            Debug.Log("Water per Person: " + waterPerPerson);

            if (waterPerPerson < 50.0f)
            {
                Debug.Log("Water per person is less than 50. Decreasing morale.");
                decreaseMoraleForWater = true;
            }
            else
            {
                Debug.Log("Water per person is 50 or more. Increasing morale.");
                IncreaseMorale();
            }

            previousWater = currentWater;
        }

        if (isFirstCheck)
        {
            isFirstCheck = false;
        }

        previousPopulation = currentPopulation;

        if (decreaseMoraleForFood)
        {
            DecreaseMorale();
            Debug.Log(morale);
        }
        if (decreaseMoraleForWater)
        {
            DecreaseMorale();
            Debug.Log(morale);
        }
        if (increaseMoraleForFood)
        {
            IncreaseMorale();
            Debug.Log(morale);
        }
        if (increaseMoraleForWater)
        {
            IncreaseMorale();
            Debug.Log(morale);
        }

        yield return new WaitForSeconds(1.0f);
        isCheckingMorale = false;
    }
}