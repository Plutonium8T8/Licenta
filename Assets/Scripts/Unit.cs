using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private GameObject selectedGameObject;

    public Rigidbody2D rigidBody;  

    public float moveSpeed = 0.1f;

    public List<Vector2> movement = new List<Vector2>();

    private void Awake()
    {
        selectedGameObject = transform.Find("Selected").gameObject;
        SetSelectedVisible(false);
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
