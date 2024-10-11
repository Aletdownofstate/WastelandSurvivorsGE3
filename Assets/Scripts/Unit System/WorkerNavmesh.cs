using UnityEngine;
using UnityEngine.AI;

public class WorkerNavmesh : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Vector3 destination;
    public bool isMoving;

    [SerializeField] private Animator anim;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        navMeshAgent.avoidancePriority = Random.Range(0, 100);
    }

    private void Update()
    {
        if (HasReachedDestination())
        {
            isMoving = false;
        }
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