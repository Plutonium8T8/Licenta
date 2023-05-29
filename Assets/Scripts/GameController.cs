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

    [SerializeField] public GameObject unit;

    [SerializeField] public GameObject enemy;

    [SerializeField] public GameObject[] buildings;

    [SerializeField] public GameObject CommandCenter;

    [SerializeField] public Grid grid;


    private Vector3 cameraPosition = new Vector3(0, 0);

    private Vector2 _startPosition;

    private List<Unit> selectedEntitiesList;

    private List<Building> selectedBuildingsList;

    private List<Building> productionBuildingsList;

    private List<GameObject> gameBuildings;

    private Building buildingData;

    private GameObject newBuilding;

    private GridManager gridManager;

    [SerializeField] private Text prodStats;

    [SerializeField] private Button[] constructionButtons;


    private float _zoom = 12f;

    private float _zoomSpeed = 100f;

    private float _cameraMoveAmount = 10f;

    private float _mapHeight = 50f;

    private float _mapWidth = 50f;


    private int woodProduction = 0;

    private int stoneProduction = 0;

    private int ironProduction = 0;

    private int goldProduction = 20;

    private int titaniumProduction = 0;


    private int woodStorage = 999;

    private int stoneStorage = 999;

    private int ironStorage = 0;

    private int goldStorage = 100;

    private int titaniumStorage = 0;

    private int foodStorage = 20;

    private int workers = 20;

    private int engineers = 10;


    private int productionTickRate = 50;

    private int currentTick = -1;


    private bool placingBuilding = false;

    private bool buildingWall = false;

    private bool buildingMenuOpen = false;

    private void Start()
    {
        _camera.Setup(() => cameraPosition, () => _zoom);

        gridManager = grid.GetComponent<GridManager>();

        Instantiate(CommandCenter, new Vector3(0f,0f), Quaternion.identity);

        gameBuildings = new List<GameObject>();

        foreach (Button button in constructionButtons)
        {
            button.gameObject.SetActive(buildingMenuOpen);
            button.onClick.AddListener(PlaceWoodCutterButton);
        }

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
        if (buildingWall && !placingBuilding)
        {
            newBuilding = Instantiate(buildings[9], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

            newBuilding.GetComponent<Rigidbody2D>().simulated = false;

            buildingData = newBuilding.GetComponent<Building>();

            buildingData.productionType = 10;

            placingBuilding = true;
        }

        if (Input.GetKey(key) && !placingBuilding)
        {
            newBuilding = Instantiate(buildings[9], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

            newBuilding.GetComponent<Rigidbody2D>().simulated = false;

            buildingData = newBuilding.GetComponent<Building>();

            buildingData.productionType = 10;

            placingBuilding = true;

            buildingWall = true;
        }
    }

    private void PlaceMilitaryAcademy(KeyCode key)
    {
        if (Input.GetKey(key) && !placingBuilding)
        {
            newBuilding = Instantiate(buildings[10], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

            newBuilding.GetComponent<Rigidbody2D>().simulated = false;

            buildingData = newBuilding.GetComponent<Building>();

            buildingData.productionType = 11;

            placingBuilding = true;
        }
    }

    private void PlaceWoodCutterButton()
    {
        newBuilding = Instantiate(buildings[0], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

        newBuilding.GetComponent<Rigidbody2D>().simulated = false;

        buildingData = newBuilding.GetComponent<Building>();

        buildingData.productionType = 1;

        placingBuilding = true;
    }

    private void CameraToCenter(KeyCode key)
    {
        if (Input.GetKeyUp(key))
        {
            cameraPosition.x = 0;
            cameraPosition.y = 0;
        }
    }

    private void DeleteBuildings(KeyCode key)
    {
        if (Input.GetKey(key))
        {
            foreach (Building building in selectedBuildingsList)
            {
                Destroy(building.GameObject());
            }

            selectedBuildingsList.Clear();
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
                buildingData.productionType == 9 ||
                buildingData.productionType == 11)
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

            if (buildingWall)
            {
                buildingWall = false;
            }

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
                buildingData.productionType == 9 ||
                buildingData.productionType == 11)
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
                    if (buildingData.productionType == 1 && woodStorage >= 4 && goldStorage >= 4 && goldProduction >= 1 && workers >= 2)
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

                        workers -= 2;
                        woodStorage -= 4;
                        goldStorage -= 4;
                        goldProduction -= 1;
                        woodProduction += buildingData.productionRate;

                        gridManager.tileMap[x, y] = 999;
                        gridManager.tileMap[x + 1, y] = 999;
                        gridManager.tileMap[x, y + 1] = 999;
                        gridManager.tileMap[x + 1, y + 1] = 999;

                        gridManager.bitMap[x, y] = 999;
                        gridManager.bitMap[x + 1, y] = 999;
                        gridManager.bitMap[x, y + 1] = 999;
                        gridManager.bitMap[x + 1, y + 1] = 999;

                        placingBuilding = false;
                        newBuilding.GetComponent<Rigidbody2D>().simulated = true;
                    }
                    else if (buildingData.productionType == 2 && woodStorage >= 4 && goldStorage >= 4 && goldProduction >= 2 && workers >= 2)
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

                        workers -= 2;
                        woodStorage -= 4;
                        goldStorage -= 4;
                        goldProduction -= 2;
                        stoneProduction += buildingData.productionRate;

                        gridManager.tileMap[x, y] = 999;
                        gridManager.tileMap[x + 1, y] = 999;
                        gridManager.tileMap[x, y + 1] = 999;
                        gridManager.tileMap[x + 1, y + 1] = 999;

                        gridManager.bitMap[x, y] = 999;
                        gridManager.bitMap[x + 1, y] = 999;
                        gridManager.bitMap[x, y + 1] = 999;
                        gridManager.bitMap[x + 1, y + 1] = 999;

                        placingBuilding = false;
                        newBuilding.GetComponent<Rigidbody2D>().simulated = true;
                    }
                    else if (buildingData.productionType == 3 && woodStorage >= 4 && goldStorage >= 8 && goldProduction >= 4 && stoneStorage >= 4 && workers >= 2)
                    {
                        for (int i = x - 2; i <= x + 3; i++)
                        {
                            for (int j = y - 2; j <= y + 3; j++)
                            {
                                if (gridManager.tileMap[i, j] == 5)
                                {
                                    buildingData.productionRate += 2 * buildingData.level; 
                                }
                            }
                        }

                        workers -= 2;
                        woodStorage -= 4;
                        goldStorage -= 8;
                        goldProduction -= 4;
                        stoneStorage -= 4;
                        ironProduction += buildingData.productionRate;

                        gridManager.tileMap[x, y] = 999;
                        gridManager.tileMap[x + 1, y] = 999;
                        gridManager.tileMap[x, y + 1] = 999;
                        gridManager.tileMap[x + 1, y + 1] = 999;

                        gridManager.bitMap[x, y] = 999;
                        gridManager.bitMap[x + 1, y] = 999;
                        gridManager.bitMap[x, y + 1] = 999;
                        gridManager.bitMap[x + 1, y + 1] = 999;

                        placingBuilding = false;
                        newBuilding.GetComponent<Rigidbody2D>().simulated = true;
                    }
                    else if (buildingData.productionType == 4 && woodStorage >= 4 && goldStorage >= 16 && ironStorage >= 8 && workers >= 2)
                    {
                        for (int i = x - 2; i <= x + 3; i++)
                        {
                            for (int j = y - 2; j <= y + 3; j++)
                            {
                                if (gridManager.tileMap[i, j] == 6)
                                {
                                    buildingData.productionRate += 8 * buildingData.level;
                                }
                            }
                        }

                        workers -= 2;
                        woodStorage -= 4;
                        goldStorage -= 16;
                        ironStorage -= 8;
                        goldProduction += buildingData.productionRate;

                        gridManager.tileMap[x, y] = 999;
                        gridManager.tileMap[x + 1, y] = 999;
                        gridManager.tileMap[x, y + 1] = 999;
                        gridManager.tileMap[x + 1, y + 1] = 999;

                        gridManager.bitMap[x, y] = 999;
                        gridManager.bitMap[x + 1, y] = 999;
                        gridManager.bitMap[x, y + 1] = 999;
                        gridManager.bitMap[x + 1, y + 1] = 999;

                        placingBuilding = false;
                        newBuilding.GetComponent<Rigidbody2D>().simulated = true;
                    }
                    else if (buildingData.productionType == 5 && woodStorage >= 4 && goldStorage >= 16 && goldProduction >= 16 && ironStorage >= 16 && workers >= 2)
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

                        workers -= 2;
                        woodStorage -= 4;
                        goldStorage -= 16;
                        goldProduction -= 16;
                        ironStorage -= 16;
                        titaniumProduction += buildingData.productionRate;

                        gridManager.tileMap[x, y] = 999;
                        gridManager.tileMap[x + 1, y] = 999;
                        gridManager.tileMap[x, y + 1] = 999;
                        gridManager.tileMap[x + 1, y + 1] = 999;

                        gridManager.bitMap[x, y] = 999;
                        gridManager.bitMap[x + 1, y] = 999;
                        gridManager.bitMap[x, y + 1] = 999;
                        gridManager.bitMap[x + 1, y + 1] = 999;

                        placingBuilding = false;
                        newBuilding.GetComponent<Rigidbody2D>().simulated = true;
                    }
                    else if (buildingData.productionType == 6 && woodStorage >= 4 && goldStorage >= 4 && goldProduction >= 2 && workers >= 2)
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

                        workers -= 2;
                        woodStorage -= 4;
                        goldStorage -= 4;
                        goldProduction -= 2;
                        foodStorage += buildingData.productionRate;

                        gridManager.tileMap[x, y] = 999;
                        gridManager.tileMap[x + 1, y] = 999;
                        gridManager.tileMap[x, y + 1] = 999;
                        gridManager.tileMap[x + 1, y + 1] = 999;

                        gridManager.bitMap[x, y] = 999;
                        gridManager.bitMap[x + 1, y] = 999;
                        gridManager.bitMap[x, y + 1] = 999;
                        gridManager.bitMap[x + 1, y + 1] = 999;

                        placingBuilding = false;
                        newBuilding.GetComponent<Rigidbody2D>().simulated = true;
                    }
                    else if (buildingData.productionType == 9 && foodStorage >= 3 && woodStorage >= 4 && goldStorage >= 2)
                    {
                        workers += buildingData.level * 4;
                        engineers += buildingData.level * 2;
                        goldProduction += buildingData.level * 4;
                        woodStorage -= 4;
                        foodStorage -= 3;
                        goldStorage -= 2;

                        gridManager.tileMap[x, y] = 999;
                        gridManager.tileMap[x + 1, y] = 999;
                        gridManager.tileMap[x, y + 1] = 999;
                        gridManager.tileMap[x + 1, y + 1] = 999;

                        gridManager.bitMap[x, y] = 999;
                        gridManager.bitMap[x + 1, y] = 999;
                        gridManager.bitMap[x, y + 1] = 999;
                        gridManager.bitMap[x + 1, y + 1] = 999;

                        placingBuilding = false;
                        newBuilding.GetComponent<Rigidbody2D>().simulated = true;
                    }
                    else if (buildingData.productionType == 11 && stoneStorage >= 5 && woodStorage >= 4 && goldStorage >= 2)
                    {
                        workers += buildingData.level * 4;
                        engineers += buildingData.level * 2;
                        goldProduction += buildingData.level * 4;
                        woodStorage -= 4;
                        stoneStorage -= 5;
                        goldStorage -= 2;

                        gridManager.tileMap[x, y] = 999;
                        gridManager.tileMap[x + 1, y] = 999;
                        gridManager.tileMap[x, y + 1] = 999;
                        gridManager.tileMap[x + 1, y + 1] = 999;

                        gridManager.bitMap[x, y] = 999;
                        gridManager.bitMap[x + 1, y] = 999;
                        gridManager.bitMap[x, y + 1] = 999;
                        gridManager.bitMap[x + 1, y + 1] = 999;

                        placingBuilding = false;
                        newBuilding.GetComponent<Rigidbody2D>().simulated = true;
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
                    if (buildingData.productionType == 8 && woodStorage >= 2 && goldStorage >= 2 && goldProduction >= 1 && workers >= 1)
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

                        workers -= 1;
                        woodStorage -= 2;
                        goldStorage -= 2;
                        goldProduction -= 1;
                        foodStorage += buildingData.productionRate;

                        placingBuilding = false;

                        gridManager.tileMap[x, y] = 999;

                        gridManager.bitMap[x, y] = 999;

                        newBuilding.GetComponent<Rigidbody2D>().simulated = true;
                    }
                    else if (buildingData.productionType == 7 && woodStorage >= 2 && goldStorage >= 2 && goldProduction >= 1 && workers >= 1)
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

                        workers -= 1;
                        woodStorage -= 2;
                        goldStorage -= 2;
                        goldProduction -= 1;
                        foodStorage += buildingData.productionRate;

                        placingBuilding = false;

                        gridManager.tileMap[x, y] = 999;

                        gridManager.bitMap[x, y] = 999;

                        newBuilding.GetComponent<Rigidbody2D>().simulated = true;
                    }

                    gameBuildings.Add(newBuilding);
                }
            }

            if (buildingData.productionType == 10 && woodStorage >= 1)
            {
                float posX = Mathf.RoundToInt(newBuilding.transform.position.x * 2f) / 2f;

                float posY = Mathf.RoundToInt(newBuilding.transform.position.y * 4f) / 4f;

                int x = Mathf.CeilToInt((posY * 4f + posX * 2f - gridManager.gridWidth) / 2f + gridManager.gridWidth) - 1;

                int y = Mathf.CeilToInt(posX * 2f - (x - gridManager.gridWidth));

                if (gridManager.tileMap[x, y] == 1 || gridManager.tileMap[x, y] == 3 && woodStorage >= 1)
                {
                    placingBuilding = false;

                    gridManager.tileMap[x, y] = 999;

                    gridManager.bitMap[x, y] = 999;

                    newBuilding.GetComponent<Rigidbody2D>().simulated = true;

                    gameBuildings.Add(newBuilding);

                    woodStorage -= 1;
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
            goldProduction = 20;
            titaniumProduction = 0;
            workers = 20;
            engineers = 10;

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
                if (collider2D.GetComponent<Unit>() != null)
                {
                    Unit unit = collider2D.GetComponent<Unit>();

                    if (unit != null && collider2DArray.Count(x => x.GetComponent<Unit>() == unit) >= 2)
                    {
                        unit.SetSelectedVisible(true);
                        selectedEntitiesList.Add(unit);
                    }
                }
                else
                {
                    Building building = collider2D.GetComponent<Building>();                

                    if (building != null)
                    {
                        if (building.productionType == 11)
                        {
                            selectedBuildingsList.Add(building);
                        }
                    }

                }
            }
        }

        // RightClick

        if (Input.GetMouseButtonDown(1))
        {
            Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(UtilsClass.GetMouseWorldPosition(), UtilsClass.GetMouseWorldPosition());

            Enemy target = null;

            foreach (Collider2D collider2D in collider2DArray.Distinct())
            {
                Enemy enemy = collider2D.GetComponent<Enemy>();

                if (enemy != null && collider2DArray.Count(x => x.GetComponent<Enemy>() == enemy) >= 2)
                {
                    target = enemy;
                    break;
                }
            }

            foreach (Unit unit in selectedEntitiesList)
            {
                if (target != null)
                {
                    if (!unit.IsDestroyed())
                    {
                        if (!Input.GetKey(KeyCode.LeftShift))
                        {
                            unit.actions.Clear();
                        }

                        Debug.Log(target);

                        unit.actions.Add(new Attack(target));
                    }
                }
                else
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

    private void ToggleBuildingMenu(KeyCode key)
    {
        if (Input.GetKeyUp(key))
        {
            buildingMenuOpen = !buildingMenuOpen;

            foreach (Button button in constructionButtons)
            {
                button.gameObject.SetActive(true);
            }
        }
    }

    private void FixedUpdate()
    {
        UpdateProductionStats();

        ToggleBuildingMenu(KeyCode.B);

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

        PlaceMilitaryAcademy(KeyCode.F11);

        PlacingBuilding();

        CancelPlacingBuilding();

        ConfirmPlacingBuilding();

        UpdateBuildingsList();

        CameraToCenter(KeyCode.LeftAlt);

        DeleteBuildings(KeyCode.Delete);


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

        if (Input.GetKey(KeyCode.P))
        {
            foreach (Building building in selectedBuildingsList)
            {
                if (!building.IsDestroyed())
                {
                    if (building.GetType() == typeof(MilitaryAcademy))
                    {
                        if (engineers >= 1 && goldProduction >= 1 && ironStorage >= 1)
                        {
                            ((MilitaryAcademy)building).AddToProductionQueue(unit, ((MilitaryAcademy)building).GetLastTimingTick() + unit.GetComponent<Unit>().GetProductionTime());
                            engineers -= 1;
                            goldProduction -= 1;
                            ironStorage -= 1;
                        }
                    }
                }
            }
        }

        if (Input.GetKey(KeyCode.M))
        {
            Instantiate(unit, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
        }

        if (Input.GetKey(KeyCode.O))
        {
            Instantiate(enemy, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
        }
    }
    private void Update()
    {
        OnMouseControls();

        MouseEdgeCameraMovement();

        WASDCameraMovement();
    }
}
