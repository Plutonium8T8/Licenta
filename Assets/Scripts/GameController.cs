using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class GameController : MonoBehaviour
{
    [SerializeField] private CameraController _camera;

    [SerializeField] private float _edgeSize = 10f;

    [SerializeField] public GameObject entity;

    [SerializeField] public GameObject[] buildings;

    [SerializeField] public Grid grid;


    private Vector3 cameraPosition = new Vector3(0, 0);

    private Vector2 _startPosition;


    private List<Unit> selectedEntitiesList;

    private List<GameObject> gameBuildings;

    private Building buildingData;

    private GameObject newBuilding;

    private GridManager gridManager;

    [SerializeField] private Text prodStats;


    private float _zoom = 12f;

    private float _zoomSpeed = 100f;

    private float _cameraMoveAmount = 10f;

    private float _mapHeight = 50f;

    private float _mapWidth = 50f;


    private int woodProduction = 0;

    private int stoneProduction = 0;

    private int ironProduction = 0;

    private int goldProduction = 0;

    private int titaniumProduction = 0;

    private int woodStorage = 0;

    private int stoneStorage = 0;

    private int ironStorage = 0;

    private int goldStorage = 0;

    private int titaniumStorage = 0;


    private bool placingBuilding = false;


    private void Start()
    {
        _camera.Setup(() => cameraPosition, () => _zoom);

        gridManager = grid.GetComponent<GridManager>();

        gameBuildings = new List<GameObject>();

        prodStats.text =
                "Wood: " + woodStorage + " (+" + woodProduction + ")\n" +
                "Stone: " + stoneStorage + " (+" + stoneProduction + ")\n" +
                "Iron: " + ironStorage + " (+" + ironProduction + ")\n" +
                "Gold: " + goldStorage + " (+" + goldProduction + ")\n" +
                "Titanium: " + titaniumStorage + " (+" + titaniumProduction + ")";

        TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickEventArgs e)
        {
            if (Application.isPlaying && e.tick % 10 == 0)
            {
                woodStorage += woodProduction;
                stoneStorage += stoneProduction;
                ironStorage += ironProduction;
                goldStorage += goldProduction;
                titaniumStorage += titaniumProduction;

                prodStats.text =
                "Wood: " + woodStorage + " (+" + woodProduction + ")\n" +
                "Stone: " + stoneStorage + " (+" + stoneProduction + ")\n" +
                "Iron: " + ironStorage + " (+" + ironProduction + ")\n" +
                "Gold: " + goldStorage + " (+" + goldProduction + ")\n" +
                "Titanium: " + titaniumStorage + " (+" + titaniumProduction + ")";
            }
        };
    }
    private void Awake()
    {
        selectedEntitiesList = new List<Unit>();

        _camera.Setup(() => cameraPosition, () => _zoom);
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
        
        if (gameBuildings.Count != gameBuildings.Where(x => !x.IsDestroyed()).Count())
        {
            woodProduction = 0;
            stoneProduction = 0;
            ironProduction = 0;
            goldProduction = 0;
            titaniumProduction = 0;

            gameBuildings = gameBuildings.Where(x => x.IsDestroyed() == false).ToList();

            foreach (GameObject building in gameBuildings)
            {
                buildingData = building.GetComponent<Building>();

                if (buildingData.productionType == 1)
                {
                    woodProduction += buildingData.productionRate;
                }
                else if (buildingData.productionType == 2)
                {
                    stoneProduction += buildingData.productionRate;
                }
                else if (buildingData.productionType == 3)
                {
                    ironProduction += buildingData.productionRate;
                }
                else if (buildingData.productionType == 4)
                {
                    goldProduction += buildingData.productionRate;
                }
                else if (buildingData.productionType == 5)
                {
                    titaniumProduction += buildingData.productionRate;
                }

                Debug.Log(buildingData.productionRate);
            }
        }

        if (Input.GetKey(KeyCode.F1) && !placingBuilding)
        {
            newBuilding = Instantiate(buildings[1], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

            newBuilding.GetComponent<Rigidbody2D>().simulated = false;

            buildingData = newBuilding.GetComponent<Building>();

            buildingData.productionType = 1;

            placingBuilding = true;
        }

        if (Input.GetKey(KeyCode.F2) && !placingBuilding)
        {
            newBuilding = Instantiate(buildings[1], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

            newBuilding.GetComponent<Rigidbody2D>().simulated = false;

            buildingData = newBuilding.GetComponent<Building>();

            buildingData.productionType = 2;

            placingBuilding = true;
        }

        if (Input.GetKey(KeyCode.F3) && !placingBuilding)
        {
            newBuilding = Instantiate(buildings[1], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

            newBuilding.GetComponent<Rigidbody2D>().simulated = false;

            buildingData = newBuilding.GetComponent<Building>();

            buildingData.productionType = 3;

            placingBuilding = true;
        }

        if (Input.GetKey(KeyCode.F4) && !placingBuilding)
        {
            newBuilding = Instantiate(buildings[1], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

            newBuilding.GetComponent<Rigidbody2D>().simulated = false;

            buildingData = newBuilding.GetComponent<Building>();

            buildingData.productionType = 4;

            placingBuilding = true;
        }

        if (Input.GetKey(KeyCode.F5) && !placingBuilding)
        {
            newBuilding = Instantiate(buildings[1], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

            newBuilding.GetComponent<Rigidbody2D>().simulated = false;

            buildingData = newBuilding.GetComponent<Building>();

            buildingData.productionType = 5;

            placingBuilding = true;
        }

        if (placingBuilding)
        {
            newBuilding.transform.position = new Vector2(UtilsClass.GetMouseWorldPosition().x - 1, UtilsClass.GetMouseWorldPosition().y + 0.25f);
        }

        if (Input.GetKey(KeyCode.Escape) && placingBuilding)
        {
            Destroy(newBuilding);
            
            placingBuilding = false;
        }

        if (Input.GetKey(KeyCode.Mouse0) && placingBuilding)
        {
            float posX = Mathf.RoundToInt(newBuilding.transform.position.x * 2f) / 2f;

            float posY = Mathf.RoundToInt(newBuilding.transform.position.y * 4f) / 4f - 0.25f;

            int x = Mathf.CeilToInt((posY * 4f + posX * 2f - gridManager.gridWidth) / 2f + gridManager.gridWidth);

            int y = Mathf.CeilToInt(posX * 2f - (x - gridManager.gridWidth));

            if ((gridManager.tileMap[x - 1, y - 1] != 999)&&
                (gridManager.tileMap[x - 1, y] != 999) &&
                (gridManager.tileMap[x - 1, y + 1] != 999) &&
                (gridManager.tileMap[x - 1, y + 2] != 999) &&

                (gridManager.tileMap[x, y - 1] != 999) &&
                (gridManager.tileMap[x, y] == 1 || gridManager.tileMap[x, y] == 3) &&
                (gridManager.tileMap[x, y + 1] == 1 || gridManager.tileMap[x, y + 1] == 3) &&
                (gridManager.tileMap[x, y + 2] != 999) &&

                (gridManager.tileMap[x + 1, y - 1] != 999) &&
                (gridManager.tileMap[x + 1, y] == 1 || gridManager.tileMap[x + 1, y] == 3) &&
                (gridManager.tileMap[x + 1, y + 1] == 1 || gridManager.tileMap[x + 1, y + 1] == 3) &&
                (gridManager.tileMap[x + 1, y + 2] != 999) &&

                (gridManager.tileMap[x + 2, y - 1] != 999) &&
                (gridManager.tileMap[x + 2, y] != 999) &&
                (gridManager.tileMap[x + 2, y + 1] != 999) &&
                (gridManager.tileMap[x + 2, y + 2] != 999))
            {
                placingBuilding = false;

                gridManager.tileMap[x, y] = 999;
                gridManager.tileMap[x + 1, y] = 999;
                gridManager.tileMap[x, y + 1] = 999;
                gridManager.tileMap[x + 1, y + 1] = 999;

                gridManager.bitMap[x, y] = 999;
                gridManager.bitMap[x + 1, y] = 999;
                gridManager.bitMap[x, y + 1] = 999;
                gridManager.bitMap[x + 1, y + 1] = 999;

                newBuilding.GetComponent<Rigidbody2D>().simulated = true;

                if (buildingData.productionType == 1)
                {
                    for (int i = x - 2; i <= x + 3; i++)
                    {
                        for (int j = y - 2; j <= y + 3; j++)
                        {
                            if (gridManager.tileMap[i, j] == 0)
                            {
                                // Debug.Log(i + " " + j);

                                buildingData.productionRate += 1 * buildingData.level;
                            }
                        }
                    }

                    woodProduction += buildingData.productionRate;
                }
                else if (buildingData.productionType == 2)
                {
                    for (int i = x - 2; i <= x + 3; i++)
                    {
                        for (int j = y - 2; j <= y + 3; j++)
                        {
                            if (gridManager.tileMap[i, j] == 2)
                            {
                                // Debug.Log(i + " " + j);

                                buildingData.productionRate += 1 * buildingData.level;
                            }
                        }
                    }

                    stoneProduction += buildingData.productionRate;
                }
                else if (buildingData.productionType == 3)
                {
                    for (int i = x - 2; i <= x + 3; i++)
                    {
                        for (int j = y - 2; j <= y + 3; j++)
                        {
                            if (gridManager.tileMap[i, j] == 5)
                            {
                                // Debug.Log(i + " " + j);

                                buildingData.productionRate += 1 * buildingData.level;
                            }
                        }
                    }

                    ironProduction += buildingData.productionRate;
                }
                else if (buildingData.productionType == 4)
                {
                    for (int i = x - 2; i <= x + 3; i++)
                    {
                        for (int j = y - 2; j <= y + 3; j++)
                        {
                            if (gridManager.tileMap[i, j] == 6)
                            {
                                // Debug.Log(i + " " + j);

                                buildingData.productionRate += 1 * buildingData.level;
                            }
                        }
                    }

                    goldProduction += buildingData.productionRate;
                }
                else if (buildingData.productionType == 5)
                {
                    for (int i = x - 2; i <= x + 3; i++)
                    {
                        for (int j = y - 2; j <= y + 3; j++)
                        {
                            if (gridManager.tileMap[i, j] == 7)
                            {
                                // Debug.Log(i + " " + j);

                                buildingData.productionRate += 1 * buildingData.level;
                            }
                        }
                    }

                    titaniumProduction += buildingData.productionRate;
                }

                Debug.Log(buildingData.productionRate);

                gameBuildings.Add(newBuilding);
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

        if (Input.GetKey(KeyCode.F11))
        {
            Debug.Log("Wood: " + woodProduction);
            Debug.Log("Stone: " + stoneProduction);
            Debug.Log("Iron: " + ironProduction);
            Debug.Log("Gold: " + goldProduction);
            Debug.Log("Titanium: " + titaniumProduction);
        }

        if (Input.GetKey(KeyCode.F12))
        {
            Debug.Log("Buildings: ");

            foreach (GameObject building in gameBuildings)
            {
                Debug.Log(building.GetComponent<Building>().productionRate);
            }
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

        _zoom = Mathf.Clamp(_zoom, 4f, 16f);

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

        // LeftClick down

        if (Input.GetMouseButtonDown(0)) 
        {
            _startPosition = UtilsClass.GetMouseWorldPosition();
        }

        // LeftClick up

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

            foreach (Collider2D collider2D in collider2DArray.Distinct())
            {
                Unit unit = collider2D.GetComponent<Unit>();

                if (unit != null && collider2DArray.Count(x => x.GetComponent<Unit>() == unit) >= 2)
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
