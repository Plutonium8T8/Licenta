using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Utils;

public class GameController : MonoBehaviour
{
    [SerializeField] private CameraController _camera;

    [SerializeField] private float _edgeSize = 10f;

    [SerializeField] public GameObject entity;

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

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.L))
        {
            foreach (Unit unit in selectedEntitiesList)
            {
                if (!unit.IsDestroyed())
                {
                    unit.healthBar.Heal(1);
                }
            }
        }

        if (Input.GetKey(KeyCode.K))
        {
            foreach (Unit unit in selectedEntitiesList)
            {
                if (!unit.IsDestroyed())
                {
                    unit.healthBar.Damage(1);
                    if (unit.healthBar.GetHealthPercent() == 0)
                    {
                        Destroy(unit.transform.gameObject);
                    }
                }
            }
        }

        if (Input.GetKey(KeyCode.J))
        {
            Instantiate(entity, new Vector3(0, 0, 0), Quaternion.identity);
        }
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

        _zoom = Mathf.Clamp(_zoom, 10f, 50f);

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

/*        if (Input.GetMouseButton(0))
        {
            Vector3 currentMousePosition = UtilsClass.GetMouseWorldPosition();

            Vector3 lowerLeft = new Vector3(
                Mathf.Min(_startPosition.x, currentMousePosition.x),
                Mathf.Min(_startPosition.y, currentMousePosition.y)
                );

            Vector3 upperRight = new Vector3(
                Mathf.Max(_startPosition.x, currentMousePosition.x),
                Mathf.Max(_startPosition.y, currentMousePosition.y)
                );

            selectionAreaTransform.position = lowerLeft;

            selectionAreaTransform.localScale = upperRight - lowerLeft;
        }*/

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
                if (!unit.IsDestroyed())
                {
                    unit.SetSelectedVisible(false);
                }
                
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
                if (!unit.IsDestroyed())
                {
                    if (!Input.GetKey(KeyCode.LeftShift))
                    {
                        unit.actions.Clear();
                    }
                    unit.actions.Add(new Move(UtilsClass.GetMouseWorldPosition()));
                }
                
            }
        }
    }
}
