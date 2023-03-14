using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private HealthSystem healthSystem;
    public void Setup(HealthSystem healthSystem)
    { 
        this.healthSystem = healthSystem;

        healthSystem.OnHealthChange += HealthSystem_OnHealthChange1; ;
    }

    private void HealthSystem_OnHealthChange1(object sender, System.EventArgs e)
    {
        transform.Find("Bar").localScale = new Vector3(healthSystem.GetHealthPercent(), 1);
    }

    public void Heal(float healAmount)
    {
        healthSystem.Heal(healAmount);
    }

    public void Damage(float damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }

    public float GetHealthPercent()
    {
        return healthSystem.GetHealthPercent();
    }
}
