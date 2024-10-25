using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicalTentObject : MonoBehaviour
{
    [HideInInspector] public bool isPlaced = false;
    [HideInInspector] public bool isChecked = false;

    [SerializeField] private Transform objectCenter;

    public void OnPlace()
    {
        if (!isPlaced)
        {
            isPlaced = true;

            FischlWorks_FogWar.csFogWar fogWar = GameObject.Find("FogWar").GetComponent<FischlWorks_FogWar.csFogWar>();
            fogWar.AddFogRevealer(new FischlWorks_FogWar.csFogWar.FogRevealer(objectCenter, 10, false));
        }
    }
}