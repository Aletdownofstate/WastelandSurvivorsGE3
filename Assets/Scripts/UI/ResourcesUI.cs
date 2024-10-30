using UnityEngine;
using TMPro;

public class ResourcesUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI woodText, waterText, foodText, metalText, populationText, moraleText, temperatureText, dayCountdownText, fpsText;
    public float deltaTime;

    private void Start()
    {
        dayCountdownText.enabled = false;
    }


    void Update()
    {
        populationText.text = $"Population: {PopulationManager.Instance.population}/{PopulationManager.Instance.populationCurrentCap}";

        moraleText.text = $"Morale: {MoraleManager.Instance.currentState}";

        temperatureText.text = $"Temp: {ClimateManager.Instance.temp}C";

        woodText.text = $"{ResourceManager.Instance.GetResourceAmount("Wood")}";
        waterText.text = $"{ResourceManager.Instance.GetResourceAmount("Water")}";
        foodText.text = $"{ResourceManager.Instance.GetResourceAmount("Food")}";
        metalText.text = $"{ResourceManager.Instance.GetResourceAmount("Metal")}";

        dayCountdownText.text = $"Days Remaining Until Winter: {TimeManager.Instance.daysRemaining}";        

        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = "FPS: " + Mathf.Ceil(fps).ToString();

        if (GameManager.Instance.currentGameState == GameManager.GameState.ChapterFive)
        {
            dayCountdownText.enabled = true;
        }
    }
}