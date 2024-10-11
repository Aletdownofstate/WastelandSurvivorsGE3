using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField] private GameObject cellIndicator;
    [SerializeField] private Material previewMaterialPrefab;

    [SerializeField] private float previewYOffset = 0.06f;
    [SerializeField] private float gridSize = 1.0f;
    [SerializeField] private float groundY = 0f;

    private Material previewMaterialInstance;
    private GameObject previewObject;
    private List<GameObject> cellIndicators = new List<GameObject>();

    private Vector2Int objectSize;

    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefab);
        cellIndicator.SetActive(false);
    }

    public void StartShowingPreview(GameObject prefab, Vector2Int size)
    {
        objectSize = size;

        previewObject = Instantiate(prefab);
        PreparePreview(previewObject);
        PrepareIndicators(size);
    }

    private void PrepareIndicators(Vector2Int size)
    {
        ClearIndicators();

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                GameObject indicator = Instantiate(cellIndicator);
                indicator.SetActive(true);
                indicator.transform.localScale = Vector3.one * gridSize;
                cellIndicators.Add(indicator);
            }
        }
    }

    private void PreparePreview(GameObject previewObject)
    {
        NavMeshObstacle[] obstacles = previewObject.GetComponentsInChildren<NavMeshObstacle>();
        foreach (NavMeshObstacle obstacle in obstacles)
        {
            obstacle.enabled = false;
        }

        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }
            renderer.materials = materials;
        }
    }

    public void StopShowingPreview()
    {
        if (previewObject != null)
        {
            Destroy(previewObject);
            previewObject = null;
        }

        ClearIndicators();
    }

    private void ClearIndicators()
    {
        foreach (var indicator in cellIndicators)
        {
            Destroy(indicator);
        }
        cellIndicators.Clear();
    }

    public void UpdatePosition(Vector3 position, bool validity)
    {
        if (previewObject == null)
        {
            return;
        }

        Vector3 snappedPosition = SnapToGrid(position);
        snappedPosition.y = groundY + previewYOffset;

        MovePreview(snappedPosition);
        MoveIndicators(snappedPosition);

        ApplyFeedback(validity && !IsOverlappingWithOtherObjects(snappedPosition));
    }

    private Vector3 SnapToGrid(Vector3 position)
    {
        float x = Mathf.Floor(position.x / gridSize) * gridSize;
        float z = Mathf.Floor(position.z / gridSize) * gridSize;
        return new Vector3(x, groundY, z);
    }

    private void MoveIndicators(Vector3 basePosition)
    {
        int index = 0;
        Vector3 startPosition = new Vector3(basePosition.x, groundY, basePosition.z);

        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                if (index < cellIndicators.Count)
                {
                    Vector3 indicatorPosition = startPosition + new Vector3(x * gridSize, 0, y * gridSize);
                    cellIndicators[index].transform.position = indicatorPosition;
                    index++;
                }
            }
        }
    }

    private void MovePreview(Vector3 position)
    {
        if (previewObject == null) return;

        position.y = groundY + previewYOffset;
        previewObject.transform.position = position;
    }

    private void ApplyFeedback(bool validity)
    {
        Color feedbackColor = validity ? Color.white : Color.red;
        foreach (var indicator in cellIndicators)
        {
            indicator.GetComponentInChildren<Renderer>().material.color = feedbackColor;
        }

        feedbackColor.a = 0.5f;
        previewMaterialInstance.color = feedbackColor;
    }

    public bool IsOverlappingWithOtherObjects(Vector3 position)
    {
        if (previewObject == null) return false;

        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        Bounds previewBounds = new Bounds(position, Vector3.zero);
        foreach (Renderer renderer in renderers)
        {
            previewBounds.Encapsulate(renderer.bounds);
        }

        Collider[] hitColliders = Physics.OverlapBox(previewBounds.center, previewBounds.extents / 2);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != previewObject)
            {
                return true;
            }
        }

        return false;
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, groundY, 0));

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 pointOnPlane = ray.GetPoint(distance);
            UpdatePosition(pointOnPlane, true);
        }
    }
}