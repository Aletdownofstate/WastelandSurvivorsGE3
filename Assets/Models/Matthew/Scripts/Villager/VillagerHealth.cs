using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public bool isDead = false; // Boolean to track if the villager is dead

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        // Add death logic here (e.g., playing death animation, disabling movement)
        Debug.Log("Villager is dead!");
    }
}
