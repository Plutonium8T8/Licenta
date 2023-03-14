using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Enemy : Entity
{
    private Vector3 startingPosition;

    private CircleCollider2D aggroCollider;

    private CapsuleCollider2D attackCollider;

    public int damage;

    private float attackRange = -0.000005f;

    public List<Vector2> movement = new List<Vector2>();

    private List<Collider2D> entities;

    private bool unitAggroFound;

    private void Start()
    {
        moveSpeed = 0.05f;

        damage = 10;

        startingPosition = transform.position;

        aggroCollider = GetComponent<CircleCollider2D>();

        attackCollider = GetComponent<CapsuleCollider2D>();

        healthBar = transform.GetChild(0).GetComponent<HealthBar>();

        HealthSystem healthSystem = new HealthSystem(100);

        healthBar.Setup(healthSystem);
    }

    private Vector3 GetRoamingPosition()
    {
        return startingPosition + Utils.UtilsClass.GetRandomDirection() * Random.Range(1f, 1f);
    }

    private void Update()
    {
        entities = new List<Collider2D>();

        ContactFilter2D colliderFiler = new ContactFilter2D();
        colliderFiler.NoFilter();

        entities.Clear();

        aggroCollider.OverlapCollider(colliderFiler, entities);

        unitAggroFound = false;

        foreach (Collider2D collider in entities)
        {
            Unit unit = collider.GetComponent<Unit>();

            if (unit != null)
            {
                unitAggroFound = true;
                moveSpeed = 0.05f;
                movement.Clear();

                if (!attackCollider.IsTouching(collider))
                {
                    movement.Add(new Vector3(unit.transform.position.x, unit.transform.position.y));
                }
                else
                {
                    movement.Clear();
                }
                break;
            }
        }

        if (!unitAggroFound)
        {
            moveSpeed = 0.005f;
            if (movement.Count == 0)
            {
                movement.Add(GetRoamingPosition());
            }
        }
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
}
