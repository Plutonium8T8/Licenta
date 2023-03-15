using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class Unit : Entity
{
    private GameObject selectedGameObject;

    private CircleCollider2D damageCollider;

    private List<Collider2D> entities;

    public List<Action> actions = new List<Action>();

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
    }

    public void Damage(float damage)
    {
        healthBar.Damage(damage);
    }

    public void Heal(float heal)
    {
        healthBar.Heal(heal);
    }

    public void SetSelectedVisible(bool visible)
    {
        selectedGameObject.SetActive(visible);
    }
    void FixedUpdate()
    {
        if (actions.Count > 0)
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

        if (healthBar.GetHealthPercent() == 0)
        {
            Destroy(transform.gameObject);
        }
    }

    public void Update()
    {
        /*entities = new List<Collider2D>();

        ContactFilter2D colliderFiler = new ContactFilter2D();

        colliderFiler.NoFilter();

        colliderFiler.useTriggers = true;

        entities.Clear();

        damageCollider.OverlapCollider(colliderFiler, entities);

        foreach (Collider2D collider in entities)
        {
            Enemy enemy = collider.GetComponent<Enemy>();

            if (enemy != null)
            {
                if (damageCollider.Distance(collider).distance < -4.5f)
                {
                    healthBar.Damage(enemy.damage * Time.deltaTime);

                    if (healthBar.GetHealthPercent() == 0)
                    {
                        Destroy(transform.gameObject);
                    }

                    break;
                }
                    
            }
        }*/
    }
}
