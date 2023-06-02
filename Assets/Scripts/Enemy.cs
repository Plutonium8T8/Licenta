using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Entity
{
    private Vector3 startingPosition;

    private CircleCollider2D aggroCollider;

    private CapsuleCollider2D enemyCollider;

    public List<Action> actions = new List<Action>();

    private List<Collider2D> entities;

    public int damage;

    private int attackRate = 10;

    private int currentTick;

    private int targetTick;

    private bool isAttacking = false;

    private bool unitAggroFound;

    private bool buildingAggroFound;

    private void Start()
    {
        moveSpeed = 0.05f;

        damage = 25;

        startingPosition = transform.position;

        aggroCollider = GetComponent<CircleCollider2D>();

        healthBar = transform.GetChild(0).GetComponent<HealthBar>();

        enemyCollider = GetComponent<CapsuleCollider2D>();

        HealthSystem healthSystem = new HealthSystem(100);

        healthBar.Setup(healthSystem);

        TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickEventArgs e)
        {
            if (Application.isPlaying && e.tick % 10 == 0)
            {
                currentTick = e.tick;
            }
        };

        gameObject.GetComponent<Renderer>().enabled = false;

        healthBar.transform.Find("BarBackground").GetComponent<Renderer>().enabled = false;

        healthBar.transform.Find("Bar").transform.Find("BarSprite").GetComponent<Renderer>().enabled = false;
    }

    public void Damage(float damage)
    {
        healthBar.Damage(damage);
    }

    public void Heal(float heal)
    {
        healthBar.Heal(heal);
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
        if (healthBar.GetHealthPercent() == 0)
        {
            Destroy(transform.gameObject);
        }

        entities = new List<Collider2D>();

        ContactFilter2D colliderFiler = new ContactFilter2D();

        colliderFiler.NoFilter();

        entities.Clear();

        aggroCollider.OverlapCollider(colliderFiler, entities);

        unitAggroFound = false;

        buildingAggroFound = false;

        Unit aggroUnit = null;

        Building aggroBuilding = null;

        float minimalDistance = float.MaxValue;

        foreach (Collider2D collider in entities)
        {
            Unit unit = collider.GetComponent<Unit>();

            if (unit != null)
            {
                if (minimalDistance > unit.GetComponent<CapsuleCollider2D>().Distance(enemyCollider).distance)
                {
                    minimalDistance = unit.GetComponent<CapsuleCollider2D>().Distance(enemyCollider).distance;

                    aggroUnit = unit;
                }
            }
            else
            {
                Building building = collider.GetComponent<Building>();

                if (building != null)
                {
                    if (minimalDistance > building.GetComponent<PolygonCollider2D>().Distance(enemyCollider).distance)
                    {
                        minimalDistance = building.GetComponent<PolygonCollider2D>().Distance(enemyCollider).distance;

                        aggroBuilding = building;
                    }
                    
                }
            }
        }

        if (aggroUnit != null && !unitAggroFound)
        {
            moveSpeed = 0.0125f;

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
        }

        if (unitAggroFound)
        {
            if (enemyCollider.Distance(aggroUnit.GetComponent<CapsuleCollider2D>()).distance <= 0.25f && !isAttacking)
            {
                targetTick = currentTick + attackRate;

                isAttacking = true;

                actions.Remove(actions.ElementAt(0));
            }

            if (enemyCollider.Distance(aggroUnit.GetComponent<CapsuleCollider2D>()).distance > 0.25f)
            {
                isAttacking = false;
            }

            if (isAttacking && currentTick == targetTick)
            {
                aggroUnit.Damage(damage);

                isAttacking = false;
            }
        }

        if (aggroBuilding != null && !buildingAggroFound)
        {
            buildingAggroFound = true;

            moveSpeed = 0.0125f;

            actions.Clear();

            actions.Add(new Move(new Vector3(aggroBuilding.transform.position.x, aggroBuilding.transform.position.y)));
        }

        if (buildingAggroFound)
        {
            if (enemyCollider.Distance(aggroBuilding.GetComponent<PolygonCollider2D>()).distance <= 0.25f && !isAttacking)
            {
                targetTick = currentTick + attackRate;

                isAttacking = true;

                actions.Remove(actions.ElementAt(0));
            }

            if (enemyCollider.Distance(aggroBuilding.GetComponent<PolygonCollider2D>()).distance > 0.25f)
            {
                isAttacking = false;
            }

            if (isAttacking && currentTick == targetTick)
            {
                aggroBuilding.Damage(damage);

                isAttacking = false;
            }
        }

        if (aggroUnit.IsDestroyed())
        {
            aggroUnit = null;
            unitAggroFound = false;
        }

        if (aggroBuilding.IsDestroyed())
        {
            aggroBuilding = null;
            buildingAggroFound = false;
        }

        if (!unitAggroFound && !buildingAggroFound)
        {
            moveSpeed = 0.005f;
            if (actions.Count == 0)
            {
                actions.Add(new Move(GetRoamingPosition()));
            }
        }
    }
}
