using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int productionRate;

    public int productionType;

    public HealthBar healthBar;

    public int level;

    public void Start()
    {
        healthBar = transform.GetChild(0).GetComponent<HealthBar>();

        HealthSystem healthSystem = new HealthSystem(200);

        healthBar.Setup(healthSystem);

        productionRate = 0;

        level = 1;
    }
    public void Damage(float damage)
    {
        healthBar.Damage(damage);
    }

    public void Heal(float heal)
    {
        healthBar.Heal(heal);
    }

    public void Update()
    {
        if (healthBar.GetHealthPercent() == 0) 
        {
            Destroy(transform.gameObject);
        }
    }
}
