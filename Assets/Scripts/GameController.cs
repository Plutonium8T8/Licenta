using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

public class GameController : MonoBehaviour
{
    // [SerializeField] private Transform selectionAreaTransform;

    // [SerializeField] public CameraController cameraController;

    private Vector2 _startPosition;

    private List<Unit> _selectedEntitiesList;

    private void Awake()
    {
        _selectedEntitiesList = new List<Unit>();

        // selectionAreaTransform.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 currentMousePosition = UtilsClass.GetMouseWorldPosition();

            Vector2 lowerLeft = new Vector2(
                Mathf.Min(_startPosition.x, currentMousePosition.x),
                Mathf.Min(_startPosition.y, currentMousePosition.y)
                );
            Vector2 upperRight = new Vector2(
                Mathf.Max(_startPosition.x, currentMousePosition.x),
                Mathf.Max(_startPosition.y, currentMousePosition.y)
                );

            // selectionAreaTransform.position = lowerLeft;

            // selectionAreaTransform.localScale = upperRight - lowerLeft;
        }

        if (Input.GetMouseButtonDown(0)) // GetMouseButtonDown(LeftClick)
        {
            // selectionAreaTransform.gameObject.SetActive(true);

            _startPosition = UtilsClass.GetMouseWorldPosition();
        }

        if (Input.GetMouseButtonUp(0)) // GetMouseButtonUp(LeftClick)
        {
            // selectionAreaTransform.gameObject.SetActive(false);

            Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(_startPosition, UtilsClass.GetMouseWorldPosition());

            foreach (Unit unit in _selectedEntitiesList)
            {
                unit.SetSelectedVisible(false);
            }

            _selectedEntitiesList.Clear();

            foreach (Collider2D collider2D in collider2DArray)
            {
                Unit unit = collider2D.GetComponent<Unit>();

                if (unit != null)
                {
                    unit.SetSelectedVisible(true);
                    _selectedEntitiesList.Add(unit);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(!Input.GetKey(KeyCode.LeftShift));
            foreach (Unit unit in _selectedEntitiesList)
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    unit.ClearMovement();
                }
                unit.AddMovement(UtilsClass.GetMouseWorldPosition());
            }
        }
    }
}
