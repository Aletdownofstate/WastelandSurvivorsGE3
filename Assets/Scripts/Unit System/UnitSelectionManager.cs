using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance { get; set; }

    public List<GameObject> allUnitsList = new List<GameObject>();
    public List<GameObject> unitsSelected = new List<GameObject>();

    [HideInInspector] public GameObject currentGroundMarker;
    
    public LayerMask clickable, groundLayerCast, woodLayer, metalLayer, foodLayer, waterLayer;
    public GameObject groundMarker;
    public Transform dropOffPoint;

    private Camera cam;
    private int unitsMoving;

    [SerializeField] private float unitSpreadRadius = 2.0f;

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
    }

    private void Start()
    {
        cam = Camera.main;

        foreach (GameObject unit in allUnitsList)
        {
            Transform spriteChild = unit.transform.GetChild(1);

            if (spriteChild != null)
            {
                spriteChild.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        HandleUnitSelection();
        HandleRightClickMovement();        
    }

    private void HandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    MultipleSelect(hit.collider.gameObject);
                }
                else
                {
                    SelectByClicking(hit.collider.gameObject);
                }
            }
            else
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    DeselectAll();
                }
            }
        }
    }

    private void HandleRightClickMovement()
    {
        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject() && unitsSelected.Count != 0)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            // Move to a position

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerCast))
            {
                Vector3 primaryDestination = hit.point;

                if (currentGroundMarker != null)
                {
                    Destroy(currentGroundMarker);
                }

                foreach (var unit in unitsSelected)
                {
                    WorkerTaskManager taskManager = unit.GetComponent<WorkerTaskManager>();

                    if (taskManager.currentWorkerState != WorkerTaskManager.WorkerState.Idle)
                    {
                        taskManager.InterruptCurrentTask();
                    }
                }

                currentGroundMarker = Instantiate(groundMarker, primaryDestination, Quaternion.identity);
                unitsMoving = unitsSelected.Count;

                DistributeUnitsToDestination(primaryDestination);
            }

            // Start gathering

            string resourceType = null;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, woodLayer))
            {
                resourceType = "Wood";
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, metalLayer))
            {
                resourceType = "Metal";
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, foodLayer))
            {
                resourceType = "Food";
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, waterLayer))
            {
                resourceType = "Water";
            }

            if (resourceType != null)
            {
                foreach (var unit in unitsSelected)
                {
                    WorkerTaskManager taskManager = unit.GetComponent<WorkerTaskManager>();

                    if (taskManager != null)
                    {
                        taskManager.StartGathering(hit.transform, dropOffPoint, resourceType);
                    }
                }
            }
        }
    }

    public void DistributeUnitsToDestination(Vector3 primaryDestination)
    {
        int unitCount = unitsSelected.Count;

        for (int i = 0; i < unitCount; i++)
        {
            WorkerNavmesh workerNavmesh = unitsSelected[i].GetComponent<WorkerNavmesh>();

            if (workerNavmesh != null)
            {
                Vector3 destination = primaryDestination;

                if (i > 0)
                {
                    destination = GetOffsetPosition(primaryDestination, i, unitCount);
                }

                workerNavmesh.MoveToDestination(destination);
            }
        }
    }

    private Vector3 GetOffsetPosition(Vector3 center, int unitIndex, int totalUnits)
    {
        float angle = unitIndex * (360f / (totalUnits - 1));
        float radians = angle * Mathf.Deg2Rad;

        float offsetX = Mathf.Cos(radians) * unitSpreadRadius;
        float offsetZ = Mathf.Sin(radians) * unitSpreadRadius;

        return new Vector3(center.x + offsetX, center.y, center.z + offsetZ);
    }

    public void OnUnitReachedDestination(WorkerNavmesh workerNavmesh)
    {
        unitsMoving--;

        if (unitsMoving <= 0)
        {
            Destroy(currentGroundMarker);
        }
    }

    private void MultipleSelect(GameObject unit)
    {
        WorkerNavmesh workerNavmesh = unit.GetComponentInParent<WorkerNavmesh>();

        if (workerNavmesh != null)
        {
            GameObject rootUnit = workerNavmesh.gameObject;

            if (!unitsSelected.Contains(rootUnit))
            {
                unitsSelected.Add(rootUnit);
                EnableUnitMovement(rootUnit, true);
            }
            else
            {
                EnableUnitMovement(rootUnit, false);
                unitsSelected.Remove(rootUnit);
            }
        }
        else
        {
            Debug.LogWarning("Attempted to select a non-unit object: " + unit.name);
        }
    }

    private void SelectByClicking(GameObject unit)
    {
        WorkerNavmesh workerNavmesh = unit.GetComponentInParent<WorkerNavmesh>();

        if (workerNavmesh != null)
        {
            DeselectAll();
            unitsSelected.Add(workerNavmesh.gameObject);
            EnableUnitMovement(workerNavmesh.gameObject, true);
        }
        else
        {
            Debug.LogWarning("Attempted to select a non-unit object: " + unit.name);
        }
    }

    public void DragSelect(GameObject unit)
    {
        if (!unitsSelected.Contains(unit))
        {
            unitsSelected.Add(unit);
            EnableUnitMovement(unit, true);
        }
    }

    private void EnableUnitMovement(GameObject unit, bool canMove)
    {
        WorkerNavmesh workerNavmesh = unit.GetComponent<WorkerNavmesh>();

        if (workerNavmesh != null)
        {
            workerNavmesh.enabled = canMove;

            Transform spriteChild = unit.transform.GetChild(1);
            if (spriteChild != null)
            {
                spriteChild.gameObject.SetActive(canMove);
            }
        }
    }

    public void DeselectAll()
    {
        foreach (var unit in unitsSelected)
        {
            EnableUnitMovement(unit, false);
        }
        unitsSelected.Clear();
    }
}