using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

public class GameController : MonoBehaviour
{
    [SerializeField] private CameraController _camera;

    [SerializeField] private float _edgeSize = 10f;

    private float _zoom = 12f;

    private float _zoomSpeed = 100f;

    private float _cameraMoveAmount = 10f;

    private float _mapHeight = 50f;

    private float _mapWidth = 50f;

    private Vector3 cameraPosition = new Vector3(0, 0);

    private Vector2 _startPosition;

    private List<Unit> selectedEntitiesList;

    private void Start()
    {
        _camera.Setup(() => cameraPosition, () => _zoom);
    }
    private void Awake()
    {
        selectedEntitiesList = new List<Unit>();
    }
    private void Update()
    {
        // Camera WASD movement
        if (Input.GetKey(KeyCode.W) && cameraPosition.y <= _mapHeight)
        {
            cameraPosition.y += _cameraMoveAmount * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) && cameraPosition.x >= -_mapWidth)
        {
            cameraPosition.x -= _cameraMoveAmount * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) && cameraPosition.y >= -_mapHeight)
        {
            cameraPosition.y -= _cameraMoveAmount * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) && cameraPosition.x <= _mapWidth)
        {
            cameraPosition.x += _cameraMoveAmount * Time.deltaTime;
        }

        // Mouse-edge camera movement

        if (Input.mouseScrollDelta.y > 0)
        {
            _zoom -= _zoomSpeed * Time.deltaTime;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            _zoom += _zoomSpeed * Time.deltaTime;
        }

        _zoom = Mathf.Clamp(_zoom, 3f, 25f);

        // Right edge
        if (Input.mousePosition.x > Screen.width - _edgeSize)
        {
            cameraPosition.x += _cameraMoveAmount * Time.deltaTime;
        }
        // Left edge
        if (Input.mousePosition.x < _edgeSize)
        {
            cameraPosition.x -= _cameraMoveAmount * Time.deltaTime;
        }
        // Top edge
        if (Input.mousePosition.y > Screen.height - _edgeSize)
        {
            cameraPosition.y += _cameraMoveAmount * Time.deltaTime;
        }
        // Bottom edge
        if (Input.mousePosition.y < _edgeSize)
        {
            cameraPosition.y -= _cameraMoveAmount * Time.deltaTime;
        }

        // LeftClick

        if (Input.GetMouseButtonDown(0)) 
        {
            _startPosition = UtilsClass.GetMouseWorldPosition();
        }

        // LeftClick + LeftShift

        if (Input.GetMouseButtonUp(0))
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

        // RightClick

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
