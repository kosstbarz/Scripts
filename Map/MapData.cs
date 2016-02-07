using UnityEngine;
using System.Collections;

public class MapData{
    
    public int xSize;
    public int ySize;
    public TileType[] tileTypes;
    public TileType[,] tileData;
    public float[,] heightData;

    public MapData(int xSize, int ySize)
    {
        this.xSize = xSize;
        this.ySize = ySize;
        tileData = new TileType[xSize, ySize];
        heightData = new float[xSize + 1, ySize + 1];
        FillTileTypes();
    }

    // TODO Make reading of types from file
    void FillTileTypes()
    {
        tileTypes = new TileType[6];
        TileType grass = new TileType(Types.Grassland, true, 1);
        
        TileType road = new TileType(Types.Road, true, 2);
        
        TileType mountain = new TileType(Types.Mountain, false, 1);
        
        TileType water = new TileType(Types.Water, false, 1);

        TileType swamp = new TileType(Types.Swamp, true, 0.5f);

        TileType unknown = new TileType(Types.Unknown, true, 1);

        tileTypes[0] = grass;
        tileTypes[1] = road;
        tileTypes[2] = mountain;
        tileTypes[3] = water;
        tileTypes[4] = swamp;
        tileTypes[5] = unknown;
    }
    // Returns type of tile with specified coordinates
    public TileType GetType(Vector2 coord)
    {
        int x = (int)coord.x;
        int y = (int)coord.y;
        if (x < 0 || x >= xSize || y < 0 || y >= ySize)
        {
            Debug.LogError("GetType is called with incorrect coordinates.");
            return null;
        }
        return tileData[x, y];
    }
    // Returns type by name
    public TileType GetType(string name)
    {

        foreach (TileType currType in tileTypes)
        {

            if (currType.name.ToString().Equals(name, System.StringComparison.OrdinalIgnoreCase))
            {
                return currType;
            }
        }
        Debug.LogError("Tile with name " + name + " not found in TyleTypes array.");
        return null;
    }

    public void SetTile(Vector2 coord, TileType type)
    {
        int x = (int)coord.x;
        int y = (int)coord.y;
        if (x < 0 || x >= xSize || y < 0 || y >= ySize)
        {
            Debug.LogError("SetTile is called with incorrect coordinates.");
            return;
        }
        tileData[(int)coord.x, (int)coord.y] = type;
        
    }

    public float GetHeight(Vector2 tile)
    {
        return heightData[(int)tile.x, (int)tile.y];
    }

    public float GetExactHeight(Vector2 tile)
    {
        // FIX ME:
        return heightData[(int)tile.x, (int)tile.y];
    }

    public float GetSlope(Vector2 corner, int x, int y)
    {
        float min = 100;
        float max = 0f;
        //Debug.Log("Corner.x = " + (int)corner.x);
        //Debug.Log("Corner.y = " + (int)corner.y);
        for (int i = (int) corner.x; i < corner.x + x + 1; i++)
        {
            for (int j = (int)corner.y; j < corner.y + y + 1; j++)
            {
                min = Mathf.Min(heightData[i, j], min);
                max = Mathf.Max(heightData[i, j], max);

            }
        }
        return max - min;
    }
}
