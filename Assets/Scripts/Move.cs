using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : Action
{
    private Vector3 movement;

    public Move(Vector3 movement)
    {
        this.movement = movement;
    }

    public Vector3 GetMovement()
    {
        return movement;
    }
}
