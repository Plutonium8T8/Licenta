using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int productionRate;

    public int productionType;

    public HealthBar healthBar;

    private CircleCollider2D fowCollider;

    private List<Collider2D> colliders;

    private ContactFilter2D colliderFiler;

    public int level;

    public bool isPlaced = false;

    public void Start()
    {
        healthBar = transform.GetChild(0).GetComponent<HealthBar>();

        HealthSystem healthSystem = new HealthSystem(200);

        healthBar.Setup(healthSystem);

        fowCollider = GetComponent<CircleCollider2D>();

        colliders = new List<Collider2D>();

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
            fowCollider.OverlapCollider(colliderFiler, colliders);

            foreach (Collider2D collider in colliders)
            {
                if (collider.GetComponent<Tile>() != null)
                {
                    if (collider.GameObject().GetComponent<Renderer>().enabled == false)
                    {
                        collider.GameObject().GetComponent<Renderer>().enabled = true;

                        collider.gameObject.transform.Find("Shadow").gameObject.GetComponent<Renderer>().enabled = false;
                    }
                    else

                    if (collider.GameObject().GetComponent<Renderer>().enabled == true && collider.Distance(fowCollider).distance == 0.1f)
                    {
                        collider.gameObject.transform.Find("Shadow").gameObject.GetComponent<Renderer>().enabled = true;
                    }
                }

                if (collider.GetComponent<Enemy>() != null)
                {
                    Enemy enemy = collider.GetComponent<Enemy>();

                    if (colliders.Count(x => x.GetComponent<Enemy>() == enemy) >= 2)
                    {
                        if (enemy.GameObject().GetComponent<Renderer>().enabled == false)
                        {
                            enemy.GameObject().GetComponent<Renderer>().enabled = true;

                            enemy.healthBar.transform.Find("BarBackground").GetComponent<Renderer>().enabled = true;

                            enemy.healthBar.transform.Find("Bar").transform.Find("BarSprite").GetComponent<Renderer>().enabled = true;
                        }
/*                        else

                        if (collider.GameObject().GetComponent<Renderer>().enabled == true)
                        {
                            Debug.Log(collider.Distance(fowCollider).distance);

                            enemy.GameObject().GetComponent<Renderer>().enabled = false;

                            enemy.healthBar.transform.Find("BarBackground").GetComponent<Renderer>().enabled = false;

                            enemy.healthBar.transform.Find("Bar").transform.Find("BarSprite").GetComponent<Renderer>().enabled = false;
                        }*/
                    }
                }
            }
        }
    }
}
