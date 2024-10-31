using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class BuildMenuHover : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buildingCost;
    [SerializeField] private ObjectsDatabaseSO database;

    private string buttonName;

    private void Start()
    {
        buildingCost.enabled = false;
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            var raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);

            foreach (var result in raycastResults)
            {
                if (result.gameObject.GetComponent<Button>() != null)
                {
                    string hoveredButtonName = result.gameObject.name;

                    if (hoveredButtonName != buttonName)
                    {
                        buttonName = hoveredButtonName;
                        OnHover();
                    }
                    break;
                }
            }
        }
        else
        {
            buttonName = null;
            buildingCost.enabled = false;
        }
    }

    public void OnHover()
    {
        switch (buttonName)
        {
            case "Torch Button":
                buildingCost.text = $"Building cost:\nWood: {database.objectsData[0].WoodRequired}";
                break;
            case "Tent Button":
                buildingCost.text = $"Building cost:\nWood: {database.objectsData[2].WoodRequired}";
                break;
            case "Warehouse Button":
                buildingCost.text = $"Building cost:\nWood: {database.objectsData[1].WoodRequired}\nMetal: {database.objectsData[1].MetalRequired}";
                break;
            case "Water Tank Button":
                buildingCost.text = $"Building cost:\nWood: {database.objectsData[3].WoodRequired}\nMetal: {database.objectsData[3].MetalRequired}";
                break;
            case "Medical Tent Button":
                buildingCost.text = $"Building cost:\nWood: {database.objectsData[4].WoodRequired}\nMetal: {database.objectsData[4].MetalRequired}";
                break;            
            default:
                buildingCost.enabled = false;
                return;
        }
        buildingCost.enabled = true;
    }

    public void OnHoverExit()
    {
        buildingCost.enabled = false;
    }
}
