using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : Entity
{
    private GameObject selectedGameObject;

    private ContactFilter2D colliderFiler;

    private CircleCollider2D damageCollider;

    private List<Collider2D> entities;

    private CircleCollider2D fowCollider;

    private List<Collider2D> currentCcollidersList;

    private List<Collider2D> previousCollidersList;

    private List<Collider2D> currentCcollidersListTiles;

    private List<Collider2D> previousCollidersListTiles;

    public List<Action> actions = new List<Action>();

    public int productionTime;

    public int maxHealth;

    private int currentTick;

    private int targetTick;

    public int damage;

    public int attackRate;

    private bool attacking = false;

    private void Awake()
    {
        selectedGameObject = transform.Find("Selected").gameObject;
        SetSelectedVisible(false);
    }

    public void Start()
    {
        healthBar = transform.GetChild(0).GetComponent<HealthBar>();

        HealthSystem healthSystem = new HealthSystem(maxHealth);

        healthBar.Setup(healthSystem);

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
                if (transform.position != ((Move)actions.ElementAt(0)).GetMovement())
                {
                    transform.transform.position = Vector2.MoveTowards(transform.position, (Vector2)((Move)actions.ElementAt(0)).GetMovement(), moveSpeed);
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
                    transform.transform.position = Vector2.MoveTowards(transform.position, (Vector2)((Attack)actions.ElementAt(0)).target.transform.position, moveSpeed);
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
