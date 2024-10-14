using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieBehavior : MonoBehaviour
{
    private NavMeshAgent agent;          // Reference to the NavMeshAgent component
    private Animator animator;           // Reference to the Animator component

    public GameObject villager;          // Reference to the villager (or player)
    public float sightRange = 10f;       // Zombie's range to detect the player
    public float attackRange = 2f;       // Zombie's attack range
    public float wanderRadius = 10f;     // How far the zombie can wander
    public float wanderTimer = 5f;       // Time between wandering points

    private Vector3 destination;         // Destination for wandering
    private float timer;                 // Timer to control wandering

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();              // Get NavMeshAgent
        animator = GetComponentInChildren<Animator>();     // Get Animator
        timer = wanderTimer;                               // Initialize timer
        SearchForNewDestination();                         // Find a destination to start wandering
    }

    void Update()
    {
        float distanceToVillager = Vector3.Distance(transform.position, villager.transform.position);
        Debug.Log("Distance to villager: " + distanceToVillager);

        if (distanceToVillager <= attackRange) // If in attack range, attack
        {
            Attack();
        }
        else if (distanceToVillager <= sightRange) // If in sight range, chase the villager
        {
            Chase();
        }
        else // If outside of sight range, wander
        {
            Wander();
        }

        // Check if the zombie is moving and set the walking animation
        if (agent.velocity.magnitude > 0.1f) // Check if the agent is moving
        {
            animator.SetBool("IsWalking", true); // Set walking animation
        }
        else
        {
            animator.SetBool("IsWalking", false); // Stop walking animation
        }
    }

    void Wander()
    {
        // If the timer exceeds the wandering time, find a new random destination
        if (timer >= wanderTimer)
        {
            SearchForNewDestination();
            timer = 0; // Reset the timer
        }
        else
        {
            timer += Time.deltaTime; // Increment timer
        }

        agent.SetDestination(destination); // Move to the destination
    }

    void Chase()
    {
        // Set the agent's destination to the villager's position
        agent.SetDestination(villager.transform.position); 
    }

    void Attack()
    {
        agent.SetDestination(transform.position); // Stop moving
        animator.SetBool("IsWalking", false);     // Stop walking animation
        animator.SetBool("IsAttacking", true);    // Trigger attack animation
    }

    void SearchForNewDestination()
    {
        // Find a new random destination within the wander radius
        float randomZ = Random.Range(-wanderRadius, wanderRadius);
        float randomX = Random.Range(-wanderRadius, wanderRadius);
        destination = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // Make sure the destination is valid (on the NavMesh)
        NavMeshHit hit;
        if (NavMesh.SamplePosition(destination, out hit, wanderRadius, NavMesh.AllAreas))
        {
            destination = hit.position; // Set valid destination on NavMesh
        }
    }
}
