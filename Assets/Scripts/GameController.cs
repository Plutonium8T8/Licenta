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

    private List<int> targetTick;


    private List<Unit> selectedEntitiesList;

    private List<Building> selectedBuildingsList;

    private List<Building> productionBuildingsList;

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

    private int foodStorage = 0;

    private int workers = 0;

    private int engineers = 0;


    private int productionTickRate = 50;

    private int currentTick = -1;


    private bool placingBuilding = false;

    private bool JPressed = false;

    private bool spawningUnits = false;

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
                "Titanium: " + titaniumStorage + " (+" + titaniumProduction + ")\n" +
                "Workers: " + workers + "\n" +
                "Engineers: " + engineers + "\n" +
                "Food: " + foodStorage;

        TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickEventArgs e)
        {
            currentTick = e.tick;

            if (Application.isPlaying && e.tick % productionTickRate == 0)
            {
                woodStorage += woodProduction;
                stoneStorage += stoneProduction;
                ironStorage += ironProduction;
                goldStorage += goldProduction;
                titaniumStorage += titaniumProduction;
            }
        };
    }
    private void Awake()
    {
        selectedEntitiesList = new List<Unit>();

        selectedBuildingsList = new List<Building>();

        productionBuildingsList = new List<Building>();

        targetTick = new List<int>();

        _camera.Setup(() => cameraPosition, () => _zoom);
    }

    private void UpdateProductionStats()
    {
        prodStats.text =
                "Wood: " + woodStorage + " (+" + woodProduction + ")\n" +
                "Stone: " + stoneStorage + " (+" + stoneProduction + ")\n" +
                "Iron: " + ironStorage + " (+" + ironProduction + ")\n" +
                "Gold: " + goldStorage + " (+" + goldProduction + ")\n" +
                "Titanium: " + titaniumStorage + " (+" + titaniumProduction + ")\n" +
                "Workers: " + workers + "\n" +
                "Engineers: " + engineers + "\n" +
                "Food: " + foodStorage;
    }

    private void PlaceWoodCutter(KeyCode key)
    {
        if (Input.GetKey(key) && !placingBuilding)
        {
            newBuilding = Instantiate(buildings[0], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

            newBuilding.GetComponent<Rigidbody2D>().simulated = false;

            buildingData = newBuilding.GetComponent<Building>();

            buildingData.productionType = 1;

            placingBuilding = true;
        }
    }

    private void PlaceStoneMine(KeyCode key)
    {
        if (Input.GetKey(key) && !placingBuilding)
        {
            newBuilding = Instantiate(buildings[1], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

            newBuilding.GetComponent<Rigidbody2D>().simulated = false;

            buildingData = newBuilding.GetComponent<Building>();

            buildingData.productionType = 2;

            placingBuilding = true;
        }
    }

    private void PlaceIronMine(KeyCode key)
    {
        if (Input.GetKey(key) && !placingBuilding)
        {
            newBuilding = Instantiate(buildings[2], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

            newBuilding.GetComponent<Rigidbody2D>().simulated = false;

            buildingData = newBuilding.GetComponent<Building>();

            buildingData.productionType = 3;

            placingBuilding = true;
        }
    }

    private void PlaceGoldMine(KeyCode key)
    {
        if (Input.GetKey(key) && !placingBuilding)
        {
            newBuilding = Instantiate(buildings[3], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

            newBuilding.GetComponent<Rigidbody2D>().simulated = false;

            buildingData = newBuilding.GetComponent<Building>();

            buildingData.productionType = 4;

            placingBuilding = true;
        }
    }

    private void PlaceTitaniumMine(KeyCode key)
    {
        if (Input.GetKey(key) && !placingBuilding)
        {
            newBuilding = Instantiate(buildings[4], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

            newBuilding.GetComponent<Rigidbody2D>().simulated = false;

            buildingData = newBuilding.GetComponent<Building>();

            buildingData.productionType = 5;

            placingBuilding = true;
        }
    }

    private void PlaceFarm(KeyCode key)
    {
        if (Input.GetKey(key) && !placingBuilding)
        {
            newBuilding = Instantiate(buildings[5], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

            newBuilding.GetComponent<Rigidbody2D>().simulated = false;

            buildingData = newBuilding.GetComponent<Building>();

            buildingData.productionType = 6;

            placingBuilding = true;
        }
    }

    private void PlaceFishermanHut(KeyCode key)
    {
        if (Input.GetKey(key) && !placingBuilding)
        {
            newBuilding = Instantiate(buildings[6], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

            newBuilding.GetComponent<Rigidbody2D>().simulated = false;

            buildingData = newBuilding.GetComponent<Building>();

            buildingData.productionType = 7;

            placingBuilding = true;
        }
    }

    private void PlaceHuntersHut(KeyCode key)
    {
        if (Input.GetKey(key) && !placingBuilding)
        {
            newBuilding = Instantiate(buildings[7], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

            newBuilding.GetComponent<Rigidbody2D>().simulated = false;

            buildingData = newBuilding.GetComponent<Building>();

            buildingData.productionType = 8;

            placingBuilding = true;
        }
    }

    private void PlaceHouse(KeyCode key)
    {
        if (Input.GetKey(key) && !placingBuilding)
        {
            newBuilding = Instantiate(buildings[8], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

            newBuilding.GetComponent<Rigidbody2D>().simulated = false;

            buildingData = newBuilding.GetComponent<Building>();

            buildingData.productionType = 9;

            placingBuilding = true;
        }
    }

    private void PlaceWall(KeyCode key)
    {
        if (Input.GetKey(key) && !placingBuilding)
        {
            newBuilding = Instantiate(buildings[9], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

            newBuilding.GetComponent<Rigidbody2D>().simulated = false;

            buildingData = newBuilding.GetComponent<Building>();

            buildingData.productionType = 10;

            placingBuilding = true;
        }
    }

    private void PlacingBuilding()
    {
        if (placingBuilding)
        {
            if (buildingData.productionType == 1 ||
                buildingData.productionType == 2 ||
                buildingData.productionType == 3 ||
                buildingData.productionType == 4 ||
                buildingData.productionType == 5 ||
                buildingData.productionType == 6 ||
                buildingData.productionType == 9)
            {
                newBuilding.transform.position = new Vector2(UtilsClass.GetMouseWorldPosition().x - 1, UtilsClass.GetMouseWorldPosition().y + 0.25f);
            }
            else if (
                buildingData.productionType == 7 ||
                buildingData.productionType == 8 ||
                buildingData.productionType == 10)
            {
                newBuilding.transform.position = new Vector2(UtilsClass.GetMouseWorldPosition().x - 0.5f, UtilsClass.GetMouseWorldPosition().y);
            }
        }
    }

    private void CancelPlacingBuilding()
    {
        if (Input.GetKey(KeyCode.Escape) && placingBuilding)
        {
            Destroy(newBuilding);

            placingBuilding = false;
        }
    }

    private void ConfirmPlacingBuilding()
    {
        if (Input.GetKey(KeyCode.Mouse0) && placingBuilding)
        {
            if (buildingData.productionType == 1 ||
                buildingData.productionType == 2 ||
                buildingData.productionType == 3 ||
                buildingData.productionType == 4 ||
                buildingData.productionType == 5 ||
                buildingData.productionType == 6 ||
                buildingData.productionType == 9)
            {
                float posX = Mathf.RoundToInt(newBuilding.transform.position.x * 2f) / 2f;

                float posY = Mathf.RoundToInt(newBuilding.transform.position.y * 4f) / 4f - 0.25f;

                int x = Mathf.CeilToInt((posY * 4f + posX * 2f - gridManager.gridWidth) / 2f + gridManager.gridWidth);

                int y = Mathf.CeilToInt(posX * 2f - (x - gridManager.gridWidth));

                if ((gridManager.tileMap[x - 1, y - 1] != 999) &&
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
                                    buildingData.productionRate += 1 * buildingData.level;
                                }
                            }
                        }

                        titaniumProduction += buildingData.productionRate;
                    }
                    else if (buildingData.productionType == 6)
                    {
                        for (int i = x - 2; i <= x + 3; i++)
                        {
                            for (int j = y - 2; j <= y + 3; j++)
                            {
                                if (gridManager.tileMap[i, j] == 1)
                                {
                                    buildingData.productionRate += 1 * buildingData.level;
                                }
                            }
                        }

                        foodStorage += buildingData.productionRate;
                    }
                    else if (buildingData.productionType == 9)
                    {
                        workers += buildingData.level * 4;
                        engineers += buildingData.level * 2;
                    }

                    gameBuildings.Add(newBuilding);
                }
            }

            if (buildingData.productionType == 7 || buildingData.productionType == 8)
            {
                float posX = Mathf.RoundToInt(newBuilding.transform.position.x * 2f) / 2f;

                float posY = Mathf.RoundToInt(newBuilding.transform.position.y * 4f) / 4f;

                int x = Mathf.CeilToInt((posY * 4f + posX * 2f - gridManager.gridWidth) / 2f + gridManager.gridWidth) - 1;

                int y = Mathf.CeilToInt(posX * 2f - (x - gridManager.gridWidth));

                if ((gridManager.tileMap[x - 1, y - 1] != 999) &&
                    (gridManager.tileMap[x - 1, y] != 999) &&
                    (gridManager.tileMap[x - 1, y + 1] != 999) &&

                    (gridManager.tileMap[x, y - 1] != 999) &&
                    (gridManager.tileMap[x, y] == 1 || gridManager.tileMap[x, y] == 3) &&
                    (gridManager.tileMap[x, y + 1] != 999) &&

                    (gridManager.tileMap[x + 1, y - 1] != 999) &&
                    (gridManager.tileMap[x + 1, y] != 999) &&
                    (gridManager.tileMap[x + 1, y + 1] != 999))

                {
                    placingBuilding = false;

                    gridManager.tileMap[x, y] = 999;

                    gridManager.bitMap[x, y] = 999;

                    newBuilding.GetComponent<Rigidbody2D>().simulated = true;

                    if (buildingData.productionType == 8)
                    {
                        for (int i = x - 2; i <= x + 2; i++)
                        {
                            for (int j = y - 2; j <= y + 2; j++)
                            {
                                if (gridManager.tileMap[i, j] == 0)
                                {
                                    buildingData.productionRate += 1 * buildingData.level;
                                }
                            }
                        }

                        foodStorage += buildingData.productionRate;
                    }
                    else if (buildingData.productionType == 7)
                    {
                        for (int i = x - 2; i <= x + 2; i++)
                        {
                            for (int j = y - 2; j <= y + 2; j++)
                            {
                                if (gridManager.tileMap[i, j] == 4)
                                {
                                    buildingData.productionRate += 1 * buildingData.level;
                                }
                            }
                        }

                        foodStorage += buildingData.productionRate;
                    }

                    gameBuildings.Add(newBuilding);
                }
            }

            if (buildingData.productionType == 10)
            {
                float posX = Mathf.RoundToInt(newBuilding.transform.position.x * 2f) / 2f;

                float posY = Mathf.RoundToInt(newBuilding.transform.position.y * 4f) / 4f;

                int x = Mathf.CeilToInt((posY * 4f + posX * 2f - gridManager.gridWidth) / 2f + gridManager.gridWidth) - 1;

                int y = Mathf.CeilToInt(posX * 2f - (x - gridManager.gridWidth));

                if (gridManager.tileMap[x, y] == 1 || gridManager.tileMap[x, y] == 3)
                {
                    placingBuilding = false;

                    gridManager.tileMap[x, y] = 999;

                    gridManager.bitMap[x, y] = 999;

                    newBuilding.GetComponent<Rigidbody2D>().simulated = true;

                    gameBuildings.Add(newBuilding);
                }
            }
        }
    }

    private void UpdateBuildingsList()
    {
        if (gameBuildings.Count != gameBuildings.Where(x => !x.IsDestroyed()).Count())
        {
            woodProduction = 0;
            stoneProduction = 0;
            ironProduction = 0;
            goldProduction = 0;
            titaniumProduction = 0;
            workers = 0;
            engineers = 0;

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
                else if (buildingData.productionType == 6)
                {
                    foodStorage += buildingData.productionRate;
                }
                else if (buildingData.productionType == 7)
                {
                    foodStorage += buildingData.productionRate;
                }
                else if (buildingData.productionType == 8)
                {
                    foodStorage += buildingData.productionRate;
                }
                else if (buildingData.productionType == 8)
                {
                    workers += buildingData.level * 4;
                    engineers += buildingData.level * 2;
                }
            }
        }
    }

    private void WASDCameraMovement()
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
    }

    private void MouseEdgeCameraMovement()
    {
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
    }

    private void OnMouseControls()
    {
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

            selectedBuildingsList.Clear();

            foreach (Collider2D collider2D in collider2DArray.Distinct())
            {
                Unit unit = collider2D.GetComponent<Unit>();

                if (unit != null && collider2DArray.Count(x => x.GetComponent<Unit>() == unit) >= 2)
                {
                    unit.SetSelectedVisible(true);
                    selectedEntitiesList.Add(unit);
                }

                Building building = collider2D.GetComponent<Building>();

                if (building != null)
                {
                    if (building.productionType == 1)
                    {
                        selectedBuildingsList.Add(building);
                    }
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

    private void FixedUpdate()
    {
        UpdateProductionStats();

        PlaceWoodCutter(KeyCode.F1);

        PlaceStoneMine(KeyCode.F2);

        PlaceIronMine(KeyCode.F3);

        PlaceGoldMine(KeyCode.F4);

        PlaceTitaniumMine(KeyCode.F5);

        PlaceFarm(KeyCode.F6);

        PlaceFishermanHut(KeyCode.F7);

        PlaceHuntersHut(KeyCode.F8);

        PlaceHouse(KeyCode.F9);

        PlaceWall(KeyCode.F10);

        PlacingBuilding();

        CancelPlacingBuilding();

        ConfirmPlacingBuilding();

        UpdateBuildingsList();


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

        if (Input.GetKeyDown(KeyCode.J) && selectedBuildingsList.Count != 0 && !JPressed)
        {
            if (targetTick.Count == 0)
            {
                targetTick.Add(currentTick + productionTickRate);
            }
            else
            {
                targetTick.Add(targetTick.ElementAt(targetTick.Count - 1) + productionTickRate);
            }

            foreach (Building building in selectedBuildingsList)
            {
                if (!productionBuildingsList.Contains(building))
                {
                    productionBuildingsList.Add(building);
                }
            }

            JPressed = true;

            spawningUnits = true;

            Debug.Log(targetTick.Count);
        }

        if (Input.GetKeyUp(KeyCode.J))
        {
            JPressed = false;
        }

        if (targetTick.Count != 0)
        {
            if (targetTick.ElementAt(0) == currentTick && spawningUnits)
            {
                Debug.Log(productionBuildingsList.Count);

                foreach (Building building in productionBuildingsList)
                {
                    Instantiate(entity, new Vector3(building.transform.position.x + 1f, building.transform.position.y + 1f, 0), Quaternion.identity);
                }

                targetTick.RemoveAt(0);

                Debug.Log(targetTick.Count);

                spawningUnits = false;
            }
        }
        else
        {
            productionBuildingsList.Clear();
        }
    }
    private void Update()
    {
        OnMouseControls();

        MouseEdgeCameraMovement();

        WASDCameraMovement();
    }
}
