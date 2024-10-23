using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddFogRevealer : MonoBehaviour
{
    private void Awake()
    {
        FischlWorks_FogWar.csFogWar fogWar = GameObject.Find("FogWar").GetComponent<FischlWorks_FogWar.csFogWar>();
        fogWar.AddFogRevealer(new FischlWorks_FogWar.csFogWar.FogRevealer(gameObject.transform, 12, false));
    }
}