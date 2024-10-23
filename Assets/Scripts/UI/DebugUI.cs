using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI foodRemainingText, woodRemainingText, metalRemainingText;

    void Update()
    {
        int foodAmount = 0;
        int woodAmount = 0;
        int metalAmount = 0;

        GameObject[] trees = GameObject.FindGameObjectsWithTag("Wood");
        GameObject[] metals = GameObject.FindGameObjectsWithTag("Metal");
        GameObject[] foods = GameObject.FindGameObjectsWithTag("Food");

        foreach (var tree in trees)
        {
            woodAmount += 500;
        }
        foreach (var metal in metals)
        {
            metalAmount += 1500;
        }
        foreach (var food in foods)
        {
            foodAmount += 350;
        }

        foodRemainingText.text = $"{foodAmount / 2} food available";
        woodRemainingText.text = $"{woodAmount / 2} wood available";
        metalRemainingText.text = $"{metalAmount / 2} metal available";
    }
}
