using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class WorkerNavmesh : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Vector3 destination;
    private bool isMoving;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void MoveToDestination(Vector3 dest)
    {
        destination = dest;
        navMeshAgent.SetDestination(dest);
        isMoving = true;
    }

    private void Update()
    {
        if (isMoving && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending)
        {
            isMoving = false;
            UnitSelectionManager.Instance.OnUnitReachedDestination(this);
        }
    }
}
