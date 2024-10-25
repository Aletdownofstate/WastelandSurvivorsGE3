using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkingState : StateMachineBehaviour
{
    float timer;
    NavMeshAgent agent;
    Vector3 wanderDestination;

    public float wanderRadius = 10f; // How far the zombie can wander
    public float wanderTime = 5f;    // Time spent wandering in one direction

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Initialize the NavMeshAgent on the zombie
        agent = animator.GetComponent<NavMeshAgent>();
        if (agent == null) return;

        // Start by finding a new random destination
        SetNewWanderDestination(animator.transform.position);
        
        timer = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;

        // If zombie has reached its current destination, find a new one
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            SetNewWanderDestination(animator.transform.position);
            timer = 0;
        }

        // If zombie has been walking for a certain time, it could return to idle or continue
        if (timer > wanderTime)
        {
            animator.SetBool("IsWalking", false);  // Return to Idle state after wandering for some time
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Stop the zombie from moving when it exits walking state
        if (agent != null)
        {
            agent.SetDestination(animator.transform.position);
        }
    }

    // Function to choose a new random destination within a given radius
    void SetNewWanderDestination(Vector3 origin)
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += origin;
        NavMeshHit hit;

        // Find a valid point on the NavMesh within the random direction
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1))
        {
            wanderDestination = hit.position;
            agent.SetDestination(wanderDestination);
        }
    }
}
