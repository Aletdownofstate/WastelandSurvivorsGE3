using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth : MonoBehaviour
{
    public int health;
    private int maxHealth;
    private int minHealth;
    
    private void Start()
    {
        minHealth = 0;
        maxHealth = 100;
        health = maxHealth;
    }

    private void Update()
    {
        health = Mathf.Clamp(health, minHealth, maxHealth);
    }

    public void DecreaseHealth(int dmgAmount)
    {
        health -= dmgAmount;
    }
}