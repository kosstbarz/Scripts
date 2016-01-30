
using System.Collections.Generic;

//Not used any more
public class TileData {

    public TileType[,] typeData;
    public float[,] heightData;

    public TileData(int xSize, int ySize)
    {
        typeData = new TileType[xSize, ySize];
        heightData = new float[xSize + 1, ySize + 1];
    }
}
