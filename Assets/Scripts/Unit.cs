using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : Entity
{
    private GameObject selectedGameObject;

    private CircleCollider2D damageCollider;

    private List<Collider2D> entities;

    public List<Action> actions = new List<Action>();

    private int productionTime = 20;

    private int currentTick;

    private int targetTick;

    public int damage;

    private int attackRate = 10;

    private bool attacking = false;

    private void Awake()
    {
        selectedGameObject = transform.Find("Selected").gameObject;
        SetSelectedVisible(false);
    }

    public void Start()
    {
        healthBar = transform.GetChild(0).GetComponent<HealthBar>();

        HealthSystem healthSystem = new HealthSystem(100);

        healthBar.Setup(healthSystem);

        damageCollider = GetComponent<CircleCollider2D>();

        damage = 50;

        TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickEventArgs e)
        {
            if (Application.isPlaying && e.tick % 10 == 0)
            {
                currentTick = e.tick;
            }
        };
    }

    public void Damage(float damage)
    {
        healthBar.Damage(damage);
    }

    public void Heal(float heal)
    {
        healthBar.Heal(heal);
    }

    public int GetProductionTime()
    {
        return productionTime;  
    }

    public void SetSelectedVisible(bool visible)
    {
        selectedGameObject.SetActive(visible);
    }
    void FixedUpdate()
    {
        if (healthBar.GetHealthPercent() == 0)
        {
            Destroy(transform.gameObject);
        }

        if (actions.Count > 0)
        {
            if (actions.ElementAt(0).GetType() == typeof(Move))
            {
                if (rigidBody.position != (Vector2)((Move)actions.ElementAt(0)).GetMovement())
                {
                    rigidBody.transform.position = Vector2.MoveTowards(rigidBody.position, (Vector2)((Move)actions.ElementAt(0)).GetMovement(), moveSpeed);
                }
                else
                {
                    actions.Remove(actions.ElementAt(0));
                }
            }
            else if (actions.ElementAt(0).GetType() == typeof(Attack))
            {
                if (((Attack)actions.ElementAt(0)).target.IsDestroyed())
                {
                    actions.RemoveAt(0);
                }
                else if (damageCollider.Distance(((Attack)actions.ElementAt(0)).target.GetComponent<CapsuleCollider2D>()).distance > 0.25 && !attacking)
                {
                    rigidBody.transform.position = Vector2.MoveTowards(rigidBody.position, (Vector2)((Attack)actions.ElementAt(0)).target.transform.position, moveSpeed);
                }
                else if (damageCollider.Distance(((Attack)actions.ElementAt(0)).target.GetComponent<CapsuleCollider2D>()).distance <= 0.25 && !attacking)
                {
                    targetTick = currentTick + attackRate;

                    attacking = true;
                }

                if (currentTick == targetTick && attacking)
                {
                    ((Attack)actions.ElementAt(0)).target.Damage(damage);

                    attacking = false;
                }
            }
        }
    }
}
