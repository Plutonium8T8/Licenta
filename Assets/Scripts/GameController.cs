using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

public class GameController : MonoBehaviour
{
    [SerializeField] private CameraController _camera;

    private Vector2 _startPosition;

    private List<Unit> selectedEntitiesList;

    private void Start()
    {
        _camera.Setup(() => new Vector3(2,2));
    }
    private void Awake()
    {
        selectedEntitiesList = new List<Unit>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // GetMouseButtonDown(LeftClick)
        {
            _startPosition = UtilsClass.GetMouseWorldPosition();
        }

        if (Input.GetMouseButtonUp(0)) // GetMouseButtonUp(LeftClick)
        {
            Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(_startPosition, UtilsClass.GetMouseWorldPosition());

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
