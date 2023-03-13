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

    public List<Vector2> movement = new List<Vector2>();

    private void Awake()
    {
        selectedGameObject = transform.Find("Selected").gameObject;
        SetSelectedVisible(false);
    }

    private void Start()
    {
        healthBar = transform.GetChild(1).GetComponent<HealthBar>();

        HealthSystem healthSystem = new HealthSystem(100);

        healthBar.Setup(healthSystem);
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
}
