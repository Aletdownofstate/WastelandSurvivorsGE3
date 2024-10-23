using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTowerObject : MonoBehaviour
{
    [HideInInspector] public bool isPlaced = false;
    [HideInInspector] public bool isChecked = false;

    [SerializeField] Transform objectCenter;

    public void OnPlace()
    {
        if (!isPlaced)
        {
            isPlaced = true;
            ResourceManager.Instance.IncreaseResourcCap("Water", 2000);

            FischlWorks_FogWar.csFogWar fogWar = GameObject.Find("FogWar").GetComponent<FischlWorks_FogWar.csFogWar>();
            fogWar.AddFogRevealer(new FischlWorks_FogWar.csFogWar.FogRevealer(objectCenter, 15, false));
        }
    }
}
