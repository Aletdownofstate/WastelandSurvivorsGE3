using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    private Dictionary<string, int> resources = new Dictionary<string, int>();

    private int foodCap, waterCap, woodCap, metalCap;

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

        InitialiseResources();
    }

    private void Update()
    {
        if (GetResourceAmount("Food") > foodCap)
        {
            resources["Food"] = foodCap;
        }
        if (GetResourceAmount("Wood") > woodCap)
        {
            resources["Wood"] = woodCap;
        }
        if (GetResourceAmount("Metal") > metalCap)
        {
            resources["Metal"] = metalCap;
        }
        if (GetResourceAmount("Water") > waterCap)
        {
            resources["Water"] = waterCap;
        }
    }

    private void InitialiseResources()
    {
        resources["Food"] = 100;
        resources["Wood"] = 100;
        resources["Water"] = 100;
        resources["Metal"] = 100;

        foodCap = 1000;
        woodCap = 1000;
        waterCap = 1000;
        metalCap = 1000;
    }

    public void AddResource(string resourceType, int amount)
    {
        if (resources.ContainsKey(resourceType))
        {
            resources[resourceType] += amount;
            Debug.Log($"{resourceType} increased by {amount}.");
        }
    }

    public bool SubtractResource(string resourceType, int amount)
    {
        if (resources.ContainsKey(resourceType) && resources[resourceType] >= amount)
        {
            resources[resourceType] -= amount;
            Debug.Log($"{resourceType} decreased by {amount}.");
            return true;
        }
        else
        {
            Debug.LogWarning($"Not enough {resourceType}.");
            return false;
        }
    }

    public int GetResourceAmount(string resourceType)
    {
        return resources[resourceType];
    }

    public void IncreaseResourcCap(string resourceType, int amount)
    {
        switch (resourceType)
        {
            case "Wood":
                woodCap += amount;
                Debug.Log($"Wood Capacity increased to {woodCap}");
                break;
            case "Water":
                waterCap += amount;
                Debug.Log($"Water Capacity increased to {waterCap}");
                break;
            case "Metal":
                metalCap += amount;
                Debug.Log($"Metal Capacity increased to {metalCap}");
                break;
            case "Food":
                foodCap += amount;
                Debug.Log($"Food Capacity increased to {foodCap}");
                break;
        }
    }
}