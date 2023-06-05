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

    private Unit aggroUnit = null;

    private Building aggroBuilding = null;

    ContactFilter2D colliderFiler;

    public int damage;

    private int attackRate = 10;

    private int currentTick;

    private int targetTick;

    private bool isAttacking = false;

    private bool unitAggroFound;

    private bool buildingAggroFound;

    private void Start()
    {
        moveSpeed = 0.005f;

        damage = 20;

        unitAggroFound = false;

        buildingAggroFound = false;

        startingPosition = transform.position;

        colliderFiler = new ContactFilter2D();

        entities = new List<Collider2D>();

        colliderFiler.NoFilter();

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
            if (transform.position != ((Move)actions.ElementAt(0)).GetMovement())
            {
                transform.transform.position = Vector2.MoveTowards(transform.position, (Vector2)((Move)actions.ElementAt(0)).GetMovement(), moveSpeed);
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

            entities.Clear();

            aggroCollider.OverlapCollider(colliderFiler, entities);

            Collider2D collider = entities
                .Where(x => x.GetType() == typeof(PolygonCollider2D) || x.GetType() == typeof(CapsuleCollider2D))
                .Where(z => z.GetComponent<Unit>() != null || z.GetComponent<Building>() != null)
                .OrderBy(y => aggroCollider.Distance(y).distance)
                .FirstOrDefault();

            if (collider != null) 
            {
                if (collider.GetComponent<Unit>() != null)
                {
                    aggroUnit = collider.GetComponent<Unit>();

                    moveSpeed = 0.0125f;

                    actions.Clear();

                    actions.Add(new Move(new Vector3(aggroUnit.transform.position.x, aggroUnit.transform.position.y)));

                    unitAggroFound = true;
                }
                else

                if (collider.GetComponent<Building>() != null)
                {
                    aggroBuilding = collider.GetComponent<Building>();

                    moveSpeed = 0.0125f;

                    actions.Clear();

                    actions.Add(new Move(new Vector3(aggroBuilding.transform.position.x, aggroBuilding.transform.position.y)));

                    buildingAggroFound = true;
                }
            }
        }

        if (unitAggroFound)
        {
            if (enemyCollider.Distance(aggroUnit.GetComponent<CapsuleCollider2D>()).distance <= 0.25f && !isAttacking)
            {
                targetTick = currentTick + attackRate;

                isAttacking = true;
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

        if (buildingAggroFound)
        {
            if (enemyCollider.Distance(aggroBuilding.GetComponent<PolygonCollider2D>()).distance <= 0.25f && !isAttacking)
            {
                targetTick = currentTick + attackRate;

                isAttacking = true;
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
    }
}
