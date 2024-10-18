using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTowerObject : MonoBehaviour
{
    [HideInInspector] public bool isPlaced = false;
    [HideInInspector] public bool isChecked = false;

    public void OnPlace()
    {
        if (!isPlaced)
        {
            isPlaced = true;
            ResourceManager.Instance.IncreaseResourcCap("Water", 2000);            
        }
    }
}
