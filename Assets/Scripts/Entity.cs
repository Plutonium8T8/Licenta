using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public HealthBar healthBar;

    public CapsuleCollider2D capsuleCollider;

    public Rigidbody2D rigidBody;

    public float moveSpeed = 0.025f;
}
