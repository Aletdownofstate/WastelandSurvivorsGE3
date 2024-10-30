using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject gridVisualisation;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;
    [SerializeField] private ObjectsDatabaseSO database;
    [SerializeField] private UnitSelectionBox unitSelectionBox;
    [SerializeField] private PreviewSystem preview;
    [SerializeField] private AudioSource buildSound, deniedSound;    

    private GridData floorData, furnitureData;
    private List<GameObject> placedGameObjects = new();

    private int selectedObjectIndex = -1;

    private Vector3 lastDetectedPosition = Vector3Int.zero;

    private void Start()
    {
        preview = FindAnyObjectByType<PreviewSystem>();

        if (preview == null)
        {
            Debug.LogError("PreviewSystem not found");
        }

        StopPlacement();
        floorData = new();
        furnitureData = new();
    }

    private void Update()
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        
        if (selectedObjectIndex < 0)
        {
            return;
        }

        if (lastDetectedPosition != gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

            gridVisualisation.transform.position = grid.WorldToCell(mousePosition);

            if (preview == null)
            {
                return;
            }

            if (!preview.gameObject.activeInHierarchy)
            {
                return;
            }

            preview.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
            lastDetectedPosition = gridPosition;            
        }
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);

        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }

        var selectedObjectData = database.objectsData[selectedObjectIndex];

        bool missingResources = false;
        bool missingWood = false;
        bool missingMetal = false;
        bool missingFood = false;
        bool missingWater = false;

        if (ResourceManager.Instance.GetResourceAmount("Wood") < selectedObjectData.WoodRequired)
        {
            missingWood = true;
            missingResources = true;
        }
        if (ResourceManager.Instance.GetResourceAmount("Metal") < selectedObjectData.MetalRequired)
        {
            missingMetal = true;
            missingResources = true;
        }
        if (ResourceManager.Instance.GetResourceAmount("Food") < selectedObjectData.FoodRequired)
        {
            missingFood = true;
            missingResources = true;
        }
        if (ResourceManager.Instance.GetResourceAmount("Water") < selectedObjectData.WaterRequired)
        {
            missingWater = true;
            missingResources = true;
        }

        if (missingResources)
        {
            Debug.Log("Not enough resources to build");
            deniedSound.Play();

            if (missingWood)
            {
                Debug.Log("Not enough wood to proceed");
            }
            if (missingMetal)
            {
                Debug.Log("Not enough metal to proceed");
            }
            if (missingFood)
            {
                Debug.Log("Not enough food to proceed");
            }
            if (missingWater)
            {
                Debug.Log("Not enough water to proceed");
            }

            missingResources = false;
            return;
        }

        gridVisualisation.SetActive(true);
        preview.StartShowingPreview(database.objectsData[selectedObjectIndex].Prefab, database.objectsData[selectedObjectIndex].Size);
        inputManager.onClicked += PlaceStructure;
        inputManager.onExit += StopPlacement;

        unitSelectionBox.enabled = false;
    }

    private void PlaceStructure()
    {
        var selectedObjectData = database.objectsData[selectedObjectIndex];

        if (inputManager.IsPointedOverUI())
        {
            return;
        }

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        if (!placementValidity || preview.IsOverlappingWithOtherObjects(grid.CellToWorld(gridPosition)))
        {
            StopPlacement();
            return;
        }

        if (ResourceManager.Instance.GetResourceAmount("Wood") < selectedObjectData.WoodRequired ||
            ResourceManager.Instance.GetResourceAmount("Metal") < selectedObjectData.MetalRequired ||
            ResourceManager.Instance.GetResourceAmount("Food") < selectedObjectData.FoodRequired ||
            ResourceManager.Instance.GetResourceAmount("Water") < selectedObjectData.WaterRequired)
        {            
            Debug.Log("Not enough resources to proceed");
            StopPlacement();
            return;
        }
        else
        {
            ResourceManager.Instance.SubtractResource("Wood", selectedObjectData.WoodRequired);
            ResourceManager.Instance.SubtractResource("Metal", selectedObjectData.MetalRequired);
            ResourceManager.Instance.SubtractResource("Food", selectedObjectData.FoodRequired);
            ResourceManager.Instance.SubtractResource("Water", selectedObjectData.WaterRequired);

            GameObject newObject = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
            newObject.transform.position = grid.CellToWorld(gridPosition);

            NavMeshObstacle[] obstacles = newObject.GetComponentsInChildren<NavMeshObstacle>();
            foreach (NavMeshObstacle obstacle in obstacles)
            {
                obstacle.enabled = true;
            }

            // Add building specific stuff here
            // Checks to see if building is actually placed or still being previewed

            TentObject tentObject = newObject.GetComponent<TentObject>();
            if (tentObject != null)
            {
                tentObject.OnPlace();
            }

            WarehouseObject warehouseObject = newObject.GetComponent<WarehouseObject>();
            if (warehouseObject != null)
            {
                warehouseObject.OnPlace();
            }

            WaterTowerObject waterTowerObject = newObject.GetComponent<WaterTowerObject>();
            if (waterTowerObject != null)
            {
                waterTowerObject.OnPlace();
            }

            MedicalTentObject medicalTentObject = newObject.GetComponent<MedicalTentObject>();
            if (medicalTentObject != null)
            {
                medicalTentObject.OnPlace();
            }

            //
            
            buildSound.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            buildSound.Play();

            placedGameObjects.Add(newObject);
            GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
            selectedData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size, database.objectsData[selectedObjectIndex].ID, placedGameObjects.Count - 1);

            StopPlacement();
        }
        preview.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        return selectedData.CanPlaceObjectsAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        gridVisualisation.SetActive(false);
        preview.StopShowingPreview();
        inputManager.onClicked -= PlaceStructure;
        inputManager.onExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;

        unitSelectionBox.enabled = true;
    }
}