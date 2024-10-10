using UnityEngine;
using UnityEngine.AI;

public class WorkerNavmesh : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Vector3 destination;
    private bool isMoving;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        navMeshAgent.avoidancePriority = Random.Range(0, 100);
    }

    public void MoveToDestination(Vector3 dest)
    {
        destination = dest;
        navMeshAgent.SetDestination(dest);
        isMoving = true;
    }

    public bool HasReachedDestination()
    {
        return isMoving && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending;
    }
}