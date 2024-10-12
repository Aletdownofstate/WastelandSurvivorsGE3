using UnityEngine;

public class TestWalkingAnimation : MonoBehaviour
{
    private Animator animator; // Reference to the Animator component

    void Start()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();

        // Set the walking animation parameter to true
        animator.SetBool("IsWalking", true); // Ensure "IsWalking" matches your Animator parameter name

        // Debug log to confirm the animator is set
        Debug.Log("Walking animation set: " + animator.GetBool("IsWalking"));
    }

    void Update()
    {
        // You can add any additional logic here if needed
    }
}


/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieBehavior : MonoBehaviour
{
    private NavMeshAgent agent;             // Reference to the NavMeshAgent component
    private Animator animator;               // Reference to the Animator component

    public GameObject villager;              // Reference to the villager (player)
    public float sightRange = 10f;           // Distance at which the zombie can see the player
    public float attackRange = 2f;           // Distance at which the zombie can attack the player
    public float wanderRadius = 10f;         // How far the zombie can wander
    public float wanderTimer = 5f;           // Time between wandering to new locations

    private Vector3 destination;              // Current destination for wandering
    private float timer;                      // Timer for wandering

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();                // Get the NavMeshAgent component
        animator = GetComponentInChildren<Animator>();       // Get the Animator from the child model
        timer = wanderTimer;                                 // Initialize the timer
        SearchForNewDestination();                           // Find the first destination
    }

    void Update()
    {
        float distanceToVillager = Vector3.Distance(transform.position, villager.transform.position);

        // Check if the villager is in sight or attack range
        if (distanceToVillager <= attackRange)
        {
            Attack();
        }
        else if (distanceToVillager <= sightRange)
        {
            Chase();
        }
        else
        {
            Wander();
        }
    }

    void Wander()
    {
        // If the timer is greater than the wandering timer, find a new destination
        if (timer >= wanderTimer)
        {
            SearchForNewDestination();
            timer = 0; // Reset the timer
        }
        else
        {
            timer += Time.deltaTime; // Increment the timer
        }

        // Move to the new destination
        agent.SetDestination(destination);
        animator.SetFloat("Speed", agent.velocity.magnitude); // Set walking animation based on speed
    }

    void Chase()
    {
        // Move towards the villager
        agent.SetDestination(villager.transform.position);
        animator.SetFloat("Speed", agent.velocity.magnitude); // Set walking animation based on speed
    }

    void Attack()
    {
        agent.SetDestination(transform.position); // Stop moving
        animator.SetTrigger("Attack"); // Trigger the attack animation
        animator.SetFloat("Speed", 0); // Ensure walking animation is not playing
    }

    void SearchForNewDestination()
    {
        float z = Random.Range(-wanderRadius, wanderRadius);
        float x = Random.Range(-wanderRadius, wanderRadius);
        destination = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        // Check if the new position is valid (on the NavMesh)
        NavMeshHit hit;
        if (NavMesh.SamplePosition(destination, out hit, wanderRadius, NavMesh.AllAreas))
        {
            destination = hit.position; // Set the new destination to the valid position on the NavMesh
        }
    }
}*/
