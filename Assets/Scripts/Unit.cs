using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using Pathfinding;

public class Unit : Entity
{
    private GameObject selectedGameObject;

    private ContactFilter2D colliderFiler;

    private CircleCollider2D damageCollider;

    private CapsuleCollider2D capsuleCollider;

    private List<Collider2D> entities;


    private List<Collider2D> currentCcollidersList;

    private List<Collider2D> previousCollidersList;

    private List<Collider2D> currentCcollidersListTiles;

    private List<Collider2D> previousCollidersListTiles;

    public List<Action> actions = new List<Action>();

    private Enemy aggroEnemy = null;

    private Seeker seeker;

    private Path unitPath;

    public int productionTime;

    public int maxHealth;

    private int currentTick;

    private int targetTick;

    public int damage;

    public int attackRate;

    private int currentWaypoint = 0;

    private float nextWaypointDistance = .6f;

    private float distance;

    private bool attacking = false;

    private bool reachedEndOfPath = false;

    private bool enemyAggroFound;

    private void Awake()
    {
        selectedGameObject = transform.Find("Selected").gameObject;
        SetSelectedVisible(false);
    }

    public void Start()
    {
        capsuleCollider = gameObject.GetComponent<CapsuleCollider2D>();

        healthBar = transform.GetChild(0).GetComponent<HealthBar>();

        HealthSystem healthSystem = new HealthSystem(maxHealth);

        healthBar.Setup(healthSystem);

        seeker = gameObject.GetComponent<Seeker>();

        entities = new List<Collider2D>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);

        damageCollider = GetComponent<CircleCollider2D>();

        currentCcollidersList = new List<Collider2D>();

        previousCollidersList = new List<Collider2D>();

        currentCcollidersListTiles = new List<Collider2D>();

        previousCollidersListTiles = new List<Collider2D>();

        colliderFiler = new ContactFilter2D();

        colliderFiler.NoFilter();

        TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickEventArgs e)
        {
            if (Application.isPlaying && e.tick % 10 == 0)
            {
                currentTick = e.tick;
            }
        };
    }

    private void UpdatePath()
    {
        if (actions.Count > 0)
        {
            if (actions.ElementAt(0).GetType() == typeof(Move))
            {
                if (transform.position != ((Move)actions.ElementAt(0)).GetMovement())
                {
                    seeker.StartPath(capsuleCollider.transform.position, (Vector2)((Move)actions.ElementAt(0)).GetMovement(), OnPathComplete);
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
                    seeker.StartPath(capsuleCollider.transform.position, (Vector2)((Attack)actions.ElementAt(0)).target.transform.position, OnPathComplete);
                }

            }
        }
    }

    private void OnPathComplete(Path path)
    {
        if (!path.error)
        {
            unitPath = path;
            currentWaypoint = 0;
        }
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
                if (unitPath != null)
                {
                    if (currentWaypoint >= unitPath.vectorPath.Count)
                    {
                        reachedEndOfPath = true;
                        actions.RemoveAt(0);
                    }
                    else
                    {
                        reachedEndOfPath = false;
                    }

                    distance = Vector2.Distance(capsuleCollider.transform.position, unitPath.vectorPath[currentWaypoint]);

                    capsuleCollider.transform.position = Vector2.MoveTowards(capsuleCollider.transform.position, (Vector2)unitPath.vectorPath[currentWaypoint], moveSpeed);

                    while (distance <= nextWaypointDistance && currentWaypoint < unitPath.vectorPath.Count - 1)
                    {
                        currentWaypoint++;

                        distance = Vector2.Distance(capsuleCollider.transform.position, unitPath.vectorPath[currentWaypoint]);
                    }
                }
            }
            else if (actions.ElementAt(0).GetType() == typeof(Attack))
            {
                if (((Attack)actions.ElementAt(0)).target.IsDestroyed())
                {
                    actions.RemoveAt(0);
                }
                else if (damageCollider.Distance(((Attack)actions.ElementAt(0)).target.GetComponent<CapsuleCollider2D>()).distance > 0 && !attacking)
                {
                    if (unitPath != null)
                    {
                        if (currentWaypoint >= unitPath.vectorPath.Count - 1)
                        {
                            reachedEndOfPath = true;
                        }
                        else
                        {
                            reachedEndOfPath = false;
                        }

                        distance = Vector2.Distance(capsuleCollider.transform.position, unitPath.vectorPath[currentWaypoint]);

                        capsuleCollider.transform.position = Vector2.MoveTowards(capsuleCollider.transform.position, (Vector2)unitPath.vectorPath[currentWaypoint], moveSpeed);

                        while (distance <= nextWaypointDistance && currentWaypoint < unitPath.vectorPath.Count - 1)
                        {
                            currentWaypoint++;

                            distance = Vector2.Distance(capsuleCollider.transform.position, unitPath.vectorPath[currentWaypoint]);
                        }
                    } 
                }
                else if (damageCollider.Distance(((Attack)actions.ElementAt(0)).target.GetComponent<CapsuleCollider2D>()).distance <= 0 && !attacking)
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
            else
            {
                if (!enemyAggroFound)
                {
                    entities.Clear();

                    damageCollider.OverlapCollider(colliderFiler, entities);

                    Collider2D collider = entities
                        .Where(x => x.GetType() == typeof(CapsuleCollider2D))
                        .Where(z => z.GetComponent<Enemy>() != null)
                        .OrderBy(y => damageCollider.Distance(y).distance)
                        .FirstOrDefault();

                    if (collider != null)
                    {
                        if (collider.GetComponent<Enemy>() != null)
                        {
                            if (aggroEnemy == null)
                            {
                                aggroEnemy = collider.GetComponent<Enemy>();

                                actions.Clear();

                                actions.Add(new Move(new Vector3(aggroEnemy.transform.position.x, aggroEnemy.transform.position.y)));

                                enemyAggroFound = true;
                            }
                        }
                    }
                }

                if (enemyAggroFound)
                {
                    if (damageCollider.Distance(aggroEnemy.GetComponent<CapsuleCollider2D>()).distance <= 0.25f && !attacking)
                    {
                        targetTick = currentTick + attackRate;

                        attacking = true;
                    }

                    if (damageCollider.Distance(aggroEnemy.GetComponent<CapsuleCollider2D>()).distance > 0.25f)
                    {
                        attacking = false;

                        if (aggroEnemy != null)
                        {
                            actions.Add(new Move(new Vector3(aggroEnemy.transform.position.x, aggroEnemy.transform.position.y)));
                        }
                    }

                    if (attacking && currentTick == targetTick)
                    {
                        aggroEnemy.Damage(damage);

                        attacking = false;
                    }
                }
            }
        }

        damageCollider.OverlapCollider(colliderFiler, currentCcollidersList);

        currentCcollidersListTiles = currentCcollidersList.Where(x => x.GetType() == typeof(PolygonCollider2D)).Where(y => y.GetComponent<Tile>() != null).ToList();

        currentCcollidersList = currentCcollidersList.Where(x => x.GetType() == typeof(CapsuleCollider2D)).Where(y => y.GetComponent<Enemy>()).ToList();

        if (!currentCcollidersList.SequenceEqual(previousCollidersList))
        {
            foreach (CapsuleCollider2D capsuleCollider2D in previousCollidersList.Where(x => !currentCcollidersList.Contains(x)))
            {
                if (!capsuleCollider2D.IsDestroyed())
                {
                    capsuleCollider2D.GetComponent<Enemy>().RemoveObserver();
                }
            }

            foreach (CapsuleCollider2D capsuleCollider2D in currentCcollidersList.Where(x => !previousCollidersList.Contains(x)))
            {
                if (!capsuleCollider2D.IsDestroyed())
                {
                    capsuleCollider2D.GetComponent<Enemy>().AddObserver();
                }   
            }

            previousCollidersList.Clear();
            previousCollidersList.InsertRange(0, currentCcollidersList);
        }

        if (!currentCcollidersListTiles.SequenceEqual(previousCollidersListTiles))
        {
            foreach (PolygonCollider2D pollygonCollider2D in previousCollidersListTiles.Where(x => !currentCcollidersListTiles.Contains(x)))
            {
                pollygonCollider2D.GetComponent<Tile>().RemoveObserver();
            }

            foreach (PolygonCollider2D pollygonCollider2D in currentCcollidersListTiles.Where(x => !previousCollidersListTiles.Contains(x)))
            {
                pollygonCollider2D.GetComponent<Tile>().AddObserver();
            }

            previousCollidersListTiles.Clear();
            previousCollidersListTiles.InsertRange(0, currentCcollidersListTiles);
        }
    }
}
