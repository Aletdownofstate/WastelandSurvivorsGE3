using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseObject : MonoBehaviour
{
    [HideInInspector] public bool isPlaced = false;
    [HideInInspector] public bool isChecked = false;

    [SerializeField] Transform objectCenter;

    public void OnPlace()
    {
        if (!isPlaced)
        {
            isPlaced = true;
            ResourceManager.Instance.IncreaseResourcCap("Wood", 10000);
            ResourceManager.Instance.IncreaseResourcCap("Food", 10000);
            ResourceManager.Instance.IncreaseResourcCap("Water", 10000);
            ResourceManager.Instance.IncreaseResourcCap("Metal", 10000);

            FischlWorks_FogWar.csFogWar fogWar = GameObject.Find("FogWar").GetComponent<FischlWorks_FogWar.csFogWar>();
            fogWar.AddFogRevealer(new FischlWorks_FogWar.csFogWar.FogRevealer(objectCenter, 17, false));
        }
    }
}
