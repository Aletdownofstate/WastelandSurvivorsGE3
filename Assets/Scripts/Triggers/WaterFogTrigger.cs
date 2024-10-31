using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFogTrigger : MonoBehaviour
{
    [SerializeField] private GameObject waterFogRevealer;
    
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider collision)
    {
        if (!hasTriggered)
        {
            Debug.Log("Water trigger activated");
            hasTriggered = true;
            FischlWorks_FogWar.csFogWar fogWar = GameObject.Find("FogWar").GetComponent<FischlWorks_FogWar.csFogWar>();
            fogWar.AddFogRevealer(new FischlWorks_FogWar.csFogWar.FogRevealer(waterFogRevealer.transform, 15, false));
        }
    }
}
