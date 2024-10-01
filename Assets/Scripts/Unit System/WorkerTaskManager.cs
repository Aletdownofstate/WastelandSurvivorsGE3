using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class WorkerTaskManager : MonoBehaviour
{
    private WorkerNavmesh workerNavmesh;
    private UnitSelectionManager unitSelectionManager;
    private Transform resourceTarget;
    private Transform dropOffPoint;

    [SerializeField] private AudioSource woodSound, metalSound;

    public enum WorkerState { Idle, MovingToResource, Gathering, ReturningToDropoff }
    public WorkerState currentState;

    [SerializeField] private PersonalityManager.PersonalityType personalityType;
    private float gatheringBonus;

    public float gatherDuration = 10.0f;
    public int maxCarryAmount = 50;
    private int currentResources = 0;
    private string resourceType;    

    [HideInInspector] public bool isInterrupted = false;

    private void Awake()
    {
        workerNavmesh = GetComponent<WorkerNavmesh>();

        unitSelectionManager = GameObject.FindGameObjectWithTag("UnitSelectionManager").GetComponent<UnitSelectionManager>();
        currentState = WorkerState.Idle;        
    }

    private void Start()
    {
        personalityType = PersonalityManager.Instance.ChoosePersonality();        

        switch (personalityType)
        {
            case PersonalityManager.PersonalityType.HardWorking:
                gatheringBonus = -2.0f;
                break;
            case PersonalityManager.PersonalityType.Lazy:
                gatheringBonus = 2.0f;
                break;
            case PersonalityManager.PersonalityType.Optimist:
                MoraleManager.Instance.IncreaseMorale();
                break;
            case PersonalityManager.PersonalityType.Pessmist:
                MoraleManager.Instance.DecreaseMorale();
                break;
            case PersonalityManager.PersonalityType.Strong:
                maxCarryAmount += 10;
                break;
            case PersonalityManager.PersonalityType.Weak:
                maxCarryAmount -= 10;
                break;
        }
    }

    private void Update()
    {
        if (isInterrupted)
        {
            return;
        }

        switch (MoraleManager.Instance.currentState)
        {
            case MoraleManager.MoraleState.High:
                gatherDuration = 8.0f + gatheringBonus;
                break;
            case MoraleManager.MoraleState.Neutral:
                gatherDuration = 10.0f + gatheringBonus;
                break;
            case MoraleManager.MoraleState.Low:
                gatherDuration = 12.0f + gatheringBonus;
                break;
        }        

        switch (currentState)
        {
            case WorkerState.MovingToResource:
                Vector3 closestPoint = GetClosestPointOnResource(resourceTarget);
                workerNavmesh.MoveToDestination(closestPoint);
                if (workerNavmesh.HasReachedDestination())
                {
                    Destroy(unitSelectionManager.currentGroundMarker);
                    StartCoroutine(GatherResources());
                }
                break;

            case WorkerState.ReturningToDropoff:
                workerNavmesh.MoveToDestination(dropOffPoint.position);
                if (workerNavmesh.HasReachedDestination())
                {
                    DepositResources();
                }
                break;

            case WorkerState.Idle:
                break;
        }
    }

    public void StartGathering(Transform resourceTransform, Transform dropOff, string resource)
    {
        GameObject[] dropOffPoints = GameObject.FindGameObjectsWithTag("DropOffPoint");
        GameObject closestDropOff = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject dropPoint in dropOffPoints)
        {
            float distance = Vector3.Distance(resourceTransform.transform.position, dropPoint.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestDropOff = dropPoint;
            }
        }

        resourceTarget = resourceTransform;
        dropOffPoint = closestDropOff.transform;
        resourceType = resource;
        currentState = WorkerState.MovingToResource;
    }

    private IEnumerator GatherResources()
    {
        currentState = WorkerState.Gathering;

        AudioSource gatheringSound = null;        

        switch (resourceTarget.tag)
        {
            case "Wood":
                gatheringSound = woodSound;
                break;

            case "Metal":
                gatheringSound = metalSound;
                gatheringSound.volume = 0.4f;
                break;
        }        

        if (gatheringSound != null)
        {
            StartCoroutine(GatheringSoundDelay(gatheringSound));
        }

        yield return new WaitForSeconds(gatherDuration);
        gatheringSound.Stop();

        currentResources = maxCarryAmount;
        currentState = WorkerState.ReturningToDropoff;
    }

    private void DepositResources()
    {
        ResourceManager.Instance.AddResource(resourceType, currentResources);
        currentResources = 0;
        currentState = WorkerState.MovingToResource;
    }

    public void InterruptCurrentTask()
    {
        isInterrupted = true;
        StopAllCoroutines();
        currentState = WorkerState.Idle;
        isInterrupted = false;
    }

    private IEnumerator GatheringSoundDelay(AudioSource gatheringSound)
    {
        float delay = Random.Range(0.75f, 1.5f);
        float pitch = Random.Range(0.95f, 1.05f);

        gatheringSound.pitch = pitch;
        gatheringSound.Play();

        yield return new WaitForSeconds(delay);

        if (currentState == WorkerState.Gathering)
        {
            StartCoroutine(GatheringSoundDelay(gatheringSound));
        }
    }

    private Vector3 GetClosestPointOnResource(Transform targetTransform)
    {
        Collider targetCollider = targetTransform.GetComponent<Collider>();

        if (targetCollider != null)
        {
            return targetCollider.ClosestPoint(transform.position);
        }
        else
        {
            return targetTransform.position;
        }
    }
}