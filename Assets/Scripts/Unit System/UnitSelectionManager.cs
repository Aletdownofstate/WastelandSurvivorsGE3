using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance { get; set; }

    public List<GameObject> allUnitsList = new List<GameObject>();
    public List<GameObject> unitsSelected = new List<GameObject>();

    [HideInInspector] public GameObject currentGroundMarker;

    [SerializeField] private TextMeshProUGUI unitNameText, unitPersonalityText, unitSkillText, currentTaskText, PopulationText, MoraleText, TempText, daysRemaining;

    [SerializeField] private AudioSource maleYes1, maleYes2, maleYes3, maleYes4;
    [SerializeField] private AudioSource femaleYes1, femaleYes2, femaleYes3, femaleYes4;
    [SerializeField] private AudioSource maleAffirm1;
    [SerializeField] private AudioSource femaleAffirm1, femaleAffirm2, femaleAffirm3;
    
    public LayerMask clickable, groundLayerCast, woodLayer, metalLayer, foodLayer, waterLayer;
    public GameObject groundMarker;

    private Transform dropOffPoint;
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

        // Update UI with worker details

        if (unitsSelected.Count > 0)
        {
            EnableWorkerDetails();

            foreach (var unit in unitsSelected)
            {
                WorkerTaskManager workerTaskManager = unit.GetComponent<WorkerTaskManager>();

                string currentWorkerState = null;
                string currentWorkerSkill = null;

                switch (workerTaskManager.currentWorkerState)
                {
                    case WorkerTaskManager.WorkerState.Idle:
                        currentWorkerState = "Idle";
                        break;
                    case WorkerTaskManager.WorkerState.MovingToResource:
                        currentWorkerState = "Moving to resource";
                        break;
                    case WorkerTaskManager.WorkerState.Gathering:
                        currentWorkerState = "Gathering";
                        break;
                    case WorkerTaskManager.WorkerState.ReturningToDropoff:
                        currentWorkerState = "Depositing resources";
                        break;
                }

                if (workerTaskManager.workerSkill == SkillsManager.Skill.NaturalLeader)
                {
                    currentWorkerSkill = "Natural Leader";
                }
                else
                {
                    currentWorkerSkill = workerTaskManager.workerSkill.ToString();
                }

                unitNameText.text = ($"Name: {workerTaskManager.workerName}");
                unitSkillText.text = ($"Skill: {currentWorkerSkill}");
                unitPersonalityText.text = ($"Personality type: {workerTaskManager.personalityType}");
                currentTaskText.text = ($"Current task: {currentWorkerState}");
            }
        }
        else if (unitsSelected.Count == 0)
        {
            DisableWorkerDetails();
        }
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

                    AffirmtiveSound(unit);

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
                unitsSelected.Remove(unit);
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
            OnUnitSelectSound(unit);
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

    private void DisableWorkerDetails()
    {
        PopulationText.enabled = true;
        MoraleText.enabled = true;
        TempText.enabled = true;        

        if (GameManager.Instance.currentGameState == GameManager.GameState.ChapterFive)
        {
            daysRemaining.enabled = true;
        }

        unitNameText.enabled = false;
        unitSkillText.enabled = false;
        unitPersonalityText.enabled = false;
        currentTaskText.enabled = false;
    }

    private void EnableWorkerDetails()
    {
        PopulationText.enabled = false;
        MoraleText.enabled = false;
        TempText.enabled = false;
        daysRemaining.enabled = false;

        unitNameText.enabled = true;
        unitSkillText.enabled = true;
        unitPersonalityText.enabled = true;
        currentTaskText.enabled = true;
    }

    private void OnUnitSelectSound(GameObject unit)
    {
        AudioSource maleYes = null;
        AudioSource femaleYes = null;

        int randomMaleYes = UnityEngine.Random.Range(0, 4);
        int randomFemaleYes = UnityEngine.Random.Range(0,4);

        float pitch = UnityEngine.Random.Range(0.9f, 1.1f);

        switch (randomMaleYes)
        {
            case (0):
                maleYes = maleYes1;
                break;
            case (1):
                maleYes = maleYes2;
                break;
            case (2):
                maleYes = maleYes3;
                break;
            case (3):
                maleYes = maleYes4;
                break;
        }

        switch (randomFemaleYes)
        {
            case (0):
                femaleYes = femaleYes1;
                break;
            case (1):
                femaleYes = femaleYes2;
                break;
            case (2):
                femaleYes = femaleYes3;
                break;
            case (3):
                femaleYes = femaleYes4;
                break;
        }

        if (unit.name.Contains("Female"))
        {
            femaleYes.pitch = pitch;
            femaleYes.volume = 1.2f;
            femaleYes.Play();
            Debug.Log("Playing female 'yes' sound");
        }
        else if (unit.name.Contains("Male"))
        {
            maleYes.pitch = pitch;
            maleYes.volume = 0.6f;
            maleYes.Play();
            Debug.Log("Playing male 'Yes' sound");
        }
    }

    private void AffirmtiveSound(GameObject unit)
    {
        AudioSource maleAffirmative = null;
        AudioSource femaleAffirmative = null;

        int randomMale = UnityEngine.Random.Range(0, 2);
        int randomFemale = UnityEngine.Random.Range(0, 2);

        float pitch = UnityEngine.Random.Range(0.9f, 1.1f);

        switch (randomMale)
        {
            case (0):
                maleAffirmative = femaleAffirm1;
                maleAffirmative.pitch = 0.85f;
                maleAffirmative.volume = 0.6f;
                break;
            case (1):
                maleAffirmative = maleAffirm1;
                maleAffirmative.pitch = pitch;
                maleAffirmative.volume = 0.4f;
                break;
            case (2):
                maleAffirmative = femaleAffirm2;
                maleAffirmative.pitch = 0.85f;
                maleAffirmative.volume = 0.6f;
                break;
        }

        switch (randomFemale)
        {
            case (0):
                femaleAffirmative = femaleAffirm1;
                break;
            case (1):
                femaleAffirmative = femaleAffirm2;
                break;
            case (2):
                femaleAffirmative = femaleAffirm3;
                break;
        }

        if (unit.name.Contains("Female"))
        {
            femaleAffirmative.pitch = pitch;
            femaleAffirmative.volume = 1.2f;
            femaleAffirmative.Play();
            Debug.Log("Playing female 'affirmative' sound");
        }
        else if (unit.name.Contains("Male"))
        {            
            maleAffirmative.Play();
            Debug.Log("Playing male 'affirmative' sound");
        }
    }
}