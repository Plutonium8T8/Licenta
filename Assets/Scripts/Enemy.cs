using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class Enemy : Entity
{
    private Vector3 startingPosition;

    private CircleCollider2D aggroCollider;

    private CapsuleCollider2D enemyCollider;

    public int damage;

    public List<Action> actions = new List<Action>();

    private List<Collider2D> entities;

    private bool unitAggroFound;

    private void Start()
    {
        moveSpeed = 0.05f;

        damage = 5;

        startingPosition = transform.position;

        aggroCollider = GetComponent<CircleCollider2D>();

        healthBar = transform.GetChild(0).GetComponent<HealthBar>();

        enemyCollider = GetComponent<CapsuleCollider2D>();

        HealthSystem healthSystem = new HealthSystem(100);

        healthBar.Setup(healthSystem);
    }

    private Vector3 GetRoamingPosition()
    {
        return startingPosition + Utils.UtilsClass.GetRandomDirection() * Random.Range(1f, 1f);
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
    }
    private void Update()
    {
        entities = new List<Collider2D>();

        ContactFilter2D colliderFiler = new ContactFilter2D();

        colliderFiler.NoFilter();

        entities.Clear();

        aggroCollider.OverlapCollider(colliderFiler, entities);

        unitAggroFound = false;

        Unit aggroUnit = null;

        float minimalDistance = float.MaxValue;

        foreach (Collider2D collider in entities)
        {
            Unit unit = collider.GetComponent<Unit>();

            if (unit != null)
            {
                Debug.Log(enemyCollider.Distance(unit.GetComponent<CapsuleCollider2D>()).distance);

                if (minimalDistance > unit.GetComponent<CapsuleCollider2D>().Distance(enemyCollider).distance)
                {
                    minimalDistance = unit.GetComponent<CapsuleCollider2D>().Distance(enemyCollider).distance;

                    aggroUnit = unit;
                }
            }
        }

        if (aggroUnit != null)
        {
            moveSpeed = 0.05f;

            actions.Clear();

            if (entities.Count(x => x.GetComponent<Unit>() == aggroUnit) >= 2)
            {
                unitAggroFound = true;

                actions.Add(new Move(new Vector3(aggroUnit.transform.position.x, aggroUnit.transform.position.y)));
            }
            else
            {
                actions.Clear();
            }

            if (enemyCollider.Distance(aggroUnit.GetComponent<CapsuleCollider2D>()).distance <= 0)
            {
                aggroUnit.Damage(damage * Time.deltaTime);

                actions.Remove(actions.ElementAt(0));
            }
        }

        if (!unitAggroFound)
        {
            moveSpeed = 0.005f;
            if (actions.Count == 0)
            {
                actions.Add(new Move(GetRoamingPosition()));
            }
        }
    }
}
