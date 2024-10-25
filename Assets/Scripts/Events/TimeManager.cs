using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    public int daysSurvived = 0;
    public int daysRemaining = 30;

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

    public void StartGameDay()
    {
        StartCoroutine(GameDay());        
    }

    private IEnumerator GameDay()
    {
        yield return new WaitForSeconds(180.0f);
        daysSurvived++;
        daysRemaining--;

        StartCoroutine(GameDay());
    }
}