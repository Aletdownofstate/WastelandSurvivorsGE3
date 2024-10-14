using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseObject : MonoBehaviour
{
    [HideInInspector] public bool isPlaced = false;
    [HideInInspector] public bool isChecked = false;
    public void OnPlace()
    {
        if (!isPlaced)
        {
            isPlaced = true;
            ResourceManager.Instance.IncreaseResourcCap("Wood", 1000);
            ResourceManager.Instance.IncreaseResourcCap("Food", 1000);
            ResourceManager.Instance.IncreaseResourcCap("Water", 1000);
            ResourceManager.Instance.IncreaseResourcCap("Metal", 1000);
        }
    }
}
