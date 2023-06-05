using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int productionRate;

    public int productionType;

    public int maxHealth;

    public HealthBar healthBar;

    private CircleCollider2D fowCollider;

    private List<Collider2D> currentCcollidersList;

    private List<Collider2D> previousCollidersList;

    private List<Collider2D> currentCcollidersListTiles;

    private List<Collider2D> previousCollidersListTiles;

    private ContactFilter2D colliderFiler;

    public int level;

    public bool isPlaced = false;

    public void Start()
    {
        healthBar = transform.GetChild(0).GetComponent<HealthBar>();

        HealthSystem healthSystem = new HealthSystem(200);

        healthBar.Setup(healthSystem);

        fowCollider = GetComponent<CircleCollider2D>();

        currentCcollidersList = new List<Collider2D>();

        previousCollidersList = new List<Collider2D>();

        currentCcollidersListTiles = new List<Collider2D>();

        previousCollidersListTiles = new List<Collider2D>();

        colliderFiler = new ContactFilter2D();

        colliderFiler.NoFilter();

        productionRate = 0;

        level = 1;
    }
    public void Damage(float damage)
    {
        healthBar.Damage(damage);
    }

    public void Heal(float heal)
    {
        healthBar.Heal(heal);
    }

    public void Update()
    {
        if (healthBar.GetHealthPercent() == 0) 
        {
            Destroy(transform.gameObject);
        }

        if (isPlaced)
        {
            fowCollider.OverlapCollider(colliderFiler, currentCcollidersList);

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
}
