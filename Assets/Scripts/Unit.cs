using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private GameObject _selectedGameObject;

    public Rigidbody2D rigidBody;  

    public float moveSpeed = 0.1f;

    private List<Vector2> _movement = new List<Vector2>();

    public void AddMovement(Vector2 position)
    {
        _movement.Add(position);
    }

    public void RemoveMovement(int index)
    {
        _movement.RemoveAt(index);
    }

    public void ClearMovement()
    {
        _movement.Clear();
    }

    private void Awake()
    {
        _selectedGameObject = transform.Find("Selected").gameObject;
        SetSelectedVisible(false);
    }

    public void SetSelectedVisible(bool visible)
    {
        _selectedGameObject.SetActive(visible);
    }
    void FixedUpdate()
    {
        if (_movement.Count > 0)
        {
            if (rigidBody.position != _movement.ElementAt(0))
            {
                rigidBody.transform.position = Vector2.MoveTowards(rigidBody.position, _movement[0], moveSpeed);
            }
            else
            {
                _movement.Remove(_movement.ElementAt(0));
            }
        }
    }
}
