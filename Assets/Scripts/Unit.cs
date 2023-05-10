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

    private Vector3 prevPos;

    private int count = 0;

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

        prevPos = transform.position;
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

        if (Vector2.Distance(transform.position, prevPos) <= 0.09f && actions.Count() > 0)
        {
            count++;
        }
        else
        {
            count = 0;
        }

        if (count == 50)
        {
            count = 0;
            actions.Remove(actions.ElementAt(0));
        }

        prevPos = transform.position;
    }
}
