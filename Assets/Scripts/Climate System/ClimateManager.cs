using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimateManager : MonoBehaviour
{
    public static ClimateManager Instance { get; private set; }

    public int temp;
    private int minTemp;
    private int maxTemp;
    private int tempOffset;
    private int prevTemp;

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
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        minTemp = 5;
        maxTemp = 25;
        temp = 15;
        prevTemp = temp;
        StartCoroutine(Temperature());
    }

    private void Update()
    {
        temp = Mathf.Clamp(temp, minTemp, maxTemp);
    }

    private IEnumerator Temperature()
    {
        tempOffset = Random.Range(-3, 3);
        temp += tempOffset;
        
        if (temp <= 10 && prevTemp > 10)
        {
            MoraleManager.Instance.DecreaseMorale();
            Debug.Log($"Temperature has decreased, decreasing morale");
        }
        if (temp >= 11 && temp < 20 && prevTemp < 11)
        {
            MoraleManager.Instance.IncreaseMorale();
            Debug.Log($"Temperate has increased, increasing morale");
        }
        if (temp <= 11 & temp < 20 && prevTemp >= 20)
        {
            MoraleManager.Instance.DecreaseMorale();
            Debug.Log($"Temperate has decreased, decreasing morale");
        }
        if (temp >= 21 && prevTemp < 21)
        {
            MoraleManager.Instance.IncreaseMorale();
            Debug.Log($"Temperature has increased, increasing morale");
        }

        prevTemp = temp;        

        yield return new WaitForSeconds(10f);       
        StartCoroutine(Temperature());       
    }
}