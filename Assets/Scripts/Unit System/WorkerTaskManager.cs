using System.Collections;
using UnityEngine;

public class WorkerTaskManager : MonoBehaviour
{
    private WorkerNavmesh workerNavmesh;
    private UnitSelectionManager unitSelectionManager;
    private Transform resourceTarget;
    private Transform dropOffPoint;

    public enum WorkerState { Idle, MovingToResource, Gathering, ReturningToDropoff }
    public WorkerState currentState;

    public float gatherDuration = 10f;
    public int maxCarryAmount = 50;
    private int currentResources = 0;

    [HideInInspector] public bool isInterrupted = false;

    private void Awake()
    {
        workerNavmesh = GetComponent<WorkerNavmesh>();
        unitSelectionManager = GameObject.FindGameObjectWithTag("UnitSelectionManager").GetComponent<UnitSelectionManager>();
        currentState = WorkerState.Idle;        
    }

    private void Update()
    {
        if (isInterrupted)
        {
            return;
        }

        switch (currentState)
        {
            case WorkerState.MovingToResource:
                workerNavmesh.MoveToDestination(resourceTarget.position);
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

    public void StartGathering(Transform resource, Transform dropOff)
    {
        resourceTarget = resource;
        dropOffPoint = dropOff;              

        currentState = WorkerState.MovingToResource;
    }

    private IEnumerator GatherResources()
    {
        currentState = WorkerState.Gathering;        
        yield return new WaitForSeconds(gatherDuration);
        currentResources = maxCarryAmount;
        currentState = WorkerState.ReturningToDropoff;
    }

    private void DepositResources()
    {
        ResourceManager.Instance.AddResource("Wood", currentResources);
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
}