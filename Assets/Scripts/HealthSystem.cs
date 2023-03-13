using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class HealthSystem
{
    private int health;

    private int maxHealth;

    public event EventHandler OnHealthChange;

    public HealthSystem(int maxHealth)
    {
        this.maxHealth = maxHealth;
        health = maxHealth;
    }

    public int GetHealth()
    {
        return health;
    }

    public void Damage(int damageAmount)
    {
        if (health - damageAmount >= 0)
        {
            health -= damageAmount;
        }
        else
        {
            health = 0;
        }

        if (OnHealthChange != null)
        {
            OnHealthChange(this, EventArgs.Empty);
        }
    }

    public float GetHealthPerent()
    {
        return (float)health / maxHealth;
    }

    public void Heal(int healAmount)
    {
        if (health + healAmount <= maxHealth)
        {
            health += healAmount;
        }
        else
        {
            health = maxHealth;
        }
        if (OnHealthChange != null)
        {
            OnHealthChange(this, EventArgs.Empty);
        }
    }
}
