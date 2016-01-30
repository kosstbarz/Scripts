using UnityEngine;
using System.Collections.Generic;

public class LandscapeGenerator {

    const int HILL_RADIUS = 20;
    const float HILL_MAX_HEIGHT = 3;
    const int RIVER_STRIGHT_DIST = 4;

    
    MapData mapData;
    int xSize;
    int ySize;
    bool[,] busyMap; // Map of tiles, which are already covered with objects e.g. river, mountain, plato...

    public LandscapeGenerator(int xSize, int ySize)
    {
        
        this.xSize = xSize;
        this.ySize = ySize;
        
        busyMap = new bool[xSize, ySize];
    }

    public void FullGenerate()
    {
        mapData = MapController.Instance.mapData;
        busyMap = new bool[xSize, ySize];
        MakeBackGround();
        MakeLake();
        MakeSwamp();
        MakeLake();
        MakeSwamp();
        MakeMountain();
    }

    void MakeBackGround()
    {
        int numberOfHills = Random.Range(0, 3);
        Debug.Log("Number of hills: " + numberOfHills);
        for (int i = 0; i < numberOfHills; i++)
        {
            int x = Random.Range(0, xSize);
            int y = Random.Range(0, ySize);
            float height = Random.Range(0, HILL_MAX_HEIGHT);
            Debug.Log("Height of hill: " + height);
            ApplyBackGround(x, y, height);
        }
    }

    void ApplyBackGround(int x, int y, float height)
    {
        for (int i = Mathf.Max(0, x - HILL_RADIUS); i < Mathf.Min(xSize, x + HILL_RADIUS); i++)
        {
            for (int j = Mathf.Max(0, y - HILL_RADIUS); j < Mathf.Min(ySize, y + HILL_RADIUS); j++)
            {
                
                float dist = Mathf.Sqrt((i - x) * (i - x) + (j - y) * (j - y));
                if (dist < HILL_RADIUS)
                {
                    mapData.heightData[i, j] += (1f - dist / HILL_RADIUS) * height;
                    
                }
            }
        }
    }

    void MakeLake()
    {
        int x = Random.Range(0, xSize);
        int y = Random.Range(0, ySize);
        Debug.Log("lake x " + x);
        Debug.Log("lake y " + y);
        int radius = Random.Range(2, 10);
        Debug.Log("lake radius " + radius);
        for (int i = Mathf.Max(0, x - radius); i < Mathf.Min(xSize, x + radius); i++)
        {
            for (int j = Mathf.Max(0, y - radius); j < Mathf.Min(ySize, y + radius); j++)
            {
                float dist = Mathf.Sqrt((i - x) * (i - x) + (j - y) * (j - y));
                if (dist < radius && !busyMap[i, j])
                {
                    mapData.heightData[i, j] = 0;
                    mapData.heightData[i + 1, j] = 0;
                    mapData.heightData[i, j + 1] = 0;
                    mapData.heightData[i + 1, j + 1] = 0;
                    mapData.tileData[i, j] = mapData.GetType("Water");
                    busyMap[i, j] = true;
                }
            }
        }
    }

    void MakeSwamp()
    {
        int x = Random.Range(0, xSize);
        int y = Random.Range(0, ySize);
        int radius = Random.Range(4, 13);
        for (int i = Mathf.Max(0, x - radius); i < Mathf.Min(xSize, x + radius); i++)
        {
            for (int j = Mathf.Max(0, y - radius); j < Mathf.Min(ySize, y + radius); j++)
            {
                float dist = Mathf.Sqrt((i - x) * (i - x) + (j - y) * (j - y));
                if (dist < radius && !busyMap[i, j])
                {
                    mapData.heightData[i, j] -= 0.1f;
                    mapData.heightData[i + 1, j] -= 0.1f;
                    mapData.heightData[i, j + 1] -= 0.1f;
                    mapData.heightData[i + 1, j + 1] -= 0.1f;
                    mapData.tileData[i, j] = mapData.GetType("Swamp");
                    busyMap[i, j] = true;
                }
            }
        }
    }

    void MakeMountain()
    {
        int x = Random.Range(0, xSize);
        int y = Random.Range(0, ySize);
        int radius = Random.Range(4, 13);
        int height = Random.Range(2, 10);
        for (int i = Mathf.Max(0, x - radius); i < Mathf.Min(xSize, x + radius); i++)
        {
            for (int j = Mathf.Max(0, y - radius); j < Mathf.Min(ySize, y + radius); j++)
            {
                float dist = Mathf.Sqrt((i - x) * (i - x) + (j - y) * (j - y));
                if (dist < radius && !busyMap[i, j])
                {
                    mapData.heightData[i, j] += (1 - dist / radius) * height;
                    mapData.tileData[i, j] = mapData.GetType("Mountain");
                    busyMap[i, j] = true;
                }
            }
        }
    }

    void MakeRiver()
    {
        int start = Random.Range(0, (xSize + ySize) * 2);
        Vector2[] point = PerimeterConverter(start);// Point[0] is coordinate, point[1] is direction from previous to current
        List<Vector2[]> river = new List<Vector2[]>();
        river.Add(point);
        int count = 0;
        
    }

    

    Vector2[] PerimeterConverter(int perimeter)
    {
        Vector2[] res = new Vector2[2];
        if (perimeter < xSize)
        {
            res[0].x = perimeter;
            res[0].y = 0;
            res[1].x = 0;
            res[1].y = 1;
            return res;
        } else
        {
            perimeter -= xSize;
        }
        if (perimeter < ySize)
        {
            res[0].y = perimeter;
            res[0].x = xSize - 1;
            res[1].x = -1;
            res[1].y = 0;
            return res;
        }
        else
        {
            perimeter -= ySize;
        }
        if (perimeter < xSize)
        {
            res[0].x = xSize - 1 - perimeter;
            res[0].y = ySize - 1;
            res[1].x = 0;
            res[1].y = -1;
            return res;
        }
        res[0].y = ySize - 1 - perimeter;
        res[0].x = 0;
        res[1].x = 1;
        res[1].y = 0;
        return res;
    }

    Vector2[] RiverNextStep(Vector2[] currStep, int dist)
    {
        Vector2[] res = new Vector2[2];
        if (dist >= RIVER_STRIGHT_DIST)
        {
            
        }
        else res[1] = currStep[1];
        res[0] = currStep[0] + res[1];
        return res;
    }
}
