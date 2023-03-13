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

    public List<Vector2> movement = new List<Vector2>();

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

    public void SetSelectedVisible(bool visible)
    {
        selectedGameObject.SetActive(visible);
    }
    void FixedUpdate()
    {
        if (movement.Count > 0)
        {
            if (rigidBody.position != movement.ElementAt(0))
            {
                rigidBody.transform.position = Vector2.MoveTowards(rigidBody.position, movement[0], moveSpeed);
            }
            else
            {
                movement.Remove(movement.ElementAt(0));
            }
        }
    }

    public void Update()
    {
        entities = new List<Collider2D>();

        ContactFilter2D colliderFiler = new ContactFilter2D();

        colliderFiler.NoFilter();

        colliderFiler.useTriggers = true;

        entities.Clear();

        damageCollider.OverlapCollider(colliderFiler, entities);

        foreach (Collider2D collider in entities)
        {
            Enemy enemy = collider.GetComponent<Enemy>();

            if (enemy != null && damageCollider.Distance(collider).distance < -4.7f)
            {
                healthBar.Damage(enemy.damage * Time.deltaTime);
                break;
            }
        }
    }
}
