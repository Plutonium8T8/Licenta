using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using static UnityEngine.UI.CanvasScaler;

public class GameController : MonoBehaviour
{
    [SerializeField] private CameraController _camera;

    [SerializeField] private float _edgeSize = 10f;

    [SerializeField] public GameObject HeavyCommander;

    [SerializeField] public GameObject Soldier;

    [SerializeField] public GameObject Scout;

    [SerializeField] public GameObject Sniper;

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

        CommandCenter.GetComponent<Building>().isPlaced = true;

        gameBuildings = new List<GameObject>();

        transform.Find("BuildingUI").transform.Find("WoodCutterButton").gameObject.GetComponent<Button>().onClick.AddListener(PlaceWoodCutterButton);

        transform.Find("BuildingUI").transform.Find("StoneMineButton").gameObject.GetComponent<Button>().onClick.AddListener(PlaceStoneMineButton);

        transform.Find("BuildingUI").transform.Find("IronMineButton").gameObject.GetComponent<Button>().onClick.AddListener(PlaceIronMineButton);

        transform.Find("BuildingUI").transform.Find("GoldMineButton").gameObject.GetComponent<Button>().onClick.AddListener(PlaceGoldMineButton);

        transform.Find("BuildingUI").transform.Find("TitaniumMineButton").gameObject.GetComponent<Button>().onClick.AddListener(PlaceTitaniumMineButton);

        transform.Find("BuildingUI").transform.Find("HouseButton").gameObject.GetComponent<Button>().onClick.AddListener(PlaceHouseButton);

        transform.Find("BuildingUI").transform.Find("FarmButton").gameObject.GetComponent<Button>().onClick.AddListener(PlaceFarmButton);

        transform.Find("BuildingUI").transform.Find("HuntersHutButton").gameObject.GetComponent<Button>().onClick.AddListener(PlaceHuntersHutButton);

        transform.Find("BuildingUI").transform.Find("FishermansHutButton").gameObject.GetComponent<Button>().onClick.AddListener(PlaceFishermanHutButton);

        transform.Find("BuildingUI").transform.Find("MilitaryAcademyButton").gameObject.GetComponent<Button>().onClick.AddListener(PlaceMilitaryAcademyButton);

        transform.Find("BuildingUI").transform.Find("WallButton").gameObject.GetComponent<Button>().onClick.AddListener(PlaceWallButton);


        transform.Find("MilitaryAcademyUI").transform.Find("CreateHeavyCommander").gameObject.GetComponent<Button>().onClick.AddListener(CreateHeavyCommander);

        transform.Find("MilitaryAcademyUI").transform.Find("CreateSoldier").gameObject.GetComponent<Button>().onClick.AddListener(CreateSoldier);

        transform.Find("MilitaryAcademyUI").transform.Find("CreateScout").gameObject.GetComponent<Button>().onClick.AddListener(CreateScout);

        transform.Find("MilitaryAcademyUI").transform.Find("CreateSniper").gameObject.GetComponent<Button>().onClick.AddListener(CreateSniper);


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

    private  void PlaceWoodCutterButton()
    {
        newBuilding = Instantiate(buildings[0], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

        newBuilding.GetComponent<Rigidbody2D>().simulated = false;

        buildingData = newBuilding.GetComponent<Building>();

        buildingData.productionType = 1;

        placingBuilding = true;
    }

    private void PlaceStoneMineButton()
    {
        newBuilding = Instantiate(buildings[1], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

        newBuilding.GetComponent<Rigidbody2D>().simulated = false;

        buildingData = newBuilding.GetComponent<Building>();

        buildingData.productionType = 2;

        placingBuilding = true;
    }

    private void PlaceIronMineButton()
    {
        newBuilding = Instantiate(buildings[2], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

        newBuilding.GetComponent<Rigidbody2D>().simulated = false;

        buildingData = newBuilding.GetComponent<Building>();

        buildingData.productionType = 3;

        placingBuilding = true;
    }

    private void PlaceGoldMineButton()
    {
        newBuilding = Instantiate(buildings[3], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

        newBuilding.GetComponent<Rigidbody2D>().simulated = false;

        buildingData = newBuilding.GetComponent<Building>();

        buildingData.productionType = 4;

        placingBuilding = true;
    }

    private void PlaceTitaniumMineButton()
    {
        newBuilding = Instantiate(buildings[4], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

        newBuilding.GetComponent<Rigidbody2D>().simulated = false;

        buildingData = newBuilding.GetComponent<Building>();

        buildingData.productionType = 5;

        placingBuilding = true;
    }

    private void PlaceFarmButton()
    {
        newBuilding = Instantiate(buildings[5], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

        newBuilding.GetComponent<Rigidbody2D>().simulated = false;

        buildingData = newBuilding.GetComponent<Building>();

        buildingData.productionType = 6;

        placingBuilding = true;
    }

    private void PlaceFishermanHutButton()
    {
        newBuilding = Instantiate(buildings[6], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

        newBuilding.GetComponent<Rigidbody2D>().simulated = false;

        buildingData = newBuilding.GetComponent<Building>();

        buildingData.productionType = 7;

        placingBuilding = true;
    }

    private void PlaceHuntersHutButton()
    {
        newBuilding = Instantiate(buildings[7], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

        newBuilding.GetComponent<Rigidbody2D>().simulated = false;

        buildingData = newBuilding.GetComponent<Building>();

        buildingData.productionType = 8;

        placingBuilding = true;
    }

    private void PlaceHouseButton()
    {
        newBuilding = Instantiate(buildings[8], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

        newBuilding.GetComponent<Rigidbody2D>().simulated = false;

        buildingData = newBuilding.GetComponent<Building>();

        buildingData.productionType = 9;

        placingBuilding = true;
    }

    private void PlaceWallButton()
    {
        if (buildingWall && !placingBuilding)
        {
            newBuilding = Instantiate(buildings[9], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

            newBuilding.GetComponent<Rigidbody2D>().simulated = false;

            buildingData = newBuilding.GetComponent<Building>();

            buildingData.productionType = 10;

            placingBuilding = true;
        }

        newBuilding = Instantiate(buildings[9], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

        

        buildingData = newBuilding.GetComponent<Building>();

        buildingData.productionType = 10;

        placingBuilding = true;

        buildingWall = true;
    }

    private void PlaceMilitaryAcademyButton()
    {
        newBuilding = Instantiate(buildings[10], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

        newBuilding.GetComponent<Rigidbody2D>().simulated = false;

        buildingData = newBuilding.GetComponent<Building>();

        buildingData.productionType = 11;

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
                        buildingData.isPlaced = true;
                        
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
                        buildingData.isPlaced = true;
                        
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
                        buildingData.isPlaced = true;
                        
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
                        buildingData.isPlaced = true;
                        
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
                        buildingData.isPlaced = true;
                       
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
                        buildingData.isPlaced = true;
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
                        buildingData.isPlaced = true;
                    }
                    else if (buildingData.productionType == 11 && stoneStorage >= 16 && woodStorage >= 8 && ironStorage >= 16 && goldStorage >= 64)
                    {
                        workers += buildingData.level * 4;
                        engineers += buildingData.level * 2;
                        goldProduction += buildingData.level * 4;
                        woodStorage -= 8;
                        stoneStorage -= 16;
                        ironStorage -= 16;
                        goldStorage -= 64;

                        gridManager.tileMap[x, y] = 999;
                        gridManager.tileMap[x + 1, y] = 999;
                        gridManager.tileMap[x, y + 1] = 999;
                        gridManager.tileMap[x + 1, y + 1] = 999;

                        gridManager.bitMap[x, y] = 999;
                        gridManager.bitMap[x + 1, y] = 999;
                        gridManager.bitMap[x, y + 1] = 999;
                        gridManager.bitMap[x + 1, y + 1] = 999;

                        placingBuilding = false;
                        buildingData.isPlaced = true;
                    }

                    gameBuildings.Add(newBuilding);
                }
            }

            if (buildingData.productionType == 7 || buildingData.productionType == 8 && placingBuilding)
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

                        buildingData.isPlaced = true;

                        gridManager.tileMap[x, y] = 999;

                        gridManager.bitMap[x, y] = 999;

                        
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

                        buildingData.isPlaced = true;

                        gridManager.tileMap[x, y] = 999;

                        gridManager.bitMap[x, y] = 999;
                    }

                    gameBuildings.Add(newBuilding);
                }
            }

            if (buildingData.productionType == 10 && woodStorage >= 1 && placingBuilding && stoneStorage >= 1)
            {
                float posX = Mathf.RoundToInt(newBuilding.transform.position.x * 2f) / 2f;

                float posY = Mathf.RoundToInt(newBuilding.transform.position.y * 4f) / 4f;

                int x = Mathf.CeilToInt((posY * 4f + posX * 2f - gridManager.gridWidth) / 2f + gridManager.gridWidth) - 1;

                int y = Mathf.CeilToInt(posX * 2f - (x - gridManager.gridWidth));

                if ((gridManager.tileMap[x, y] == 1 || gridManager.tileMap[x, y] == 3) && woodStorage >= 1 && stoneStorage >= 1)
                {
                    placingBuilding = false;

                    buildingData.isPlaced = true;

                    gridManager.tileMap[x, y] = 999;

                    gridManager.bitMap[x, y] = 999;

                    gameBuildings.Add(newBuilding);

                    woodStorage -= 1;

                    stoneStorage -= 1;
                }
            }

            if (buildingData.isPlaced) 
            { 
                newBuilding.GetComponent<Rigidbody2D>().simulated = true;
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

        if (Input.GetMouseButtonUp(0) && !placingBuilding)
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

                    if (building != null && collider2DArray.Count(x => x.GetComponent<Building>() == building) >= 2)
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

    public void ToggleBuildingMenu()
    {
        buildingMenuOpen = !buildingMenuOpen;

        transform.Find("BuildingUI").transform.Find("WoodCutterButton").gameObject.GetComponent<Button>().gameObject.SetActive(buildingMenuOpen);

        transform.Find("BuildingUI").transform.Find("StoneMineButton").gameObject.GetComponent<Button>().gameObject.SetActive(buildingMenuOpen);

        transform.Find("BuildingUI").transform.Find("IronMineButton").gameObject.GetComponent<Button>().gameObject.SetActive(buildingMenuOpen);

        transform.Find("BuildingUI").transform.Find("GoldMineButton").gameObject.GetComponent<Button>().gameObject.SetActive(buildingMenuOpen);

        transform.Find("BuildingUI").transform.Find("TitaniumMineButton").gameObject.GetComponent<Button>().gameObject.SetActive(buildingMenuOpen);

        transform.Find("BuildingUI").transform.Find("HouseButton").gameObject.GetComponent<Button>().gameObject.SetActive(buildingMenuOpen);

        transform.Find("BuildingUI").transform.Find("FarmButton").gameObject.GetComponent<Button>().gameObject.SetActive(buildingMenuOpen);

        transform.Find("BuildingUI").transform.Find("HuntersHutButton").gameObject.GetComponent<Button>().gameObject.SetActive(buildingMenuOpen);

        transform.Find("BuildingUI").transform.Find("FishermansHutButton").gameObject.GetComponent<Button>().gameObject.SetActive(buildingMenuOpen);

        transform.Find("BuildingUI").transform.Find("MilitaryAcademyButton").gameObject.GetComponent<Button>().gameObject.SetActive(buildingMenuOpen);

        transform.Find("BuildingUI").transform.Find("WallButton").gameObject.GetComponent<Button>().gameObject.SetActive(buildingMenuOpen);
    }
    
    private void CreateHeavyCommander()
    {
        foreach (Building building in selectedBuildingsList.Where(x => x.GetType() == typeof(MilitaryAcademy)))
        {
            if (!building.IsDestroyed())
            {
                if (engineers >= 1 && goldProduction >= 1 && ironStorage >= 1)
                {
                    ((MilitaryAcademy)building).AddToProductionQueue(HeavyCommander, ((MilitaryAcademy)building).GetLastTimingTick() + HeavyCommander.GetComponent<HeavyCommander>().GetProductionTime());
                    engineers -= 1;
                    goldProduction -= 1;
                    ironStorage -= 1;
                }
            }
        }
    }

    private void CreateSoldier()
    {
        foreach (Building building in selectedBuildingsList.Where(x => x.GetType() == typeof(MilitaryAcademy)))
        {
            if (!building.IsDestroyed())
            {
                if (engineers >= 1 && goldProduction >= 1 && ironStorage >= 1)
                {
                    ((MilitaryAcademy)building).AddToProductionQueue(Soldier, ((MilitaryAcademy)building).GetLastTimingTick() + Soldier.GetComponent<Soldier>().GetProductionTime());
                    engineers -= 1;
                    goldProduction -= 1;
                    ironStorage -= 1;
                }
            }
        }
    }

    private void CreateScout()
    {
        foreach (Building building in selectedBuildingsList.Where(x => x.GetType() == typeof(MilitaryAcademy)))
        {
            if (!building.IsDestroyed())
            {
                if (engineers >= 1 && goldProduction >= 1 && ironStorage >= 1)
                {
                    ((MilitaryAcademy)building).AddToProductionQueue(Scout, ((MilitaryAcademy)building).GetLastTimingTick() + Scout.GetComponent<Scout>().GetProductionTime());
                    engineers -= 1;
                    goldProduction -= 1;
                    ironStorage -= 1;
                }
            }
        }
    }

    private void CreateSniper()
    {
        foreach (Building building in selectedBuildingsList.Where(x => x.GetType() == typeof(MilitaryAcademy)))
        {
            if (!building.IsDestroyed())
            {
                if (engineers >= 1 && goldProduction >= 1 && ironStorage >= 1)
                {
                    ((MilitaryAcademy)building).AddToProductionQueue(Sniper, ((MilitaryAcademy)building).GetLastTimingTick() + Sniper.GetComponent<Sniper>().GetProductionTime());
                    engineers -= 1;
                    goldProduction -= 1;
                    ironStorage -= 1;
                }
            }
        }
    }

    private void MilitaryAcademyCreateUnitsUI()
    {
        if (!buildingMenuOpen)
        {
            if (selectedBuildingsList.Where(x => x.GetType() == typeof(MilitaryAcademy)).Count() > 0)
            {
                transform.Find("MilitaryAcademyUI").transform.Find("CreateHeavyCommander").gameObject.GetComponent<Button>().gameObject.SetActive(true);

                transform.Find("MilitaryAcademyUI").transform.Find("CreateSoldier").gameObject.GetComponent<Button>().gameObject.SetActive(true);

                transform.Find("MilitaryAcademyUI").transform.Find("CreateScout").gameObject.GetComponent<Button>().gameObject.SetActive(true);

                transform.Find("MilitaryAcademyUI").transform.Find("CreateSniper").gameObject.GetComponent<Button>().gameObject.SetActive(true);
            }
            else
            {
                if (transform.Find("MilitaryAcademyUI").transform.Find("CreateHeavyCommander").gameObject.GetComponent<Button>().gameObject.activeInHierarchy)
                {
                    transform.Find("MilitaryAcademyUI").transform.Find("CreateHeavyCommander").gameObject.GetComponent<Button>().gameObject.SetActive(false);

                    transform.Find("MilitaryAcademyUI").transform.Find("CreateSoldier").gameObject.GetComponent<Button>().gameObject.SetActive(false);

                    transform.Find("MilitaryAcademyUI").transform.Find("CreateScout").gameObject.GetComponent<Button>().gameObject.SetActive(false);

                    transform.Find("MilitaryAcademyUI").transform.Find("CreateSniper").gameObject.GetComponent<Button>().gameObject.SetActive(false);
                }
            }
        }
    }

    public void CreateEnemy()
    {
        Instantiate(enemy, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
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

        PlaceMilitaryAcademy(KeyCode.F11);

        PlacingBuilding();

        CancelPlacingBuilding();

        ConfirmPlacingBuilding();

        UpdateBuildingsList();

        CameraToCenter(KeyCode.LeftAlt);

        DeleteBuildings(KeyCode.Delete);

        MilitaryAcademyCreateUnitsUI();
    }
    private void Update()
    {
        OnMouseControls();

        MouseEdgeCameraMovement();

        WASDCameraMovement();
    }
}
