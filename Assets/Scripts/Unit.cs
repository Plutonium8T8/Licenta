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

    private int pathFindingTimer = 0;

    private int elapsedTime = 0;    

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
                /*Vector2 previousPosition;

                if (elapsedTime % 100 == 0) 
                { 
                    previousPosition = rigidBody.position; Vector2 deltaPreviousPosition = rigidBody.position - previousPosition;

                    *//*                Debug.Log("Delta: " + deltaPreviousPosition);
                                    Debug.Log("Delta x: " + deltaPreviousPosition.x);
                                    Debug.Log("Delta y: " + deltaPreviousPosition.y);*//*

                    Debug.Log("1: " + rigidBody.position);
                    Debug.Log("2: " + previousPosition);

                    if (Mathf.Abs(deltaPreviousPosition.x) == 0 && Mathf.Abs(deltaPreviousPosition.y) == 0)
                    {
                        pathFindingTimer++;
                    }
                }*/
   

                rigidBody.transform.position = Vector2.MoveTowards(rigidBody.position, movement[0], moveSpeed);

                

                if (pathFindingTimer > 100)
                {
                    pathFindingTimer = 0;
                    movement.Remove(movement.ElementAt(0));
                }

                elapsedTime++;
            }
            else
            {
                pathFindingTimer = 0;
                movement.Remove(movement.ElementAt(0));
            }
        }
    }
}
