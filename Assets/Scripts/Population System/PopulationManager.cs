using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    public static PopulationManager Instance { get; private set;}

    public List<GameObject> numberOfUnits = new List<GameObject>();
    
    private int populationMax;
    public int populationCurrentCap;
    public int population;

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

        InitialiseCap();
        CalculatePopulation();
    }

    private void Update()
    {
        CalculatePopulation();
    }

    private void InitialiseCap()
    {
        populationCurrentCap = 4;
        populationMax = 20;
    }

    private void CalculatePopulation()
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");

        foreach (GameObject unit in units)
        {
            if (!numberOfUnits.Contains(unit))
            {
                numberOfUnits.Add(unit);
            }
        }
        population = numberOfUnits.Count;

        if (populationCurrentCap > populationMax)
        {
            populationCurrentCap = populationMax;
        }
    }

    public void IncreaseCap()
    {
        if (populationCurrentCap < populationMax)
        {
            populationCurrentCap += 2;
        }
        else
        {
            return;
        }
    }    
}