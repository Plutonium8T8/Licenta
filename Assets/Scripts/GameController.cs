using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

public class GameController : MonoBehaviour
{
    // [SerializeField] private Transform selectionAreaTransform;

    private Vector2 startPosition;

    private List<Unit> selectedEntitiesList;

    private void Awake()
    {
        selectedEntitiesList = new List<Unit>();

        // selectionAreaTransform.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 currentMousePosition = UtilsClass.GetMouseWorldPosition();

            Vector2 lowerLeft = new Vector2(
                Mathf.Min(startPosition.x, currentMousePosition.x),
                Mathf.Min(startPosition.y, currentMousePosition.y)
                );
            Vector2 upperRight = new Vector2(
                Mathf.Max(startPosition.x, currentMousePosition.x),
                Mathf.Max(startPosition.y, currentMousePosition.y)
                );

            // selectionAreaTransform.position = lowerLeft;

            // selectionAreaTransform.localScale = upperRight - lowerLeft;
        }

        if (Input.GetMouseButtonDown(0)) // GetMouseButtonDown(LeftClick)
        {
            // selectionAreaTransform.gameObject.SetActive(true);

            startPosition = UtilsClass.GetMouseWorldPosition();
        }

        if (Input.GetMouseButtonUp(0)) // GetMouseButtonUp(LeftClick)
        {
            // selectionAreaTransform.gameObject.SetActive(false);

            Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(startPosition, UtilsClass.GetMouseWorldPosition());

            foreach (Unit unit in selectedEntitiesList)
            {
                unit.SetSelectedVisible(false);
            }

            selectedEntitiesList.Clear();

            foreach (Collider2D collider2D in collider2DArray)
            {
                Unit unit = collider2D.GetComponent<Unit>();

                if (unit != null)
                {
                    unit.SetSelectedVisible(true);
                    selectedEntitiesList.Add(unit);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            foreach (Unit unit in selectedEntitiesList)
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    unit.movement.Clear();
                }
                unit.movement.Add(UtilsClass.GetMouseWorldPosition());
            }
        }
    }
}
