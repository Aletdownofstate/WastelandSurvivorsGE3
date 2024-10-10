using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentObject : MonoBehaviour
{
    private bool isPlaced = false;

    public void OnPlace()
    {
        if (!isPlaced)
        {
            isPlaced = true;
            PopulationManager.Instance.IncreaseCap();
        }
    }
}