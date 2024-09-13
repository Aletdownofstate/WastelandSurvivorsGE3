using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField] private float previewYOffset = 0.06f;
    [SerializeField] private GameObject cellIndicator;
    [SerializeField] private Material previewMaterialPrefab;    
    
    private Material previewMaterialInstance;
    private GameObject previewObject;

    private Renderer cellIndicatorRenderer;

    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefab);
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartShowingPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        PreparePreview(previewObject);
        PrepareCursor(size);
        cellIndicator.SetActive(true);
    }

    private void PrepareCursor(Vector2Int size)
    {
        if (size.x > 0 && size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
            cellIndicatorRenderer.material.mainTextureScale = size;
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
        cellIndicator.SetActive(false);
        Destroy(previewObject);
    }

    public void UpdatePosition(Vector3 position, bool validity)
    {
        bool isOverlapping = IsOverlappingWithOtherObjects(position);

        validity = validity && !isOverlapping;
        MovePreview(position);
        MoveCursor(position);
        ApplyFeedback(validity);
    }

    public bool IsOverlappingWithOtherObjects(Vector3 position)
    {
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

    private void ApplyFeedback(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        cellIndicatorRenderer.material.color = c;
        c.a = 0.5f;
        previewMaterialInstance.color = c;
    }

    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position;
    }

    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
    }
}