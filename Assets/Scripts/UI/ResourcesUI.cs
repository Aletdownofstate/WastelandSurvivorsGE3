using UnityEngine;
using TMPro;

public class ResourcesUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI woodText, waterText, foodText, metalText, populationText, moraleText, temperatureText, dayCountdownText;

    void Update()
    {
        populationText.text = $"Population: {PopulationManager.Instance.population}/{PopulationManager.Instance.populationCurrentCap}";

        moraleText.text = $"Morale: {MoraleManager.Instance.currentState}";

        temperatureText.text = $"Temp: {ClimateManager.Instance.temp}C";

        woodText.text = $"Wood: {ResourceManager.Instance.GetResourceAmount("Wood")}";
        waterText.text = $"Water: {ResourceManager.Instance.GetResourceAmount("Water")}";
        foodText.text = $"Food: {ResourceManager.Instance.GetResourceAmount("Food")}";
        metalText.text = $"Metal: {ResourceManager.Instance.GetResourceAmount("Metal")}";

        dayCountdownText.text = $"Days Remaining Until Winter: {TimeManager.Instance.daysRemaining}";
    }
}