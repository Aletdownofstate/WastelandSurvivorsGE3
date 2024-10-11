using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalResource : MonoBehaviour
{
    public enum ResourceState { Depleted, Active }
    public ResourceState currentResourceState;

    private int minResource = 0;
    private int maxResource = 1500;
    public int currentResource;

    void Start()
    {
        currentResourceState = ResourceState.Active;
        currentResource = maxResource;
    }

    void Update()
    {
        if (currentResource <= minResource)
        {
            currentResource = 0;
            currentResourceState = ResourceState.Depleted;
        }

        if (currentResourceState == ResourceState.Depleted)
        {
            DestroyResource();
        }
    }

    public void DepleteResource(int amount)
    {
        currentResource -= amount;
    }

    public void DestroyResource()
    {
        Destroy(gameObject);
    }
}
