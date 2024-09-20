using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    private Dictionary<string, int> resources = new Dictionary<string, int>();

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

    private void InitialiseResources()
    {
        resources["Food"] = 0;
        resources["Wood"] = 0;
        resources["Water"] = 0;
        resources["Metal"] = 0;
    }

    public void AddResource(string resourceType, int amount)
    {
        if (resources.ContainsKey(resourceType))
        {
            resources[resourceType] += amount;
            Debug.Log($"{resourceType} increased by {amount}. Current: {resources[resourceType]}");
        }
    }

    public bool SubtractResource(string resourceType, int amount)
    {
        if (resources.ContainsKey(resourceType) && resources[resourceType] >= amount)
        {
            resources[resourceType] -= amount;
            Debug.Log($"{resourceType} decreased by {amount}. Current: {resources[resourceType]}");
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
}