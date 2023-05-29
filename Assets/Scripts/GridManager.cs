using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] GameObject[] tile;
    [SerializeField] public int gridHeight = 100;
    [SerializeField] public int gridWidth = 100;
    [SerializeField] public float tileSize = 1f;
    [SerializeField] float magnification = 10.0f;
    [SerializeField] float grassMnification = 4.0f;
    [SerializeField] float oreMagnification = 16.0f;
    [SerializeField] int x_offset = 0;
    [SerializeField] int y_offset = 0;

    [SerializeField] float ironProbability = 0.25f;
    [SerializeField] float goldProbability = 0.15f;
    [SerializeField] float titaniumProbability = 0.10f;
    [SerializeField] float oreProbability = 0.2f;

    [SerializeField] float dirtProbability = 0.15f;
    [SerializeField] float grassProbability = 0.35f;
    [SerializeField] float woodProbability = 0.1f;
    [SerializeField] float waterProbability = 0.4f;
    [SerializeField] float stoneProbability = 0.6f;

    [SerializeField] float passableProbability = 0.7f;
    [SerializeField] float impassableProbability = 0.3f;

    public int[,] bitMap;

    public int[,] tileMap;
    void Start()
    {
        GenerateGrid();
    }

    private void GetTerrainPerlin(int x, int y)
    {
        float raw_perlin = Mathf.PerlinNoise((x - x_offset) / magnification, (y - y_offset) / magnification);

        float clamp_perlin = Mathf.Clamp(raw_perlin, 0.0f, 1.0f);

        if (clamp_perlin <= passableProbability)
        {
            bitMap[x, y] = 0;
        }
        else if (clamp_perlin > passableProbability)
        {
            bitMap[x, y] = 999;
        }
    }

    private void GetGrassPerlin(int x, int y)
    {
        float raw_perlin = Mathf.PerlinNoise((x - x_offset) / grassMnification, (y - y_offset) / grassMnification);

        float clamp_perlin = Mathf.Clamp(raw_perlin, 0.0f, 1.0f);

        if (clamp_perlin < grassProbability)
        {
            bitMap[x, y] = 1;
        }
        else if (clamp_perlin <= grassProbability + dirtProbability)
        {
            bitMap[x, y] = 3;
        }
    }

    private void GetOrePerlin(int x, int y)
    {
        float raw_perlin = Mathf.PerlinNoise((x - x_offset) / oreMagnification, (y - y_offset) / oreMagnification);

        float clamp_perlin = Mathf.Clamp(raw_perlin, 0.0f, 1.0f);

        if (clamp_perlin < oreProbability)
        {
            bitMap[x, y] = -2;
        }
    }

    private void GenerateBitMap() 
    {
        bitMap = new int[gridHeight,gridWidth];
    }

    private void GenerateTileMap()
    {
        tileMap = new int[gridHeight, gridWidth];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                tileMap[x, y] = -1;
            }
        }
    }

    public void CorrectBitMap()
    {
        for (int x = 1; x < gridWidth - 1; x++)
        {
            for (int y = 1; y < gridHeight - 1; y++)
            {
                if (bitMap[x, y] == 0)
                {
                    bitMap[x, y] = 999;
                }
            }
        }
    }
    private void GenerateTerrainMap()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                GetTerrainPerlin(x, y);
            }
        }

        for (int x = 1; x < gridWidth - 1; x++)
        {
            for (int y = 1; y < gridHeight - 1; y++)
            {
                if (bitMap[x, y] == 999)
                {
                    if (
                        tileMap[x - 1, y - 1] == -1 &&
                        tileMap[x - 1, y] == -1 &&
                        tileMap[x - 1, y + 1] == -1 &&
                        tileMap[x, y - 1] == -1 &&
                        tileMap[x, y + 1] == -1 &&
                        tileMap[x + 1, y - 1] == -1 &&
                        tileMap[x + 1, y] == -1 &&
                        tileMap[x + 1, y + 1] == -1
                        )
                    {
                        float random = UnityEngine.Random.Range(0.0f, 100.0f) / 100.0f;

                        if (random >= 0.0f && random < waterProbability)
                        {
                            tileMap[x, y] = 4;
                            bitMap[x, y] = 999;
                        }
                        else if (random >= waterProbability && random < waterProbability + woodProbability)
                        {
                            tileMap[x, y] = 0;
                            bitMap[x, y] = 999;
                        }
                        else if (random >= waterProbability + woodProbability && random < waterProbability + woodProbability + stoneProbability)
                        {
                            tileMap[x, y] = 2;
                            bitMap[x, y] = 999;
                        }
                    }
                    else
                    {
                        if (tileMap[x - 1, y - 1] != -1)
                        {
                            tileMap[x, y] = tileMap[x - 1, y - 1];
                            bitMap[x, y] = 999;
                        }
                        else if (tileMap[x - 1, y] != -1)
                        {
                            tileMap[x, y] = tileMap[x - 1, y];
                            bitMap[x, y] = 999;
                        }
                        else if (tileMap[x - 1, y + 1] != -1)
                        {
                            tileMap[x, y] = tileMap[x - 1, y + 1];
                            bitMap[x, y] = 999;
                        }
                        else if (tileMap[x, y - 1] != -1)
                        {
                            tileMap[x, y] = tileMap[x, y - 1];
                            bitMap[x, y] = 999;
                        }
                        else if (tileMap[x, y + 1] != -1)
                        {
                            tileMap[x, y] = tileMap[x, y + 1];
                            bitMap[x, y] = 999;
                        }
                        else if (tileMap[x + 1, y - 1] != -1)
                        {
                            tileMap[x, y] = tileMap[x + 1, y - 1];
                            bitMap[x, y] = 999;
                        }
                        else if (tileMap[x + 1, y] != -1)
                        {
                            tileMap[x, y] = tileMap[x + 1, y];
                            bitMap[x, y] = 999;
                        }
                        else if (tileMap[x + 1, y + 1] != -1)
                        {
                            tileMap[x, y] = tileMap[x + 1, y + 1];
                            bitMap[x, y] = 999;
                        }
                    }
                }
            }
        }

        for (int x = 1; x < gridWidth - 1; x++)
        {
            for (int y = 1; y < gridHeight - 1; y++)
            {
                if (bitMap[x, y] == 0)
                {
                    GetGrassPerlin(x, y);

                    tileMap[x, y] = bitMap[x, y];
                }
            }
        }

        for (int x = 1; x < gridWidth - 1; x++)
        {
            for (int y = 1; y < gridHeight - 1; y++)
            {
                if (bitMap[x, y] == 1 || bitMap[x, y] == 3)
                {
                    GetOrePerlin(x, y);
                }
            }
        }

        for (int x = 1; x < gridWidth - 1; x++)
        {
            for (int y = 1; y < gridHeight - 1; y++)
            {
                if (bitMap[x, y] == -2)
                {
                    if (
                        ( tileMap[x - 1, y - 1] == 1 || tileMap[x - 1, y - 1] == 3 ) &&
                        ( tileMap[x - 1, y] == 1 || tileMap[x - 1, y] == 3 ) &&
                        ( tileMap[x - 1, y + 1] == 1 || tileMap[x - 1, y + 1] ==3 ) &&
                        ( tileMap[x, y - 1] == 1 || tileMap[x, y - 1] == 3 ) &&
                        ( tileMap[x, y + 1] == 1 || tileMap[x, y + 1] == 3 ) &&
                        ( tileMap[x + 1, y - 1] == 1 || tileMap[x + 1, y - 1] == 3 ) &&
                        ( tileMap[x + 1, y] == 1 || tileMap[x + 1, y] == 3 ) &&
                        ( tileMap[x + 1, y + 1] == 1 || tileMap[x + 1, y + 1] == 3 )
                        )
                    {
                        float random = UnityEngine.Random.Range(0.0f, 100.0f) / 100.0f;

                        if (random >= 0.0f && random < ironProbability)
                        {
                            tileMap[x, y] = 5;
                        }
                        else if (random >= ironProbability && random < ironProbability + goldProbability)
                        {
                            tileMap[x, y] = 6;
                        }
                        else if (random >= ironProbability + goldProbability && random < ironProbability + goldProbability + titaniumProbability)
                        {
                            tileMap[x, y] = 7;
                        }
                    }
                    else
                    {
                        if (!(tileMap[x - 1, y - 1] == 1 || tileMap[x - 1, y - 1] == 3))
                        {
                            tileMap[x, y] = tileMap[x - 1, y - 1];
                        }
                        else if (!(tileMap[x - 1, y] == 1 || tileMap[x - 1, y] == 3))
                        {
                            tileMap[x, y] = tileMap[x - 1, y];
                        }
                        else if (!(tileMap[x - 1, y + 1] == 1 || tileMap[x - 1, y + 1] == 3))
                        {
                            tileMap[x, y] = tileMap[x - 1, y + 1];
                        }
                        else if (!(tileMap[x, y - 1] == 1 || tileMap[x, y - 1] == 3))
                        {
                            tileMap[x, y] = tileMap[x, y - 1];
                        }
                        else if (!(tileMap[x, y + 1] == 1 || tileMap[x, y + 1] == 3))
                        {
                            tileMap[x, y] = tileMap[x, y + 1];
                        }
                        else if (!(tileMap[x + 1, y - 1] == 1 || tileMap[x + 1, y - 1] == 3))
                        {
                            tileMap[x, y] = tileMap[x + 1, y - 1];
                        }
                        else if (!(tileMap[x + 1, y] == 1 || tileMap[x + 1, y] == 3))
                        {
                            tileMap[x, y] = tileMap[x + 1, y];
                        }
                        else if (!(tileMap[x + 1, y + 1] == 1 || tileMap[x + 1, y + 1] == 3))
                        {
                            tileMap[x, y] = tileMap[x + 1, y + 1];
                        }
                    }
                }
            }
        }
    }

    private void GenerateGrid()
    {
        GenerateBitMap();
         
        GenerateTileMap();

        GenerateTerrainMap();

        CorrectBitMap();

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (tileMap[x, y] != -1)
                {
                    GameObject newTile = Instantiate(tile[tileMap[x, y]], transform);

                    float posX = ((x - gridWidth) * tileSize + y * tileSize) / 2f;

                    float posY = (x * tileSize - y * tileSize) / 4f;

                    newTile.transform.position = new Vector2(posX, posY);

                    newTile.name = x + " , " + y + " : " + posX + " , " + posY + " , " + bitMap[x,y];
                }
            }
        }
    }
    public GridManager GetGridManager()
    {
        return this;
    }
}

// posX * 2f - (x - gridWidth) = y;
// (posY * 4f + posX * 2f - gridWidth) / 2f = x);