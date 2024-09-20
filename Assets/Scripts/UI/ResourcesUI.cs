using UnityEngine;
using TMPro;

public class ResourcesUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI woodText, waterText, foodText, metalText;

    void Update()
    {
        woodText.text = ($"Wood: {ResourceManager.Instance.GetResourceAmount("Wood")}");
        waterText.text = ($"Water: {ResourceManager.Instance.GetResourceAmount("Water")}");
        foodText.text = ($"Food: {ResourceManager.Instance.GetResourceAmount("Food")}");
        metalText.text = ($"Metal: {ResourceManager.Instance.GetResourceAmount("Metal")}");
    }
}